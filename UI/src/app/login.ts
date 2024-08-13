import { User } from "./ticket.service";
import { Userdata } from "./userdata";
import { Users } from "./users";
import { UserType } from "./enums/user.enum";

export interface Login {
    userName: String ,
    password: String 
}

export interface LoginData{
  token: string ;  
  fullName: string ;
  email: string ;
  userType: UserType ; 
  }


