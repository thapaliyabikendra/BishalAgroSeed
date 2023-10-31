import type { CreateUpdateProductDto, GetProductDto, GetUnitTypeDto, ProductDto, ProductFilter } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { DropdownDto, FileBlobDto } from '../dtos/models';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  apiName = 'Default';
  

  create = (input: any, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ProductDto>({
      method: 'POST',
      url: '/api/app/product',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/product/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ProductDto>({
      method: 'GET',
      url: `/api/app/product/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: PagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<ProductDto>>({
      method: 'GET',
      url: '/api/app/product',
      params: { sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getListByFilter = (input: PagedAndSortedResultRequestDto, filter: ProductFilter, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<ProductDto>>({
      method: 'GET',
      url: '/api/app/product/by-filter',
      params: { sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount, displayName: filter.displayName, categoryName: filter.categoryName, brandName: filter.brandName, unitTypeName: filter.unitTypeName, priceFrom: filter.priceFrom, priceTo: filter.priceTo, description: filter.description },
    },
    { apiName: this.apiName,...config });
  

  getProductImage = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, FileBlobDto>({
      method: 'GET',
      url: `/api/app/product/${id}/product-image`,
    },
    { apiName: this.apiName,...config });
  

  getProducts = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, GetProductDto[]>({
      method: 'GET',
      url: '/api/app/product/products',
    },
    { apiName: this.apiName,...config });
  

  getUnitTypes = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, GetUnitTypeDto[]>({
      method: 'GET',
      url: '/api/app/product/unit-types',
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: any, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ProductDto>({
      method: 'PUT',
      url: `/api/app/product/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
