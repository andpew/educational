import { Injectable } from '@angular/core';
import { LocalStorageKey } from '../enums/local-storage-key';

@Injectable({
  providedIn: 'root'
})
export class LocalStorageService {
  constructor() { }

  setItem(key: LocalStorageKey, value: string): void {
    localStorage.setItem(key, value);
  }

  getItem(key: LocalStorageKey): string | null {
    return localStorage.getItem(key);
  }

  removeItem(key: LocalStorageKey): void {
    localStorage.removeItem(key);
  }

  clear(): void {
    localStorage.clear();
  }
}
