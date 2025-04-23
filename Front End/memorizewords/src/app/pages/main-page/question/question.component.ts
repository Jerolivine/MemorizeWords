import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { AlertifyService } from 'src/app/core/services/alertify-service.service';
import { WordService } from 'src/app/services/http/word.service';
import { Question } from './model/question';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AnswerResponse } from 'src/app/services/http/model/back/answer-response';
import { StringCompare } from 'src/app/core/utility/string-utility';
import { WordAnswerRequest } from 'src/app/services/http/model/call/WordAnswerRequest';
import { TextToSpeechService } from 'src/app/core/services/text-to-speech.service';
import { WordAnswerService } from 'src/app/services/http/word-answer.service';
import { POLISH_TO_LATIN_DICTIONARY } from 'src/app/core/constants/alphabet';
import { POLISH_LANGUAGE, TURKISH_LANGUAGE } from 'src/app/core/constants/languages';
import { QuestionType } from './enum/question.type';
import { WordAnswerWordRequest } from 'src/app/services/http/model/call/WordAnswerWordRequest';

@Component({
  selector: 'question',
  templateUrl: './question.component.html',
  styleUrls: ['./question.component.css']
})
export class QuestionComponent implements OnInit {

  submitted: boolean = false;
  private questions: Question[] = [];

  QuestionType = QuestionType;
  public questionType: QuestionType;
  public form: FormGroup;
  public question: Question | undefined;

  public tip: string;

  get canTipGiven() {
    return this.tip?.length < 3;
  }

  constructor(private dialogRef: MatDialogRef<QuestionComponent>,
    private wordService: WordService,
    private wordAnswerService: WordAnswerService,
    private alertifyService: AlertifyService,
    private formBuilder: FormBuilder,
    private textToSpeechService: TextToSpeechService) { }

  ngOnInit() {
    this.getQuestionWords();
    this.createForm();
  }

  private createForm() {

    if (this.questionType == QuestionType.AskMeaning) {
      this.createAskMeaningForm();
    }
    else {
      this.createAskWordForm();
    }

  }

  private createAskMeaningForm() {
    this.form = this.formBuilder.group({
      meaning: ['', Validators.required],
    });
  }

  private createAskWordForm() {
    this.form = this.formBuilder.group({
      word: ['', Validators.required],
    });
  }

  private getQuestionWords() {
    this.wordService.getQuestionWords().subscribe(response => {

      this.question = undefined;
      if (response?.length === 0) {
        return;
      }

      response.map(questionWord => {
        const question: Question = {
          id: questionWord.id,
          word: questionWord.word,
          writingInLanguage: questionWord.writingInLanguage,
          meaning: questionWord.meaning
        };
        this.questions.push(question);
      })

      this.askQuestion();
    });
  }

  askQuestion() {
    this.resetForm();
    let question: Question | undefined = this.questions.pop();
    if (question === undefined) {
      this.getQuestionWords();
      return;
    }

    this.submitted = false;
    this.question = question;

    if (this.questionType == QuestionType.AskMeaning) {
      this.textToSpeechService.speak(this.question!.writingInLanguage, POLISH_LANGUAGE);
    }
  }

  private resetForm() {
    this.form.reset();
    this.tip = "";
  }

  answer() {
    this.submitted = true;
    this.form.markAllAsTouched();
    if (!this.form.valid) {
      return;
    }

    if (this.questionType == QuestionType.AskMeaning) {
      this.answerMeaning();
    }
    else {
      this.answerWord();
    }

  }

  private answerMeaning() {
    const wordAnswerRequest: WordAnswerRequest = { wordId: this.question!.id, givenAnswerMeaning: this.getFormValue("meaning") };
    this.wordAnswerService.answer<AnswerResponse>(wordAnswerRequest).subscribe(response => {
      this.checkAnswer(response);
      this.askQuestion();
    });
  }

  private answerWord() {
    const wordAnswerWordRequest: WordAnswerWordRequest = { wordId: this.question!.id, givenAnswerWord: this.getFormValue("word") };
    this.wordAnswerService.answerWord<AnswerResponse>(wordAnswerWordRequest).subscribe(response => {
      this.checkAnswer(response);
      this.askQuestion();
    });
  }

  private checkAnswer(answerResponse: AnswerResponse) {
    if (answerResponse.isAnswerTrue) {
      this.alertifyService.success("That is correct!");
    }
    else {
      this.alertifyService.warning(`Incorrect. "${this.question?.word} means ${answerResponse.meaning}"`);

      if(this.questionType == QuestionType.AskWord){
        this.textToSpeechService.speak(this.question!.writingInLanguage,POLISH_LANGUAGE);
      }
    }
  }

  get formControls() { return this.form.controls; }

  public errorHandling = (control: string, error: string) => {
    let result = this.formControls[control]?.hasError(error) &&
      (this.formControls[control].touched || (this.submitted && this.form.invalid));

    return result;
  }

  getFormControl(control: string) {
    return this.formControls[control];
  }

  getFormValue(control: string) {
    return this.getFormControl(control).value;
  }

  onTipClick() {
    this.giveTip();
  }

  giveTip() {
    if (this.tip.length === 3) {
      return;
    }

    this.tip += this.questionType === QuestionType.AskMeaning
      ? this.question?.meaning?.[this.tip.length] ?? ''
      : this.question?.word?.[this.tip.length] ?? '';
  }

  onCtrlKeyUpForTip(event: KeyboardEvent) {
    if (event.key === 'Control') {
      this.giveTip();
    }
  }

}
