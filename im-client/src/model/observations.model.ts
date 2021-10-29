import { ObservationStatus } from "./enums.model";

export class Observation  {
  observationID: string;
  studentID: string;
  studentFirstName: string;
  studentLastName: string;
  description: string;
  observationDate: Date;
  severity: number;
  action: string;
  event: string;
  status: ObservationStatus;

  studentFullName(): string {
    return (this.studentFirstName + ' ' + this.studentLastName).trim();
  }
}
