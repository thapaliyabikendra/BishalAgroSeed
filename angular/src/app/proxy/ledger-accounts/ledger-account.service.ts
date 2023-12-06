import type { LedgerAccountDto, LedgerAccountFilter } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { FileBlobDto } from '../dtos/models';

@Injectable({
  providedIn: 'root',
})
export class LedgerAccountService {
  apiName = 'Default';
  

  exportExcel = (filter: LedgerAccountFilter, config?: Partial<Rest.Config>) =>
    this.restService.request<any, FileBlobDto>({
      method: 'GET',
      url: '/api/app/ledger-account/export-excel',
      body: filter,
    },
    { apiName: this.apiName,...config });
  

  getListByFilter = (input: PagedAndSortedResultRequestDto, filter: LedgerAccountFilter, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<LedgerAccountDto>>({
      method: 'GET',
      url: '/api/app/ledger-account/by-filter',
      params: { sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount, customerId: filter.customerId, fromTranDate: filter.fromTranDate, toTranDate: filter.toTranDate },
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
