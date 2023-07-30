import { HttpClient } from '@angular/common/http';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef, ICellRendererParams, RowNode } from 'ag-grid-community';
import { WordResponse } from 'src/app/services/http/model/back/word-response';
import { WordService } from 'src/app/services/http/word.service';
import { Answer } from './model/answer';
import { BooleanAgColumnComponent } from 'src/app/core/components/ag-grid/column/boolean-ag-column/boolean-ag-column.component';
import { BehaviorSubject, Observable, concatMap, of } from 'rxjs';
import { ProgressbarAgColumnComponent } from 'src/app/core/components/ag-grid/column/progressbar-ag-column/progressbar-ag-column.component';
import { TextColumnComponent } from 'src/app/core/components/ag-grid/column/text-column/text-column.component';
import { WordUpdateIsLearnedRequest } from 'src/app/services/http/model/call/WordUpdateIsLearnedRequest';
import { WordAnswer } from 'src/app/services/http/model/dto/word-answer';

@Component({
  selector: 'answers',
  templateUrl: './answers.component.html',
  styleUrls: ['./answers.component.css']
})

export class AnswersComponent implements OnInit {

  private readonly MAX_WIDTH_ANSWER: number = 70;
  private readonly SEQUENT_TRUE_ANSWER_COUNT = 10; // TODO-Arda Get this value from service

  public selectedUnlearnedGridRows: any[] = [];

  get hasSelectedUnlearnedGridRows(): boolean {
    return this.selectedUnlearnedGridRows.length > 0;
  }

  public selectedLearnedGridRows: any[] = [];

  get hasSelectedLearnedGridRows(): boolean {
    return this.selectedLearnedGridRows.length > 0;
  }

  @Input() public refreshData$: BehaviorSubject<boolean> = new BehaviorSubject(false);
  @Input() userGuessedWordsHub$: BehaviorSubject<WordResponse[]> = new BehaviorSubject<WordResponse[]>([]);

  @ViewChild('gridUnlearned') gridUnlearned: AgGridAngular;

  columnDefs: ColDef[] = [
    { headerName: '', field: 'checkbox', checkboxSelection: true, width: 30 },
    // { field: 'id', colId: 'id' },
    { headerName: 'Word', field: 'word', filter: true, width: 150, },
    { headerName: 'Word', field: 'word', filter: true, width: 150, },
    { headerName: 'Writing In Language', field: 'writingInLanguage', filter: true, width: 150, cellRenderer: TextColumnComponent },
    { headerName: 'Meaning', field: 'meaning', filter: true, width: 150 },
    { headerName: 'Percentage', field: 'percentage', filter: true, width: 150, cellRenderer: ProgressbarAgColumnComponent },
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
    this.listenUserGuessedWordsHub();

    this.refreshData$.subscribe(response => {
      if (!response) {
        return;
      }
      this.refreshGrids();
    });
  }

  private listenUserGuessedWordsHub() {
    this.userGuessedWordsHub$.subscribe(datas => {

      const unLearnedWordsHub = datas.filter(x => !x.isLearned);
      const learnedWordsHub = datas.filter(x => x.isLearned);

      if (unLearnedWordsHub?.length > 0) {
        unLearnedWordsHub.forEach(unlearnedWord => {
          const unlearnedWordAnswers: WordAnswer[] = unlearnedWord.wordAnswers;

          const unlearnedWordDataSource = this.unlearnedWords.find(x => x["id"] == unlearnedWord.wordId);
          unlearnedWordDataSource

        });
      }

      console.log("listened Data :" + JSON.stringify(datas));
      debugger;

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

          answerObj["id"] = wordAnswer.wordId;
          answerObj["word"] = wordAnswer.word;
          answerObj["meaning"] = wordAnswer.meaning;
          answerObj["percentage"] = wordAnswer.percentage;
          answerObj["writingInLanguage"] = {
            value: wordAnswer.writingInLanguage,
            hasTextToSpeech: true
          }

          // for (let index = 0; index < this.SEQUENT_TRUE_ANSWER_COUNT; index++) {
          //   const answerProperty = 'answer' + (index + 1).toString();
          //   answerObj[answerProperty] = null;
          // }

          // let lastIndex = 0;
          // wordAnswer.wordAnswers.forEach((wordAnswer, index) => {
          //   lastIndex++;
          //   const answerProperty = 'answer' + (index + 1);
          //   answerObj[answerProperty] = wordAnswer.answer;
          // });

          // this.unlearnedWords.push(answerObj);
        });
      }

    });
  }

  private setAnswers() {

    const answers = {};

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
  }

  private refreshlearnedWords() {
    this.wordService.getLearnedWords().subscribe(response => {
      this.clearLearnedWordsGridData();
      var wordAnswers = response as WordResponse[];
      if (wordAnswers) {
        wordAnswers.map((wordAnswer) => {

          const answerObj: Answer = {} as Answer;

          answerObj["id"] = wordAnswer.wordId;
          answerObj["word"] = wordAnswer.word;
          answerObj["meaning"] = wordAnswer.meaning;
          answerObj["percentage"] = "100";
          answerObj["writingInLanguage"] = {
            value: wordAnswer.writingInLanguage,
            hasTextToSpeech: true
          }

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

  public onGridUnlearnedRowClicked(event: any) {
    this.selectedUnlearnedGridRows = this.getSelectedRows(event);
  }

  public onGridLearnedRowClicked(event: any) {
    this.selectedLearnedGridRows = this.getSelectedRows(event);
  }

  private getSelectedRows(event: any) {
    const selectedRows: RowNode[] = event.api.getSelectedNodes();
    const selectedRowData = selectedRows.map(node => node.data);

    return selectedRowData;
  }

  public onMoveToLearnedWordsClick() {
    const ids: number[] = this.selectedUnlearnedGridRows.map(x => x.id);
    const wordUpdateIsLearnedRequest: WordUpdateIsLearnedRequest = {
      ids: ids,
      isLearned: true
    };

    this.wordService.updateIsLearned(wordUpdateIsLearnedRequest).pipe(
      concatMap(response => {
        this.selectedUnlearnedGridRows = [];
        this.refreshGrids();
        return of();
      })).subscribe();

  }

  public onMoveToUnLearnedWordsClick() {
    const ids: number[] = this.selectedLearnedGridRows.map(x => x.id);
    const wordUpdateIsLearnedRequest: WordUpdateIsLearnedRequest = {
      ids: ids,
      isLearned: false
    };

    this.wordService.updateIsLearned(wordUpdateIsLearnedRequest).pipe(
      concatMap(response => {
        this.selectedLearnedGridRows = [];
        this.refreshGrids();
        return of();
      })).subscribe();

  }

  public onDeleteFromUnlearnedWords(){
    const ids = this.selectedUnlearnedGridRows.map(row => {
      return row["id"];
    });

    this.wordService.deleteWords(ids).pipe(
      concatMap(response => {
        this.refreshUnlearnedWords();
        return of();
      })).subscribe();
  }

  public onDeleteFromLearnedWords(){
    const ids = this.selectedLearnedGridRows.map(row => {
      return row["id"];
    });

    this.wordService.deleteWords(ids).pipe(
      concatMap(response => {
        this.refreshlearnedWords();
        return of();
      })).subscribe();
  }

}
