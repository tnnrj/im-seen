<template>
  <div class="observation-content">
    <template v-if="obs && !saving">
      <div class="p-d-flex observation-panel">
        <Panel header="Student" class="p-mb-1" style="width:80%">
          <template #icons>
            <button v-if="editingStudent" class="p-panel-header-icon p-link" @click="save" v-tooltip="'Save'"><i class="pi pi-check" /></button>
            <button v-else class="p-panel-header-icon p-link" @click="editingStudent = true"><i class="pi pi-pencil" /></button>
          </template>
          <template v-if="editingStudent">
            <AutoComplete v-model="selectedStudent" :suggestions="filteredStudents" @complete="searchStudents($event)" field="fullName" class="autocomplete-stuName">
              <template #item="slotProps">
                    <div class="autocomplete-item">                     
                        <div class="autocomplete-col">{{slotProps.item.fullName}}</div>
                        <div class="autocomplete-col">{{slotProps.item.externalID}}</div>
                    </div>
              </template>
            </AutoComplete>
          </template>
          <template v-else>
            <span>{{(obs.studentFirstName + ' ' + obs.studentLastName).trim()}}</span>
          </template>
          <span v-if="!obs.studentID" class="unidentified-notice">
          </span>
        </Panel>
        <Panel header="Score" class="p-mb-1 p-pl-1" style="width:20%">
          <template #icons>
            <i class="pi pi-question-circle icon-offset question-icon" v-tooltip="'Weighted score takes into account severity, recency, similar nearby observations, and current status to approximate an appropriate level of concern'"/>
          </template>
          <span>{{obs.weightedScore}}</span>
        </Panel>
        <Panel header="Description" class="p-mb-1" style="width:100%" :toggleable="true">
          <span>{{obs.description}}</span>
        </Panel>
        <Panel header="Severity" class="p-mb-1" style="width:20%">
          <span>{{obs.severity}}</span>
        </Panel>
        <Panel header="Status" class="p-mb-1 p-pl-1 p-pr-1" style="width:40%">
          <template #icons>
            <button v-if="editingStatus" class="p-panel-header-icon p-link" @click="save" v-tooltip="'Save'"><i class="pi pi-check" /></button>
            <button v-else class="p-panel-header-icon p-link" @click="editingStatus = true"><i class="pi pi-pencil" /></button>
          </template>
          <template v-if="editingStatus">
            <Dropdown v-model="obs.status" :options="statusOptions" />
          </template>
          <template v-else>
            <span>{{obs.status ?? 'None'}}</span>
          </template>
        </Panel>
        <Panel header="Event" class="p-mb-1" style="width:40%" :toggleable="true">
          <template #icons>
            <button v-if="editingEvent" class="p-panel-header-icon p-link" @click="save" v-tooltip="'Save'"><i class="pi pi-check" /></button>
            <button v-else class="p-panel-header-icon p-link" @click="editingEvent = true"><i class="pi pi-pencil" /></button>
          </template>
          <template v-if="editingEvent">
            <TextArea v-model="obs.event" rows="4" style="width:100%"></TextArea>
          </template>
          <template v-else>
            <span>{{obs.event ?? 'None'}}</span>
          </template>
        </Panel>
        <Panel header="Action" class="p-mb-1" style="width:100%" :toggleable="true">
          <template #icons>
            <button v-if="editingAction" class="p-panel-header-icon p-link" @click="save" v-tooltip="'Save'"><i class="pi pi-check" /></button>
            <button v-else class="p-panel-header-icon p-link" @click="editingAction = true"><i class="pi pi-pencil" /></button>
          </template>
          <template v-if="editingAction">
            <TextArea v-model="obs.action" rows="4" style="width:100%"></TextArea>
          </template>
          <template v-else>
            <span>{{obs.action ?? 'No action taken'}}</span>
          </template>
        </Panel>
      </div>
    </template>
    <template v-else>
      <Loader />
    </template>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, watchEffect } from "vue";
import { Observation } from "@/model/observations.model";
import observationService from "@/services/observations.service";
import { ObservationStatus } from "@/model/enums.model";
import { useStore } from "@/store";
import studentsService from "@/services/students.service";
import { Student } from "@/model/student.model";
import * as _ from "lodash";

export default defineComponent({
  name: "Observation",
  props: {
    observation: Object
  },
  setup(props) {
    const store = useStore();
    const obs = ref<Observation>();
    obs.value = props.observation as Observation;
    watchEffect(() => {
      obs.value = props.observation as Observation;
    });

    const editingStudent = ref<boolean>(false);
    const editingStatus = ref<boolean>(false);
    const editingEvent = ref<boolean>(false);
    const editingAction = ref<boolean>(false);
    const statusOptions = ref<ObservationStatus[]>(Object.keys(ObservationStatus) as ObservationStatus[]);

    const selectedStudent = ref();
    studentsService.getAllStudents().then(students => {
      allStudents.value = students.map(s => { return {...s, fullName: s.firstName + ' ' + s.lastName}; });
      if (obs.value) {
        selectedStudent.value = _.find(allStudents.value, s => s.studentID == obs.value?.studentID);
        if (!selectedStudent.value) {
          selectedStudent.value = {firstName: obs.value?.studentFirstName, lastName: obs.value?.studentLastName, fullName: obs.value?.studentFirstName + ' ' + obs.value?.studentLastName};
        }
      }
    });
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

    const saving = ref<boolean>(false);
    const save = async function () {
      if (obs.value) {
        saving.value = true;
        if (selectedStudent.value) {
          obs.value.studentID = selectedStudent.value.studentID;
          obs.value.studentFirstName = selectedStudent.value.firstName;
          obs.value.studentLastName = selectedStudent.value.lastName;
        }
        try {
          await observationService.saveObservation(obs.value);
          store.dispatch('loadAllObservations');
          editingStudent.value = false;
          editingAction.value = false;
          editingEvent.value = false;
          editingStatus.value = false;
        }
        finally {
          saving.value = false;
        }
      }
    };
    return { obs, editingStudent, editingStatus, statusOptions, editingEvent, editingAction, 
      selectedStudent, filteredStudents, searchStudents, save, saving };
  }
});
</script>

<style lang="scss">
.observation-content {
  height: 100%;
}
.observation-panel {  
  width: 100%;
  height: 100%;  
  flex-flow: row wrap;
}
.p-panel-header {
  padding: .5rem 1rem !important;
}
.p-panel-title {
  height: 2rem;
  padding-top: .5rem;
}
.unidentified-notice {
  font-size: 10pt;
  color: var(--bluegray-700);
  float: right;
  i {
    font-size: 10pt;
  }
}
.autocomplete-item {
  display: flex;
  justify-content: space-between;
}
.pi-question-circle {
  color: var(--blue-500);
}
</style>