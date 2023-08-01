import { Component, Input } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AddNewWordComponent } from './add-new-word/add-new-word.component';
import { QuestionComponent } from './question/question.component';
import { BehaviorSubject, Observable } from 'rxjs';
import { WordResponse } from 'src/app/services/http/model/back/word-response';
import { SignalRService } from 'src/app/core/services/hub/signalr/signalr.service';
import { USER_GUESSED_WORDS_HUB, USER_GUESSED_WORDS_HUB_EVENT } from 'src/app/constants/hub-constants';

@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.css']
})
export class MainPageComponent {

  @Input() refreshAnswers$: BehaviorSubject<boolean> = new BehaviorSubject(false);
  @Input() userGuessedWordsHub$: BehaviorSubject<WordResponse[]> = new BehaviorSubject<WordResponse[]>([]);
  private userGuessedWordsHubObservable:Observable<any>;

  constructor(private dialog: MatDialog,
    private signalRService: SignalRService) { }

  addNewWordClick() {
    const dialogRef = this.dialog.open(AddNewWordComponent);

    dialogRef.afterClosed().subscribe(result => {
      this.refreshAnswers();
    });
  }

  openQuestion() {
    const dialogRef = this.dialog.open(QuestionComponent);
    this.startUserGuessedWordsHub();

    dialogRef.afterClosed().subscribe(result => {
      this.stopUserGuessedWordsHub();
      this.refreshAnswers();
    });
  }

  private refreshAnswers() {
    this.refreshAnswers$.next(true);
  }

  private startUserGuessedWordsHub(){
    // this.userGuessedWordsHubObservable = this.signalRService.startEvent(USER_GUESSED_WORDS_HUB, USER_GUESSED_WORDS_HUB_EVENT);
    // this.userGuessedWordsHubObservable.subscribe(data =>{
    //   debugger;
    //   console.log("Next Data :" + JSON.stringify(data));
    // });

    this.userGuessedWordsHubObservable = this.signalRService.startEvent(USER_GUESSED_WORDS_HUB, USER_GUESSED_WORDS_HUB_EVENT);
    this.userGuessedWordsHubObservable.subscribe(data =>{
      this.userGuessedWordsHub$.next(data);
    });
  }

  private stopUserGuessedWordsHub(){
    this.signalRService.stopEvent(USER_GUESSED_WORDS_HUB_EVENT);
  }

}
