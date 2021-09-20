import axios, { AxiosInstance } from 'axios';

// altered from https://tinyurl.com/537udhf4
let http: AxiosInstance = axios.create({}); // not possible to create a private property in JavaScript, so we move it outside of the class, so that it's only accessible within this module

class APIProvider {
  constructor (url: string) {
    // singleton axios instance
    http = axios.create({
      baseURL: url,
      headers: { 'Content-Type': 'application/json' },
      // for cookie auth
      // withCredentials: true
    });
    // login redirect on 401 unauth
    http.interceptors.response.use(undefined, async function (error) {
      if (error) {
        const originalRequest = error.config;
        if (error.response.status === 401 && !originalRequest._retry) {
          http.defaults.headers.common.Authorization = '';
          localStorage.removeItem('token');
          window.location.href = 'login'
        }

        return Promise.reject(error);
      }
    });
    // check for existing auth
    let token = localStorage.getItem('token');
    if (token) {
      http.defaults.headers.common['Authorization'] = `Bearer ${token}`;
    }
  }

  // for token auth (call from store)
  login(token: string) {
    http.defaults.headers.common.Authorization = `Bearer ${token}`;
    localStorage.setItem('token', token);
  }

  logout() {
    http.defaults.headers.common.Authorization = '';
    localStorage.removeItem('token');
  }

  loggedIn() {
    return !!http.defaults.headers.common.Authorization;
  }

  get(resource: string, query: any = null) {
    return http.get(`${resource}`, {
      params: query
    }).then(response => {
      return response.data;
    }, err => {
      return err;
    });
  }

  getById(resource: string, id: string, query: any = null) {
    return http.get(`${resource}/${id}`, {
      params: query
    }).then(response => {
      return response.data;
    }, err => {
      return err;
    });
  }

  post(resource: string, data: string, query: any = null) {
    return http.post(resource, data, {
      params: query
    }).then(response => {
      return response.data;
    }, err => {
      return err;
    });
  }
}

export default new APIProvider(process.env.VUE_APP_API_URL);