import type { CreateTransactionPaymentDto } from '../cash-transactions/models';
import type { CreateTransactionDetailDto } from '../trades/models';

export interface CreateTransactionDto {
  customerId?: string;
  amount?: number;
  transactionTypeId?: string;
  discountAmount?: number;
  transportCharge?: number;
  voucherNo?: string;
  payment: CreateTransactionPaymentDto;
  details: CreateTransactionDetailDto[];
}

export interface DropdownDto {
  value?: string;
  name?: string;
}

export interface FileBlobDto {
  content: number[];
  fileName?: string;
  contentType?: string;
}
