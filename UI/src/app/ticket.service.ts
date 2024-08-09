import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Ticket } from './ticket';

@Injectable({
  providedIn: 'root'
})
export class TicketService {

  constructor(private http: HttpClient) { }

  getTickets(): Observable<Ticket[]> {
    const URL = 'https://localhost:7125/api/Tickets/Summary';
    return this.http.get<Ticket[]>(URL);
  }

  getTicketById(id: string): Observable<Ticket> {
    const URL = `https://localhost:7125/api/Tickets/${id}`;
    return this.http.get<Ticket>(URL);
  }
}
