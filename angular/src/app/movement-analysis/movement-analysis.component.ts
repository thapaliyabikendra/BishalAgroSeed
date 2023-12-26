import { ListService, PagedResultDto } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { MovementAnalysis } from '@proxy';
import { MovementAnalysisDto, MovementAnalysisFilter, MovementAnalysisService } from '@proxy/movement-analysis';
import { filter } from 'rxjs';
import { DownloadService } from '../helpers/download.service';
import { CustomerService } from '@proxy/customers';
import { DropdownDto } from '@proxy/dtos';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-movement-analysis',
  templateUrl: './movement-analysis.component.html',
  styleUrls: ['./movement-analysis.component.scss'],
  providers:[ListService]
})
export class MovementAnalysisComponent implements OnInit {
  data = { items: [], totalcount: 0 } as PagedResultDto<MovementAnalysisDto>
  filter = {
    fromTranDate : '2020-10-01',
    toTranDate : '2024-10-01'
  } as MovementAnalysisFilter;
  showFilter = false as boolean;
  customers = [] as DropdownDto[];
  displayedColumns= ['particulars', 'purchases_quantity', 'purchases_effRate', 'purchases_value', 'sales_quantity', 'sales_effRate', 'sales_value'];

  constructor(
    private movementAnaylsisService: MovementAnalysisService,
    private customerService: CustomerService,
    private tost: ToasterService,
    public readonly list: ListService,
    private downloadservice: DownloadService

  ) { }
   ngOnInit(): void {
    const streamCreator = (query) => this.movementAnaylsisService.getListByFilter(query, this.filter);
    this.list.hookToQuery(streamCreator).subscribe((resp) => {
      this.data = resp;
    });

    this.customerService.getCustomers().subscribe((resp) =>{
      this.customers = resp;
    })
   }
    getData() {
      this.list.get();
    }
  
    clearFilter() {
      this.filter = {} as MovementAnalysisFilter;
    }
  
    toggleFilter(){
      this.showFilter = !this.showFilter;
    }
    export() {
      this.movementAnaylsisService.exportExcel(this.filter).subscribe((file) => {
        this.downloadservice.download(file);
      });
  }

}
