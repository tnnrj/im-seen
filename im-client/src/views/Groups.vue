<template>
  <Toast />
</template>

<script lang="ts">
import Loader from "@/components/Loader.vue";
import { defineComponent, ref } from "vue";
import studentGroupsService from "@/services/student-groups.service";
import * as _ from "lodash";
import studentsService from "@/services/students.service";
import { Student } from "@/model/student.model";
import { useToast } from "primevue/usetoast";

export default defineComponent({
  name: 'Groups',
  components: { Loader },
  setup() {
    const toast = useToast();

    const studentGroupData = ref();
    Promise.all([studentGroupsService.getStudentGroups(), studentGroupsService.getStudentsForGroups(), studentsService.getAllStudents()])
    .then(data => {
      let studentGroups = data[0];
      let delegations = data[1];
      let students = data[2];
      studentGroupData.value = studentGroups.map(sg => {
        let sgd = {...sg, delegations: undefined};
        sgd.delegations = _.filter(delegations, d => d.studentGroupID === sg.studentGroupID)
          .map(d => { return {...d, student: _.find(students, s => s.studentID === d.studentID)} });
        return sgd;
      });
      allStudents.value = students.map(s => { return {...s, fullName: s.firstName + ' ' + s.lastName}; });
    });

    const selectedGroup = ref();
    const addStudentToGroup = () => {
      studentGroupsService.addStudentToGroup(selectedStudent.value.studentID, selectedGroup.value.studentGroupID).then(newDelegation => {        
        toast.add({severity:'success', summary:'Success', detail:'Student add succeeded', life:3000});
        _.find(studentGroupData.value, sgd => sgd.studentGroupID === newDelegation.studentGroupID).delegations.push(newDelegation);
      }).catch(() => {
        toast.add({severity:'error', summary:'Error', detail:'Student add failed; please try again later', life:3000});
      });
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

    return { studentGroupData, selectedGroup, addStudentToGroup, removeStudentFromGroup,
      selectedStudent, filteredStudents, searchStudents };
  }
});
</script>

<style>

</style>