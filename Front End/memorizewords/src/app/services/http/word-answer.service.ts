import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from '../../core/services/http/base-service';
import { WordAnswerRequest } from './model/call/WordAnswerRequest';
import { WordAnswerWordRequest } from './model/call/WordAnswerWordRequest';

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

    public answerWord<AnswerResponse>(wordAnswerWordRequest: WordAnswerWordRequest) {
        return this.post<AnswerResponse>("answerWord", wordAnswerWordRequest);
    }

}