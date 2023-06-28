import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from './base-service';

@Injectable({
    providedIn: 'root',
})
export class ExceptionService extends BaseService {

    constructor(http: HttpClient) {
        super(http);
    }

    public businessException() {
        return this.get("/businessException");
    }

}