import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TicketService, User } from 'app/ticket.service';

@Component({
  selector: 'app-assign-ticket',
  templateUrl: './assign-ticket.component.html',
  styleUrls: ['./assign-ticket.component.scss']
})
export class AssignTicketComponent implements OnInit {
  assignForm: FormGroup;
  supportTeamMembers: User[] = [];

  constructor(
    private fb: FormBuilder,
    private ticketService: TicketService,
    public dialogRef: MatDialogRef<AssignTicketComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { ticketId: string }
  ) {
    this.assignForm = this.fb.group({
      assignedTo: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.ticketService.getSupportTeamMembers().subscribe({
      next: (users) => {
        this.supportTeamMembers = users;
      },
      error: (error) => console.error('Error fetching support team members', error)
    });
  }

  onAssign(): void {
    if (this.assignForm.valid) {
      console.log('Form is valid, submitting assignment'); // Debug log
      this.ticketService.assignTicket(this.data.ticketId, this.assignForm.value.assignedTo).subscribe({
        next: () => {
          console.log('Ticket assigned successfully'); // Debug log
          this.dialogRef.close(true);
        },
        error: (error) => console.error('Error assigning ticket', error)
      });
    } else {
      console.error('Form is invalid'); // Debug log
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
