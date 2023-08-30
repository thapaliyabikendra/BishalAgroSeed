import { mapEnumToOptions } from '@abp/ng.core';

export enum NumberGenerationTypes {
  CycleCount = 1,
}

export const numberGenerationTypesOptions = mapEnumToOptions(NumberGenerationTypes);
