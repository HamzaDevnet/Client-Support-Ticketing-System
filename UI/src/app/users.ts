import { UserStatus } from "./enums/user.enum";
export interface Users {
  userId: string ; 
  firstName: string;
  lastName: string;
  userStatus: UserStatus ;
  fullName:string ;
  userName: string;
  mobileNumber: string;
  email: string;
  password: string;
  userImage: null  ;
  dateOfBirth: Date;
  strDateOfBirth: string ;
  address: string ;
  

}

