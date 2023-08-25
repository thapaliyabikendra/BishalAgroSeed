import type { AuditedEntityDto } from '@abp/ng.core';

export interface CategoryDto extends AuditedEntityDto<string> {
  displayName?: string;
  parentId?: string;
  isActive: boolean;
}

export interface CreateUpdateCategoryDto {
  displayName: string;
  parentId?: string;
  isActive: boolean;
}
