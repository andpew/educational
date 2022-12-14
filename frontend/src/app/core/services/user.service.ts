import { HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../models/user/user.model';
import { HttpService } from './http.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private routePrefix = '/api/user';

  constructor(private httpService: HttpService) { }

  getAllUsers(): Observable<User[]> {
    return this.httpService.getRequest<User[]>(this.routePrefix + '/all');
  }

  getUserFromToken(): Observable<User> {
    return this.httpService.getRequest<User>(this.routePrefix + '/from-token');
  }
}
