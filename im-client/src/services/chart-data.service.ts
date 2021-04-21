import http from "@/services/base-api.service";

export function getChartData(queryId: string) {
  // http.get('ChartData/GetChartData', queryId);
  return [queryId, queryId]
}