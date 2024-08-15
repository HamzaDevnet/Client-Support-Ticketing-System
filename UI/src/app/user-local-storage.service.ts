import { Injectable } from '@angular/core';
import { Userdata } from './userdata';
import { User } from './ticket.service';
import { jwtDecode } from 'jwt-decode';



@Injectable({
  providedIn: 'root'
})
export class UserLocalStorageService {

  private tokenKey ='token';
  private userKey = 'user';

  constructor() { }

  setUserId(userdata:string):void{
    localStorage.setItem(this.userKey, JSON.stringify(userdata));
  }

  setToken(token:string): void {
   localStorage.setItem(this.tokenKey, token);
  }

  clearToken():void{
    localStorage.removeItem(this.tokenKey);
  }

  
  getCurrentUser():Userdata | null {
    const user = localStorage.getItem(this.userKey);
    const userCliams = this.getClaims(this.tokenKey);
    console.log(userCliams);
    return userCliams ? JSON.parse(user) : null ;
  }

  getClaims(token: string): any {
    try {
      const decodedToken: any = jwtDecode(token);
      return decodedToken;
    } catch (error) {
      console.error('Invalid token', error);
      return null;
    }
  }

  getToken():string | null {
    return localStorage.getItem(this.tokenKey);
  }

  
}

