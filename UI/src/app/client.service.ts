import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Users } from './users';
import { WebResponse } from './ticket.service';
import { map, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ClientService {

  private usersUrl = 'https://localhost:7125/api/Users';
  constructor(private http: HttpClient) {}


  getSupportTeam(): Observable<Users[]> {
    return this.http.get<WebResponse<Users[]>>(`${this.usersUrl}/external-clients`).pipe(
      map(response => response.data)
    );
  }
}
