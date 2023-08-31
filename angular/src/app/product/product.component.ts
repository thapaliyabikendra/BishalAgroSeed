import { ListService, PagedResultDto } from '@abp/ng.core';
import { Confirmation, ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BrandService } from '@proxy/brands';
import { CategoryDto, CategoryService, GetCategoryFilter } from '@proxy/categories';
import { DropdownDto } from '@proxy/dtos';
import { CreateUpdateProductDto, ProductDto, ProductService } from '@proxy/products';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.scss'],
  providers: [ListService]
})

export class ProductComponent implements OnInit {
  data = { items: [], totalCount: 0 } as PagedResultDto<ProductDto>;
  form: FormGroup;
  isModalOpen = false;
  selected = {} as ProductDto;
  categories: DropdownDto[] = [];
  brands: DropdownDto[] = [];
  unitTypes: DropdownDto[] = [];
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
    const streamCreator = (query) => this.service.getList(query);
    this.list.hookToQuery(streamCreator).subscribe((resp) => {
      this.data = resp;
    });

    let filter = {} as GetCategoryFilter;
    this.categoryService.getCategories(filter).subscribe((res) => {
      this.categories = res;
    });
    
    this.brandService.getBrands().subscribe((res) =>{
      this.brands = res;
    });
    this.service.getUnitTypes().subscribe((res) =>{
      this.unitTypes = res;
    });
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
    this.selected = {} as ProductDto;
    this.isModalOpen = true;
    this.buildForm();
  }

  save() {
    if (this.form.invalid) {
      return;
    }
    const dto: CreateUpdateProductDto = {
      displayName: this.form.value.displayName,
      categoryId: this.form.value.categoryId,
      brandId: this.form.value.brandId,
      unit: this.form.value.unit,
      unitTypeId: this.form.value.unitTypeId,
      price: this.form.value.price,
      description: this.form.value.description
    };
    const request = this.selected.id ? this.service.update(this.selected.id, dto) : this.service.create(dto);
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
    this.service.get(id).subscribe((res) => {
      this.selected = res;
      this.buildForm();
      this.isModalOpen = true;
    });
  }
}