import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NumberGenerationComponent } from './number-generation.component';

const routes: Routes = [{ path: '', component: NumberGenerationComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class NumberGenerationRoutingModule { }
