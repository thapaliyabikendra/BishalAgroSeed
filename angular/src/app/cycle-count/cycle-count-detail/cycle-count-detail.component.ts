import { ListService, PagedResultDto } from '@abp/ng.core';
import { ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CycleCountDetailDto, CycleCountDetailFilter, CycleCountService } from '@proxy/cycle-counts';

@Component({
  selector: 'app-cycle-count-detail',
  templateUrl: './cycle-count-detail.component.html',
  styleUrls: ['./cycle-count-detail.component.scss'],
  providers: [ListService]
})
export class CycleCountDetailComponent implements OnInit {
  data = { items: [], totalCount: 0 } as PagedResultDto<CycleCountDetailDto>;
  filter = {} as CycleCountDetailFilter;
  constructor(
    private service: CycleCountService,
    public readonly list: ListService,
    private toast: ToasterService,
    private confirmationService: ConfirmationService,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      var cycleCountId = params["id"];
      this.filter.cycleCountId = cycleCountId;
      const streamCreator = (query) => this.service.getCycleCountDetailListByFilter(query, this.filter);
      this.list.hookToQuery(streamCreator).subscribe((resp) => {
        this.data = resp;
      })
    });
  }
  
  bulkUpdate(){
    console.log(this.data);
  }
}
