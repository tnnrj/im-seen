import http from "@/services/base-api.service";
import { AxiosResponse } from "axios";

export default { 
  getAllUsers
}

export async function getAllUsers(): Promise<AxiosResponse<any>> {
  return http.get('Users/');
}