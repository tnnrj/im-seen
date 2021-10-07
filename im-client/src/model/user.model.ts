import { UserRole } from "./enums.model";

export type User = {
  firstName: string;
  lastName: string;
  userName: string;
  email: string;
  jobTitle: string;
  role: UserRole;
}