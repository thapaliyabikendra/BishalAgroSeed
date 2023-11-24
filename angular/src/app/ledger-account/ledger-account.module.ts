import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LedgerAccountRoutingModule } from './ledger-account-routing.module';
import { LedgerAccountComponent } from './ledger-account.component';
import { SharedModule } from '../shared/shared.module';


@NgModule({
  declarations: [
    LedgerAccountComponent
  ],
  imports: [
    CommonModule,
    LedgerAccountRoutingModule,
    SharedModule
  ]
})
export class LedgerAccountModule { }
