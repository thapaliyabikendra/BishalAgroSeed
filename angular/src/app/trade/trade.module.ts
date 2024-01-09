import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TradeRoutingModule } from './trade-routing.module';
import { TradeComponent } from './trade.component';
import { SharedModule } from '../shared/shared.module';
import { CreateTradeComponent } from './create-trade/create-trade.component';


@NgModule({
  declarations: [
    TradeComponent,
    CreateTradeComponent
  ],
  imports: [
    CommonModule,
    TradeRoutingModule,
    SharedModule
  ]
})
export class TradeModule { }
