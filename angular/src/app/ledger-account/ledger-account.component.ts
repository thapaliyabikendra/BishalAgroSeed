import { ListService, PagedResultDto } from '@abp/ng.core';
import { ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { CustomerDto, CustomerService } from '@proxy/customers';
import { DropdownDto } from '@proxy/dtos';
import { LedgerAccountDto, LedgerAccountFilter, LedgerAccountService } from '@proxy/ledger-accounts';
import { DownloadService } from '../helpers/download.service';

@Component({
  selector: 'app-ledger-account',
  templateUrl: './ledger-account.component.html',
  styleUrls: ['./ledger-account.component.scss'],
  providers: [ListService]
})
export class LedgerAccountComponent implements OnInit {
  data = { items: [], totalCount: 0 } as PagedResultDto<LedgerAccountDto>;
  filter = {} as LedgerAccountFilter;
  customers = [] as DropdownDto[];
  showFilter = false as boolean;
  constructor(
    private customerService: CustomerService,
    private service: LedgerAccountService,
    private toast: ToasterService,
    public readonly list: ListService,
    private downloadservice: DownloadService,
  ) { }


  ngOnInit(): void {
    this.customerService.getCustomers().subscribe((resp) => {
      this.customers = resp;
    })

    const streamCreator = (query) => this.service.getListByFilter(query, this.filter);
    this.list.hookToQuery(streamCreator).subscribe((resp) => {
      this.data = resp;
    });
  }

  getData() {
    this.list.get();
  }

  clearFilter() {
    this.filter = {} as LedgerAccountFilter;
  }

  toggleFilter(){
    this.showFilter = !this.showFilter;
  }
  export() {
    this.service.exportExcel(this.filter).subscribe((file) => {
      this.downloadservice.download(file);
    });
  }
  
}
