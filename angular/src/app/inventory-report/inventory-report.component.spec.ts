import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InventoryReportComponent } from './inventory-report.component';

describe('InventoryReportComponent', () => {
  let component: InventoryReportComponent;
  let fixture: ComponentFixture<InventoryReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [InventoryReportComponent]
    });
    fixture = TestBed.createComponent(InventoryReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
