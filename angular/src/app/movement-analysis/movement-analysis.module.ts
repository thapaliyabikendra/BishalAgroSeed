import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MovementAnalysisRoutingModule } from './movement-analysis-routing.module';
import { MovementAnalysisComponent } from './movement-analysis.component';
import { SharedModule } from '../shared/shared.module';


@NgModule({
  declarations: [
    MovementAnalysisComponent
  ],
  imports: [
    CommonModule,
    MovementAnalysisRoutingModule,
    SharedModule
  ]
})
export class MovementAnalysisModule { }
