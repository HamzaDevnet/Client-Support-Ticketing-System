import { Component, OnInit } from '@angular/core';
import { User } from 'app/ticket.service';
import { Users } from 'app/users';
import { UsersService } from 'app/users.service';
import { SheardServiceService } from 'app/sheard-service.service';
import { environment } from 'environments/environment';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit {

  userProfile: Users ;
  backendURL : string = environment.BackendURL;
  constructor(private UsersService : UsersService ,
     private SheardServiceService : SheardServiceService ) { }

  ngOnInit(): void {
   const userId=  this.SheardServiceService.getUserId();
   this.getUserData(userId);
  }


  getUserData(userId: string):void{
    // this.userProfile = {
    //   firstName: "Alanoud",
    //   lastName: "Abdullah",
    //   fullName: "Alanoud Abdullah",
    //   mobileNumber: "1234567890",
    //   email: "john.doe@example.com",
    //   userStatus: 1,
    //   dateOfBirth: new Date("1990-01-01T00:00:00"),
    //   strDateOfBirth: "Monday, January 1, 1990",
    //   address: "123 Main St, Anytown, USA",
    //   password : "",
    //   userId : "" , 
    //   userImage : null ,
    //   userName : " "
      
    // } 
    this.UsersService.getUserInfo(userId).subscribe({
      next: (response)=>{
       console.log(response);
       this.userProfile = response.data; 
      }, 
      error:(error)=>{
        console.log("Error fechting user information",error);
      }
    })
  }

}
