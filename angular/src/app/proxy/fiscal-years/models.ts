import type { FullAuditedEntityDto } from '@abp/ng.core';

export interface CreateUpdateFiscalYearDto {
  displayName: string;
  fromDate?: string;
  toDate?: string;
  isCurrent: boolean;
}

export interface FiscalYearDto extends FullAuditedEntityDto<string> {
  displayName?: string;
  fromDate?: string;
  toDate?: string;
  isCurrent: boolean;
}
