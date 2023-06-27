import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProgressbarAgColumnComponent } from './progressbar-ag-column.component';

describe('ProgressbarAgColumnComponent', () => {
  let component: ProgressbarAgColumnComponent;
  let fixture: ComponentFixture<ProgressbarAgColumnComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ProgressbarAgColumnComponent]
    });
    fixture = TestBed.createComponent(ProgressbarAgColumnComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
