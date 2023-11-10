import { ToasterService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { CustomerService } from '@proxy/customers';
import { CreateTransactionDto, DropdownDto } from '@proxy/dtos';
import { GetProductDto, ProductService } from '@proxy/products';
import { CreateTransactionDetailDto, TradeService } from '@proxy/trades';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-trade',
  templateUrl: './trade.component.html',
  styleUrls: ['./trade.component.scss']

})
export class TradeComponent implements OnInit {
  tradeTypes = [] as DropdownDto[];
  customers = [] as DropdownDto[];
  products = [] as GetProductDto[];
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
      amount: { value: 0, disabled: true },
      discountAmount: [0],
      transportCharge: [0],
      voucherNo: ["", Validators.required],
      transactionDetails: this.fb.array([])
    });
  }

  addTransactionDetail() {
    let detailGroup = this.fb.group({
      productId: ["", Validators.required],
      cases: ["", [Validators.required, Validators.min(1)]],
      quantity: ["", [Validators.required, Validators.min(1)]],
      pricePerUnit: { value: 0, disabled: true },
      price: { value: 0, disabled: true }
    });

    this.detailControls.push(detailGroup);
  }

  removeTransactionDetail(i: number) {
    this.detailControls.removeAt(i);
  }

  get detailControls() {
    return <FormArray>this.form.get('transactionDetails');
  }

  save() {
    if (this.form.invalid) {
      return;
    }

    let details = this.detailControls.controls.map(s => {
      return {
      productId : s.get('productId')?.value,
      cases : s.get('cases')?.value,
      quantity : s.get('quantity')?.value,
      price : s.get('price')?.value,

    } as CreateTransactionDetailDto});

    const dto: CreateTransactionDto = {
      transactionTypeId: this.form.value.transactionTypeId,
      customerId: this.form.value.customerId,
      amount: this.form.get('amount')?.value,
      discountAmount: this.form.value.discountAmount ?? 0,
      transportCharge: this.form.value.transportCharge ?? 0,
      voucherNo: this.form.value.voucherNo,
      tranDate: this.form.value.tranDate,
      details: details,
      payment: null
    };
    this.tradeService.saveTransaction(dto).subscribe(() => {
      this.toast.success('::Transaction:Save');
      this.form.reset();
      this.buildForm();
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
    let totalProductAmt = this.detailControls.controls.reduce((acc, s) => acc + s.get('price')?.value, 0);
    let amt = totalProductAmt -discountAmount + transportCharge;
    this.form.get('amount')?.setValue(amt);
  }

  displayPrice(i: any, p: any) {
    let product = this.detailControls.at(i) as FormGroup;
    let productCount = this.detailControls.controls.filter(s => s.get('productId')?.value == p.id).length;
    if(productCount > 1){
      this.toast.warn('::Transaction:DuplicateProduct');
      product.get('productId')?.setValue('');
      return;
    }

    product.get('pricePerUnit')?.setValue(p.price);

    this.calculateProductAmount(i);
  }

  calculateProductAmount(i) {
    let product = this.detailControls.at(i) as FormGroup;
    let quantity = product.get('quantity')?.value;
    let pricePerUnit = product.get('pricePerUnit')?.value;
    let amount = (quantity * pricePerUnit);
    // product.get('price')?.setValue(amount);
    product.patchValue({price : amount});

    this.calculateAmount();
  }
}