import type { TradeDto, TradeFilter } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { CreateTransactionDto, DropdownDto, FileBlobDto } from '../dtos/models';

@Injectable({
  providedIn: 'root',
})
export class TradeService {
  apiName = 'Default';
  

  exportExcel = (filter: TradeFilter, config?: Partial<Rest.Config>) =>
    this.restService.request<any, FileBlobDto>({
      method: 'POST',
      url: '/api/app/trade/export-excel',
      body: filter,
    },
    { apiName: this.apiName,...config });
  

  getListByFilter = (input: PagedAndSortedResultRequestDto, filter: TradeFilter, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<TradeDto>>({
      method: 'GET',
      url: '/api/app/trade/by-filter',
      params: { sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount, tradeTypeId: filter.tradeTypeId, customerId: filter.customerId, voucherNo: filter.voucherNo, fromTranDate: filter.fromTranDate, toTranDate: filter.toTranDate },
    },
    { apiName: this.apiName,...config });
  

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
