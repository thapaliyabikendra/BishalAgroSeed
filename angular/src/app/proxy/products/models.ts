import type { AuditedEntityDto } from '@abp/ng.core';

export interface CreateUpdateProductDto {
  displayName: string;
  categoryId: string;
  brandId: string;
  unit: number;
  unitTypeId: string;
  price: number;
  description?: string;
  imgFileName?: string;
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
  price: number;
  description?: string;
  imgFileName?: string;
}
