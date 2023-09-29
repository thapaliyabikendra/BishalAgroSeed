import { ListService, PagedResultDto } from '@abp/ng.core';
import { Confirmation, ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CompanyInfoDto, CompanyInfoService, CreateUpdateCompanyInfoDto } from '@proxy/company-infos';

@Component({
  selector: 'app-company-info',
  templateUrl: './company-info.component.html',
  styleUrls: ['./company-info.component.scss'],
  providers: [ListService]
})
export class CompanyInfoComponent implements OnInit {
  data = { items: [], totalCount: 0 } as PagedResultDto<CompanyInfoDto>;
  form: FormGroup;
  isModalOpen = false;
  selected = {} as CompanyInfoDto;
  constructor(
    private fb: FormBuilder,
    private service: CompanyInfoService,
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
      address: [this.selected.address],
      contactNo: [this.selected.contactNo],
      panNo: [this.selected.panNo],

    });
  }
  create() {
    this.selected = {} as CompanyInfoDto;
    this.isModalOpen = true;
    this.buildForm();
  }

  save() {
    if (this.form.invalid) {
      return;
    }
    const dto: CreateUpdateCompanyInfoDto = {
      displayName: this.form.value.displayName,
      address: this.form.value.address,
      contactNo: this.form.value.contactNo,
      panNo: this.form.value.panNo
    };

    const request = this.selected.id ? this.service.update(this.selected.id, dto) : this.service.create(dto);
    request.subscribe(() => {
      this.toast.success(this.selected.id ? '::UpdatedCompanyInfo' : '::CreatedCompanyInfo');
      this.isModalOpen = false;
      this.form.reset();
      this.list.get();
    });
  }
  delete(id: any) {
    this.confirmation.warn('::AreYouSureToDelete', 'AbpAccount::AreYouSure').subscribe((status) => {
      if (status === Confirmation.Status.confirm) {
        this.service.delete(id).subscribe(() => {
          this.toast.success('::DeletedCompanyInfo');
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