import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CompanyInfoRoutingModule } from './company-info-routing.module';
import { CompanyInfoComponent } from './company-info.component';
import { SharedModule } from '../shared/shared.module';


@NgModule({
  declarations: [
    CompanyInfoComponent
  ],
  imports: [
    CommonModule,
    CompanyInfoRoutingModule,
    SharedModule
  ]
})
export class CompanyInfoModule { }
