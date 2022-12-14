import { HttpClient, HttpHeaders, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class HttpService {
  public baseUrl: string = environment.apiUrl;

  constructor(private http: HttpClient) { }

  public getRequest<T>(url: string): Observable<T> {
    return this.http.get<T>(this.buildUrl(url));
  }

  public postRequest<T>(url: string, payload: object): Observable<T> {
    return this.http.post<T>(this.buildUrl(url), payload);
  }

  public putRequest<T>(url: string, payload: object): Observable<T> {
    return this.http.put<T>(this.buildUrl(url), payload);
  }

  public deleteRequest<T>(url: string): Observable<T> {
    return this.http.delete<T>(this.buildUrl(url));
  }

  // Http params are also known as query strings
  public getFullRequest<T>(url: string, httpParams?: HttpParams): Observable<HttpResponse<T>> {
    return this.http.get<T>(this.buildUrl(url), {
      observe: 'response',
      params: httpParams
    });
  }

  public postFullRequest<T>(url: string, payload: object): Observable<HttpResponse<T>> {
    return this.http.post<T>(this.buildUrl(url), payload, { observe: 'response' });
  }

  public putFullRequest<T>(url: string, payload?: object): Observable<HttpResponse<T>> {
    return this.http.put<T>(this.buildUrl(url), payload, { observe: 'response' });
  }

  // Http params are also known as query strings
  public deleteFullRequest<T>(url: string, httpParams?: HttpParams): Observable<HttpResponse<T>> {
    return this.http.delete<T>(this.buildUrl(url), {
      observe: 'response',
      params: httpParams
    });
  }

  public buildUrl(url: string): string {
    if (url.startsWith('http://') || url.startsWith('https://')) {
      return url;
    }
    return this.baseUrl + url;
  }
}
