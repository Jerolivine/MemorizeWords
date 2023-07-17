import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from '../../core/services/http/base-service';
import { WordAnswerRequest } from './model/call/WordAnswerRequest';

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