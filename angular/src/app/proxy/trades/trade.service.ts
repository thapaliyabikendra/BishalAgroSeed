import type { CreateTransactionDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { DropdownDto } from '../dtos/models';

@Injectable({
  providedIn: 'root',
})
export class TradeService {
  apiName = 'Default';
  

  getTradeTypes = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, DropdownDto[]>({
      method: 'GET',
      url: '/api/app/trade/trade-types',
    },
    { apiName: this.apiName,...config });
  

  saveTransaction = (input: CreateTransactionDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/app/trade/save-transaction',
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
