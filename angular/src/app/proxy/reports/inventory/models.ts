
export interface InventoryReportDto {
  productName?: string;
  count: number;
  countDate?: string;
}

export interface InventoryReportFilter {
  productName?: string;
  countDate: string;
}
