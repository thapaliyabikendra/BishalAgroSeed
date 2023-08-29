import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FiscalYearRoutingModule } from './fiscal-year-routing.module';
import { FiscalYearComponent } from './fiscal-year.component';


@NgModule({
  declarations: [
    FiscalYearComponent
  ],
  imports: [
    CommonModule,
    FiscalYearRoutingModule
  ]
})
export class FiscalYearModule { }
