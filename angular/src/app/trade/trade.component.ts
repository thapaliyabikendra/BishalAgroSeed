import { ToasterService } from '@abp/ng.theme.shared';
import { Component } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CustomerService } from '@proxy/customers';
import { DropdownDto } from '@proxy/dtos';
import { ProductService } from '@proxy/products';
import { CreateTransactionDto, TradeService } from '@proxy/trades';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-trade',
  templateUrl: './trade.component.html',
  styleUrls: ['./trade.component.scss']

})
export class TradeComponent {
  tradeTypes = [] as DropdownDto[];
  customers = [] as DropdownDto[];
  products = [] as DropdownDto[];
  form: FormGroup;

  constructor(
    private fb: FormBuilder,
    private productService: ProductService,
    private customerService: CustomerService,
    private tradeService: TradeService,
    private toast: ToasterService,
  ) { }

  ngOnInit(): void {
    var request1 = this.tradeService.getTradeTypes();
    var request2 = this.customerService.getCustomers();
    var request3 = this.productService.getProducts();
    forkJoin([request1, request2, request3])
      .subscribe((resp) => {
        this.tradeTypes = resp[0];
        this.customers = resp[1];
        this.products = resp[2];
      });
    this.buildForm();
  }

  buildForm() {
    this.form = this.fb.group({
      transactionTypeId: ["", Validators.required],
      customerId: ["", Validators.required],
      amount: ["", [Validators.required, Validators.min(1)]],
      discountAmount: [""],
      transportCharge: [""],
      voucherNo: ["", Validators.required],
      transactionDetails: this.fb.array([])
    });
  }

  addTransactionDetail() {
    let detailGroup = this.fb.group({
      productId: ["", Validators.required],
      cases: ["", [Validators.required, Validators.min(1)]],
      quantity: ["", [Validators.required, Validators.min(1)]],
      price: ["", [Validators.required, Validators.min(1)]]
    });

    this.detailControls.push(detailGroup);
  }

  removeTransactionDetail(i: number) {
    this.detailControls.removeAt(i);
  }

  get detailControls(){
    return <FormArray>this.form.get('transactionDetails');
  }

  save() {
    if (this.form.invalid) {
      return;
    }

    const dto: CreateTransactionDto = {
      transactionTypeId: this.form.value.transactionTypeId,
      customerId: this.form.value.customerId,
      amount: this.form.value.amount,
      discountAmount: this.form.value.discountAmount ?? 0,
      transportCharge: this.form.value.transportCharge ?? 0,
      voucherNo: this.form.value.voucherNo,
      details: this.form.value.transactionDetails
    };
    this.tradeService.saveTransaction(dto).subscribe(() => {
      this.toast.success('::SaveTransaction');
      this.form.reset();
    });
  }
}