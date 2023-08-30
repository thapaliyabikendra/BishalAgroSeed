import type { CreateUpdateFiscalYearDto, FiscalYearDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class FiscalYearService {
  apiName = 'Default';
  

  create = (input: CreateUpdateFiscalYearDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, FiscalYearDto>({
      method: 'POST',
      url: '/api/app/fiscal-year',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/fiscal-year/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, FiscalYearDto>({
      method: 'GET',
      url: `/api/app/fiscal-year/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: PagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<FiscalYearDto>>({
      method: 'GET',
      url: '/api/app/fiscal-year',
      params: { sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: CreateUpdateFiscalYearDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, FiscalYearDto>({
      method: 'PUT',
      url: `/api/app/fiscal-year/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
