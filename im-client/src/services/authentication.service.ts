import http from '@/services/base-api.service';

export default {
  login,
  changePassword
}

async function login(form: { username: string; password: string }) {
  return http.post('Authentication/Login', "", form);
}

async function changePassword(form: { firstname: string; lastname: string; job: string; currentPassword: string; password: string }) {
  return http.post('Authentication/ChangePassword', "", form);
}