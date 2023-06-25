import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';

@Component({
  selector: 'answers',
  templateUrl: './answers.component.html',
  styleUrls: ['./answers.component.css']
})

export class AnswersComponent {

  private readonly MAX_WIDTH_ANSWER: number = 50;

  columnDefs: ColDef[] = [
    { headerName: 'Word', field: 'word'},
    { headerName: 'Meaning', field: 'meaning'},
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

  rowData = [
    { word: 'Dziekuje', meaning: 'Teşekkürler', answer1: true, answer2: true, answer3: true, answer4: true, answer5: true, answer6: true, answer7: true, answer8: true, answer9: true, answer10: true },
    { word: 'Dziekuje', meaning: 'Teşekkürler', answer1: false, answer2: true, answer3: true, answer4: true, answer5: true, answer6: true, answer7: true, answer8: true, answer9: true, answer10: true },
    { word: 'Dziekuje', meaning: 'Teşekkürler', answer1: true, answer2: false, answer3: true, answer4: true, answer5: true, answer6: true, answer7: true, answer8: true, answer9: true, answer10: true },
    { word: 'Dziekuje', meaning: 'Teşekkürler', answer1: true, answer2: true, answer3: true, answer4: true, answer5: true, answer6: true, answer7: true, answer8: true, answer9: true, answer10: true },
    { word: 'Dziekuje', meaning: 'Teşekkürler', answer1: true, answer2: true, answer3: true, answer4: true, answer5: true, answer6: true, answer7: true, answer8: true, answer9: true, answer10: true },
    { word: 'Dziekuje', meaning: 'Teşekkürler', answer1: true, answer2: true, answer3: true, answer4: true, answer5: true, answer6: true, answer7: true, answer8: true, answer9: true, answer10: true },
    { word: 'Dziekuje', meaning: 'Teşekkürler', answer1: true, answer2: true, answer3: true, answer4: true, answer5: true, answer6: true, answer7: true, answer8: true, answer9: true, answer10: true },
    { word: 'Dziekuje', meaning: 'Teşekkürler', answer1: true, answer2: true, answer3: true, answer4: true, answer5: true, answer6: true, answer7: true, answer8: true, answer9: true, answer10: true },
    { word: 'Dziekuje', meaning: 'Teşekkürler', answer1: true, answer2: true, answer3: true, answer4: true, answer5: true, answer6: true, answer7: true, answer8: true, answer9: true, answer10: true },
    { word: 'Dziekuje', meaning: 'Teşekkürler', answer1: true, answer2: true, answer3: true, answer4: true, answer5: true, answer6: true, answer7: true, answer8: true, answer9: true, answer10: true },
    { word: 'Dziekuje', meaning: 'Teşekkürler', answer1: true, answer2: true, answer3: true, answer4: true, answer5: true, answer6: true, answer7: true, answer8: true, answer9: true, answer10: true },
    { word: 'Dziekuje', meaning: 'Teşekkürler', answer1: true, answer2: true, answer3: true, answer4: true, answer5: true, answer6: true, answer7: true, answer8: true, answer9: true, answer10: true },
    { word: 'Dziekuje', meaning: 'Teşekkürler', answer1: true, answer2: true, answer3: true, answer4: true, answer5: true, answer6: true, answer7: true, answer8: true, answer9: true, answer10: true },
    { word: 'Dziekuje', meaning: 'Teşekkürler', answer1: true, answer2: true, answer3: true, answer4: true, answer5: true, answer6: true, answer7: true, answer8: true, answer9: true, answer10: true },
    { word: 'Dziekuje', meaning: 'Teşekkürler', answer1: true, answer2: true, answer3: true, answer4: true, answer5: true, answer6: true, answer7: true, answer8: true, answer9: true, answer10: true },
    { word: 'Dziekuje', meaning: 'Teşekkürler', answer1: true, answer2: true, answer3: true, answer4: true, answer5: true, answer6: true, answer7: true, answer8: true, answer9: true, answer10: true },
  
  ];
  defaultColDef: any;

  ngOnInit() {
      this.defaultColDef = {
          sortable: true,
          filter: true,
      };
  }

}
