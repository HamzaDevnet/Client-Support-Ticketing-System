import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Ticket } from 'app/ticket';
import { CreateTicketDTO , TicketService } from 'app/ticket.service';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-addclientticket',
  templateUrl: './addclientticket.component.html',
  styleUrls: ['./addclientticket.component.scss']
})

export class AddclientticketComponent {
  ticketData = {
    product: '',
    problemDescription: '',
    files: []
  };


  constructor(public dialogRef: MatDialogRef<Ticket> , private ticketService: TicketService  , private ToastrService:ToastrService) {}

  onFileSelected(event: Event): void {
    const reader = new FileReader();
    const target = event.target as HTMLInputElement;
    const files = target.files;

    if (files && files.length > 0) {
      const readFile = (index) => {
        if( index >= files.length ) return;
        var file = files[index];

        reader.onload = () => { 
          this.ticketData.files.push({
            file: reader.result.toString().split('base64,')[1],
            fileName: file.name
          }); 
          readFile(index+1)
        }
        reader.readAsDataURL(file);
      }
      readFile(0);
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  onSubmit(): void {

    const newTicket: CreateTicketDTO = {
      product: this.ticketData.product,
      problemDescription: this.ticketData.problemDescription,
      attachments:this.ticketData.files 
    };
    this.ticketService.addTicket(newTicket).subscribe({
      next: (ticket) => {
        if(ticket.code === 200){
        this.dialogRef.close({isdone:true});
      }else{
        this.ToastrService.error(ticket.message);
      } 
    }, 
      error: (error) => {
        console.error('Error adding ticket', error);
      }
    });
  }
}
