import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { DropdownDto } from '../dtos/models';

@Injectable({
  providedIn: 'root',
})
export class TransactionTypeService {
  apiName = 'Default';
  

  getTransactionTypes = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, DropdownDto[]>({
      method: 'GET',
      url: '/api/app/transaction-type/transaction-types',
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
