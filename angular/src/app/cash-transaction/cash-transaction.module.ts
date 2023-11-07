import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CashTransactionRoutingModule } from './cash-transaction-routing.module';
import { CashTransactionComponent } from './cash-transaction.component';
import { SharedModule } from '../shared/shared.module';


@NgModule({
  declarations: [
    CashTransactionComponent
  ],
  imports: [
    CommonModule,
    CashTransactionRoutingModule,
    SharedModule
  ]
})
export class CashTransactionModule { }
