import { Observation } from "@/model/observations.model";
import http from "@/services/base-api.service";

export default {
  getObservations
}

export function getObservations(): Promise<Observation[]> {
  return http.get('Observations/');
}
