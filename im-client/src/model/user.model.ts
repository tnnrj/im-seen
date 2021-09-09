import { UserRole } from "./enums.model";

export type User = {
  fullName: string;
  username: string;
  userRole: UserRole;
}