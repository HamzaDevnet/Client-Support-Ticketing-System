import { Component, OnInit } from '@angular/core';
import { User } from 'app/ticket.service';
import { Users } from 'app/users';
import { UsersService } from 'app/users.service';
import { SheardServiceService } from 'app/sheard-service.service';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit {

  userProfile: Users ;
  constructor(private UsersService : UsersService , private SheardServiceService : SheardServiceService ) { }

  ngOnInit(): void {
    this.getUserData();
    // return user id or make api for returing user data by token s 
   const x=  this.SheardServiceService.getUserClaims();
   console.log(x);
  }


  getUserData():void{
    this.userProfile = {
      firstName: "Alanoud",
      lastName: "Abdullah",
      fullName: "Alanoud Abdullah",
      mobileNumber: "1234567890",
      email: "john.doe@example.com",
      userStatus: 1,
      dateOfBirth: new Date("1990-01-01T00:00:00"),
      strDateOfBirth: "Monday, January 1, 1990",
      address: "123 Main St, Anytown, USA",
      password : "",
      userId : "" , 
      userImage : null ,
      userName : " "
      
    } 
    // this.UsersService.getUserInfo(id).subscribe({
    //   next: (response)=>{
    //    console.log(response);
    //   }, 
    //   error:(error)=>{
    //     console.log("Error fechting user information",error);
    //   }
    // })
  }

}
