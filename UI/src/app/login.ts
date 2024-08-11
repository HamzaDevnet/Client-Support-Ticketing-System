import { User } from "./ticket.service";
import { Userdata } from "./userdata";
import { Users } from "./users";

export interface Login {
    EmailOrUserName: String ,
    password: String 
}

export interface LoginResponse {
    token: string;
    user: Userdata;
  }
