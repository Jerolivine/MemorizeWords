import { Injectable } from "@angular/core";
import { POLISH_LANGUAGE } from "../core/constants/languages";
import { ApplicationVariablesService } from "./application-variables.service";

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
        debugger;
        utterance.lang = this.applicationVariablesService.language;
        // utterance.lang = 'pl-PL';

        // Optional configurations
        // utterance.lang = 'en-US'; // Specify the language
        // utterance.rate = 1.0; // Speech rate (0.1 to 10)
        // utterance.volume = 1.0; // Speech volume (0 to 1)

        speechSynthesis.speak(utterance);
    }
}
