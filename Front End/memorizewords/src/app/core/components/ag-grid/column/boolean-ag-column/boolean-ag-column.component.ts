import { Component } from '@angular/core';
import { ICellRendererAngularComp } from 'ag-grid-angular';
import { ICellRendererParams } from 'ag-grid-community';

@Component({
  selector: 'ag-boolean-ag-column',
  templateUrl: './boolean-ag-column.component.html',
  styleUrls: ['./boolean-ag-column.component.css']
})
export class BooleanAgColumnComponent implements ICellRendererAngularComp {

  public selection: boolean | undefined;

  agInit(params: ICellRendererParams<any, any, any>): void {
    this.selection = params.value;
  }
  refresh(params: ICellRendererParams<any, any, any>): boolean {
    return true;
  }

  getIconClass(): string {
    debugger;
    if (this.selection === true) {
      return 'fa-check';
    } else if (this.selection === false) {
      return 'fa-times';
    } else {
      return 'fa-question'
    }
  }


}