import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { TicketService } from 'app/ticket.service';
import { Ticket } from 'app/ticket'; 
import { TicketStatus } from '../../enums/ticket.enum';

@Component({
  selector: 'app-myopenticket',
  templateUrl: './myopenticket.component.html',
  styleUrls: ['./myopenticket.component.scss']
})
export class MyopenticketComponent implements OnInit {
  tickets: Ticket[] = [];
  displayedColumns: string[] = ['TicketID'];
  dataSource = new MatTableDataSource<Ticket>(this.tickets);
  selectedFilter: string = 'Status';
  ccomments: { user: string, text: string }[] = [];
  newComment: string = '';
  statuses = Object.values(TicketStatus);
  ticket: Ticket | undefined;

  constructor(private ticketService: TicketService) {}

  ngOnInit(): void {
    this.getTickets();
  }

  getTickets(): void {
    this.ticketService.getTickets().subscribe({
      next: (tickets) => {
        this.tickets = tickets;
        this.dataSource.data = tickets;
      },
      error: (error) => {
        console.error('Error fetching tickets', error);
      },
    });
  }

  getStatusText(status: TicketStatus): string {
    switch (status) {
      case TicketStatus.New:
        return 'New';
      case TicketStatus.Assigned:
        return 'Assigned';
      case TicketStatus.InProgress:
        return 'In Progress';
      case TicketStatus.Closed:
        return 'Closed';
      default:
        return 'Unknown';
    }
  }

  selectTicket(ticket: Ticket): void {
    this.ticket = ticket;
  }

  updateStatus(status: TicketStatus): void {
    if (this.ticket) {
      this.ticket.status = status;
      this.ticketService.updateTicketStatus(this.ticket.ticketId, status).subscribe({
        next: () => {
          console.log('Ticket status updated successfully');
        },
        error: (error) => {
          console.error('Error updating ticket status', error);
        },
      });
    }
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
