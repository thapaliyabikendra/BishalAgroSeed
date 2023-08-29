import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { OpeningBalanceRoutingModule } from './opening-balance-routing.module';
import { OpeningBalanceComponent } from './opening-balance.component';


@NgModule({
  declarations: [
    OpeningBalanceComponent
  ],
  imports: [
    CommonModule,
    OpeningBalanceRoutingModule
  ]
})
export class OpeningBalanceModule { }
