export interface CycleCountDetailDto {
  id?: string;
  productId?: string;
  productName?: string;
  cciNumber?: string;
  systemQuantity: number;
  physicalQuantity?: number;
  remarks?: string;
}

export interface CycleCountDetailFilter {
  cycleCountId: string;
  productName?: string;
  remarks?: string;
}

export interface CycleCountDto {
  id?: string;
  cciNumber?: string;
  isClosed: boolean;
  closedDate?: string;
  closedByName?: string;
  creationTime?: string;
}

export interface CycleCountFilter {
  cciNumber?: string;
  isClosed?: boolean;
  closedFromDate?: string;
  closedToDate?: string;
  closedByName?: string;
  openedFromDate?: string;
  openedToDate?: string;
}

export interface UpdateCycleCountDetailDto {
  sn: number;
  id?: string;
  productName?: string;
  physicalQuantityName?: string;
  physicalQuantity?: number;
  remarks?: string;
}

export interface UpdateCycleCountDetailFileDto {
  cycleCountId: string;
  file: File;
}
