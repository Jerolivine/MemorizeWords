import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { WordService } from 'src/app/services/word.service';
import { Word } from 'src/app/services/model/call/word'
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-add-new-word',
  templateUrl: './add-new-word.component.html',
  styleUrls: ['./add-new-word.component.css']
})
export class AddNewWordComponent implements OnInit {

  public form: FormGroup;

  constructor(private dialogRef: MatDialogRef<AddNewWordComponent>,
    private wordService: WordService,
    private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.createForm();
  }

  get formControls() { return this.form.controls; }

  private createForm(){
    this.form = this.formBuilder.group({
      word: ['', Validators.required],
      meaning: ['', Validators.required],
    });
  }

  closeDialog(): void {
    this.dialogRef.close();
  }

  addNewWord() {
    const word: Word = {
      word: '',
      meaning: ''
    };

    this.wordService.addWord(word).subscribe(response => {
      this.dialogRef.close();
    });
  }

  public errorHandling = (control: string, error: string) => {
    return this.formControls[control]?.hasError(error);
  }

}
