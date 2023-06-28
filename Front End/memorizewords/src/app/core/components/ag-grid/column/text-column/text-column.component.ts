import { Component } from '@angular/core';
import { ICellRendererAngularComp } from 'ag-grid-angular';
import { ICellRendererParams } from 'ag-grid-community';
import { GoogleTTS } from 'google-tts-api';

@Component({
  selector: 'ag-text-column',
  templateUrl: './text-column.component.html',
  styleUrls: ['./text-column.component.css']
})
export class TextColumnComponent implements ICellRendererAngularComp {

  public value: any;

  constructor(private googleTTS: GoogleTTS) {}

  speak(text: string): void {
    this.googleTTS.getAudioUrl(text, {
      lang: 'pl', // Language code (e.g., 'en', 'es', 'fr')
      slow: false, // Speak slowly (optional)
      host: 'https://translate.google.com', // Google Translate host (optional)
    }).then((url: string) => {
      const audio = new Audio(url);
      audio.play();
    });
  }

  agInit(params: ICellRendererParams<any, any, any>): void {
    this.value = params.value;
  }
  refresh(params: ICellRendererParams<any, any, any>): boolean {
    return true;
  }
  onSpeechClick(){
    
  }

}