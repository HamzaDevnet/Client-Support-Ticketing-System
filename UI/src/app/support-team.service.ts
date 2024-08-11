import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { SupportTeam } from './support-team';
import { HttpClient } from '@angular/common/http';
import { User, WebResponse } from './ticket.service';
import { Users } from './users';

@Injectable({
  providedIn: 'root',
})
export class SupportTeamService {

  private usersUrl = 'https://localhost:7125/api/Users';
  constructor(private http: HttpClient) {}


  getSupportTeam(): Observable<Users[]> {
    return this.http.get<WebResponse<Users[]>>(`${this.usersUrl}/support-team-members`).pipe(
      map(response => response.data)
    );
  }
}