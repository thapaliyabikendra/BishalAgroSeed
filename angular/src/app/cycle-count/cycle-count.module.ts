import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CycleCountRoutingModule } from './cycle-count-routing.module';
import { CycleCountComponent } from './cycle-count.component';


@NgModule({
  declarations: [
    CycleCountComponent
  ],
  imports: [
    CommonModule,
    CycleCountRoutingModule
  ]
})
export class CycleCountModule { }
