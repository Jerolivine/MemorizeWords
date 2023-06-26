import { HttpClient } from '@angular/common/http';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef, ICellRendererParams } from 'ag-grid-community';
import { WordResponse } from 'src/app/services/http/model/back/word-response';
import { WordService } from 'src/app/services/http/word.service';
import { Answer } from './model/answer';
import { BooleanAgColumnComponent } from 'src/app/core/components/ag-grid/column/boolean-ag-column/boolean-ag-column.component';
import { BehaviorSubject, Observable } from 'rxjs';

@Component({
  selector: 'answers',
  templateUrl: './answers.component.html',
  styleUrls: ['./answers.component.css']
})

export class AnswersComponent implements OnInit {

  private readonly MAX_WIDTH_ANSWER: number = 70;
  private readonly SEQUENT_TRUE_ANSWER_COUNT = 10; // TODO-Arda Get this value from service

  @Input() public refreshData$: BehaviorSubject<boolean> = new BehaviorSubject(false);

  columnDefs: ColDef[] = [
    { headerName: 'Word', field: 'word', filter: true },
    { headerName: 'Meaning', field: 'meaning', filter: true },
    { headerName: 'Answer1', field: 'answer1', maxWidth: this.MAX_WIDTH_ANSWER, cellRenderer: BooleanAgColumnComponent },
    { headerName: 'Answer2', field: 'answer2', maxWidth: this.MAX_WIDTH_ANSWER, cellRenderer: BooleanAgColumnComponent },
    { headerName: 'Answer3', field: 'answer3', maxWidth: this.MAX_WIDTH_ANSWER, cellRenderer: BooleanAgColumnComponent },
    { headerName: 'Answer4', field: 'answer4', maxWidth: this.MAX_WIDTH_ANSWER, cellRenderer: BooleanAgColumnComponent },
    { headerName: 'Answer5', field: 'answer5', maxWidth: this.MAX_WIDTH_ANSWER, cellRenderer: BooleanAgColumnComponent },
    { headerName: 'Answer6', field: 'answer6', maxWidth: this.MAX_WIDTH_ANSWER, cellRenderer: BooleanAgColumnComponent },
    { headerName: 'Answer7', field: 'answer7', maxWidth: this.MAX_WIDTH_ANSWER, cellRenderer: BooleanAgColumnComponent },
    { headerName: 'Answer8', field: 'answer8', maxWidth: this.MAX_WIDTH_ANSWER, cellRenderer: BooleanAgColumnComponent },
    { headerName: 'Answer9', field: 'answer9', maxWidth: this.MAX_WIDTH_ANSWER, cellRenderer: BooleanAgColumnComponent },
    { headerName: 'Answer10', field: 'answer10', maxWidth: this.MAX_WIDTH_ANSWER, cellRenderer: BooleanAgColumnComponent }
  ];

  unlearnedWords: Answer[] = [];
  learnedWords: Answer[] = [];

  defaultColDef: any;

  constructor(private wordService: WordService) {

  }

  ngOnInit() {
    this.setGridDefaultColDef();
    this.refreshGrids();

    this.refreshData$.subscribe(response => {
      debugger;
      if (!response) {
        return;
      }
      this.refreshGrids();
    });
  }

  private setGridDefaultColDef() {
    this.defaultColDef = {
      sortable: true,
      filter: true,
    };
  }

  private clearUnlearnedWordsGridData() {
    this.unlearnedWords = [];
  }

  private clearLearnedWordsGridData() {
    this.learnedWords = [];
  }

  private refreshUnlearnedWords() {
    this.wordService.getUnlearnedWords().subscribe(response => {
      this.clearUnlearnedWordsGridData();
      var wordAnswers = response as WordResponse[];
      if (wordAnswers) {
        wordAnswers.map((wordAnswer) => {

          const answerObj: Answer = {} as Answer;

          answerObj["word"] = wordAnswer.word;
          answerObj["meaning"] = wordAnswer.meaning;

          for (let index = 0; index < this.SEQUENT_TRUE_ANSWER_COUNT; index++) {
            const answerProperty = 'answer' + (index + 1).toString();
            answerObj[answerProperty] = null;
          }

          let lastIndex = 0;
          wordAnswer.wordAnswers.forEach((wordAnswer, index) => {
            lastIndex++;
            const answerProperty = 'answer' + (index + 1);
            answerObj[answerProperty] = wordAnswer.answer;
          });

          this.unlearnedWords.push(answerObj);
        });
      }

    });
  }

  private refreshlearnedWords() {
    this.wordService.getLearnedWords().subscribe(response => {
      this.clearLearnedWordsGridData();
      var wordAnswers = response as WordResponse[];
      if (wordAnswers) {
        wordAnswers.map((wordAnswer) => {

          const answerObj: Answer = {} as Answer;

          answerObj["word"] = wordAnswer.word;
          answerObj["meaning"] = wordAnswer.meaning;

          for (let index = 0; index < this.SEQUENT_TRUE_ANSWER_COUNT; index++) {
            const answerProperty = 'answer' + (index + 1).toString();
            answerObj[answerProperty] = true;
          }

          this.learnedWords.push(answerObj);
        });
      }
    });
  }

  private refreshGrids() {
    this.refreshUnlearnedWords();
    this.refreshlearnedWords();
  }

}
