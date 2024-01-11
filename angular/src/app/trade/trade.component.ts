import { ListService, PagedResultDto } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { TradeDto, TradeFilter, TradeService } from '@proxy/trades';
import { DownloadService } from '../helpers/download.service';
import { CustomerService } from '@proxy/customers';
import { forkJoin } from 'rxjs';
import { DropdownDto } from '@proxy/dtos';

@Component({
  selector: 'app-trade',
  templateUrl: './trade.component.html',
  styleUrls: ['./trade.component.scss'],
  providers: [ListService]
})

export class TradeComponent implements OnInit {
  data = { items: [], totalCount: 0 } as PagedResultDto<TradeDto>;
  filter = {} as TradeFilter;
  showFilter = false as boolean;
  customers = [] as DropdownDto[];
  tradeTypes = [] as DropdownDto[];
  
  constructor(
    public readonly list: ListService,
    private service: TradeService,
    private downloadservice: DownloadService,
    private customerservice: CustomerService,
    private tradeservice : TradeService

  ) { }

  ngOnInit(): void {
    var request1 = this.customerservice.getCustomers();
    var request2 = this.tradeservice.getTradeTypes();
    forkJoin([request1, request2]).
    subscribe((resp) =>
    {
      this.customers = resp[0];
      this.tradeTypes = resp[1];
    });
    
     const streamCreator = (query) => this.service.getListByFilter(query, this.filter);
     this.list.hookToQuery(streamCreator).subscribe((resp) => {
      this.data = resp;
     });
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
     this.service.exportExcel(this.filter).subscribe((file) => {
       this.downloadservice.download(file);
     });
  }
}
