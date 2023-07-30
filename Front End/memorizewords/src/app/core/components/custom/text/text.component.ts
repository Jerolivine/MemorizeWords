import { Component, Input } from '@angular/core';
import { TextToSpeechService } from 'src/app/services/text-to-speech.service';

@Component({
  selector: 'app-text',
  templateUrl: './text.component.html',
  styleUrls: ['./text.component.css']
})
export class TextComponent {

  @Input() public Text: string;
  @Input() public HasTextToSpeech: string;

  constructor(private textToSpeechService: TextToSpeechService) { }

  onSpeechClick() {
    if (this.Text && this.HasTextToSpeech) {
      this.textToSpeechService.speak(this.Text);
    }
  }
}
