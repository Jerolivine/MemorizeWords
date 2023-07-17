import { Injectable } from "@angular/core";
import { POLISH_LANGUAGE } from "../constants/languages";
import { ApplicationVariablesService } from "../../services/application-variables.service";

@Injectable({
    providedIn: 'root'
})

export class TextToSpeechService {
    
    constructor(private applicationVariablesService:ApplicationVariablesService){

    }

    speak(value: string) {
        var utterance = new SpeechSynthesisUtterance();

        // Set the text to be spoken in Polish
        utterance.text = value;

        // Set the language to Polish (Poland)
        utterance.lang = this.applicationVariablesService.language;
        // utterance.lang = 'pl-PL';

        // Optional configurations
        // utterance.lang = 'en-US'; // Specify the language
        utterance.rate = 0.8; // Speech rate (0.1 to 10)
        // utterance.volume = 1.0; // Speech volume (0 to 1)

        speechSynthesis.speak(utterance);
    }
}
