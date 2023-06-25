import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

export class BaseService {
    
    apiURL = environment.apiURL;
    constructor(private http: HttpClient) {

    }

    protected get<T>(url: string) {
        return this.http.get<T>(this.getUrl(url));
    }

    protected post<T>(url: string, model: any) {
        return this.http.post<T>(this.getUrl(url), model);
    }

    private getUrl(url: string): string {
        return `${this.apiURL}${url}`;
    }
}