<template>
  <div class="students-content p-d-flex">
    <Toast />
    <DataTable
      class="students-table"
      :value="students"
      dataKey="studentID"
      :scrollable="true"
      scrollHeight="flex"
      :paginator="true"
      :rows="25"
      paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown"
      :rowsPerPageOptions="[10, 25, 50]"
      currentPageReportTemplate="Showing {first} to {last} of {totalRecords} entries"
      responsiveLayout="scroll"
    >
      <template #header>
        <div class="table-header">
          <div>
            <Button class="p-button-primary" label="New Student" icon="pi pi-user-plus" @click="openStudent(null)" />
          </div>
          <div>
            <Button class="p-button-primary" label="Bulk Upload" icon="pi pi-upload" @click="openUpload()" />
          </div>
          <!-- <span class="p-input-icon-left">
            <i class="pi pi-search" />
            <InputText
              
              placeholder="Keyword Search"
            />
          </span> -->
        </div>
      </template>
      <Column
        field="lastName"
        header="Last Name"
        :sortable="true"
        style="flex: 1 1 20%"
      ></Column>
      <Column
        field="firstName"
        header="First Name"
        :sortable="true"
        style="flex: 1 1 20%"
      ></Column>
      <Column
        field="externalID"
        header="External ID"
        :sortable="true"
        style="flex: 1 1 20%"
      ></Column>
      <Column
        field="dob"
        header="Date Of Birth"
        dataType="date"
        :sortable="true"
        style="flex: 1 1 25%"
      >
        <template #body="{ data }">
          {{ formatDate(data.dob) }}
        </template>
      </Column>
      <Column
        field="middleName"
        header="Middle Name"
        :sortable="true"
        style="flex: 1 1 15%"
      ></Column>
      <Column style="flex: 1 1 5%">
        <template #body="slotProps">
          <button
            class="p-row-toggler p-link"
            @click="openStudent(slotProps.data.studentID)"
          >
            <span class="p-row-toggler-icon pi pi-pencil"></span>
          </button>
        </template>
      </Column>
      <template #empty>
        <div style="height: 100%; width: 100%">
          <template v-if="students"> No records to display </template>
          <template v-else>
            <Loader />
          </template>
        </div>
      </template>
    </DataTable>
  </div>
  <Dialog
    header="Student"
    v-model:visible="showStudentDialog"
    :modal="true"
    :contentStyle="{ width: '45em', 'max-height': '80vh' }"
  >
    <StudentEditComponent
      :student="currentStudent"
      @exit="closeStudentDialog"
      @refresh="loadStudents"
    />
  </Dialog>
  <Dialog
    header="Bulk Upload"
    v-model:visible="showUploadDialog"
    :modal="true"
    :closeOnEscape="false"
    :contentStyle="{ width: '45em', 'max-height': '80vh' }"
  >
    <file-upload-component />
  </Dialog>
</template>

<script lang="ts">
import { computed, defineComponent, ref } from "vue";
import { useToast } from "primevue/usetoast";
import FileUploadComponent from "@/components/FileUploadComponent.vue";
import StudentEditComponent from "@/components/StudentEdit.vue";
import { Student } from "@/model/student.model";
import StudentsService from "@/services/students.service";
import * as _ from "lodash";

export default defineComponent({
  name: "Students",
  components: { FileUploadComponent, StudentEditComponent },
  setup() {
    const toast = useToast();

    const students = ref();

    // loads students list
    const loadStudents = async () => {
      await StudentsService.getAllStudents().then((data) => {
        students.value = data;
        students.value.forEach((stu) => (stu.dob = new Date(stu.dob)));
      });
    };
    loadStudents();

    // handles student edit dialog
    const currentStudent = ref<Student>();
    const showStudentDialog = ref<boolean>(false);
    const openStudent = (studentID) => {
      currentStudent.value = _.find(
        students.value,
        (o) => o.studentID === studentID
      );
      showStudentDialog.value = true;
    };

    const formatDate = (value) => {
      return value.toLocaleDateString("en-US", {
        day: "2-digit",
        month: "2-digit",
        year: "numeric",
      });
    };

    const closeStudentDialog = () => {
      showStudentDialog.value = false;
    };

    // handles Bulk Upload dialog
    const showUploadDialog = ref<boolean>(false);
    const openUpload = () => {
      showUploadDialog.value = true;
    };

    return {
      students,
      currentStudent,
      showStudentDialog,
      openStudent,
      formatDate,
      closeStudentDialog,
      loadStudents,
      openUpload,
      showUploadDialog
    };
  },
});
</script>

<style lang="scss">

.students-content {
  height: 100%;
  padding: 1em;
  //width: 100%;
}

.students-table {
  flex: 1;
  margin-right: 0.5em;
}

.table-header {
  width: 100%;
  display: flex;
  justify-content: space-between;
  align-items: center;
}
</style>