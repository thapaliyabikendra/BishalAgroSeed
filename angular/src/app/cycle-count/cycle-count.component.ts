import { ListService, PagedResultDto } from '@abp/ng.core';
import { Confirmation, ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { CycleCountDto, CycleCountFilter, CycleCountService } from '@proxy/cycle-counts';

@Component({
  selector: 'app-cycle-count',
  templateUrl: './cycle-count.component.html',
  styleUrls: ['./cycle-count.component.scss'],
  providers: [ListService]
})
export class CycleCountComponent implements OnInit {
  data = { items: [], totalCount: 0 } as PagedResultDto<CycleCountDto>;
  filter = {} as CycleCountFilter;
  showFilter = false as Boolean;
  constructor(
    private service: CycleCountService,
    public readonly list: ListService,
    private toast: ToasterService,
    private confirmationService: ConfirmationService
  ) { }

  ngOnInit(): void {
    const streamCreator = (query) => this.service.getListByFilter(query, this.filter);
    this.list.hookToQuery(streamCreator).subscribe((resp) => {
      this.data = resp;
    });
  }

  request() {
    this.confirmationService.warn("::PressOKToContinue", "AbpAccount::AreYouSure").subscribe((status) => {
      if (status === Confirmation.Status.confirm) {
        this.service.create().subscribe(() => {
          this.toast.success('::CycleCount:Requested');
          this.list.get();
        });
      }
    });
  }

  toggleFilter() {
    this.showFilter = !this.showFilter;
  }

  getData() {
    this.list.get();
  }

  clearFilter() {
    this.filter = {};
    this.getData();
  }

  close(id) {
    this.confirmationService.warn("::PressOKToContinue", "AbpAccount::AreYouSure").subscribe((status) => {
      if (status === Confirmation.Status.confirm) {
        this.service.close(id).subscribe(() => {
          this.toast.success(':CycleCount::Closed');
          this.list.get();
        });
      }
    });
  }
}



