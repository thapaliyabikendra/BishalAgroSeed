import type { AuditedEntityDto } from '@abp/ng.core';

export interface CreateUpdateOpeningBalanceDto {
  amount: number;
  tranDate?: string;
  customerId?: string;
  isReceivable: boolean;
}

export interface OpeningBalanceDto extends AuditedEntityDto<string> {
  amount: number;
  tranDate?: string;
  customerId?: string;
  isReceivable: boolean;
}
