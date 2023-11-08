import { mapEnumToOptions } from '@abp/ng.core';

export enum PaymentType {
  Cash = 1,
  Banking = 2,
}

export const paymentTypeOptions = mapEnumToOptions(PaymentType);
