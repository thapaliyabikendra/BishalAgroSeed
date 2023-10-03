import { ListService, PagedResultDto } from '@abp/ng.core';
import { Confirmation, ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BrandService } from '@proxy/brands';
import { CategoryDto, CategoryService, GetCategoryFilter } from '@proxy/categories';
import { DropdownDto } from '@proxy/dtos';
import { CreateUpdateProductDto, GetUnitTypeDto, ProductDto, ProductFilter, ProductService } from '@proxy/products';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.scss'],
  providers: [ListService]
})

export class ProductComponent implements OnInit {
  data = { items: [], totalCount: 0 } as PagedResultDto<ProductDto>;
  filter = {} as ProductFilter;
  showFilter = false as Boolean;
  form: FormGroup;
  isModalOpen = false;
  selected = {} as ProductDto;
  categories: DropdownDto[] = [];
  brands: DropdownDto[] = [];
  unitTypes: GetUnitTypeDto[] = [];
  isViewImageModalOpen: boolean;
  base64Data:any;
  unitTypeDescription: string= "";
  constructor(
    private fb: FormBuilder,
    private service: ProductService,
    private categoryService: CategoryService,
    private brandService: BrandService,
    private toast: ToasterService,
    public readonly list: ListService,
    private confirmation: ConfirmationService
  ) { }


  ngOnInit(): void {
    const streamCreator = (query) => this.service.getListByFilter(query, this.filter);
    this.list.hookToQuery(streamCreator).subscribe((resp) => {
      this.data = resp;
    });

    let categoryFilter = {} as GetCategoryFilter;
    forkJoin([
      this.categoryService.getCategories(categoryFilter),
      this.brandService.getBrands(),
      this.service.getUnitTypes()
    ])
    .subscribe((res) => {
      this.categories = res[0];
      this.brands = res[1];
      this.unitTypes = res[2];
    });
  }

  toggleFilter() {
    this.showFilter = !this.showFilter;
  }

  getData() {
    this.list.get();
  }

  clearFilter(){
    this.filter = {};
    this.getData();
  }

  buildForm() {
    this.form = this.fb.group({
      displayName: [this.selected.displayName, Validators.required],
      categoryId: [this.selected.categoryId],
      brandId: [this.selected.brandId],
      unit: [this.selected.unit],
      unitTypeId: [this.selected.unitTypeId],
      price: [this.selected.price],
      description: [this.selected.description]
    });
  }
  create() {
    this.unitTypeDescription = "";
    this.selected = {} as ProductDto;
    this.isModalOpen = true;
    this.buildForm();
  }

  save() {
    if (this.form.invalid) {
      return;
    }
    // const dto: CreateUpdateProductDto = {
    //   displayName: this.form.value.displayName,
    //   categoryId: this.form.value.categoryId,
    //   brandId: this.form.value.brandId,
    //   unit: this.form.value.unit,
    //   unitTypeId: this.form.value.unitTypeId,
    //   price: this.form.value.price,
    //   description: this.form.value.description,
    //   file: this.selectedFile
    // };

    const formData = new FormData();
    formData.append("displayName", this.form.value.displayName);
    formData.append("categoryId", this.form.value.categoryId);
    formData.append("brandId", this.form.value.brandId);
    formData.append("unit", this.form.value.unit);
    formData.append("unitTypeId", this.form.value.unitTypeId);
    formData.append("price", this.form.value.price);
    formData.append("description", this.form.value.description);
    formData.append("file", this.selectedFile);

    const request = this.selected.id ? this.service.update(this.selected.id, formData) : this.service.create(formData);
    request.subscribe(() => {
      this.toast.success(this.selected.id ? '::UpdatedProduct' : '::CreatedProduct');
      this.isModalOpen = false;
      this.form.reset();
      this.list.get();
    });
  }
  delete(id: any) {
    this.confirmation.warn('::AreYouSureToDelete', 'AbpAccount::AreYouSure').subscribe((status) => {
      if (status === Confirmation.Status.confirm) {
        this.service.delete(id).subscribe(() => {
          this.toast.success('::Poduct');
          this.list.get();
        });
      }
    });
  }
  edit(id: any) {
    this.unitTypeDescription = "";
    this.service.get(id).subscribe((res) => {
      this.selected = res;
      this.unitTypeDescription = res.unitTypeDescription;
      this.buildForm();
      this.isModalOpen = true;
    });
  }
  selectedFile: File;
  upload(event: any) {
    this.selectedFile = event.target.files[0];
  }

  viewImage(id: any) {
    this.service.getProductImage(id).subscribe((res) => {
      this.base64Data = `data:image/*;base64,${res.content}`;
      this.isViewImageModalOpen = true;
    });
  }
  changeUnitType(unitType: any){
    this.unitTypeDescription = unitType.description;
  }
}
