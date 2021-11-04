<template>
  <div class="observation-table-content">
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
        <template #body="slotProps">{{slotProps.data.observationDateString}}</template>
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
import { Observation } from '@/model/observations.model';
import ObservationComponent from '@/components/Observation.vue';
import * as _ from 'lodash';
import moment from 'moment';

interface UiObservation extends Observation {
  studentName: string;
  fullDescription: string;
  expanded: boolean;
  observationDateString: string;
}

export default defineComponent({
  name: 'ObservationTable',
  components: { Loader, ObservationComponent },
  props: {
    records: Object
  },
  setup(props) {
    const observations = ref();
    watchEffect(() => {
      observations.value = props.records?.map(r => {
        let uir = _.cloneDeep(r) as UiObservation;
        uir.studentName = (r.studentFirstName + ' ' + r.studentLastName).trim();
        uir.observationDate = new Date(r.observationDate);
        uir.observationDateString = moment(uir.observationDate).format('MMM DD, YYYY h:mm A');
        uir.fullDescription = r.description;
        uir.expanded = false;
        return uir;
      });
    })
    
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
.observation-table-content {
  height: 100%;
}
.clamp-text {
  display: -webkit-box;
  -webkit-line-clamp: 1;
  -webkit-box-orient: vertical;
  overflow: hidden;
}
</style>