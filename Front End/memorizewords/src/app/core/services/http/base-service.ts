import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../../../environments/environment';

export abstract class BaseService {
    
    apiURL = environment.apiURL;
    protected abstract baseApiName:string; 

    constructor(private http: HttpClient) {

    }

    protected get<T>(url: string) {
        return this.http.get<T>(this.getUrl(url));
    }

    protected post<T>(url: string, model: any) {
        return this.http.post<T>(this.getUrl(url), model);
    }

    protected put<T>(url: string, model: any) {
        return this.http.put<T>(this.getUrl(url), model);
    }

    protected delete<T>(url: string, model: any) {
        return this.http.delete<T>(this.getUrl(url), {body:model, headers:this.headers});
    }

    private getUrl(url: string): string {
        return `${this.apiURL}/${this.baseApiName}/${url}`;
    }

    private get headers(){
        const headers = new HttpHeaders().set('Content-Type', 'application/json');

        return headers;
    }
}