import http from "@/services/base-api.service";

export default { 
  getChartData
}

export function getChartData(queryId: string) {
  return http.get('Reports/Student-Severity').then(response => {
    response.data.data = response.data;
    response.data.name = 'Students Grouped by Total Report Severity';
    return response;
  });
}