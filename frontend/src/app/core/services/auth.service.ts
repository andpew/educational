import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { map, Observable, take } from 'rxjs';
import { LocalStorageKey } from '../enums/local-storage-key';
import { AuthTokens } from '../models/auth/auth-tokens.model';
import { RefreshToken } from '../models/auth/refresh-token.model';
import { UserLogin } from '../models/auth/user-login.model';
import { UserRegister } from '../models/auth/user-register.model';
import { HttpService } from './http.service';
import { LocalStorageService } from './local-storage.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private routePrefix = '/api/auth';

  constructor(
    private httpService: HttpService,
    private localStorageService: LocalStorageService,
    private router: Router) { }

  register(userRegister: UserRegister): Observable<string> {
    return this.httpService.postRequest<string>(this.routePrefix + '/register', userRegister);
  }

  login(userLogin: UserLogin): Observable<AuthTokens> {
    return this.httpService.postRequest<AuthTokens>(this.routePrefix + '/login', userLogin);
  }

  refresh(): Observable<AuthTokens> {
    return this.httpService.postRequest<AuthTokens>(this.routePrefix + '/refresh', this.getAuthTokens())
      .pipe(
        map((tokens) => {
          this.setAuthTokens(tokens);
          return tokens;
        })
      );
  }

  revoke(): Observable<string> {
    const refreshToken = {
      token: this.localStorageService.getItem(LocalStorageKey.refreshToken)
    } as RefreshToken;

    return this.httpService.postRequest<string>(this.routePrefix + '/revoke', refreshToken);
  }

  logout(): void {
    this.revoke().subscribe().add(() => {
      this.removeAuthTokens();
      this.router.navigate(['/auth']);
    });
  }

  getAuthTokens(): AuthTokens {
    return {
      accessToken: this.localStorageService.getItem(LocalStorageKey.accessToken),
      refreshToken: this.localStorageService.getItem(LocalStorageKey.refreshToken)
    } as AuthTokens;
  }

  setAuthTokens(tokens: AuthTokens): void {
    this.localStorageService.setItem(LocalStorageKey.accessToken, tokens.accessToken);
    this.localStorageService.setItem(LocalStorageKey.refreshToken, tokens.refreshToken);
  }

  removeAuthTokens(): void {
    this.localStorageService.removeItem(LocalStorageKey.accessToken);
    this.localStorageService.removeItem(LocalStorageKey.refreshToken);
  }

  areTokensExist(): boolean {
    const tokens = this.getAuthTokens();
    return tokens.accessToken !== null && tokens.refreshToken !== null;
  }
}
