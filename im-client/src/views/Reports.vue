<template>
  <div class="reports-content">
    <DataTable :value="reports" sortField="reportDate" :sortOrder="-1" dataKey="reportID" :autoLayout="true" 
        v-model:expandedRows="expandedRows" @row-expand="onRowExpand" @row-collapse="onRowCollapse" :scrollable="true" scrollHeight="flex">
      <Column :expander="true" style="flex: 0 0 3em"></Column>
      <Column field="studentName" header="Name" :sortable="true" style="flex: 0 0 calc(20%-3em)"></Column>
      <Column field="severity" header="Severity" :sortable="true" style="flex: 0 0 10%"></Column>
      <Column field="description" header="Description" style="flex: 0 0 50%">
        <template #body="slotProps">
          <span :class="{ 'clamp-text': !slotProps.data.expanded }">{{slotProps.data.description}}</span>
        </template>
      </Column>
      <Column field="reportDate" header="Submit Date" :sortable="true" style="flex: 0 0 20%">
        <template #body="slotProps">{{slotProps.data.reportDate.toLocaleString()}}</template>
      </Column>
      <template #empty>
        <template v-if="reports">
          No records to display
        </template>
        <template v-else>
          <Loader />
        </template>
      </template>
    </DataTable>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, watchEffect } from "vue";
import Loader from "@/components/Loader.vue";
import { useStore } from "@/store";
import { Report } from '@/model/reports.model';

interface UiReport extends Report {
  fullDescription: string;
  expanded: boolean;
}

export default defineComponent({
  name: 'Reports',
  components: { Loader },
  setup() {
    const store = useStore();

    const reports = ref();
    watchEffect(() => {
      if (!store.state.reports) store.dispatch('loadAllReports'); // here so we enforce watch dependency
      setTimeout(() => { // needs to happen async so we show loader immediately
        reports.value = store.state.reports?.map(r => {
          let uir = r as UiReport;
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

    return { reports, expandedRows, onRowExpand, onRowCollapse };
  }
})
</script>

<style lang="scss">
.reports-content {
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