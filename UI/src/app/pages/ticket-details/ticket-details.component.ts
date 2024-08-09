import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { TicketService } from 'app/ticket.service';
import { Ticket } from 'app/ticket';
import { TicketStatus } from 'app/enums/ticket.enum';

@Component({
  selector: 'app-ticket-details',
  templateUrl: './ticket-details.component.html',
  styleUrls: ['./ticket-details.component.css'] // Ensure this is correct
})
export class TicketDetailsComponent implements OnInit {
  ticket: Ticket | undefined;

  constructor(
    public dialogRef: MatDialogRef<TicketDetailsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { id: string },
    private ticketService: TicketService
  ) {}

  ngOnInit(): void {
    this.ticketService.getTicketById(this.data.id).subscribe({
      next: (ticket) => {
        this.ticket = ticket;
      },
      error: (error) => {
        console.error('Error fetching ticket details', error);
      },
    });
  }

  onClose(): void {
    this.dialogRef.close();
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
}
