import type { NumberGenerationTypes } from '../number-generation-types/number-generation-types.enum';
import type { AuditedEntityDto } from '@abp/ng.core';

export interface CreateUpdateNumberGenerationDto {
  prefix?: string;
  number: number;
  suffix?: string;
  numberGenerationTypeId: NumberGenerationTypes;
}

export interface NumberGenerationDto extends AuditedEntityDto<string> {
  prefix?: string;
  number: number;
  suffix?: string;
  numberGenerationTypeId: NumberGenerationTypes;
}
