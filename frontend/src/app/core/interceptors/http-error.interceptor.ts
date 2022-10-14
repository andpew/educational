import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse,
  HttpStatusCode
} from '@angular/common/http';
import { catchError, Observable, switchMap, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {

  constructor(private authService: AuthService) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((err: HttpErrorResponse) => {
        switch (err.status) {
          case 0:
            return this.handleNoConnection(err);
          case HttpStatusCode.Unauthorized:
            return this.handleUnauthorized(request, next, err);
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

  private handleUnauthorized(request: HttpRequest<unknown>, next: HttpHandler, err: HttpErrorResponse):
    Observable<HttpEvent<unknown>> | Observable<never> {
    if (err.headers.has('Token-Expired')) {
      return this.authService.refresh().pipe(
        switchMap((resp) => {
          request = request.clone({
            setHeaders: {
              Authorization: `Bearer ${resp.accessToken}`
            }
          });

          return next.handle(request);
        })
      );
    }

    const error = {
      name: 'Unauthorized',
      message: err.url
    } as Error;

    this.authService.logout();

    return throwError(() => error);
  }
}
