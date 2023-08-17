import { ListService, PagedResultDto } from '@abp/ng.core';
import { Confirmation, ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CreateUpdateCustomerDto, CustomerDto, CustomerService } from '@proxy/customers';


@Component({
  selector: 'app-customer',
  templateUrl: './customer.component.html',
  styleUrls: ['./customer.component.scss'],
  providers: [ListService]
})

export class CustomerComponent implements OnInit{
  data = { items: [], totalCount: 0 } as PagedResultDto<CustomerDto>;
  form: FormGroup;
  isModalOpen = false;
  selected = {} as CustomerDto;
  constructor(
    private fb: FormBuilder,
    private service: CustomerService,
    private toast: ToasterService,
    public readonly list: ListService,
    private confirmation: ConfirmationService
  ){ }
  
  
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
      isCustomer: [this.selected.isCustomer],
      isVendor: [this.selected.isVendor]
    });
  }
  create() {
    this.selected = {} as CustomerDto;
    this.isModalOpen = true;
    this.buildForm();
  }

  save() {
    if (this.form.invalid) {
      return;
    }
    const dto: CreateUpdateCustomerDto = {
      displayName: this.form.value.displayName,
      address: this.form.value.address,
      contactNo: this.form.value.contactNo,
      isCustomer: this.form.value.isCustomer,
      isVendor: this.form.value.isVendor
};
    const request = this.selected.id ? this.service.update(this.selected.id, dto) : this.service.create(dto);
    request.subscribe(() => {
      this.toast.success(this.selected.id ? '::UpdatedCustomer' : '::CreatedCustomer');
      this.isModalOpen = false;
      this.form.reset();
      this.list.get();
    });
  }
  delete(id: any) {
    this.confirmation.warn('::AreYouSureToDelete', 'AbpAccount::AreYouSure').subscribe((status) => {
      if (status === Confirmation.Status.confirm) {
        this.service.delete(id).subscribe(() => {
          this.toast.success('::DeletedCustomer');
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



