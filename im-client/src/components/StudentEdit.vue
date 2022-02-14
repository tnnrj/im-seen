<template>
  <div class="student-content">
    <template v-if="stu && !saving">
      <div class="p-d-flex student-panel">
        <span class="note" v-if="!isValid">
          First Name, Last Name, External ID are required!
        </span>
        <span class="input p-float-label">
          <InputText id="firstname" v-model="stu.firstName" aria-describedby="firstname-help" required />
          <label for="firstname">First Name *</label>       
        </span>
        <span class="input p-float-label">
          <InputText id="lastname" v-model="stu.lastName" required />
          <label for="lastname">Last Name *</label>
        </span>
        <span class="input p-float-label">
          <InputText id="middlename" v-model="stu.middleName" />
          <label for="middlename">Middle Name</label>
        </span>
        <span class="input p-float-label">
          <Calendar v-model="stu.dob" dateFormat="mm/dd/yy" placeholder="mm/dd/yyyy" />
          <label for="dob">Date of Birth</label>
        </span>
        <span class="input p-float-label">
          <InputNumber id="externalid" v-model="stu.externalID" required />
          <label for="externalid">External ID *</label>
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
        <div class="buttons">
          <ConfirmPopup></ConfirmPopup>
          <div width="100%">
            <Button class="p-button-primary" label="Save" icon="pi pi-check" @click="save" />
          </div>
          <div width="100%" v-if="!isNew">
            <Button class="p-button-danger" label="Delete" icon="pi pi-trash" @click="confirmDel($event)" />
          </div>
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
import { useConfirm } from "primevue/useconfirm";
import InputNumber from 'primevue/inputnumber';
import * as _ from "lodash";
import Calendar from 'primevue/calendar';
import { Student } from "@/model/student.model";
import StudentsService from "@/services/students.service";

export default defineComponent({
  name: "StudentEdit",
  components: { Calendar, InputNumber },
  props: {
    student: Object,
  },
  setup(props, { emit }) {
    const confirm = useConfirm();

    const stu = ref<Student>();
    const isNew = ref<boolean>(false);
    const isValid = ref<boolean>(true);

    // gets student object from input
    if (props.student) {
      isNew.value = false;
      stu.value = props.student as Student;
      watchEffect(() => {
        stu.value = props.student as Student;
      });
    } else {
      isNew.value = true;
      stu.value = {
        studentID: 0,
        firstName: "",
        lastName: "",
        dob: new Date(),
        isArchived: false
      };
    }
    
    const saving = ref<boolean>(false);

    const updateUI = () => {
      saving.value = false;
      // emits event to parent to close dialog
      emit('exit');
      // tells datatable to refresh data
      emit('refresh');
    };

    const validate = () => {
      if (stu.value?.firstName == "" || stu.value?.lastName == "" || stu.value?.externalID == null) {
        isValid.value = false;
        return false;
      }
      isValid.value = true;
      return true;
    };

    const save =  () => {
      if (stu.value && validate()) {
        saving.value = true;
        stu.value.dob = new Date(stu.value.dob);
        
        if (isNew.value) {

          StudentsService.newStudent(stu.value).then(
            () => {
             updateUI();
            }
          );
        } else {
          StudentsService.updateStudent(stu.value.studentID.toString(), stu.value).then(
            () => {
             updateUI();
            }
          );
        }       
      }
    };

    const remove = () => {
      if (stu.value) {
        saving.value = true;
        
        StudentsService.deleteStudent(stu.value.studentID.toString()).then(
          () => {
             updateUI();
            }
        );
      }
    };

    const confirmDel = (event) => {
            confirm.require({
                target: event.currentTarget,
                message: 'Do you want to delete this record?',
                icon: 'pi pi-info-circle',
                acceptClass: 'p-button-danger',
                accept: () => {
                    remove();
                },
                reject: () => {
                    //
                }
            });
        }

    const archivedOptions = [
      { name: "Yes", value: true },
      { name: "No", value: false },
    ];

    return { stu, isNew, isValid, save, confirmDel, saving, archivedOptions};
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

  input,.p-inputnumber,.p-calendar {
    width: 90%;
  }
}
.note {
  width: 100%;
  font-style: italic;
  color: orangered;
}
.buttons {
  width: 100%;
  display: flex;
  justify-content: right;

  .p-button-danger {
    margin-left: 1em;
  }
}
</style>