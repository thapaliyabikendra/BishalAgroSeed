import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CashTransactionComponent } from './cash-transaction.component';

const routes: Routes = [{ path: '', component: CashTransactionComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CashTransactionRoutingModule { }
