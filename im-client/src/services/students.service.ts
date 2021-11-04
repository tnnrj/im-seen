import http from "@/services/base-api.service";

export default {
  getMyStudents
}

export function getMyStudents(): Promise<string[]> {
  return http.get('Students/MyStudents');
}
