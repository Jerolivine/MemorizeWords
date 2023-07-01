import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TextColumnComponent } from './text-column.component';

describe('TextColumnComponent', () => {
  let component: TextColumnComponent;
  let fixture: ComponentFixture<TextColumnComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TextColumnComponent]
    });
    fixture = TestBed.createComponent(TextColumnComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
