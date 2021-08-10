import http from "@/services/base-api.service";

export default {
  getReports
}

export function getReports() {
  return http.get('Reports/');
}
