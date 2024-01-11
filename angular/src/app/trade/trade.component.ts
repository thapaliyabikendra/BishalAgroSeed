import { ListService, PagedResultDto } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { TradeService } from '@proxy/trades';
import { DownloadService } from '../helpers/download.service';

@Component({
  selector: 'app-trade',
  templateUrl: './trade.component.html',
  styleUrls: ['./trade.component.scss'],
  providers: [ListService]
})

export class TradeComponent implements OnInit {
  data = { items: [], totalCount: 0 } ;
  filter = {} ;
  showFilter = false as boolean;
  
  constructor(
    public readonly list: ListService,
    private service: TradeService,
    private downloadservice: DownloadService

  ) { }

  ngOnInit(): void {
    // const streamCreator = (query) => this.service.getListByFilter(query, this.filter);
    // this.list.hookToQuery(streamCreator).subscribe((resp) => {
    //   this.data = resp;
    // });
  }
  getData() {
    this.list.get();
  }

  clearFilter() {
    this.filter = {} ;
  }

  toggleFilter() {
    this.showFilter = !this.showFilter;
  }

  export() {
    // this.service.exportExcel(this.filter).subscribe((file) => {
    //   this.downloadservice.download(file);
    // });
  }
}
