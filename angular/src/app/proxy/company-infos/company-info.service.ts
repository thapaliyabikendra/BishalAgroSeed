import type { CompanyInfoDto, CreateUpdateCompanyInfoDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class CompanyInfoService {
  apiName = 'Default';
  

  create = (input: CreateUpdateCompanyInfoDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CompanyInfoDto>({
      method: 'POST',
      url: '/api/app/company-info',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/company-info/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CompanyInfoDto>({
      method: 'GET',
      url: `/api/app/company-info/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: PagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<CompanyInfoDto>>({
      method: 'GET',
      url: '/api/app/company-info',
      params: { sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: CreateUpdateCompanyInfoDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CompanyInfoDto>({
      method: 'PUT',
      url: `/api/app/company-info/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
