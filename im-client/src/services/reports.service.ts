import http from "@/services/base-api.service";

export function getReports() {
  return http.get('Reports/Get');
}