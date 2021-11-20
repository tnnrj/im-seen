<template>
  <div class="groups-content">
    <template v-if="studentGroupData">
      <DataTable :value="studentGroupData" dataKey="studentGroupID" sortField="studentGroupName" :sortOrder="1" :scrollable="true" scrollHeight="flex">
        <Column field="studentGroupName" header="Group Name" :sortable="true" style="flex: 1 1 45%"></Column>
        <Column field="primaryUserFullName" header="Primary User" :sortable="true" style="flex: 1 1 45%"></Column>
        <Column style="flex: 1 1 10%">
          <template #body="slotProps">
            <button class="p-row-toggler p-link" @click="openGroup(slotProps.data.studentGroupID)">
              <span class="p-row-toggler-icon pi pi-pencil"></span>
            </button>
          </template>
        </Column>
        <template #empty>
          No records to display
        </template>
      </DataTable>
    </template>
    <template v-else>
      <Loader />
    </template>
  </div>
  <Dialog header="Edit Group" v-model:visible="showGroupEditDialog" :modal="true" :contentStyle="{'max-height':'80vh', 'width':'45em'}">
    <div class="p-d-flex group-panel">
      <Panel header="Group Name" class="p-mb-1 p-pr-1" style="width:50%"><template #icons>
          <button v-if="editingName" class="p-panel-header-icon p-link" @click="saveGroup" v-tooltip="'Save'"><i class="pi pi-check" /></button>
          <button v-else class="p-panel-header-icon p-link" @click="editingName = true"><i class="pi pi-pencil" /></button>
        </template>
        <template v-if="editingName">
          <InputText v-model="newGroupName" />
        </template>
        <template v-else>
          <span>{{selectedGroup.studentGroupName}}</span>
        </template>
      </Panel>
      <Panel header="Primary User" class="p-mb-1" style="width:50%"><template #icons>
          <button v-if="editingUser" class="p-panel-header-icon p-link" @click="saveGroup" v-tooltip="'Save'"><i class="pi pi-check" /></button>
          <button v-else class="p-panel-header-icon p-link" @click="editingUser = true"><i class="pi pi-pencil" /></button>
        </template>
        <template v-if="editingUser">
          <AutoComplete v-model="newGroupPrimary" :suggestions="filteredUsers" @complete="searchUsers($event, false)" field="fullName" />
        </template>
        <template v-else>
          <span>{{selectedGroup.primaryUserFullName}}</span>
        </template>
      </Panel>
      <Panel header="Supporting Staff" class="p-mb-1" style="width:100%" :toggleable="true">
        <div class="p-d-flex p-ai-center p-mb-1">
          <AutoComplete v-model="selectedSupporter" :suggestions="filteredUsers" @complete="searchUsers($event, true)" field="fullName" placeholder="Add user to group" />
          <Button v-if="selectedSupporter && selectedSupporter.userName" icon="pi pi-check" class="p-button-rounded p-button-text p-button-success" @click="addSupporterToGroup" />
        </div>
        <div class="p-d-flex p-flex-wrap">
          <div class="p-d-flex p-ai-center p-mr-4" v-for="sup in selectedGroup.supporters" :key="sup.supporterID">
            <span>{{sup.user.fullName}}</span>
            <Button icon="pi pi-times" class="p-button-rounded p-button-text p-button-secondary p-button-sm" @click="removeSupporterFromGroup(sup.supporterID)" />
          </div>
        </div>
      </Panel>
      <Panel header="Students in Group" class="p-mb-1" style="width:100%" :toggleable="true">
        <div class="p-d-flex p-ai-center p-mb-1">
          <AutoComplete v-model="selectedStudent" :suggestions="filteredStudents" @complete="searchStudents($event)" field="fullName" placeholder="Add student to group" />
          <Button v-if="selectedStudent && selectedStudent.studentID" icon="pi pi-check" class="p-button-rounded p-button-text p-button-success" @click="addStudentToGroup" />
        </div>
        <div class="p-d-flex p-flex-wrap">
          <div class="p-d-flex p-ai-center p-mr-4" v-for="del in selectedGroup.delegations" :key="del.delegationID">
            <span>{{del.student.fullName}}</span>
            <Button icon="pi pi-times" class="p-button-rounded p-button-text p-button-secondary p-button-sm" @click="removeStudentFromGroup(del.delegationID)" />
          </div>
        </div>
      </Panel>
    </div>
  </Dialog>
  <Toast />
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import studentGroupsService from "@/services/student-groups.service";
import * as _ from "lodash";
import studentsService from "@/services/students.service";
import { Student } from "@/model/student.model";
import { useToast } from "primevue/usetoast";
import { User } from "@/model/user.model";
import usersService from "@/services/users.service";
import { UserRole } from "@/model/enums.model";

