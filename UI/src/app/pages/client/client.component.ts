import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { Users } from 'app/users';
import { UsersService } from 'app/users.service';
import { EditDialogComponent } from '../edit-dialog/edit-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { UserStatus } from 'app/enums/user.enum';

@Component({
  selector: 'app-client',
  templateUrl: './client.component.html',
  styleUrls: ['./client.component.scss']
})
export class ClientComponent implements OnInit {
  clients: Users[] = [];
  UserStatus = UserStatus;
  displayedColumns: string[] = ['FirstName', 'LastName', 'MobileNumber', 'address','Deactivate','edit'];
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
  openEditClientDialog(client: Users): void {
    const dialogRef = this.dialog.open(EditDialogComponent, {
      width: '600px',
      data: client
    });
  
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.getClients();
      }
    });
  }

  deactivateclient(client:Users):void{
    this.UsersService.deactivatecUser(client.userId).subscribe({
      next: (response)=>{
        if(response.data === true){
          this.getClients();
        }

      }
    })
  }

  activateclient(client:Users):void{
    this.UsersService.activateUser(client.userId).subscribe({
      next: (response)=>{
        if(response.data === true){
          this.getClients();
        }

      }
    })
  }
  
  }


  // if (this.supportForm.valid) {
  //   this.SupportTeamService.addSupportMember(this.supportForm.value).subscribe({
  //     next:(response)=>{
  //       this.dialogRef.close({
  //         isdone:true
  //       });