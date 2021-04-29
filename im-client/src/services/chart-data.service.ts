import http from "@/services/base-api.service";

export function getChartData(queryId: string) {
  return http.get('Reports/Student-Severity').then(response => {
    response.data.chartData = response.data;
    response.data.chartName = 'Students Grouped by Total Report Severity';
    return response;
  });
}