import { HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserLogin } from '../models/user-login.model';
import { UserRegister } from '../models/user-register.model';
import { User } from '../models/user.model';
import { HttpService } from './http.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private routePrefix = '/api/auth';

  constructor(private httpService: HttpService) { }

  register(userRegister: UserRegister): Observable<HttpResponse<User>> {
    return this.httpService.postFullRequest<User>(this.routePrefix + '/register', userRegister);
  }

  login(userLogin: UserLogin): Observable<HttpResponse<User>> {
    return this.httpService.postFullRequest<User>(this.routePrefix + '/login', userLogin);
  }
}
