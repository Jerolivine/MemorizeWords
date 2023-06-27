import { Component, Input } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AddNewWordComponent } from './add-new-word/add-new-word.component';
import { QuestionComponent } from './question/question.component';
import { BehaviorSubject, Observable } from 'rxjs';

@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.css']
})
export class MainPageComponent {

  @Input() refreshAnswers$: BehaviorSubject<boolean> = new BehaviorSubject(false);

  constructor(private dialog: MatDialog) { }

  addNewWordClick() {
    const dialogRef = this.dialog.open(AddNewWordComponent);

    dialogRef.afterClosed().subscribe(result => {
      this.refreshAnswers();
    });
  }

  openQuestion() {
    const dialogRef = this.dialog.open(QuestionComponent);

    dialogRef.afterClosed().subscribe(result => {
      this.refreshAnswers();
    });
  }

  private refreshAnswers() {
    this.refreshAnswers$.next(true);
  }

}
