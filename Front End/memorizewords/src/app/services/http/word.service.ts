import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from '../../core/services/http/base-service';
import { Word } from './model/call/word';
import { WordResponse } from './model/back/word-response';
import { Observable } from 'rxjs';
import { QuestionWordResponse } from './model/back/question-word-response';
import { WordUpdateIsLearnedRequest } from './model/call/WordUpdateIsLearnedRequest';

@Injectable({
    providedIn: 'root',
})
export class WordService extends BaseService {

    protected override baseApiName: string="word";

    constructor(http: HttpClient) {
        super(http);
    }

    public addWord<WordResponse>(word: Word) {
        return this.post<WordResponse>("", word);
    }

    public getUnlearnedWords(): Observable<WordResponse[]> {
        return this.get("unlearned-words");
    }

    public getLearnedWords(): Observable<WordResponse[]> {
        return this.get("learned-words");
    }

    public getQuestionWords(): Observable<QuestionWordResponse[]> {
        return this.get<QuestionWordResponse[]>("question-words");
    }

    public updateIsLearned(wordUpdateIsLearnedRequest: WordUpdateIsLearnedRequest) {
        return this.post("update-is-learned", wordUpdateIsLearnedRequest);
    }

    public deleteWords(ids:number[]) {
        return this.delete("", ids);
    }

}