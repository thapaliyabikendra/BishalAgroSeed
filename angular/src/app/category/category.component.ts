import { ListService, PagedResultDto } from '@abp/ng.core';
import { Confirmation, ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CategoryDto, CategoryService, CreateUpdateCategoryDto } from '@proxy/categories';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.scss'],
  providers: [ListService]
})
export class CategoryComponent implements OnInit {
  data = { items: [], totalCount: 0 } as PagedResultDto<CategoryDto>;
  form: FormGroup;
  isModalOpen = false;
  selected = {} as CategoryDto;
  constructor(
    private fb: FormBuilder,
    private service: CategoryService,
    private toast: ToasterService,
    public readonly list: ListService,
    private confirmation: ConfirmationService
  ) { }

  ngOnInit(): void {
    const streamCreator = (query) => this.service.getList(query);
    this.list.hookToQuery(streamCreator).subscribe((resp) => {
      this.data = resp;
    });
  }

  buildForm() {
    this.form = this.fb.group({
      displayName: [this.selected.displayName, Validators.required],
      parentId: [this.selected.parentId],
      isActive: [this.selected.isActive]
    });
  }
  create() {
    this.selected = {} as CategoryDto;
    this.isModalOpen = true;
    this.buildForm();
  }

  save() {
    if (this.form.invalid) {
      return;
    }
    const dto: CreateUpdateCategoryDto = {
      displayName: this.form.value.displayName,
      parentId: this.form.value.paentId,
      isActive: this.form.value.isActive
    };
    const request = this.selected.id ? this.service.update(this.selected.id, dto) : this.service.create(dto);
    request.subscribe(() => {
      this.toast.success(this.selected.id ? '::UpdatedCategory' : '::CreatedCustomer');
      this.isModalOpen = false;
      this.form.reset();
      this.list.get();
    });
  }

  delete(id: any) {
    this.confirmation.warn('::AreYouSureToDelete', 'AbpAccount::AreYouSure').subscribe((status) => {
      if (status === Confirmation.Status.confirm) {
        this.service.delete(id).subscribe(() => {
          this.toast.success('::DeletedCategory');
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