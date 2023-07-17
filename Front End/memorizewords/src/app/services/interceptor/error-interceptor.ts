import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AlertifyService } from '../../core/services/alertify-service.service';


@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private alertifyService: AlertifyService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {

        this.alertifyService.error(error.error.userMessage);
        console.error("error happened");
        console.error(JSON.stringify(error.error));

        return throwError(error);
      })
    );
  }
}
