import axios, { AxiosInstance } from 'axios';
import { useStore } from '@/store/index';
import router from '@/router';

// altered from https://tinyurl.com/537udhf4
let http: AxiosInstance = axios.create({}); // not possible to create a private property in JavaScript, so we move it outside of the class, so that it's only accessible within this module
let store = useStore();

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
    http.interceptors.response.use(undefined, function (error) {
      if (error) {
        const originalRequest = error.config;
        if (error.response.status === 401 && !originalRequest._retry) {
      
            originalRequest._retry = true;
            store.dispatch('auth/logOut');
            return router.push('/login');
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

  get(resource: string, query: any = null) {
    return http.get(`${resource}`, {
      params: query
    });
  }

  getById(resource: string, id: string, query: any = null) {
    return http.get(`${resource}/${id}`, {
      params: query
    });
  }

  post(resource: string, data: string, query: any = null) {
    return http.post(resource, data, {
      params: query
    });
  }
}

export default new APIProvider(process.env.VUE_APP_API_URL);