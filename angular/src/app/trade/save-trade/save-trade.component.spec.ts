import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SaveTradeComponent } from './save-trade.component';

describe('SaveTradeComponent', () => {
  let component: SaveTradeComponent;
  let fixture: ComponentFixture<SaveTradeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SaveTradeComponent]
    });
    fixture = TestBed.createComponent(SaveTradeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
