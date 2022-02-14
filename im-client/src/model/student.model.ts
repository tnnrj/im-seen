export type Student = {
  studentID: number;
  firstName: string;
  lastName: string;
  middleName?: string;
  dob: Date;
  externalID?: number;
  isArchived: boolean;
}