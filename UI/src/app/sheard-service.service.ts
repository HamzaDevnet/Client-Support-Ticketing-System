import { HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})

export class SheardServiceService {
  private tokenKey ='token';

  constructor() { }

  getToken():HttpHeaders{
    return new HttpHeaders({
      'Authorization': `Bearer ${localStorage.getItem(this.tokenKey)}`
    });
  }
}

