import axios from 'axios'

// altered from https://tinyurl.com/537udhf4
let http: any = null // not possible to create a private property in JavaScript, so we move it outside of the class, so that it's only accessible within this module

class APIProvider {
  constructor (url: string) {
    http = axios.create({
      baseURL: url,
       headers: { 'Content-Type': 'application/json' }
    });
  }

  login(token: string) {
    http.defaults.headers.common.Authorization = `Bearer ${token}`;
  }

  logout() {
    http.defaults.headers.common.Authorization = '';
  }

  get(resource: string, query: any) {
    return http.get(`${resource}`, {
      params: query
    });
  }

  getById(resource: string, id: string, query: any) {
    return http.get(`${resource}/${id}`, {
      params: query
    });
  }

  post(resource: string, data: string, query: any) {
    return http.post(resource, data, {
      params: query
    });
  }
}

export default new APIProvider(process.env.VUE_APP_API_URL);