import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MovementAnalysisComponent } from './movement-analysis.component';

const routes: Routes = [{ path: '', component: MovementAnalysisComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MovementAnalysisRoutingModule { }
