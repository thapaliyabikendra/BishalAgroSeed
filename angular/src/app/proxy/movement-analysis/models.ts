
export interface MovementAnalysisDto {
  fromTranMiti?: string;
  fromTranDate?: string;
  toTranMiti?: string;
  toTranDate?: string;
  particulars?: string;
  purchases: TradeMADto;
  sales: TradeMADto;
}

export interface MovementAnalysisFilter {
  productName?: string;
  fromTranDate: string;
  toTranDate: string;
  customerId: string;
}

export interface TradeMADto {
  quantity: number;
  effRate: number;
  value: number;
}
