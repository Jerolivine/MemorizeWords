import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef, RowNode } from 'ag-grid-community';
import { WordResponse } from 'src/app/services/http/model/back/word-response';
import { WordService } from 'src/app/services/http/word.service';
import { Answer } from './model/answer';
import { BooleanAgColumnComponent } from 'src/app/core/components/ag-grid/column/boolean-ag-column/boolean-ag-column.component';
import { BehaviorSubject, concatMap, delay, exhaustMap, of } from 'rxjs';
import { ProgressbarAgColumnComponent } from 'src/app/core/components/ag-grid/column/progressbar-ag-column/progressbar-ag-column.component';
import { TextColumnComponent } from 'src/app/core/components/ag-grid/column/text-column/text-column.component';
import { WordUpdateIsLearnedRequest } from 'src/app/services/http/model/call/WordUpdateIsLearnedRequest';
import { ListUtility } from 'src/app/core/utility/list-utility';
import { RefreshType } from 'src/app/enums/refresh-type.enum';

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

  @Input() public refreshData$: BehaviorSubject<RefreshType> = new BehaviorSubject<RefreshType>(RefreshType.RefreshGrid);
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
  listenedUnlearnedWords: Answer[] = [];
  learnedWords: Answer[] = [];
  listenedLearnedWords: Answer[] = [];

  defaultColDef: any;

  constructor(private wordService: WordService) {

  }

  ngOnInit() {
    this.setGridDefaultColDef();
    this.refreshData(RefreshType.RefreshGrid);

    // TODO-Arda :Question open dialog call with RefreshData
    this.listenUserGuessedWordsHub();
    this.refreshData$.subscribe(refreshType => {

      if (!refreshType) {
        return;
      }

      this.refreshData(refreshType);

    });
  }

  private refreshData(refreshType: RefreshType) {
    if (refreshType === RefreshType.RefreshGrid) {
      this.refreshGrids();
    }
    else {
      // this.listenUserGuessedWordsHub();
    }
  }

  private refreshGridWithListenedData() {

    this.unlearnedWords = [...this.listenedUnlearnedWords];
    this.listenedUnlearnedWords = [];

    this.learnedWords = [...this.listenedLearnedWords];
    this.listenedLearnedWords = [];

  }

  private listenUserGuessedWordsHub() {
    this.userGuessedWordsHub$
      .pipe(exhaustMap((datas) => {
        return of(datas);
      })
      ).subscribe(datas => {

        console.log("event data : " + JSON.stringify(datas));

        if (this.listenedUnlearnedWords.length === 0) {
          this.listenedUnlearnedWords = [...this.unlearnedWords];
        }

        if (this.listenedLearnedWords.length === 0) {
          this.listenedLearnedWords = [...this.learnedWords];
        }

        const unLearnedWordsHub = datas.filter(x => !x.isLearned);
        const learnedWordsHub = datas.filter(x => x.isLearned);

        this.arrangeListenedUnlearnedWords(unLearnedWordsHub, learnedWordsHub);
        this.arrangeListenedLearnedWords(learnedWordsHub);

        this.refreshGridWithListenedData();

      });
  }

  private arrangeListenedUnlearnedWords(unLearnedWordsHub: WordResponse[], learnedWordsHub: WordResponse[]) {

    this.removeLearnedWordsFromListenedUnlearnedWordList(learnedWordsHub);

    if (unLearnedWordsHub?.length > 0) {
      unLearnedWordsHub.forEach(unLearnedWordHub => {
        let index = this.listenedUnlearnedWords.findIndex(x => x["id"] == unLearnedWordHub.wordId);
        this.listenedUnlearnedWords[index] = { ...this.listenedUnlearnedWords[index], ...this.createAnswers(unLearnedWordHub) };
        this.listenedUnlearnedWords[index]["percentage"] = unLearnedWordHub.percentage;
      });
    }
  }

  private removeLearnedWordsFromListenedUnlearnedWordList(learnedWordsHub: WordResponse[]) {
    const learnedWordIds: number[] = this.getLearnedHubWordIds(learnedWordsHub);
    this.listenedUnlearnedWords = ListUtility.removeItemsByPropertyValues(this.listenedUnlearnedWords, learnedWordIds, "id");
  }

  private arrangeListenedLearnedWords(learnedWordsHub: WordResponse[]) {
    if (learnedWordsHub?.length !== 0) {
      const listenedLearnedWords = this.mapWordResponseToAnswer(learnedWordsHub);

      listenedLearnedWords.forEach(listenedLearnedWord => {
        this.listenedLearnedWords.push(listenedLearnedWord);
      });
    }
  }

  private getLearnedHubWordIds(learnedWordsHub: WordResponse[]) {
    return learnedWordsHub.map(x => {
      return x.wordId;
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

      const wordAnswers = response as WordResponse[];
      const unlearnedWords = this.mapWordResponseToAnswer(wordAnswers);

      unlearnedWords.forEach(unlearnedWord => {
        this.unlearnedWords.push(unlearnedWord);
      });


    });
  }

  private mapWordResponseToAnswer(wordResponse: WordResponse[], defaultAnswer?: boolean): Answer[] {

    if (wordResponse?.length === 0) {
      return [];
    }

    return wordResponse.map((wordResponse) => {

      let answerObj: Answer = {} as Answer;

      answerObj["id"] = wordResponse.wordId;
      answerObj["word"] = wordResponse.word;
      answerObj["meaning"] = wordResponse.meaning;
      answerObj["percentage"] = wordResponse.percentage;
      answerObj["writingInLanguage"] = {
        value: wordResponse.writingInLanguage,
        hasTextToSpeech: true
      }

      return answerObj = { ...answerObj, ...this.createAnswers(wordResponse, defaultAnswer) }
    });

  }

  private createAnswers(wordAnswer?: WordResponse, defaultAnswer?: boolean) {

    const answers: { [key: string]: any } = {};

    for (let index = 0; index < this.SEQUENT_TRUE_ANSWER_COUNT; index++) {
      const answerProperty = 'answer' + (index + 1).toString();
      answers[answerProperty] = defaultAnswer;
    }

    if (wordAnswer?.wordAnswers?.length === undefined || wordAnswer?.wordAnswers?.length === 0) {
      return answers;
    }

    let lastIndex = 0;
    wordAnswer!.wordAnswers.forEach((wordAnswer, index) => {
      lastIndex++;
      const answerProperty = 'answer' + (index + 1);
      answers[answerProperty] = wordAnswer.answer;
    });


    return answers;
  }

  private refreshlearnedWords() {
    this.wordService.getLearnedWords().subscribe(response => {
      this.clearLearnedWordsGridData();
      var wordAnswers = response as WordResponse[];

      const learnedWords = this.mapWordResponseToAnswer(wordAnswers, true);

      learnedWords.forEach(learnedWord => {

        learnedWord["percentage"] = "100";
        this.learnedWords.push(learnedWord);
      });
      // if (wordAnswers) {
      //   wordAnswers.map((wordAnswer) => {

      //     const answerObj: Answer = {} as Answer;

      //     answerObj["id"] = wordAnswer.wordId;
      //     answerObj["word"] = wordAnswer.word;
      //     answerObj["meaning"] = wordAnswer.meaning;
      //     answerObj["percentage"] = "100";
      //     answerObj["writingInLanguage"] = {
      //       value: wordAnswer.writingInLanguage,
      //       hasTextToSpeech: true
      //     }

      //     for (let index = 0; index < this.SEQUENT_TRUE_ANSWER_COUNT; index++) {
      //       const answerProperty = 'answer' + (index + 1).toString();
      //       answerObj[answerProperty] = true;
      //     }

      //     this.learnedWords.push(answerObj);
      //   });
      // }
    });
  }

  private refreshGrids() {
    this.listenedUnlearnedWords = [];
    this.listenedLearnedWords = [];

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
        this.refreshData(RefreshType.RefreshGrid);
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
        this.refreshData(RefreshType.RefreshGrid);
        return of();
      })).subscribe();

  }

  public onDeleteFromUnlearnedWords() {
    const ids = this.selectedUnlearnedGridRows.map(row => {
      return row["id"];
    });

    this.wordService.deleteWords(ids).pipe(
      concatMap(response => {
        this.refreshUnlearnedWords();
        return of();
      })).subscribe();
  }

  public onDeleteFromLearnedWords() {
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
