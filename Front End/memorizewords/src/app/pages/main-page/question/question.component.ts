import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { AlertifyService } from 'src/app/services/alertify-service.service';
import { WordService } from 'src/app/services/http/word.service';
import { Question } from './model/question';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Answer } from 'src/app/services/http/model/call/answer';

@Component({
  selector: 'question',
  templateUrl: './question.component.html',
  styleUrls: ['./question.component.css']
})
export class QuestionComponent implements OnInit {

  submitted: boolean = false;
  private questions: Question[] = [];

  public form: FormGroup;
  public question: Question | undefined;

  constructor(private dialogRef: MatDialogRef<QuestionComponent>,
    private wordService: WordService,
    private alertifyService: AlertifyService,
    private formBuilder: FormBuilder,) { }

  ngOnInit() {
    this.getQuestionWords();
    this.createForm();
  }

  private createForm() {
    this.form = this.formBuilder.group({
      meaning: ['', Validators.required],
    });
  }

  private getQuestionWords() {
    this.wordService.getQuestionWords().subscribe(response => {

      if (response?.length === 0) {
        return;
      }

      response.map(questionWord => {
        const question: Question = {
          id: questionWord.id,
          word: questionWord.word
        };
        this.questions.push(question);
      })

      this.askQuestion();
    });
  }

  askQuestion() {
    let question: Question | undefined = this.questions.pop();
    if (question === undefined) {
      this.getQuestionWords();
    }

    this.submitted = false;
    this.question = question;
  }

  answer() {
    this.submitted = true;
    this.form.markAllAsTouched();
    if (!this.form.valid) {
      return;
    }

    const answer: Answer = { wordId: this.question!.id, givenAnswerMeaning: this.getFormValue("meaning") };
    this.wordService.answer(answer).subscribe(response => {
      this.askQuestion();
    });

  }

  get formControls() { return this.form.controls; }

  public errorHandling = (control: string, error: string) => {
    let result = this.formControls[control]?.hasError(error) &&
      (this.formControls[control].touched || (this.submitted && this.form.invalid));

    return result;
  }

  getFormValue(control: string) {
    return this.formControls[control].value;
  }

}