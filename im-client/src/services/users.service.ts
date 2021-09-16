import { User } from "@/model/user.model";
import http from "@/services/base-api.service";
import { AxiosResponse } from "axios";

export default { 
  getAllUsers
}

export async function getAllUsers(): Promise<AxiosResponse<User[]>> {
  return http.get('Users/');
}