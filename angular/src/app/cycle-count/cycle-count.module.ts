import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CycleCountRoutingModule } from './cycle-count-routing.module';
import { CycleCountComponent } from './cycle-count.component';
import { SharedModule } from '../shared/shared.module';
import { CycleCountDetailComponent } from './cycle-count-detail/cycle-count-detail.component';


@NgModule({
  declarations: [
    CycleCountComponent,
    CycleCountDetailComponent
  ],
  imports: [
    CommonModule,
    CycleCountRoutingModule,
    SharedModule
  ]
})
export class CycleCountModule { }
