import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { Ticket } from 'app/ticket';
import { TicketService } from 'app/ticket.service';
import { AddTicketComponent } from '../add-ticket/add-ticket.component';
import { TicketDetailsComponent } from '../ticket-details/ticket-details.component';
import { TicketStatus } from 'app/enums/ticket.enum';

@Component({
  moduleId: module.id,
  selector: 'app-ticket',
  templateUrl: 'ticket.component.html',
})
export class TicketComponent implements OnInit {
  tickets: Ticket[] = [];
  displayedColumns: string[] = ['id', 'title', 'createdDate', 'assignedTo', 'status', 'action'];
  dataSource = new MatTableDataSource<Ticket>(this.tickets);
  selectedFilter: string = 'Filter Tickets';

  constructor(private ticketService: TicketService, public dialog: MatDialog) {}

  ngOnInit(): void {
    this.getTickets();
  }

  getTickets(): void {
    this.ticketService.getTickets().subscribe({
      next: (tickets) => {
        this.tickets = tickets.map((ticket, index) => ({
          ...ticket,
          sequentialId: index + 1
        }));
        this.dataSource.data = this.tickets;
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

  allTickets(): void {
    this.getTickets();
    console.log('All tickets successfully displayed');
  }

  filterTickets(filter: string): void {
    this.selectedFilter = filter;

    if (filter === 'All Tickets') {
      this.getTickets();
    }
  }

  openAssignedDialog(ticket: Ticket): void {
    const dialogRef = this.dialog.open(AddTicketComponent, {
      width: '400px',
      data: { ticket }
    });
    dialogRef.afterClosed().subscribe((result) => {
      console.log('The dialog was closed');
      // Handle result if needed
    });
  }

  openTicketDetails(ticketId: string): void {
    const dialogRef = this.dialog.open(TicketDetailsComponent, {
      width: '600px',
      data: { id: ticketId }
    });
    dialogRef.afterClosed().subscribe((result) => {
      console.log('The dialog was closed');
      // Handle result if needed
    });
  }
}
