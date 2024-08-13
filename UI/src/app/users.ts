import { UserStatus } from "./enums/user.enum";
export interface Users {
  userId: string ; 
  firstName: string;
  lastName: string;
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

