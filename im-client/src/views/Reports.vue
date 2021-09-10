<template>
  <div class="observations-content">
    <DataTable :value="observations" sortField="observationDate" :sortOrder="-1" dataKey="observationId"
        v-model:expandedRows="expandedRows" @row-expand="onRowExpand" @row-collapse="onRowCollapse" :scrollable="true" scrollHeight="flex">
      <Column :expander="true" style="flex: 1 1 5%"></Column>
      <Column field="studentName" header="Name" :sortable="true" style="flex: 1 1 15%"></Column>
      <Column field="severity" header="Severity" :sortable="true" style="flex: 1 1 10%"></Column>
      <Column field="description" header="Description" style="flex: 1 1 50%">
        <template #body="slotProps">
          <span :class="{ 'clamp-text': !slotProps.data.expanded }">{{slotProps.data.description}}</span>
        </template>
      </Column>
      <Column field="observationDate" header="Submit Date" :sortable="true" style="flex: 1 1 20%">
        <template #body="slotProps">{{slotProps.data.observationDate.toLocaleString('en-US')}}</template>
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
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, watchEffect } from "vue";
import Loader from "@/components/Loader.vue";
import { useStore } from "@/store";
import { Observation } from '@/model/observations.model';

interface UiObservation extends Observation {
  fullDescription: string;
  expanded: boolean;
}

export default defineComponent({
  name: 'Reports',
  components: { Loader },
  setup() {
    const store = useStore();

    const observations = ref();
    watchEffect(() => {
      if (!store.state.observations) store.dispatch('loadAllObservations'); // here so we enforce watch dependency
      setTimeout(() => { // needs to happen async so we show loader immediately
        observations.value = store.state.observations?.map(r => {
          let uir = r as UiObservation;
          uir.observationDate = new Date(r.observationDate);
          uir.fullDescription = r.description;
          uir.expanded = false;
          return uir;
        });
      }, 0);
    });
    
    const expandedRows = ref([]);
    const onRowExpand = (event) => {
      event.data.expanded = true;
    }
    const onRowCollapse = (event) => {
      event.data.expanded = false;
    }

    return { observations, expandedRows, onRowExpand, onRowCollapse };
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