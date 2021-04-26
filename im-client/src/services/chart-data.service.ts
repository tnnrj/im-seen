import http from "@/services/base-api.service";
import { AxiosResponse } from "axios";

export function getChartData(queryId: string) {
  return http.get('ChartData/GetChartData', queryId);
}