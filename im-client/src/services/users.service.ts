import { User } from "@/model/user.model";
import http from "@/services/base-api.service";

export default { 
  getAllUsers
}

export async function getAllUsers(): Promise<User[]> {
  return http.get('Users/');
}