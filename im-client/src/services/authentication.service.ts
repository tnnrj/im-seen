import http from '@/services/base-api.service';

export default {
  login,
  changePassword
}

async function login(form: { username: string; password: string }) {
  return http.post('Authentication/Login', "", form)
    .then(response => {
      console.log(response.data.token);
      http.login(response.data.token);
    });
}

async function changePassword(form: { firstname: string; lastname: string; job: string; currentPassword: string; password: string }) {
  return http.post('Authentication/ChangePassword', "", form);
}