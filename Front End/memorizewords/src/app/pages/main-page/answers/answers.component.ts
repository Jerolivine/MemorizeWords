import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';
import { WordResponse } from 'src/app/services/http/model/back/word-response';
import { WordService } from 'src/app/services/http/word.service';
import { Answer } from './model/answer';

@Component({
  selector: 'answers',
  templateUrl: './answers.component.html',
  styleUrls: ['./answers.component.css']
})

export class AnswersComponent implements OnInit {

  private readonly MAX_WIDTH_ANSWER: number = 50;

  columnDefs: ColDef[] = [
    { headerName: 'Word', field: 'word' },
    { headerName: 'Meaning', field: 'meaning' },
    { headerName: 'Answer1', field: 'answer1', maxWidth: this.MAX_WIDTH_ANSWER },
    { headerName: 'Answer2', field: 'answer2', maxWidth: this.MAX_WIDTH_ANSWER },
    { headerName: 'Answer3', field: 'answer3', maxWidth: this.MAX_WIDTH_ANSWER },
    { headerName: 'Answer4', field: 'answer4', maxWidth: this.MAX_WIDTH_ANSWER },
    { headerName: 'Answer5', field: 'answer5', maxWidth: this.MAX_WIDTH_ANSWER },
    { headerName: 'Answer6', field: 'answer6', maxWidth: this.MAX_WIDTH_ANSWER },
    { headerName: 'Answer7', field: 'answer7', maxWidth: this.MAX_WIDTH_ANSWER },
    { headerName: 'Answer8', field: 'answer8', maxWidth: this.MAX_WIDTH_ANSWER },
    { headerName: 'Answer9', field: 'answer9', maxWidth: this.MAX_WIDTH_ANSWER },
    { headerName: 'Answer10', field: 'answer10', maxWidth: this.MAX_WIDTH_ANSWER }
  ];

  unlearnedWords: Answer[] = [];
  learnedWords: Answer[] = [];

  defaultColDef: any;

  constructor(private wordService: WordService) {

  }

  ngOnInit() {
    this.setGridDefaultColDef();
    this.refreshGrids();
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

          debugger;
          const answerObj: Answer = {} as Answer;

          answerObj["word"] = wordAnswer.word;
          answerObj["meaning"] = wordAnswer.meaning;

          for (let index = 0; index < 10; index++) {
            const answerProperty = 'answer' + (index + 1).toString();
            answerObj[answerProperty] = false;
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

      console.log(JSON.stringify(this.unlearnedWords));
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

          for (let index = 0; index < 10; index++) {
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
