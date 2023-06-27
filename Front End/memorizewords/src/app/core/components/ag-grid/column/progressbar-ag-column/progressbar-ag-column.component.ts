import { Component } from '@angular/core';
import { ICellRendererAngularComp } from 'ag-grid-angular';
import { ICellRendererParams } from 'ag-grid-community';

@Component({
  selector: 'ag-progressbar-ag-column',
  templateUrl: './progressbar-ag-column.component.html',
  styleUrls: ['./progressbar-ag-column.component.css']
})
export class ProgressbarAgColumnComponent implements ICellRendererAngularComp {

  public progress: number;

  agInit(params: ICellRendererParams<any, any, any>): void {
    this.progress = Number(params.value);
  }

  refresh(params: ICellRendererParams<any, any, any>): boolean {
    return true;
  }
  
}