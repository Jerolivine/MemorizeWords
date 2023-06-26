import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from './base-service';
import { Word } from './model/call/word';
import { Answer } from './model/call/answer';
import { WordResponse } from './model/back/word-response';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class WordService extends BaseService {

    constructor(http: HttpClient) {
        super(http);
    }

    public addWord<WordResponse>(word: Word) {
        return this.post<WordResponse>("/word", word);
    }

    public answer(answer:Answer){
        return this.post("/answer", answer);
    }

    public getUnlearnedWords():Observable<WordResponse[]>{
        return this.get("/unlearnedWords");
    }

    public getLearnedWords():Observable<WordResponse[]>{
        return this.get("/learnedWords");
    }
}