import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { NumberGenerationRoutingModule } from './number-generation-routing.module';
import { NumberGenerationComponent } from './number-generation.component';
import { SharedModule } from '../shared/shared.module';


@NgModule({
  declarations: [
    NumberGenerationComponent
  ],
  imports: [
    CommonModule,
    NumberGenerationRoutingModule,
    SharedModule
  ]
})
export class NumberGenerationModule { }
