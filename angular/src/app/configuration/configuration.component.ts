import { ListService, PagedResultDto } from '@abp/ng.core';
import { Confirmation, ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ConfigurationDto, ConfigurationService, CreateUpdateConfigurationDto } from '@proxy/configurations';
import { subscribeOn } from 'rxjs';

@Component({
  selector: 'app-configuration',
  templateUrl: './configuration.component.html',
  styleUrls: ['./configuration.component.scss'],
  providers: [ListService]
})
export class ConfigurationComponent implements OnInit {
  data = { items: [], totalCount: 0 } as PagedResultDto<ConfigurationDto>;
  form: FormGroup;
  isModalOpen = false;
  selected = {} as ConfigurationDto;
  constructor(
    private fb: FormBuilder,
    private service: ConfigurationService,
    private toast: ToasterService,
    public readonly list: ListService,
    private confirmation: ConfirmationService

  ) { }

  ngOnInit(): void {
    const streamCreator = (query) => this.service.getList(query);
    this.list.hookToQuery(streamCreator).subscribe((resp) => {
      this.data = resp;
    });

  }

  buildForm() {
    this.form = this.fb.group({
      key: [this.selected.key, Validators.required],
      value: [this.selected.value, Validators.required],
      description: [this.selected.description]
    });
  }

  create() {
    this.selected = {} as ConfigurationDto;
    this.isModalOpen = true;
    this.buildForm();
  }

  save() {
    if (this.form.invalid) {
      return;
    }
    const dto: CreateUpdateConfigurationDto = {
      key: this.form.value.key,
      value: this.form.value.value,
      description: this.form.value.description
    };
    const request = this.selected.id ? this.service.update(this.selected.id, dto) : this.service.create(dto);
    request.subscribe(() => {
      this.toast.success(this.selected.id ? '::UpdatedConfiguration' : '::CreatedConfiguration');
      this.isModalOpen = false;
      this.form.reset();
      this.list.get();
    });
  }

  delete(id: any) {
    this.confirmation.warn('::AreYouSureToDelete', 'AbpAccount::AreYouSure').subscribe((status) => {
      if(status === Confirmation.Status.confirm){
        this.service.delete(id).subscribe(() => {
          this.toast.success('::DeletedConfiguration');
          this.list.get();
        });
      }
    });
  }
  edit(id:any){
    this.service.get(id).subscribe((res) => {
      this.selected = res;
      this.buildForm();
      this.isModalOpen = true;
    })
  }
}
