import { StudentGroup } from "@/model/student-groups.model";
import http from "@/services/base-api.service";
import * as _ from "lodash";

export default {
  getStudentGroups,
  getStudentsForGroups,
  addStudentToGroup,
  removeStudentFromGroup
}

export function getStudentGroups(): Promise<StudentGroup[]> {
  return http.get('StudentGroups');
}

export function getStudentsForGroups() {
  return http.get('Delegations');
}

export function addStudentToGroup(studentID: string, studentGroupID: string) {
  return http.post('Delegations', JSON.stringify({studentID, studentGroupID}));
}

export function removeStudentFromGroup(delegationID: string) {
  return http.delete('Delegations', delegationID);
}