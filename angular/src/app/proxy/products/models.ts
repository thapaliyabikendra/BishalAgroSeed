import type { AuditedEntityDto } from '@abp/ng.core';

export interface CreateUpdateProductDto {
  displayName: string;
  brandId: string;
  unit: number;
  unitTypeId: string;
  price: number;
  description?: string;
}

export interface ProductDto extends AuditedEntityDto<string> {
  displayName?: string;
  brandId?: string;
  unit: number;
  unitTypeId?: string;
  price: number;
  description?: string;
}
