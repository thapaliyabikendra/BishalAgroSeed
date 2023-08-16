import type { AuditedEntityDto } from '@abp/ng.core';

export interface ConfigurationDto extends AuditedEntityDto<string> {
  key?: string;
  value?: string;
  description?: string;
}

export interface CreateUpdateConfigurationDto {
  key: string;
  value: string;
  description?: string;
}
