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
export class WordAnswerService extends BaseService {

    protected override baseApiName: string="wordanswer";

    constructor(http: HttpClient) {
        super(http);
    }

    public answer<AnswerResponse>(wordAnswerRequest: WordAnswerRequest) {
        return this.post<AnswerResponse>("answer", wordAnswerRequest);
    }

}