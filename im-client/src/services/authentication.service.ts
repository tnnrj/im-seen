import http from '@/services/base-api.service';
export default {
  login,
  logout,
  getUser,
  isLoggedIn,
  addUser,
  changePassword
}

async function login(form: { username: string; password: string }): Promise<any> {
  return http.post('Authentication/Login', '', form)
    .then(response => {
      http.login(response.token, response.refreshToken);
      return response;
    });
}

async function logout() {
  http.logout();
}

async function getUser() {
  return http.get('Authentication/User').then(u => {
    return { ...u.user, role: u.role }
  });
}

function isLoggedIn() {
  return http.loggedIn();
}

async function addUser(form: { email: string }): Promise<any> {
  return http.post('Authentication/Register', '', form);
}

async function changePassword(form: { firstname: string; lastname: string; job: string; currentPassword: string; password: string }): Promise<any> {
  return http.post('Authentication/ChangePassword', '', form);
}