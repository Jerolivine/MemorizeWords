import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { WordService } from 'src/app/services/word.service';
import { Word } from 'src/app/services/model/call/word'

@Component({
  selector: 'app-add-new-word',
  templateUrl: './add-new-word.component.html',
  styleUrls: ['./add-new-word.component.css']
})
export class AddNewWordComponent {

  public form: any;
  constructor(private dialogRef: MatDialogRef<AddNewWordComponent>,
    private wordService: WordService) { }

  closeDialog(): void {
    this.dialogRef.close();
  }

  addNewWord() {
    const word : Word = {
      word: '',
      meaning: ''
    };

    this.wordService.addWord(word).subscribe(response => {
      this.dialogRef.close();
    });
  }

}
