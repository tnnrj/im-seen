import { Student } from "@/model/student.model";
import http from "@/services/base-api.service";

export default {
  getMyStudentIds,
  getAllStudents,
  getStudent
}

export function getMyStudentIds(): Promise<string[]> {
  return http.get('Students/MyStudents');
}

export function getAllStudents(): Promise<Student[]> {
  return http.get('Students');
}

export function getStudent(id: string): Promise<Student> {
  return http.get('Students/' + id);
}