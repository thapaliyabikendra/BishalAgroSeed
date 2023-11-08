import type { PaymentType } from '../payment-types/payment-type.enum';

export interface CreateTransactionPaymentDto {
  paymentTypeId?: PaymentType;
  chequeNo?: string;
  bankName?: string;
}
