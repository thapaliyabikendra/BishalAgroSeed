import type { AuditedEntityDto } from '@abp/ng.core';

export interface CreateUpdateCustomerDto {
  displayName: string;
  address?: string;
  contactNo?: string;
  isCustomer: boolean;
  isVendor: boolean;
}

export interface CustomerDto extends AuditedEntityDto<string> {
  displayName?: string;
  address?: string;
  contactNo?: string;
  isVendor: boolean;
  isCustomer: boolean;
}
