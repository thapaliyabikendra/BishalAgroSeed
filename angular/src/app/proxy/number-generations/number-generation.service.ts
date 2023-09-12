import type { CreateUpdateNumberGenerationDto, NumberGenerationDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { DropdownDto } from '../dtos/models';

@Injectable({
  providedIn: 'root',
})
export class NumberGenerationService {
  apiName = 'Default';
  

  create = (input: CreateUpdateNumberGenerationDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, NumberGenerationDto>({
      method: 'POST',
      url: '/api/app/number-generation',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/number-generation/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, NumberGenerationDto>({
      method: 'GET',
      url: `/api/app/number-generation/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: PagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<NumberGenerationDto>>({
      method: 'GET',
      url: '/api/app/number-generation',
      params: { sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getNumberGenerationTypes = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, DropdownDto[]>({
      method: 'GET',
      url: '/api/app/number-generation/number-generation-types',
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: CreateUpdateNumberGenerationDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, NumberGenerationDto>({
      method: 'PUT',
      url: `/api/app/number-generation/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
