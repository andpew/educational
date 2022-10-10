import { HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { LocalStorageKey } from '../enums/local-storage-key';
import { AuthToken } from '../models/auth/auth-token.model';
import { UserLogin } from '../models/auth/user-login.model';
import { UserRegister } from '../models/auth/user-register.model';
import { User } from '../models/user.model';
import { HttpService } from './http.service';
import { LocalStorageService } from './local-storage.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private routePrefix = '/api/auth';

  constructor(
    private httpService: HttpService,
    private localStorageService: LocalStorageService) { }

  register(userRegister: UserRegister): Observable<AuthToken> {
    return this.httpService.postRequest<AuthToken>(this.routePrefix + '/register', userRegister).pipe();
  }

  login(userLogin: UserLogin): Observable<AuthToken> {
    return this.httpService.postRequest<AuthToken>(this.routePrefix + '/login', userLogin);
  }

  setAuthToken(token: AuthToken): void {
    this.localStorageService.setItem(LocalStorageKey.token, token.token);
  }
}
