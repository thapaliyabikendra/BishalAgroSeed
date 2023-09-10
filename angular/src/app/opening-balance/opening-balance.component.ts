import { ListService, PagedResultDto } from '@abp/ng.core';
import { Confirmation, ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CustomerService } from '@proxy/customers';
import { DropdownDto } from '@proxy/dtos';
import { OpeningBalanceDto, OpeningBalanceService } from '@proxy/opening-balances';

@Component({
  selector: 'app-opening-balance',
  templateUrl: './opening-balance.component.html',
  styleUrls: ['./opening-balance.component.scss'],
  providers: [ListService]
})
export class OpeningBalanceComponent implements OnInit {
  data = { items: [], totalCount: 0 } as PagedResultDto<OpeningBalanceDto>;
  form: FormGroup;
  isModalOpen = false;
  selected = {} as OpeningBalanceDto;
  customers: DropdownDto[] = [];
  constructor(
    private fb: FormBuilder,
    private service: OpeningBalanceService,
    private toast: ToasterService,
    public readonly list: ListService,
    private customerService: CustomerService,
    private confirmation: ConfirmationService
  ){ }

  ngOnInit(): void {
    const streamCreator = (query) => this.service.getList(query);
    this.list.hookToQuery(streamCreator).subscribe((resp) => {
      this.data = resp;
    });
    this.customerService.getCustomers().subscribe((resp) => {
      this.customers = resp;
    });
  }

  buildForm() {
    this.form = this.fb.group({
      amount: [this.selected.amount, Validators.required],
      tranDate: [this.selected.tranDate?.split('T')[0], Validators.required],
      customerId: [this.selected.customerId, Validators.required],
      isReceivable: [this.selected.isReceivable],
    });
  }

  create() {
    this.selected = {} as OpeningBalanceDto;
    this.isModalOpen = true;
    this.buildForm();
  }

   save(){
    if (this.form.invalid) {
      return;
    }
    const dto: OpeningBalanceDto = {
      amount: this.form.value.amount,
      tranDate: this.form.value.tranDate,
      customerId: this.form.value.customerId,
      isReceivable: this.form.value.isReceivable
   };

   const request = this.selected.id ? this.service.update(this.selected.id, dto) : this.service.create(dto);
   request.subscribe(() => {
     this.toast.success(this.selected.id ? '::UpdatedOpeningBalance' : '::CreatedOpeningBalance');
     this.isModalOpen = false;
     this.form.reset();
     this.list.get();
   });
 }

 delete(id: any) {
   this.confirmation.warn ('::AreYouSureToDelete', 'AbpAccount::AreYouSure').subscribe((status) => {
     if (status === Confirmation.Status.confirm) {
       this.service.delete(id).subscribe(() => {
         this.toast.success('::DeletedOpeningBalance');
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