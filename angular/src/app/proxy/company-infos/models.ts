import type { AuditedEntityDto } from '@abp/ng.core';

export interface CompanyInfoDto extends AuditedEntityDto<string> {
  displayName?: string;
  address?: string;
  contactNo?: string;
  panNo?: string;
}

export interface CompanyInfoFilter {
  displayName?: string;
  address?: string;
  contactNo?: string;
  panNo?: string;
}

export interface CreateUpdateCompanyInfoDto {
  displayName: string;
  address?: string;
  contactNo?: string;
  panNo?: string;
}
