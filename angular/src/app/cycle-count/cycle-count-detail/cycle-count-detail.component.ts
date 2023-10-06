import { ListService, PagedResultDto } from '@abp/ng.core';
import { Confirmation, ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CycleCountDetailDto, CycleCountDetailFilter, CycleCountService, UpdateCycleCountDetailDto } from '@proxy/cycle-counts';

@Component({
  selector: 'app-cycle-count-detail',
  templateUrl: './cycle-count-detail.component.html',
  styleUrls: ['./cycle-count-detail.component.scss'],
  providers: [ListService]
})
export class CycleCountDetailComponent implements OnInit {
  data = { items: [], totalCount: 0 } as PagedResultDto<CycleCountDetailDto>;
  filter = {} as CycleCountDetailFilter;
  cycleCountId : any;
  constructor(
    private service: CycleCountService,
    public readonly list: ListService,
    private toast: ToasterService,
    private confirmationService: ConfirmationService,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.cycleCountId = params["id"];
      this.filter.cycleCountId = this.cycleCountId;
      const streamCreator = (query) => this.service.getCycleCountDetailListByFilter(query, this.filter);
      this.list.hookToQuery(streamCreator).subscribe((resp) => {
        this.data = resp;
      })
    });
  }
  
  bulkUpdate(){
    this.confirmationService.warn("::PressOKToContinue", "AbpAccount::AreYouSure").subscribe((status) => {
      if (status === Confirmation.Status.confirm) {
        const request = this.data.items.map(s => ( {
          id : s.id,
          physicalQuantity : s.physicalQuantity,
          remarks : s.remarks
        })) as UpdateCycleCountDetailDto[];

        console.log(request);
        this.service.bulkUpdateCycleCountDetail(this.cycleCountId, request).subscribe(() => {
          this.toast.success('::UpdatedCycleCountDetails');
          this.list.get();
        });
      }
    });
   
  }
}
