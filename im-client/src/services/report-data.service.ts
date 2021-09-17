import http from "@/services/base-api.service";
import { AxiosResponse } from "axios";

export default { 
  getReportData
}

export async function getReportData(reportId: string): Promise<AxiosResponse<any>> {
  return http.get('Reports/GetDataForReport', { id: reportId });
}