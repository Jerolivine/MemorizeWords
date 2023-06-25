import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { WordService } from 'src/app/services/http/word.service';
import { Word } from 'src/app/services/http/model/call/word'
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AlertifyService } from 'src/app/services/alertify-service.service';
import { WordResponse } from 'src/app/services/http/model/back/word-response';

@Component({
  selector: 'app-add-new-word',
  templateUrl: './add-new-word.component.html',
  styleUrls: ['./add-new-word.component.css']
})
export class AddNewWordComponent implements OnInit {

  submitted: boolean = false;
  public form: FormGroup;

  constructor(private dialogRef: MatDialogRef<AddNewWordComponent>,
    private wordService: WordService,
    private formBuilder: FormBuilder,
    private alertifyService: AlertifyService) { }

  ngOnInit() {
    this.createForm();
  }

  get formControls() { return this.form.controls; }

  private createForm() {
    this.form = this.formBuilder.group({
      word: ['', Validators.required],
      meaning: ['', Validators.required],
    });
  }

  closeDialog(): void {
    this.dialogRef.close();
  }

  addNewWord() {
    this.submitted = true;
    this.form.markAllAsTouched();
    if (!this.form.valid) {
      return;
    }

    const word: Word = {
      word: this.getFormValue("word"),
      meaning: this.getFormValue("meaning")
    };

    this.wordService.addWord<WordResponse>(word).subscribe(response => {
      this.alertifyService.success(`The word "${response.word}" is added successfully`);
      this.dialogRef.close();
    });
  }

  public errorHandling = (control: string, error: string) => {
    let result = this.formControls[control]?.hasError(error) &&
      (this.formControls[control].touched || (this.submitted && this.form.invalid));

    return result;
  }

  getFormValue(control: string) {
    return this.formControls[control].value;
  }

}
