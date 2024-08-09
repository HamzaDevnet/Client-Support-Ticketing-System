import { Injectable } from '@angular/core';
import { Userdata } from './userdata';

@Injectable({
  providedIn: 'root'
})
export class UserLocalStorageService {
  private tokenKey = 'token';
  private userKey = 'user';

  constructor() { }

  setUserData(userdata: Userdata): void {
    localStorage.setItem(this.userKey, JSON.stringify(userdata));
  }

  setUserToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
  }

  getCurrentUser(): Userdata | null  {
    const user = localStorage.getItem(this.userKey);
    return user ? JSON.parse(user) : null;
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  clearUserData(): void {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.userKey);
  }
}

