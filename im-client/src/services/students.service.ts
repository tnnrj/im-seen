import { Student } from "@/model/student.model";
import http from "@/services/base-api.service";

export default {
  getMyStudentIds,
  getAllStudents
}

export function getMyStudentIds(): Promise<string[]> {
  return http.get('Students/MyStudents');
}

export function getAllStudents(): Promise<Student[]> {
  return http.get('Students');
}