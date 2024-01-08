import { ListService, PagedResultDto } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { InventoryReportDto, InventoryReportFilter, InventoryReportService } from '@proxy/reports/inventory';
import { DownloadService } from '../helpers/download.service';

@Component({
  selector: 'app-inventory-report',
  templateUrl: './inventory-report.component.html',
  styleUrls: ['./inventory-report.component.scss'],
  providers: [ListService]
})
export class InventoryReportComponent implements OnInit {
  data = { items: [], totalCount: 0 } as PagedResultDto<InventoryReportDto>;
  filter = {} as InventoryReportFilter;
  showFilter = false as boolean;
  
  constructor(
    public readonly list: ListService,
    private service: InventoryReportService,
    private downloadservice: DownloadService

  ) { }

  ngOnInit(): void {
    const streamCreator = (query) => this.service.getListByFilter(query, this.filter);
    this.list.hookToQuery(streamCreator).subscribe((resp) => {
      this.data = resp;
    });
  }
  getData() {
    this.list.get();
  }

  clearFilter() {
    this.filter = {} as InventoryReportFilter;
  }

  toggleFilter() {
    this.showFilter = !this.showFilter;
  }

  export() {
    this.service.exportExcel(this.filter).subscribe((file) => {
      this.downloadservice.download(file);
    });
  }
}