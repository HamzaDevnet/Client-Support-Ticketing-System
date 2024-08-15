import { UserStatus } from "./enums/user.enum";
export interface Users {
  userId: string;
  firstName: string;
  lastName: string;
  userStatus: UserStatus;
  fullName: string;
  userName: string;
  mobileNumber: string;
  email: string;
  password: string;
  dateOfBirth: Date;
  strDateOfBirth: string;
  image:  string | null;
  address: string;
}
