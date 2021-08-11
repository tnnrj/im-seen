import http from "@/services/base-api.service";
import { AxiosResponse } from "axios";

export default { 
  getChartData
}

export function getChartData(queryId: string): Promise<AxiosResponse<any>> {
  return http.get('Reports/Student-Severity').then(response => {
    response.data.data = response.data;
    response.data.name = 'Students Grouped by Total Report Severity';
    return response;
  });
}