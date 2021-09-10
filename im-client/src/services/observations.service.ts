import { Observation } from "@/model/observations.model";
import http from "@/services/base-api.service";
import { AxiosResponse } from "axios";

export default {
  getObservations
}

export function getObservations(): Promise<AxiosResponse<Observation[]>> {
  return http.get('Observations/');
}
