import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AddNewWordComponent } from './add-new-word/add-new-word.component';

@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.css']
})
export class MainPageComponent {

  constructor(private dialog: MatDialog) {}

  addNewWordClick(){
    const dialogRef = this.dialog.open(AddNewWordComponent);

    // dialogRef.afterClosed().subscribe(result => {
    //   // Handle dialog close event, if needed
    //   console.log('Dialog closed with result:', result);
    // });
  }

}
