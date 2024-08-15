import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Ticket } from 'app/ticket';

@Component({
  selector: 'app-addclientticket',
  templateUrl: './addclientticket.component.html',
  styleUrls: ['./addclientticket.component.scss']
})

export class AddclientticketComponent {
  ticketData = {
    product: '',
    problemDescription: '',
    file: null
  };

  constructor(public dialogRef: MatDialogRef<Ticket>) {}

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.ticketData.file = input.files[0];
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  onSubmit(): void {
    this.dialogRef.close(this.ticketData);
  }
}
