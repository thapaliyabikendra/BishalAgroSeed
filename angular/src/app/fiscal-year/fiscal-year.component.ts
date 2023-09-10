import { ListService, PagedResultDto } from '@abp/ng.core';
import { Confirmation, ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CreateUpdateFiscalYearDto, FiscalYearDto, FiscalYearService } from '@proxy/fiscal-years';

@Component({
  selector: 'app-fiscal-year',
  templateUrl: './fiscal-year.component.html',
  styleUrls: ['./fiscal-year.component.scss'],
  providers: [ListService]
})
export class FiscalYearComponent implements OnInit {
  data = { items: [], totalCount: 0 } as PagedResultDto<FiscalYearDto>;
  form: FormGroup;
  isModalOpen = false;
  selected = {} as FiscalYearDto;
  constructor(
    private fb: FormBuilder,
    private service: FiscalYearService,
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
      fromDate: [this.selected.fromDate?.split('T')[0], Validators.required],
      toDate: [this.selected.toDate?.split('T')[0], Validators.required],
      isCurrent: [this.selected.isCurrent],
    });
  }

  create() {
    this.selected = {} as FiscalYearDto;
    this.isModalOpen = true;
    this.buildForm();
  }

  save() {
    if (this.form.invalid) {
      return;
    }
    const dto: CreateUpdateFiscalYearDto = {
      displayName: this.form.value.displayName,
      fromDate: this.form.value.fromDate,
      toDate: this.form.value.toDate,
      isCurrent: this.form.value.isCurrent
    };

    const request = this.selected.id ? this.service.update(this.selected.id, dto) : this.service.create(dto);
    request.subscribe(() => {
      this.toast.success(this.selected.id ? '::UpdatedFiscalYear' : '::CreatedFiscalYear');
      this.isModalOpen = false;
      this.form.reset();
      this.list.get();
    });
  }
  delete(id: any) {
    this.confirmation.warn('::AreYouSureToDelete', 'AbpAccount::AreYouSure').subscribe((status) => {
      if (status === Confirmation.Status.confirm) {
        this.service.delete(id).subscribe(() => {
          this.toast.success('::DeletedFiscalYear');
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