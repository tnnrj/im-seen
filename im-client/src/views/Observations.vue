<template>
  <div class="observations-content">
    <DataTable :value="observations" sortField="observationDate" :sortOrder="-1" dataKey="observationID"
        v-model:expandedRows="expandedRows" @row-expand="onRowExpand" @row-collapse="onRowCollapse" :scrollable="true" scrollHeight="flex">
      <Column :expander="true" style="flex: 1 1 5%"></Column>
      <Column field="studentName" header="Name" :sortable="true" style="flex: 1 1 15%"></Column>
      <Column field="severity" header="Severity" :sortable="true" style="flex: 1 1 10%"></Column>
      <Column field="description" header="Description" style="flex: 1 1 50%">
        <template #body="slotProps">
          <span :class="{ 'clamp-text': !slotProps.data.expanded }">{{slotProps.data.description}}</span>
        </template>
      </Column>
      <Column field="observationDate" header="Submit Date" :sortable="true" style="flex: 1 1 15%">
        <template #body="slotProps">{{slotProps.data.observationDate.toLocaleString('en-US')}}</template>
      </Column>
      <Column style="flex: 1 1 5%">
        <template #body="slotProps">
          <button class="p-row-toggler p-link" @click="openObs(slotProps.data.observationID)">
            <span class="p-row-toggler-icon pi pi-window-maximize"></span>
          </button>
        </template>
      </Column>
      <template #empty>
        <div style="height:100%; width:100%">
          <template v-if="observations">
            No records to display
          </template>
          <template v-else>
            <Loader />
          </template>
        </div>
      </template>
    </DataTable>
    <Dialog header="Observation" v-model:visible="showObsDialog" :modal="true" :contentStyle="{ 'width': '45em', 'max-height': '80vh' }">
      <ObservationComponent :observation="currentObs" />
    </Dialog>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, watchEffect } from "vue";
import Loader from "@/components/Loader.vue";
import { useStore } from "@/store";
import { Observation } from '@/model/observations.model';
import ObservationComponent from '@/components/Observation.vue';
import * as _ from 'lodash';

interface UiObservation extends Observation {
  studentName: string;
  fullDescription: string;
  expanded: boolean;
}

export default defineComponent({
  name: 'Observations',
  components: { Loader, ObservationComponent },
  setup() {
    const store = useStore();

    const observations = ref();
    watchEffect(() => {
      if (!store.state.observations) store.dispatch('loadAllObservations'); // here so we enforce watch dependency
      setTimeout(() => { // needs to happen async so we show loader immediately
        observations.value = store.state.observations?.map(r => {
          let uir = r as UiObservation;
          uir.studentName = (r.studentFirstName + ' ' + r.studentLastName).trim();
          uir.observationDate = new Date(r.observationDate);
          uir.fullDescription = r.description;
          uir.expanded = false;
          return uir;
        });
      }, 0);
    });
    
    const expandedRows = ref<any>([]);
    const onRowExpand = (event) => {
      event.data.expanded = true;
    }
    const onRowCollapse = (event) => {
      event.data.expanded = false;
    }

    const showObsDialog = ref<boolean>(false);
    const currentObs = ref<Observation>();
    const openObs = (id) => {
      currentObs.value = _.find(observations.value, o => o.observationID === id);
      showObsDialog.value = true;
    }

    return { observations, expandedRows, onRowExpand, onRowCollapse, showObsDialog, currentObs, openObs };
  }
})
</script>

<style lang="scss">
.observations-content {
  height: 100%;
  padding: 1em;
}

.minimized-row {
  max-height: 4em;
}

.expanded-row {
  max-height: none;
}

.clamp-text {
  display: -webkit-box;
  -webkit-line-clamp: 1;
  -webkit-box-orient: vertical;
  overflow: hidden;
}
</style>