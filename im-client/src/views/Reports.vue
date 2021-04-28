<template>
  <div class="reports-content">
    <DataTable :value="reports" sortField="reportDate" :sortOrder="-1" dataKey="reportID" :autoLayout="true" :rowClass="rowClass"
        v-model:expandedRows="expandedRows" @row-expand="onRowExpand" @row-collapse="onRowCollapse" :scrollable="true" scrollHeight="flex">
      <Column :expander="true" style="flex: 0 0 3em"></Column>
      <Column field="studentName" header="Name" :sortable="true" style="flex: 0 0 calc(20%-3em)"></Column>
      <Column field="severity" header="Severity" :sortable="true" style="flex: 0 0 10%"></Column>
      <Column field="description" header="Description" style="flex: 0 0 50%">
        <template #body="slotProps">
          <span class="desc-cell">{{slotProps.data.description}}</span>
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

<script>
import { defineComponent, ref } from "vue";
import { getReports } from "@/services/reports.service";
import Loader from "@/components/Loader.vue";

export default defineComponent({
  name: 'Reports',
  components: { Loader },
  setup() {
    let maxDescLength = 100;
    const reports = ref();
    getReports()
      .then(response => reports.value = response.data
        .map(r => {
          //r.fullDescription = r.description;
          //r.description = r.description.length > maxDescLength + 4 ? r.description.substr(0, maxDescLength) + '...' : r.description;
          r.reportDate = new Date(r.reportDate);
          return r;
        }));
    const expandedRows = ref([]);
    const onRowExpand = (event) => {
      //event.data.description = event.data.fullDescription;
    }
    const onRowCollapse = (event) => {
      //event.data.description = event.data.fullDescription.length > maxDescLength + 4 ? event.data.fullDescription.substr(0, maxDescLength) + '...' : event.data.fullDescription;
    }

    const rowClass = (data) => {
      if (expandedRows.value.filter(r => r == data).length > 0) {
        return "expanded-row";
      }
      return "minimized-row";
    }

    return { reports, expandedRows, onRowExpand, onRowCollapse, rowClass };
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

.desc-cell {
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  max-width: 100em;
}
</style>