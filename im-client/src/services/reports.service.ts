import { Report } from "@/model/reports.model";
import http from "@/services/base-api.service";
import { AxiosResponse } from "axios";

export default {
  getReports
}

export function getReports(): Promise<AxiosResponse<Report[]>> {
  return http.get('Reports/');
}
