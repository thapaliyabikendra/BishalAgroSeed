import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CycleCountComponent } from './cycle-count.component';

const routes: Routes = [{ path: '', component: CycleCountComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CycleCountRoutingModule { }
