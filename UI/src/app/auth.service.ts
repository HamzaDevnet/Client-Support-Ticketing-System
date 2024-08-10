import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { Userdata } from './userdata';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private userdata:Subject<Userdata>=new Subject();
  constructor() { }

  public setUserData(userdata:Userdata){
    this.userdata.next(userdata);
  }

  public getUserDate(){
    return this.userdata.asObservable();
  }
}
