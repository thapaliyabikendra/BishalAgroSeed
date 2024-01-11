
export interface CreateTransactionDetailDto {
  productId: string;
  cases: number;
  quantity: number;
  price: number;
}

export interface TradeDto {
  transactionId?: string;
  tradeTypeId?: string;
  tradeTypeName?: string;
  customerName?: string;
  discountAmount: number;
  transportCharge: number;
  voucherNo?: string;
  tranDate?: string;
  amount: number;
}

export interface TradeFilter {
  tradeTypeId?: string;
  customerId?: string;
  voucherNo?: string;
  fromTranDate?: string;
  toTranDate?: string;
}
