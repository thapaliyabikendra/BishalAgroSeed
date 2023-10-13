import { ListService, PagedResultDto } from '@abp/ng.core';
import { Confirmation, ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CycleCountDetailDto, CycleCountDetailFilter, CycleCountService, UpdateCycleCountDetailDto } from '@proxy/cycle-counts';
import { DownloadService } from 'src/app/helpers/download.service';

@Component({
  selector: 'app-cycle-count-detail',
  templateUrl: './cycle-count-detail.component.html',
  styleUrls: ['./cycle-count-detail.component.scss'],
  providers: [ListService]
})
export class CycleCountDetailComponent implements OnInit {
  data = { items: [], totalCount: 0 } as PagedResultDto<CycleCountDetailDto>;
  filter = {} as CycleCountDetailFilter;
  showFilter = false as Boolean;
  cycleCountId: any;
  isView = 1;
  constructor(
    private service: CycleCountService,
    public readonly list: ListService,
    private toast: ToasterService,
    private confirmationService: ConfirmationService,
    private route: ActivatedRoute,
    private downloadservice: DownloadService,
    private location: Location
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.cycleCountId = params["id"];
      this.isView = params["isView"];
      this.filter.cycleCountId = this.cycleCountId;
      const streamCreator = (query) => this.service.getCycleCountDetailListByFilter(query, this.filter);
      this.list.hookToQuery(streamCreator).subscribe((resp) => {
        this.data = resp;
      })
    });
  }

  bulkUpdate() {
    this.confirmationService.warn("::PressOKToContinue", "AbpAccount::AreYouSure").subscribe((status) => {
      if (status === Confirmation.Status.confirm) {
        const request = this.data.items.map(s => ({
          id: s.id,
          physicalQuantity: s.physicalQuantity,
          remarks: s.remarks
        })) as UpdateCycleCountDetailDto[];

        console.log(request);
        this.service.bulkUpdateCycleCountDetail(this.cycleCountId, request).subscribe(() => {
          this.toast.success('::UpdatedCycleCountDetails');
          this.list.get();
        });
      }
    });
  }
  export() {
    this.service.exportCycleCountDetailExcel(this.filter).subscribe((file) => {
      this.downloadservice.download(file);
    })
  }

  import(event: any){
    let input = new FormData();
    input.append("file", event.target.files[0]);
    input.append("cycleCountId", this.cycleCountId);
    this.service.bulkUpdateCycleCountDetailByExcel(input).subscribe(() =>{
      this.toast.success('::ImportedCycleCountDetails');
      this.list.get();
    })
  }

  toggleFilter() {
    this.showFilter = !this.showFilter;
  }

  getData() {
    this.list.get();
  }

  clearFilter() {
    this.filter = {cycleCountId: this.cycleCountId} as CycleCountDetailFilter;
    this.getData();
  }
  goBack(){
    this.location.back();
  }
}
