import { Report } from "@/model/reports.model";
import http from "@/services/base-api.service";

export default { 
  getReportData,
  getAllReports
}

export async function getReportData(reportID: string): Promise<any> {
  return http.get('Reports/GetDataForReport', { id: reportID });
}

export async function getAllReports(): Promise<Report[]> {
  return http.get('Reports');
}