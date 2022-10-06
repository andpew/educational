import { HttpClient, HttpHeaders, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class HttpService {
  public baseUrl: string = environment.apiUrl;
  public headers = new HttpHeaders();

  constructor(private http: HttpClient) { }

  public getHeaders(): HttpHeaders {
    return this.headers;
  }

  public getFullRequest<T>(url: string, httpParams?: HttpParams): Observable<HttpResponse<T>> {
    return this.http.get<T>(this.buildUrl(url), {
      observe: 'response', headers: this.getHeaders(),
      params: httpParams
    });
  }

  public postFullRequest<T>(url: string, payload: object): Observable<HttpResponse<T>> {
    return this.http.post<T>(this.buildUrl(url), payload, { headers: this.getHeaders(), observe: 'response' });
  }

  public putFullRequest<T>(url: string, payload?: object): Observable<HttpResponse<T>> {
    return this.http.put<T>(this.buildUrl(url), payload, { headers: this.getHeaders(), observe: 'response' });
  }

  public deleteFullRequest<T>(url: string, httpParams?: HttpParams): Observable<HttpResponse<T>> {
    return this.http.delete<T>(this.buildUrl(url), {
      headers: this.getHeaders(), observe: 'response',
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
