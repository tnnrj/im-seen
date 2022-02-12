<template>
  <div class="file-upload-section">
    <file-upload-component />
  </div>
  <div class="students-content p-d-flex">
    <Toast />
    <DataTable class="students-table" :value="students" dataKey="email" :scrollable="true" scrollHeight="flex">
      <Column field="lastName" header="Last Name" :sortable="true" style="flex: 1 1 20%"></Column>
      <Column field="firstName" header="First Name" :sortable="true" style="flex: 1 1 20%"></Column>
      <Column field="externalID" header="External ID" :sortable="true" style="flex: 1 1 20%"></Column>
      <Column field="dob" header="Date Of Birth" :sortable="true" style="flex: 1 1 25%"></Column>      
      <Column field="middleName" header="Middle Name" :sortable="true" style="flex: 1 1 15%"></Column>
      <Column style="flex: 1 1 5%">
        <template #body="slotProps">
          <button class="p-row-toggler p-link" @click="openStudent(slotProps.data.studentID)">
            <span class="p-row-toggler-icon pi pi-pencil"></span>
          </button>
        </template>
      </Column>
      <template #empty>
        <div style="height:100%; width:100%">
          <template v-if="students">
            No records to display
          </template>
          <template v-else>
            <Loader />
          </template>
        </div>
      </template>
    </DataTable>
  </div>
</template>

<script lang="ts">
import { computed, defineComponent, ref } from "vue";
import { useToast } from "primevue/usetoast";
import FileUploadComponent from "@/components/FileUploadComponent.vue";
import { Student } from "@/model/student.model";
import StudentsService from "@/services/students.service";
import * as _ from "lodash";

export default defineComponent({
  name: "Students",
  components: { FileUploadComponent },
  setup() {
    const toast = useToast();

    const students = ref();

    const loadStudents = async () => {
      let data = await StudentsService.getAllStudents();
      students.value = data.map(s => {
        let obj = {
          studentID: s.studentID,
          firstName: s.firstName,
          lastName: s.lastName,
          middleName: s.middleName,
          dob: new Date(s.dob).toLocaleDateString('en-US', {year: 'numeric', month: 'short', day: 'numeric'}),
          externalID: s.externalID,
          isArchived: s.isArchived,
        };
        return obj;
      });
    };
    loadStudents();

    return { students };
  },
});
</script>

<style lang="scss">
.file-upload-section {
  background: white;
  margin: 1em;
  padding: 1em;
}

.students-content {
  height: 100%;
  padding: 1em;
  //width: 100%;
}

.students-table {
  flex: 1;
  margin-right: .5em;
}
</style>