import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MovementAnalysisRoutingModule } from './movement-analysis-routing.module';
import { MovementAnalysisComponent } from './movement-analysis.component';


@NgModule({
  declarations: [
    MovementAnalysisComponent
  ],
  imports: [
    CommonModule,
    MovementAnalysisRoutingModule
  ]
})
export class MovementAnalysisModule { }
