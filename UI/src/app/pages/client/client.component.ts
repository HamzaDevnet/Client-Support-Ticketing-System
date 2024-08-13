import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { Users } from 'app/users';
import { UsersService } from 'app/users.service';
import { EditDialogComponent } from '../edit-dialog/edit-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-client',
  templateUrl: './client.component.html',
  styleUrls: ['./client.component.scss']
})
export class ClientComponent implements OnInit {
  clients: Users[] = [];
  displayedColumns: string[] = ['FirstName', 'LastName', 'username', 'MobileNumber', 'email', 'DateOfBirth', 'address','edit'];
  dataSource = new MatTableDataSource<Users>(this.clients);


  constructor(private UsersService : UsersService, public dialog: MatDialog) { }

  ngOnInit(): void {
    this.getClients();
  }


  getClients():void{
    this.UsersService.getClientsbyManager().subscribe({
      next: (clients)=>{
        this.clients = clients ;
        this.dataSource.data = clients;
        console.log(clients);
      }, 
      error:(error)=>{
        console.log("Error fechting clients for manager",error);
      }
    })
  }
  openEditSupportDialog(client: Users):void{
      const dialogRef = this.dialog.open(EditDialogComponent, {
        width: '600px',
        data: {
          firstName: client.firstName,
          lastName: client.lastName,
          username: client.userName,
          mobileNumber: client.mobileNumber,
          email: client.email,
          password: client.password,
          dateOfBirth: client.dateOfBirth,
          address: client.address
        }
      });
    
      dialogRef.afterClosed().subscribe(result => {
        if (result) {
          client.firstName = result.firstName;
          client.lastName = result.lastName;
          client.userName = result.username;
          client.mobileNumber = result.mobileNumber;
          client.email = result.email;
          client.password = result.password;
          client.dateOfBirth = result.dateOfBirth;
          client.address = result.address;
          this.dataSource.data = [...this.clients];
        }
      });
    }
  
  }
