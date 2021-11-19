<template>
  <template v-if="studentGroupData">
    <DataTable class="groups-table" :value="studentGroupData" dataKey="studentGroupID" :scrollable="true" scrollHeight="flex">
      <Column field="studentGroupName" header="Group Name" :sortable="true" style="flex: 1 1 45%"></Column>
      <Column field="primaryUserName" header="Primary User" :sortable="true" style="flex: 1 1 45%"></Column>
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
  <Dialog header="Edit Group" v-model:visible="showGroupEditDialog" :modal="true" :contentStyle="{'max-height':'80vh', 'width':'45em'}">
    <div class="p-d-flex group-panel">
      <Panel header="Group Name" class="p-mb-1" style="width:50%" :toggleable="true"><template #icons>
          <button v-if="editingUser" class="p-panel-header-icon p-link" @click="save" v-tooltip="'Save'"><i class="pi pi-check" /></button>
          <button v-else class="p-panel-header-icon p-link" @click="editingUser = true"><i class="pi pi-pencil" /></button>
        </template>
        <template v-if="editingUser">
          <InputText v-model="newGroupName" />
        </template>
        <template v-else>
          <span>{{selectedGroup.studentGroupName}}</span>
        </template>
      </Panel>
      <Panel header="Primary User" class="p-mb-1" style="width:50%"><template #icons>
          <button v-if="editingUser" class="p-panel-header-icon p-link" @click="save" v-tooltip="'Save'"><i class="pi pi-check" /></button>
          <button v-else class="p-panel-header-icon p-link" @click="editingUser = true"><i class="pi pi-pencil" /></button>
        </template>
        <template v-if="editingUser">
          <AutoComplete v-model="newGroupPrimary" :suggestions="filteredUsers" @complete="searchUsers($event)" field="userName" />
        </template>
        <template v-else>
          <span>{{selectedGroup.primaryUserName}}</span>
        </template>
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

export default defineComponent({
  name: 'Groups',
  setup() {
    const toast = useToast();

    // student group data
    const studentGroupData = ref();
    Promise.all([studentGroupsService.getStudentGroups(), studentGroupsService.getStudentsForGroups(), studentsService.getAllStudents(), studentGroupsService.getAllSupporters()])
    .then(data => {
      let studentGroups = data[0];
      let delegations = data[1];
      let students = data[2];
      let supporters = data[3];
      studentGroupData.value = studentGroups.map(sg => {
        let sgd = {...sg, delegations: undefined, supporters: undefined};
        sgd.delegations = _.filter(delegations, d => d.studentGroupID === sg.studentGroupID)
          .map(d => { return {...d, student: _.find(students, s => s.studentID === d.studentID)} });
        sgd.supporters = _.filter(supporters, s => s.studentGroupID === sg.studentGroupID);
        return sgd;
      });
      allStudents.value = students.map(s => { return {...s, fullName: s.firstName + ' ' + s.lastName}; });
    });

    // group editing
    const showGroupEditDialog = ref<boolean>(false);
    const selectedGroup = ref();
    const openGroup = (studentGroupID: string) => {
      selectedGroup.value = _.find(studentGroupData.value, sgd => sgd.studentGroupID === studentGroupID);
      showGroupEditDialog.value = true;
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
      }
    }
    const removeStudentFromGroup = (studentID: string) => {
      const delegation = _.find(selectedGroup.value.delegations, d => d.studentID === studentID);
      studentGroupsService.removeStudentFromGroup(delegation.delegationID).then(() => {
        toast.add({severity:'success', summary:'Success', detail:'Student remove succeeded', life:3000});
        _.remove(_.find(studentGroupData.value, sgd => sgd.studentGroupID === delegation.studentGroupID).delegations, d => d.delegationID === delegation.delegationID);
      }).catch(() => {
        toast.add({severity:'error', summary:'Error', detail:'Student remove failed; please try again later', life:3000});
      });
    }
    const addSupporterToGroup = () => {
      if (selectedSupporter.value) {
        studentGroupsService.addSupporterToGroup(selectedSupporter.value.userName, selectedGroup.value.studentGroupID).then(newSupporter => {        
          toast.add({severity:'success', summary:'Success', detail:'Supporting actor add succeeded', life:3000});
          _.find(studentGroupData.value, sgd => sgd.studentGroupID === newSupporter.studentGroupID).supporters.push(newSupporter);
        }).catch(() => {
          toast.add({severity:'error', summary:'Error', detail:'Supporting actor add failed; please try again later', life:3000});
        });
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
    usersService.getAllUsers().then(u => {
      allUsers.value = u;
    });
    const filteredUsers = ref<User[]>();
    const searchUsers = (event) => {
      if (!allUsers.value) {
        setTimeout(() => searchUsers(event), 250);
        return;
      }
      setTimeout(() => {
        if (!event.query.trim().length && allUsers.value) {
          filteredUsers.value = [...allUsers.value];
        }
        else {
          filteredUsers.value = _.filter(allUsers.value, u => (`${u.firstName} ${u.lastName}`).toLowerCase().startsWith(event.query.toLowerCase()) 
            || u.lastName.toLowerCase().startsWith(event.query.toLowerCase()) || u.userName.toLowerCase().startsWith(event.query.toLowerCase()));
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
          filteredStudents.value = _.filter(allStudents.value, s => s.fullName.toLowerCase().startsWith(event.query.toLowerCase()) 
            || s.lastName.toLowerCase().startsWith(event.query.toLowerCase()));
        }
      }, 0);
    };

    return { studentGroupData, showGroupEditDialog, selectedGroup, openGroup,
      addStudentToGroup, removeStudentFromGroup, addSupporterToGroup, removeSupporterFromGroup,
      showGroupAddDialog, newGroupName, addGroup, deleteGroup,
      newGroupPrimary, selectedSupporter, filteredUsers, searchUsers,
      selectedStudent, filteredStudents, searchStudents };
  }
});
</script>

<style lang="scss">
.group-panel {  
  width: 100%;
  height: 100%;  
  flex-flow: row wrap;
}
</style>