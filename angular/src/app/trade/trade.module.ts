import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TradeRoutingModule } from './trade-routing.module';
import { TradeComponent } from './trade.component';
import { SharedModule } from '../shared/shared.module';
import { SaveTradeComponent } from './save-trade/save-trade.component';
import { NgSelectModule } from '@ng-select/ng-select';


@NgModule({
  declarations: [
    TradeComponent,
    SaveTradeComponent
  ],
  imports: [
    CommonModule,
    TradeRoutingModule,
    SharedModule
  ],
})
export class TradeModule { }
