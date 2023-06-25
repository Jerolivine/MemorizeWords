import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-add-new-word',
  templateUrl: './add-new-word.component.html',
  styleUrls: ['./add-new-word.component.css']
})
export class AddNewWordComponent {

  constructor(private dialogRef: MatDialogRef<AddNewWordComponent>) {}

  closeDialog(): void {
    this.dialogRef.close();
  }
  
}
