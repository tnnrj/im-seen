import { Observation } from "@/model/observations.model";
import http from "@/services/base-api.service";

export default {
  getObservations,
  saveObservation
}

export function getObservations(): Promise<Observation[]> {
  return http.get('Observations/');
}

export function saveObservation(observation: Observation): Promise<any> {
  return http.post('Observations/', '', observation);
}
