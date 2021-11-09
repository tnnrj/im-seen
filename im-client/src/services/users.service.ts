import { User } from "@/model/user.model";
import http from "@/services/base-api.service";

export default { 
  getAllUsers,
  updateUser
}

export async function getAllUsers(): Promise<User[]> {
  return http.get('Users/');
}

export async function updateUser(user: User): Promise<void> {
  return http.post('Users/Update', JSON.stringify(user));
}