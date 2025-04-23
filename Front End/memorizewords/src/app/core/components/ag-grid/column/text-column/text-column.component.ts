import { Component } from '@angular/core';
import { ICellRendererAngularComp } from 'ag-grid-angular';
import { ICellRendererParams } from 'ag-grid-community';
import { POLISH_TO_LATIN_DICTIONARY } from 'src/app/core/constants/alphabet';
import { POLISH_LANGUAGE } from 'src/app/core/constants/languages';
import { TextToSpeechService } from 'src/app/core/services/text-to-speech.service';

@Component({
  selector: 'ag-text-column',
  templateUrl: './text-column.component.html',
  styleUrls: ['./text-column.component.css']
})
export class TextColumnComponent implements ICellRendererAngularComp {

  public value: any;

  constructor(private textToSpeechService: TextToSpeechService) { }

  agInit(params: ICellRendererParams<any, any, any>): void {
    this.value = params.value;
  }
  refresh(params: ICellRendererParams<any, any, any>): boolean {
    return true;
  }
  onSpeechClick() {
    this.textToSpeechService.speak(this.value.value, POLISH_LANGUAGE);
  }

}