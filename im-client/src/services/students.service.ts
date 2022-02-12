import { Student } from "@/model/student.model";
import http from "@/services/base-api.service";

export default {
  getMyStudentIds,
  getAllStudents,
  getStudent,
  sendStudentsCSV,
  getFileCSV
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

export async function sendStudentsCSV(file: FormData): Promise<any> {
  return http.sendFile('Students/CSVBulkUpload', file);
}

export async function getFileCSV(): Promise<any> {
  return http.get('Students/CSVDownload', { responseType: 'blob' });
}