import { ListService, PagedResultDto } from '@abp/ng.core';
import { Confirmation, ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NumberGenerationDto, NumberGenerationService } from '@proxy/number-generations';

@Component({
  selector: 'app-number-generation',
  templateUrl: './number-generation.component.html',
  styleUrls: ['./number-generation.component.scss'],
  providers: [ListService]
})
export class NumberGenerationComponent implements OnInit {
  data = { items: [], totalCount: 0 } as PagedResultDto<NumberGenerationDto>;
  form: FormGroup;
  isModalOpen = false;
  selected = {} as NumberGenerationDto;
  constructor(
    private fb: FormBuilder,
    private service: NumberGenerationService,
    private toast: ToasterService,
    public readonly list: ListService,
    private confirmation: ConfirmationService
  ){ }

  ngOnInit(): void {
    const streamCreator = (query) => this.service.getList(query);
    this.list.hookToQuery(streamCreator).subscribe((resp) => {
    this.data = resp;
  });

  }

  buildForm() {
    this.form = this.fb.group({
      prefix: [this.selected.prefix, Validators.required],
      number: [this.selected.number],
      suffix: [this.selected.suffix],
      numberGenerationTypeId: [this.selected.numberGenerationTypeId],

    });
  }
  create() {
    this.selected = {} as NumberGenerationDto;
    this.isModalOpen = true;
    this.buildForm();
  }

   save(){
    if (this.form.invalid) {
      return;
    }
    const dto: NumberGenerationDto = {
      prefix: this.form.value.perfix,
      number: this.form.value.number,
      suffix: this.form.value.suffix,
      numberGenerationTypeId: this.form.value.numberGenerationTypeId
   };

   const request = this.selected.id ? this.service.update(this.selected.id, dto) : this.service.create(dto);
   request.subscribe(() => {
     this.toast.success(this.selected.id ? '::UpdatedNumberGeneration' : '::CreatedNumberGeneration');
     this.isModalOpen = false;
     this.form.reset();
     this.list.get();
   });
 }
 delete(id: any) {
   this.confirmation.warn ('::AreYouSureToDelete', 'AbpAccount::AreYouSure').subscribe((status) => {
     if (status === Confirmation.Status.confirm) {
       this.service.delete(id).subscribe(() => {
         this.toast.success('::DeletedNumberGeneration');
         this.list.get();
       });
     }
   });
 }
 edit(id: any) {
   this.service.get(id).subscribe((res) => {
     this.selected = res;
     this.buildForm();
     this.isModalOpen = true;
   });
 }


}











