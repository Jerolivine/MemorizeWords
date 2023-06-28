import { Injectable } from "@angular/core";
import { POLISH_LANGUAGE } from "../core/constants/languages";

@Injectable({
    providedIn: 'root'
})

export class ApplicationVariablesService {
    language: string;

    constructor() {
        this.configure();
    }

    configure() {
        this.language = POLISH_LANGUAGE;
    }
}