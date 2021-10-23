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
    this.addErrorHandler(undefined);
    // check for existing auth
    let token = localStorage.getItem('token');
    if (token) {
      http.defaults.headers.common['Authorization'] = `Bearer ${token}`;
    }
  }

  // for token auth (call from store)
  login(token: string, refreshToken = '') {
    http.defaults.headers.common.Authorization = `Bearer ${token}`;
    localStorage.setItem('token', token);
    if (refreshToken) {
      localStorage.setItem('refreshToken', refreshToken);
    }
  }

  logout() {
    http.defaults.headers.common.Authorization = '';
    localStorage.removeItem('token');
    localStorage.removeItem('refreshToken');
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
      throw err;
    });
  }

  getById(resource: string, id: string, query: any = null) {
    return http.get(`${resource}/${id}`, {
      params: query
    }).then(response => {
      return response.data;
    }, err => {
      throw err;
    });
  }

  post(resource: string, data: string, query: any = null) {
    return http.post(resource, data, {
      params: query
    }).then(response => {
      return response.data;
    }, err => {
      throw err;
    });
  }

  addErrorHandler(callbackFn) {
    let $this = this;
    http.interceptors.response.use(undefined, async function (error) {
      if (error) {
        const originalRequest = error.config;
        if (error.response.status === 401 && originalRequest.url.includes("Tokens/refresh")) {
          $this.logout();
          window.location.href = 'login';
        }
        if (error.response.status === 401 && !originalRequest._retry) {
          let accessToken = localStorage.getItem('token');
          let refreshToken = localStorage.getItem('refreshToken');
          $this.logout();
          if (accessToken && refreshToken) {
            originalRequest._retry = true;
            return $this.post("Tokens/refresh", '', { accessToken: accessToken, refreshToken: refreshToken })
              .then(response => {         
              $this.login(response.token, response.refreshToken);
              return http(originalRequest);
            }, () => {
              window.location.href = 'login';
              return Promise.reject(error);
            });
          }
          else {
            window.location.href = 'login';
          }
        }
        else if (callbackFn) {
          callbackFn(error);
        }
        
        return Promise.reject(error);
      }
    })
  }
}

export default new APIProvider(process.env.VUE_APP_API_URL);