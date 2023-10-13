import type { CycleCountDetailDto, CycleCountDetailFilter, CycleCountDto, CycleCountFilter, UpdateCycleCountDetailDto, UpdateCycleCountDetailFileDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { FileBlobDto } from '../dtos/models';

@Injectable({
  providedIn: 'root',
})
export class CycleCountService {
  apiName = 'Default';
  

  bulkUpdateCycleCountDetail = (cycleCountId: string, input: UpdateCycleCountDetailDto[], config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/cycle-count/bulk-update-cycle-count-detail/${cycleCountId}`,
      body: input,
    },
    { apiName: this.apiName,...config });
  

  bulkUpdateCycleCountDetailByExcel = (input: any, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/app/cycle-count/bulk-update-cycle-count-detail-by-excel',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  close = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/cycle-count/${id}/close`,
    },
    { apiName: this.apiName,...config });
  

  create = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/app/cycle-count',
    },
    { apiName: this.apiName,...config });
  

  downloadBulkUpdateCycleCountDetailByExcelTemplate = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, FileBlobDto>({
      method: 'GET',
      url: '/api/app/cycle-count/download-bulk-update-cycle-count-detail-by-excel-template',
    },
    { apiName: this.apiName,...config });
  

  exportCycleCountDetailExcel = (filter: CycleCountDetailFilter, config?: Partial<Rest.Config>) =>
    this.restService.request<any, FileBlobDto>({
      method: 'GET',
      url: '/api/app/cycle-count/export-cycle-count-detail-excel',
      params: { cycleCountId: filter.cycleCountId, categoryName: filter.categoryName, productName: filter.productName, remarks: filter.remarks },
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CycleCountDto>({
      method: 'GET',
      url: `/api/app/cycle-count/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getCycleCountDetailListByFilter = (input: PagedAndSortedResultRequestDto, filter: CycleCountDetailFilter, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<CycleCountDetailDto>>({
      method: 'GET',
      url: '/api/app/cycle-count/cycle-count-detail-list-by-filter',
      params: { sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount, cycleCountId: filter.cycleCountId, categoryName: filter.categoryName, productName: filter.productName, remarks: filter.remarks },
    },
    { apiName: this.apiName,...config });
  

  getListByFilter = (input: PagedAndSortedResultRequestDto, filter: CycleCountFilter, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<CycleCountDto>>({
      method: 'GET',
      url: '/api/app/cycle-count/by-filter',
      params: { sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount, cciNumber: filter.cciNumber, isClosed: filter.isClosed, closedFromDate: filter.closedFromDate, closedToDate: filter.closedToDate, closedByName: filter.closedByName, openedFromDate: filter.openedFromDate, openedToDate: filter.openedToDate, requestedByName: filter.requestedByName },
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
