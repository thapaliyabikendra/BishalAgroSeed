import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MovementAnalysisComponent } from './movement-analysis.component';

describe('MovementAnalysisComponent', () => {
  let component: MovementAnalysisComponent;
  let fixture: ComponentFixture<MovementAnalysisComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MovementAnalysisComponent]
    });
    fixture = TestBed.createComponent(MovementAnalysisComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
