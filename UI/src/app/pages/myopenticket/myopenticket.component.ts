import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { Ticket } from 'app/ticket';

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

  ngOnInit(): void {
    
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
