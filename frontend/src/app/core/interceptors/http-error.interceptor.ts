import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {

  constructor() { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((err: HttpErrorResponse) => {
        switch (err.status) {
          case 0:
            return this.handleNoConnection(err);
        }
        return throwError(() => err);
      })
    );
  }

  private handleNoConnection(err: HttpErrorResponse): Observable<never> {
    const error = {
      name: 'No connection',
      message: err.url
    } as Error;

    return throwError(() => error);
  }
}
