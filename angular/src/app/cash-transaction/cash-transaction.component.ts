import { ToasterService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CashTransactionService, CreateTransactionPaymentDto } from '@proxy/cash-transactions';
import { CustomerService } from '@proxy/customers';
import { CreateTransactionDto, DropdownDto } from '@proxy/dtos';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-cash-transaction',
  templateUrl: './cash-transaction.component.html',
  styleUrls: ['./cash-transaction.component.scss']
})
export class CashTransactionComponent implements OnInit {
  customers = [] as DropdownDto[];
  cashTransactionTypes = [] as DropdownDto[];
  paymentTypes = [] as DropdownDto[];
  form: FormGroup;
  showBankPaymentMode: boolean = false;

  constructor(
    private fb: FormBuilder,
    private customerService: CustomerService,
    private cashTransactionService: CashTransactionService,
    private toast: ToasterService,
  ) { }
  ngOnInit() {
    var request1 = this.customerService.getCustomers();
    var request2 = this.cashTransactionService.getCashTransactionTypes();
    var request3 = this.cashTransactionService.getPaymentTypes();
    forkJoin([request1, request2, request3])
      .subscribe((resp) => {
        this.customers = resp[0];
        this.cashTransactionTypes = resp[1];
        this.paymentTypes = resp[2];
      });
    this.buildForm();
  }
  buildForm() {
    this.form = this.fb.group({
      customerId: ["", Validators.required],
      transactionTypeId: ["", Validators.required],
      subTotalAmount: [0, Validators.required],
      amount: { value: 0, disabled: true },
      discountAmount: [0],
      transportCharge: [0],
      voucherNo: ["", Validators.required],
      paymentTypeId: ["", Validators.required],
      chequeNo: [],
      bankName: [],
      tranDate: ["", Validators.required]
    });

  }

  calculateAmount() {
    let discountAmount = this.form.value.discountAmount ?? 0;
    if (discountAmount < 0) {
      this.toast.warn('::Transaction:InvalidDiscountAmount');
      this.form.get('discountAmount')?.setValue(0);
      return;
    }
    let transportCharge = this.form.value.transportCharge ?? 0;
    if (transportCharge < 0) {
      this.toast.warn('::Transaction:InvalidTransportCharge');
      this.form.get('transportCharge')?.setValue(0);
      return;
    }

    let subTotalAmount = this.form.value.subTotalAmount ?? 0;
    if (subTotalAmount < 0) {
      this.toast.warn('::Transaction:InvalidAmount');
      this.form.get('subTotalAmount')?.setValue(0);
      return;
    }

    let amt = subTotalAmount - discountAmount + transportCharge;
    this.form.get('amount')?.setValue(amt);
  }
  save() {
    if (this.form.invalid) {
      return;
    }

    const payment: CreateTransactionPaymentDto = {
      paymentTypeId: this.form.value.paymentTypeId,
      chequeNo: this.form.value.chequeNo,
      bankName: this.form.value.bankName
    };

    const dto: CreateTransactionDto = {
      transactionTypeId: this.form.value.transactionTypeId,
      customerId: this.form.value.customerId,
      amount: this.form.get('amount')?.value,
      discountAmount: this.form.value.discountAmount ?? 0,
      transportCharge: this.form.value.transportCharge ?? 0,
      voucherNo: this.form.value.voucherNo,
      tranDate: this.form.value.tranDate,
      details: null,
      payment: payment
    };
    this.cashTransactionService.saveTransaction(dto).subscribe(() => {
      this.toast.success('::Transaction:Save');
      this.form.reset();
      this.buildForm();
    });
  }

  changePaymentType(paymentType: any) {
    this.showBankPaymentMode = paymentType.value == "2";
    this.form.get('chequeNo')?.setValue('');
    this.form.get('bankName')?.setValue('');
  }
}