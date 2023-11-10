import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { CreateTransactionDto, DropdownDto } from '../dtos/models';

@Injectable({
  providedIn: 'root',
})
export class CashTransactionService {
  apiName = 'Default';
  

  getCashTransactionTypes = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, DropdownDto[]>({
      method: 'GET',
      url: '/api/app/cash-transaction/cash-transaction-types',
    },
    { apiName: this.apiName,...config });
  

  getPaymentTypes = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, DropdownDto[]>({
      method: 'GET',
      url: '/api/app/cash-transaction/payment-types',
    },
    { apiName: this.apiName,...config });
  

  saveTransaction = (input: CreateTransactionDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/app/cash-transaction/save-transaction',
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
