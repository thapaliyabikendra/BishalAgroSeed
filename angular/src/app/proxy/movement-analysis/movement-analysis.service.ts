import type { MovementAnalysisDto, MovementAnalysisFilter } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { FileBlobDto } from '../dtos/models';

@Injectable({
  providedIn: 'root',
})
export class MovementAnalysisService {
  apiName = 'Default';
  

  exportExcel = (filter: MovementAnalysisFilter, config?: Partial<Rest.Config>) =>
    this.restService.request<any, FileBlobDto>({
      method: 'GET',
      url: '/api/app/movement-analysis/export-excel',
      params: { productName: filter.productName, fromTranDate: filter.fromTranDate, toTranDate: filter.toTranDate, customerId: filter.customerId },
    },
    { apiName: this.apiName,...config });
  

  getListByFilter = (input: PagedAndSortedResultRequestDto, filter: MovementAnalysisFilter, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<MovementAnalysisDto>>({
      method: 'GET',
      url: '/api/app/movement-analysis/by-filter',
      params: { sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount, productName: filter.productName, fromTranDate: filter.fromTranDate, toTranDate: filter.toTranDate, customerId: filter.customerId },
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
