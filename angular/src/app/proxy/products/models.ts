import type { AuditedEntityDto } from '@abp/ng.core';

export interface CreateUpdateProductDto {
  displayName: string;
  categoryId: string;
  brandId: string;
  unit: number;
  unitTypeId: string;
  price: number;
  description?: string;
  file: File;
}

export interface GetUnitTypeDto {
  id?: string;
  displayName?: string;
  description?: string;
}

export interface ProductDto extends AuditedEntityDto<string> {
  displayName?: string;
  categoryId?: string;
  categoryName?: string;
  brandId?: string;
  brandName?: string;
  unit: number;
  unitTypeId?: string;
  unitTypeName?: string;
  unitTypeDescription?: string;
  price: number;
  description?: string;
  imgFileName?: string;
  creationTime?: string;
}

export interface ProductFilter {
  displayName?: string;
  categoryName?: string;
  brandName?: string;
  unitTypeName?: string;
  priceFrom?: number;
  priceTo?: number;
  description?: string;
}

export interface GetProductDto {
  id?: string;
  productName?: string;
  price?: number;
}
