<template>
  <div class="student-content">
    <template v-if="stu && !saving">
      <div class="p-d-flex student-panel">
        <span class="input p-float-label">
          <InputText id="firstname" v-model="stu.firstName" required />
          <label for="firstname">First Name</label>
        </span>
        <span class="input p-float-label">
          <InputText id="lastname" v-model="stu.lastName" required />
          <label for="lastname">Last Name</label>
        </span>
        <span class="input p-float-label">
          <InputText id="middlename" v-model="stu.middleName" />
          <label for="middlename">Middle Name</label>
        </span>
        <span class="input p-float-label">
          <!-- <InputText id="dob" v-model="stu.dob" /> -->
          <Calendar v-model="stu.dob" dateFormat="mm/dd/yy" placeholder="mm/dd/yyyy" />
          <label for="dob">Date of Birth</label>
        </span>
        <span class="input p-float-label">
          <InputText id="externalid" v-model="stu.externalID" required />
          <label for="externalid">External ID</label>
        </span>
        <span class="input">
          <label for="isArchived">Archived?</label>
          <Dropdown
            id="isArchived"
            :options="archivedOptions"
            v-model="stu.isArchived"
            optionLabel="name"
            optionValue="value"
          />
          
        </span>
        <div width="100%">
          <Button class="p-button-primary" label="Save" @click="save" />
        </div>
      </div>
    </template>
    <template v-else>
      <Loader />
    </template>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, watchEffect } from "vue";
import * as _ from "lodash";
import Calendar from 'primevue/calendar';
import { Student } from "@/model/student.model";
import StudentsService from "@/services/students.service";

export default defineComponent({
  name: "StudentEdit",
  components: { Calendar },
  props: {
    student: Object,
  },
  setup(props, { emit }) {
    const stu = ref<Student>();
    // gets student object from input
    stu.value = props.student as Student;
    watchEffect(() => {
      stu.value = props.student as Student;
    });
 
    const saving = ref<boolean>(false);

    const save =  () => {
      if (stu.value) {
        saving.value = true;
        stu.value.dob = new Date(stu.value.dob);
        
        StudentsService.updateStudent(stu.value.studentID, stu.value).then(
          () => {
             saving.value = false;
             // emits event to parent to close dialog
             emit('exit');
            }
        );
      }
    };

    const archivedOptions = [
      { name: "Yes", value: true },
      { name: "No", value: false },
    ];

    return { stu, save, saving, archivedOptions};
  },
});
</script>

<style lang="scss">
.student-content {
  height: 100%;
}
.student-panel {
  width: 100%;
  height: 100%;
  flex-flow: row wrap;
  text-align: left;
}
#isArchived {
  margin-left: 2em;
}
.input {
  width: 50%;
  margin: 1.25em 0;

  input {
    width: 90%;
  }
}
</style>