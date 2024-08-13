import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { TicketStatus } from 'app/enums/ticket.enum';
import { Ticket } from 'app/ticket';
import { TicketService } from 'app/ticket.service';

@Component({
  selector: 'app-clienttickectdetails',
  templateUrl: './clienttickectdetails.component.html',
  styleUrls: ['./clienttickectdetails.component.scss']
})
export class ClienttickectdetailsComponent  {
  // ticket: Ticket | undefined;
  ticket = {
    ticketId: '123456',
    product: 'Welcome to our Support',
    problemDescription: 'Please let us know how we can help you.',
    status: 'Open',
    createdDate: new Date(),
    assignedToUserName: 'John Doe'
  };

  messages = [
    {
      profileIcon: 'assets/images/client-profile.jpg',
      sender: 'Client',
      text: 'Hi, I have an issue with your product.',
      time: '10:30 AM'
    },
    {
      profileIcon: 'assets/images/support-profile.jpg',
      sender: 'Support',
      text: "Hi, I'm here to assist you. Can you please provide more details about the issue?",
      time: '10:35 AM'
    },
    {
      profileIcon: 'assets/images/client-profile.jpg',
      sender: 'Client',
      text: 'The product is not working as expected. Can you help me troubleshoot?',
      time: '10:40 AM'
    },
    {
      profileIcon: 'assets/images/support-profile.jpg',
      sender: 'Support',
      text: "Okay, let's take a look. Can you please provide the following information: ...",
      time: '10:45 AM'
    }
  ];

  newMessage: string = '';

  sendMessage() {
    if (this.newMessage.trim() !== '') {
      const newMessage = {
        profileIcon: 'assets/images/client-profile.jpg',
        sender: 'Client',
        text: this.newMessage,
        time: '11:00 AM'
      };
      this.messages.push(newMessage);
      console.log('New message added:', newMessage);
      this.newMessage = '';
    }
  }
  getStatusText(status: string): string {
    switch (status) {
      case 'Open':
        return 'Open';
      case 'Closed':
        return 'Closed';
      case 'Pending':
        return 'Pending';
      default:
        return 'Unknown';
    }
  }
}

  // constructor(
    // public dialogRef: MatDialogRef<ClienttickectdetailsComponent>,
    // @Inject(MAT_DIALOG_DATA) public data: { id: string },
    // private ticketService: TicketService
  // ) {}

  // ngOnInit(): void {
    // this.ticketService.getTicketById(this.data.id).subscribe({
    //   next: (ticket) => {
    //     this.ticket = ticket;
    //   },
    //   error: (error) => {
    //     console.error('Error fetching ticket details', error);
    //   },
    // });
  //}

  // onClose(): void {
  //   this.dialogRef.close();
  // }

  // getStatusText(status: TicketStatus): string {
  //   switch (status) {
  //     case TicketStatus.New:
  //       return 'New';
  //     case TicketStatus.Assigned:
  //       return 'Assigned';
  //     case TicketStatus.InProgress:
  //       return 'In Progress';
  //     case TicketStatus.Closed:
  //       return 'Closed';
  //     default:
  //       return 'Unknown';
  //   }
  // }
//}



