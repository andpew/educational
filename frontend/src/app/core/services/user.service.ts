import { HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../models/user.model';
import { HttpService } from './http.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private routePrefix = '/api/user';

  constructor(private httpService: HttpService) { }

  getAllUsers(): Observable<HttpResponse<User[]>> {
    return this.httpService.getFullRequest<User[]>(this.routePrefix + '/all');
  }
}