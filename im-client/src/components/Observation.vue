<template>
  <div class="observation-content">
    <template v-if="obs && !saving">
      <div class="p-d-flex observation-panel">
        <Panel header="Student" class="p-mb-1" style="width:100%">
          <span>{{(obs.studentFirstName + ' ' + obs.studentLastName).trim()}}</span>
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
        <Panel header="Event" class="p-mb-1" style="width:40%">
          <span>{{obs.event ?? 'None'}}</span>
        </Panel>
        <Panel header="Action" class="p-mb-1" style="width:100%" :toggleable="true">
          <template #icons>
            <button v-if="editingAction" class="p-panel-header-icon p-link" @click="save" v-tooltip="'Save'"><i class="pi pi-check" /></button>
            <button v-else class="p-panel-header-icon p-link" @click="editingAction = true"><i class="pi pi-pencil" /></button>
          </template>
          <template v-if="editingAction">
            <textarea v-model="obs.action" rows="4" style="width:100%"></textarea>
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
import { defineComponent, onUpdated, ref } from "vue";
import Loader from "@/components/Loader.vue";
import { Observation } from "@/model/observations.model";
import observationService from "@/services/observations.service";
import { ObservationStatus } from "@/model/enums.model";
import { useStore } from "@/store";

export default defineComponent({
  name: "Observation",
  components: { Loader },
  props: {
    observation: Object
  },
  setup(props) {
    const store = useStore()
    const obs = ref<Observation>();
    obs.value = props.observation as Observation;
    onUpdated(() => {
      obs.value = props.observation as Observation;
    });

    const editingStudent = ref<boolean>(false);
    const editingStatus = ref<boolean>(false);
    const editingAction = ref<boolean>(false);
    const statusOptions = ref<ObservationStatus[]>(Object.keys(ObservationStatus) as ObservationStatus[]);

    const saving = ref<boolean>(false);
    const save = async function () {
      if (obs.value) {
        saving.value = true;
        try {
          await observationService.saveObservation(obs.value);
          store.dispatch('loadAllObservations');
          editingStudent.value = false;
          editingAction.value = false;
          editingStatus.value = false;
        }
        finally {
          saving.value = false;
        }
      }
    };
    return { obs, editingStudent, editingStatus, statusOptions, editingAction, save, saving };
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
</style>