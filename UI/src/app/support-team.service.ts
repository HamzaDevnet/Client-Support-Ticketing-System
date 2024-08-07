import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SupportTeam } from './support-team';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class SupportTeamService {
  constructor(private http: HttpClient) {}

  getSupportTeam(): Observable<SupportTeam[]> {
    const URL = 'https://dummyapi.online/api/users';
    return this.http.get<SupportTeam[]>(URL);
  }
}