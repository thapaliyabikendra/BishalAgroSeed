import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FiscalYearRoutingModule } from './fiscal-year-routing.module';
import { FiscalYearComponent } from './fiscal-year.component';
import { SharedModule } from '../shared/shared.module';


@NgModule({
  declarations: [
    FiscalYearComponent
  ],
  imports: [
    CommonModule,
    FiscalYearRoutingModule,
    SharedModule
  ]
})
export class FiscalYearModule { }
