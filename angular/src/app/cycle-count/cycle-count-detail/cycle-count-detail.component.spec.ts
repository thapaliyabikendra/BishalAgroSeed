import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CycleCountDetailComponent } from './cycle-count-detail.component';

describe('CycleCountDetailComponent', () => {
  let component: CycleCountDetailComponent;
  let fixture: ComponentFixture<CycleCountDetailComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CycleCountDetailComponent]
    });
    fixture = TestBed.createComponent(CycleCountDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
