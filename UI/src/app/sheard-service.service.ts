import { HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';
import { UserType } from './enums/user.enum';

@Injectable({
  providedIn: 'root'
})

export class SheardServiceService {
  private tokenKey ='token';

  constructor() { }

  getToken(): HttpHeaders {
    return new HttpHeaders({
      'Authorization': `Bearer ${localStorage.getItem(this.tokenKey)}`
    });
  }

  Header_Get(): HttpHeaders {
    return new HttpHeaders({
      'Authorization': `Bearer ${localStorage.getItem(this.tokenKey)}`
    });
  }

  Header_Post(): HttpHeaders {
    return new HttpHeaders({
      'Authorization': `Bearer ${localStorage.getItem(this.tokenKey)}`,
      'Content-Type': 'application/json'
    });
  }

  getUserClaims(): any {
    try {
      const decodedToken: any = jwtDecode(localStorage.getItem(this.tokenKey));
      return decodedToken;
    } catch (error) {
      console.error('Invalid token', error);
      return null;
    }
  }

  getUserType(): UserType {
    try {
      const decodedToken: any = jwtDecode(localStorage.getItem(this.tokenKey));
      return decodedToken.UserType;
    } catch (error) {
      console.error('Invalid token', error);
      return null;
    }
  }


}


