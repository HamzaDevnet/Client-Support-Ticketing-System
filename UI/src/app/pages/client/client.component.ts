import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { Users } from 'app/users';

@Component({
  selector: 'app-client',
  templateUrl: './client.component.html',
  styleUrls: ['./client.component.scss']
})
export class ClientComponent implements OnInit {
  clients: Users[] = [];
  displayedColumns: string[] = ['FirstName', 'LastName', 'username', 'MobileNumber', 'email', 'DateOfBirth', 'edit'];
  dataSource = new MatTableDataSource<Users>(this.clients);


  constructor() { }

  ngOnInit(): void {
  }

}
