import type { CreateUpdateOpeningBalanceDto, OpeningBalanceDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class OpeningBalanceService {
  apiName = 'Default';
  

  create = (input: CreateUpdateOpeningBalanceDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, OpeningBalanceDto>({
      method: 'POST',
      url: '/api/app/opening-balance',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/opening-balance/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, OpeningBalanceDto>({
      method: 'GET',
      url: `/api/app/opening-balance/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: PagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<OpeningBalanceDto>>({
      method: 'GET',
      url: '/api/app/opening-balance',
      params: { sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: CreateUpdateOpeningBalanceDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, OpeningBalanceDto>({
      method: 'PUT',
      url: `/api/app/opening-balance/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
