import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { Ticket } from 'app/ticket';
import { TicketService } from 'app/ticket.service';
import { AssignTicketComponent } from '../assign-ticket/assign-ticket.component';
import { TicketDetailsComponent } from '../ticket-details/ticket-details.component';
import { TicketStatus } from 'app/enums/ticket.enum';

@Component({
  moduleId: module.id,
  selector: 'app-ticket',
  templateUrl: 'ticket.component.html',
})
export class TicketComponent implements OnInit {
  tickets: Ticket[] = [];
  TicketStatus = TicketStatus ;
  filteredTickets: Ticket[] = [];
  displayedColumns: string[] = ['id', 'title', 'createdDate', 'assignedTo', 'status', 'action'];
  dataSource = new MatTableDataSource<Ticket>(this.tickets);
  selectedFilter: string = 'All Tickets';

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
        this.applyFilter(this.selectedFilter);
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

  applyFilter(filter: string): void {
    this.selectedFilter = filter;

    switch (filter) {
      case 'All Tickets':
        this.filteredTickets = this.tickets;
        break;
      case 'Active Tickets':
        this.filteredTickets = this.tickets.filter(ticket => ticket.status !== TicketStatus.Closed);
        break;
      case 'Assigned Tickets':
        this.filteredTickets = this.tickets.filter(ticket => ticket.status !== TicketStatus.New,ticket => ticket.status !== TicketStatus.Closed);
        break;
      default:
        this.filteredTickets = this.tickets;
        break;
    }

    this.dataSource.data = this.filteredTickets;
  }

  openAssignedDialog(ticket: Ticket): void {
    const dialogRef = this.dialog.open(AssignTicketComponent, {
      width: '400px',
      data: { ticketId: ticket.ticketId }
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.getTickets();
      }
      console.log('The dialog was closed');
    });
  }

  openTicketDetails(ticketId: string): void {
    console.log('Opening ticket details for ticketId:', ticketId);
    const dialogRef = this.dialog.open(TicketDetailsComponent, {
      width: '600px',
      data: { id: ticketId }
    });
    dialogRef.afterClosed().subscribe((result) => {
      console.log('The dialog was closed');
    });
  }  
}
