import type { InventoryReportDto, InventoryReportFilter } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { FileBlobDto } from '../../dtos/models';

@Injectable({
  providedIn: 'root',
})
export class InventoryReportService {
  apiName = 'Default';
  

  exportExcel = (filter: InventoryReportFilter, config?: Partial<Rest.Config>) =>
    this.restService.request<any, FileBlobDto>({
      method: 'GET',
      url: '/api/app/inventory-report/export-excel',
      params: { productName: filter.productName, countDate: filter.countDate },
    },
    { apiName: this.apiName,...config });
  

  getListByFilter = (input: PagedAndSortedResultRequestDto, filter: InventoryReportFilter, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<InventoryReportDto>>({
      method: 'GET',
      url: '/api/app/inventory-report/by-filter',
      params: { sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount, productName: filter.productName, countDate: filter.countDate },
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
