import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-myopenticket',
  templateUrl: './myopenticket.component.html',
  styleUrls: ['./myopenticket.component.scss']
})
export class MyopenticketComponent implements OnInit {
  displayedColumns: string[] = ['TicketID'];
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
