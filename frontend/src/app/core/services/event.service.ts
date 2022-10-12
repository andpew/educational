import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class EventService {
  private userSubj = new Subject<User>();
  public user$ = this.userSubj.asObservable();

  constructor() { }

  public userChanged(user: User): void {
    this.userSubj.next(user);
  }
}
