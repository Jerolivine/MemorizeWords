import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from './base-service';
import { Word } from './model/call/word';
import { WordResponse } from './model/back/word-response';
import { Observable } from 'rxjs';
import { QuestionWordResponse } from './model/back/question-word-response';
import { WordAnswerRequest } from './model/call/WordAnswerRequest';
import { WordUpdateIsLearnedRequest } from './model/call/WordUpdateIsLearnedRequest';

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

    public answer<AnswerResponse>(wordAnswerRequest: WordAnswerRequest) {
        return this.post<AnswerResponse>("/answer", wordAnswerRequest);
    }

    public getUnlearnedWords(): Observable<WordResponse[]> {
        return this.get("/unlearnedWords");
    }

    public getLearnedWords(): Observable<WordResponse[]> {
        return this.get("/learnedWords");
    }

    public getQuestionWords(): Observable<QuestionWordResponse[]> {
        return this.get<QuestionWordResponse[]>("/questionWords");
    }

    public updateIsLearned(wordUpdateIsLearnedRequest: WordUpdateIsLearnedRequest) {
        return this.post("/updateIsLearned", wordUpdateIsLearnedRequest);
    }

}