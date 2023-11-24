import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LedgerAccountComponent } from './ledger-account.component';

describe('LedgerAccountComponent', () => {
  let component: LedgerAccountComponent;
  let fixture: ComponentFixture<LedgerAccountComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LedgerAccountComponent]
    });
    fixture = TestBed.createComponent(LedgerAccountComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
