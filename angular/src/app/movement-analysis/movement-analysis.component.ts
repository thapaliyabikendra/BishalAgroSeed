import { ListService, PagedResultDto } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { MovementAnalysis } from '@proxy';
import { MovementAnalysisDto, MovementAnalysisFilter, MovementAnalysisService } from '@proxy/movement-analysis';
import { filter } from 'rxjs';
import { DownloadService } from '../helpers/download.service';
import { CustomerService } from '@proxy/customers';
import { DropdownDto } from '@proxy/dtos';

@Component({
  selector: 'app-movement-analysis',
  templateUrl: './movement-analysis.component.html',
  styleUrls: ['./movement-analysis.component.scss'],
  providers:[ListService]
})
export class MovementAnalysisComponent implements OnInit {
  data = { items: [], totalcount: 0 } as PagedResultDto<MovementAnalysisDto>
  filter = {} as MovementAnalysisFilter;
  showFilter = false as boolean;
  customers = [] as DropdownDto[];

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
