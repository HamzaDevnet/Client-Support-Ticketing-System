import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Users } from './users';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  constructor(private http: HttpClient) { }



getUsers(): Observable<Users[]> {
  const URL ='https://localhost:7125/api/Users/support-team-members';
  return this.http.get<Users[]>(URL);
}

}