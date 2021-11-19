import { StudentGroup } from "@/model/student-groups.model";
import http from "@/services/base-api.service";
import * as _ from "lodash";

export default {
  getStudentGroups,
  getStudentsForGroups,
  addStudentToGroup,
  removeStudentFromGroup,
  addGroup,
  deleteGroup,
  getAllSupporters,
  addSupporterToGroup,
  removeSupporterFromGroup
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

export function addGroup(studentGroupName: string, primaryUserName: string) {
  return http.post('StudentGroups', JSON.stringify({studentGroupName, primaryUserName}));
}

export function deleteGroup(studentGroupID: string) {
  return http.delete('StudentGroups', studentGroupID);
}

export function getAllSupporters() {
  return http.get('Supporters');
}

export function addSupporterToGroup(userName: string, studentGroupID: string) {
  return http.post('Supporters', JSON.stringify({userName, studentGroupID}));
}

export function removeSupporterFromGroup(supporterID: string) {
  return http.delete('Supporters', supporterID);
}