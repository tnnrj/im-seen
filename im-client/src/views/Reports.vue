<template>
  <div class="reports-content">
    <DataTable :value="reports" v-if="reports && reports.length">
      <Column field="name" header="Name"></Column>
      <Column field="severity" header="Severity"></Column>
      <Column field="text" header="Description"></Column>
      <Column field="date" header="Submit Date"></Column>
    </DataTable>
    <template v-else>
      <Loader />
    </template>
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
    const reports = ref();
    getReports().then(response => reports.value = response.data);

    return { reports };
  }
})
</script>

<style>
.reports-content {
  height: 100%;
  background-color: white;
  border-radius: 10px;
}
</style>