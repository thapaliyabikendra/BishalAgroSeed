import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { OpeningBalanceRoutingModule } from './opening-balance-routing.module';
import { OpeningBalanceComponent } from './opening-balance.component';
import { SharedModule } from '../shared/shared.module';


@NgModule({
  declarations: [
    OpeningBalanceComponent
  ],
  imports: [
    CommonModule,
    OpeningBalanceRoutingModule,
    SharedModule
  ]
})
export class OpeningBalanceModule { }
