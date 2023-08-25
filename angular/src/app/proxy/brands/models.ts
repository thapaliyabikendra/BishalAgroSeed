import type { AuditedEntityDto } from '@abp/ng.core';

export interface BrandDto extends AuditedEntityDto<string> {
  displayName?: string;
  address?: string;
  contactNo?: string;
}

export interface CreateUpdateBrandDto {
  displayName: string;
  address?: string;
  contactNo?: string;
}
