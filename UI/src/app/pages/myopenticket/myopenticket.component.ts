import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { Ticket } from 'app/ticket';
import { TicketService } from 'app/ticket.service';

@Component({
  selector: 'app-myopenticket',
  templateUrl: './myopenticket.component.html',
  styleUrls: ['./myopenticket.component.scss']
})
export class MyopenticketComponent implements OnInit {
  tickets: Ticket[] = [];
  displayedColumns: string[] = ['ticketId'];
  dataSource = new MatTableDataSource<Ticket>(this.tickets);
  selectedFilter: string = 'Status';
  ccomments: { user: string, text: string }[] = [];
  newComment: string = '';
  selectedTicket: Ticket | null = null;

  constructor(private ticketService: TicketService, public dialog: MatDialog) {}

  ngOnInit(): void {
    this.getTickets();
  }

  getTickets(): void {
    this.ticketService.getTickets().subscribe({
      next: (tickets) => {
        this.tickets = tickets;
        this.dataSource.data = tickets;
        console.log('Successfully fetched tickets');
      },
      error: (error) => {
        console.error('Error fetching tickets', error);
      }
    });
  }

  selectTicket(ticket: Ticket): void {
    this.selectedTicket = ticket;
  }

  setFilter(filter: string) {
    this.selectedFilter = filter;
  }

  postComment() {
    if (this.newComment.trim()) {
      this.ccomments.push({ user: 'You', text: this.newComment });
      this.newComment = '';
    }
  }
}