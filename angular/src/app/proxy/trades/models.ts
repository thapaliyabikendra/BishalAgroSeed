
export interface CreateTransactionDetailDto {
  productId: string;
  cases: number;
  quantity: number;
  price: number;
}

export interface CreateTransactionDto {
  customerId?: string;
  amount?: number;
  transactionTypeId?: string;
  discountAmount?: number;
  transportCharge?: number;
  voucherNo?: string;
  details: CreateTransactionDetailDto[];
}
