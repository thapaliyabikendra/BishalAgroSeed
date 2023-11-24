
export interface LedgerAccountDto {
  miti?: string;
  date?: string;
  particulars?: string;
  vchType?: string;
  vchNo?: string;
  debit: number;
  credit: number;
  balance: number;
}

export interface LedgerAccountFilter {
  customerId: string;
  fromTranDate: string;
  toTranDate: string;
}
