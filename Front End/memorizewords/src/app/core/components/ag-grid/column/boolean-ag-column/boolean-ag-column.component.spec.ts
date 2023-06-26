import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BooleanAgColumnComponent } from './boolean-ag-column.component';

describe('BooleanAgColumnComponent', () => {
  let component: BooleanAgColumnComponent;
  let fixture: ComponentFixture<BooleanAgColumnComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [BooleanAgColumnComponent]
    });
    fixture = TestBed.createComponent(BooleanAgColumnComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
