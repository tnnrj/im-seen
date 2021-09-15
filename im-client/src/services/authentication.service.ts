import http from '@/services/base-api.service';
import { AxiosResponse } from 'axios';

export default {
  login,
  logout,
  addUser,
  changePassword
}

async function login(form: { username: string; password: string }): Promise<AxiosResponse<any>> {
  return http.post('Authentication/Login', "", form)
    .then(response => {
      http.login(response.data.token);
      return response;
    });
}

async function logout() {
  http.logout();
}

async function addUser(form: { username: string }): Promise<AxiosResponse<any>> {
  return http.post('Authentication/Register', '', form);
}

async function changePassword(form: { firstname: string; lastname: string; job: string; currentPassword: string; password: string }): Promise<AxiosResponse<any>> {
  return http.post('Authentication/ChangePassword', '', form);
}