export default defineComponent({
  name: 'Groups',
  setup() {
    const toast = useToast();

    // student group data
    const studentGroupData = ref();
    Promise.all([studentGroupsService.getStudentGroups(),
      studentGroupsService.getStudentsForGroups(),
      studentsService.getAllStudents(),
      studentGroupsService.getAllSupporters(),
      usersService.getAllUsers()])
    .then(data => {
      let studentGroups = data[0];
      let delegations = data[1];
      let students = data[2];
      let supporters = data[3];
      let users = data[4];
      allStudents.value = students.map(s => { return {...s, fullName: s.firstName+' '+s.lastName}; });
      allUsers.value = _.filter(users, u => u.role != UserRole.Observer).map(u => { return {...u, fullName: u.firstName+' '+u.lastName }; })
      studentGroupData.value = studentGroups.map(sg => {
        let sgd = {...sg, primaryUserFullName: undefined, delegations: undefined, supporters: undefined};
        sgd.primaryUserFullName = _.find(allUsers.value, u => u.userName === sg.primaryUserName).fullName;
        sgd.delegations = _.filter(delegations, d => d.studentGroupID === sg.studentGroupID)
          .map(d => { return {...d, student: _.find(allStudents.value, s => s.studentID === d.studentID)} });
        sgd.supporters = _.filter(supporters, s => s.studentGroupID === sg.studentGroupID)
          .map(s => { return {...s, user: _.find(allUsers.value, u => u.userName === s.userName) }; });
        return sgd;
      });
    });

    // group editing
    const showGroupEditDialog = ref<boolean>(false);
    const selectedGroup = ref();
    const editingUser = ref<boolean>(false);
    const editingName = ref<boolean>(false);
    const openGroup = (studentGroupID: string) => {
      selectedGroup.value = _.find(studentGroupData.value, sgd => sgd.studentGroupID === studentGroupID);
      newGroupName.value = selectedGroup.value.studentGroupName;
      newGroupPrimary.value = _.find(allUsers.value, u => u.userName === selectedGroup.value.primaryUserName);
      showGroupEditDialog.value = true;
    }
    const saveGroup = () => {
      if (newGroupName.value !== selectedGroup.value.studentGroupName || newGroupPrimary.value?.userName !== selectedGroup.value.primaryUserName) {
        selectedGroup.value = newGroupName.value;
        selectedGroup.value.primaryUserName = newGroupPrimary.value?.userName;
        selectedGroup.value.primaryUserFullName = newGroupPrimary.value?.firstName+' '+newGroupPrimary.value?.lastName;
        studentGroupsService.updateGroup(selectedGroup.value.studentGroupID, selectedGroup.value.studentGroupName, selectedGroup.value.primaryUserName);
        editingUser.value = false;
        editingName.value = false;
      }
    }
    const addStudentToGroup = () => {
      if (selectedStudent.value) {
        studentGroupsService.addStudentToGroup(selectedStudent.value.studentID, selectedGroup.value.studentGroupID).then(newDelegation => {        
          toast.add({severity:'success', summary:'Success', detail:'Student add succeeded', life:3000});
          let student = _.find(allStudents.value, s => s.studentID === newDelegation.studentID);
          _.find(studentGroupData.value, sgd => sgd.studentGroupID === newDelegation.studentGroupID).delegations.push({...newDelegation, student: student});
        }).catch(() => {
          toast.add({severity:'error', summary:'Error', detail:'Student add failed; please try again later', life:3000});
        });
        selectedStudent.value = undefined;
      }
    }
    const removeStudentFromGroup = (delegationID: string) => {
      const delegation = _.find(selectedGroup.value.delegations, d => d.delegationID === delegationID);
      studentGroupsService.removeStudentFromGroup(delegationID).then(() => {
        toast.add({severity:'success', summary:'Success', detail:'Student remove succeeded', life:3000});
        _.remove(_.find(studentGroupData.value, sgd => sgd.studentGroupID === delegation.studentGroupID).delegations, d => d.delegationID === delegationID);
      }).catch(() => {
        toast.add({severity:'error', summary:'Error', detail:'Student remove failed; please try again later', life:3000});
      });
    }
    const addSupporterToGroup = () => {
      if (selectedSupporter.value) {
        studentGroupsService.addSupporterToGroup(selectedSupporter.value.userName, selectedGroup.value.studentGroupID).then(newSupporter => {        
          toast.add({severity:'success', summary:'Success', detail:'Supporting actor add succeeded', life:3000});
          let user = _.find(allUsers.value, u => u.userName === newSupporter.userName);
          _.find(studentGroupData.value, sgd => sgd.studentGroupID === newSupporter.studentGroupID).supporters.push({...newSupporter, user: user});
        }).catch(() => {
          toast.add({severity:'error', summary:'Error', detail:'Supporting actor add failed; please try again later', life:3000});
        });
        selectedSupporter.value = undefined;
      }
    }
    const removeSupporterFromGroup = (supporterID: string) => {
      const supporter = _.find(selectedGroup.value.supporters, s => s.supporterID === supporterID);
      studentGroupsService.removeSupporterFromGroup(supporterID).then(() => {
        toast.add({severity:'success', summary:'Success', detail:'Supporting actor remove succeeded', life:3000});
        _.remove(_.find(studentGroupData.value, sgd => sgd.studentGroupID === supporter.studentGroupID).supporters, s => s.supporterID === supporterID);
      }).catch(() => {
        toast.add({severity:'error', summary:'Error', detail:'Supporting actor remove failed; please try again later', life:3000});
      });
    }  

    // group adding
    const showGroupAddDialog = ref<boolean>(false);
    const newGroupName = ref<string>();
    const addGroup = () => {
      if (newGroupName.value && newGroupPrimary.value) {
        studentGroupsService.addGroup(newGroupName.value, newGroupPrimary.value.userName);
      }
    }
    const deleteGroup = (id: string) => {
      studentGroupsService.deleteGroup(id);
    }

    // user search
    const newGroupPrimary = ref<User>();
    const selectedSupporter = ref<User>();
    const allUsers = ref<User[]>();
    const filteredUsers = ref<User[]>();
    const searchUsers = (event, supportingOnly: boolean) => {
      if (!allUsers.value) {
        setTimeout(() => searchUsers(event, supportingOnly), 250);
        return;
      }
      setTimeout(() => {
        if (!event.query.trim().length && allUsers.value) {
          filteredUsers.value = [...allUsers.value];
        }
        else {
          filteredUsers.value = _.filter(allUsers.value, u => {
            if (supportingOnly) {
              if (u.role != UserRole.SupportingActor || _.some(selectedGroup.value.supporters, s => s.userName === u.userName)) {
                return false;
              }
            }
            if (!supportingOnly) {
              if (u.role == UserRole.SupportingActor || selectedGroup.value.primaryUserName === u.userName) {
                return false;
              }
            }
            let nameMatches = (`${u.firstName} ${u.lastName}`).toLowerCase().startsWith(event.query.toLowerCase()) 
            || u.lastName.toLowerCase().startsWith(event.query.toLowerCase()) || u.userName.toLowerCase().startsWith(event.query.toLowerCase());
            return nameMatches;
          });
        }
      }, 0);
    }

    // student search
    const selectedStudent = ref();
    const allStudents = ref<Student[]>();
    const filteredStudents = ref<Student[]>();
    const searchStudents = (event) => {
      if (!allStudents.value) {
        setTimeout(() => searchStudents(event), 250);
        return;
      }
      setTimeout(() => {
        if (!event.query.trim().length && allStudents.value) {
          filteredStudents.value = [...allStudents.value];
        }
        else {
          filteredStudents.value = _.filter(allStudents.value, s => {
            if (_.some(selectedGroup.value.delegations, d => d.studentID === s.studentID)) {
              return false;
            }
            let nameMatches = s.fullName.toLowerCase().startsWith(event.query.toLowerCase()) 
              || s.lastName.toLowerCase().startsWith(event.query.toLowerCase());
            return nameMatches;
          });
        }
      }, 0);
    };

    return { studentGroupData, showGroupEditDialog, editingUser, editingName, selectedGroup, openGroup, saveGroup,
      addStudentToGroup, removeStudentFromGroup, addSupporterToGroup, removeSupporterFromGroup,
      showGroupAddDialog, newGroupName, addGroup, deleteGroup,
      newGroupPrimary, selectedSupporter, filteredUsers, searchUsers,
      selectedStudent, filteredStudents, searchStudents };
  }
});
</script>

<style lang="scss">
.groups-content {
  height: 100%;
  padding: 1em;
}
.group-panel {  
  width: 100%;
  height: 100%;
  flex-flow: row wrap;
}
</style>