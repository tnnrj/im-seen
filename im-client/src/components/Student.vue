<template>
<div>
    <h1 class="p-mb-0">{{student.name}}</h1>
    <div>
        <h4>Statistics</h4>
        <div id="stats">
            <!-- todo -->
            <!-- possible fields: X reports in last two weeks; average score increase/decrease over last two weeks -->
            <!-- Frequency <!-- SELECT count(*) FROM Observations GROUP BY StudentID; find desired student and calculate percentile x students below out of total; do we want a date range?? -->
            <!-- Average Severity <!-- SELECT avg(Severity) FROM Observations WHERE StudentId = id; do we want percentile or just average? -->
            <!-- Weighted Score <!-- are weighted scores stored somewhere?? how do I use the weighted score calculator here?  -->
        </div>
    </div>
    <ObservationTable :records="observations" /> <!-- TODO: SELECT * FROM Observations WHERE studentId = student.id -->
  <div v-if="openStudent" class="element-content p-d-flex p-flex-column p-jc-center p-ai-center">
    <template v-if="pieChartData && pieChartData.data">
      <h4 class="p-mb-0">{{pieChartData.name}}</h4>
      <PieChart :chartData="pieChartData.data" :id="idx" @openStudent="openStudent" />
    </template>
    <template v-else>
      <Loader />
    </template>
  </div>
    <div v-if="openStudent" class="element-content p-d-flex p-flex-column p-jc-center p-ai-center">
    <template v-if="lineChartData && lineChartData.data">
      <h4 class="p-mb-0">{{lineChartData.name}}</h4>
      <LineChart :chartData="lineChartData.data" :axis1Name="lineChartData.axis1Name" 
        :axis2Name="lineChartData.axis2Name" :id="idx" @openStudent="openStudent" />
    </template>
    <template v-else>
      <Loader />
    </template>
  </div>
  </div>
  
</template>

<script lang="ts">
import { computed, defineComponent, onBeforeUpdate, ref } from "vue";
import StudentService from "@/services/students.service";
import LineChart from "@/components/charts/LineChart.vue";
import PieChart from "@/components/charts/PieChart.vue";
import ObservationTable from "@/components/ObservationTable.vue"
import Loader from "@/components/Loader.vue";
import { useStore } from "@/store/index";
import reportsService from "@/services/report-data.service";

export default defineComponent({
  name: "Student",
  components: { LineChart, PieChart, Loader, ObservationTable },
  props: {
    idx: Number
  },
  emits: {
    // event payload with validation
    openStudent: (id: string) => {
      return !!id;
    }
  },
  setup(props, { emit }) {
    const store = useStore();

    const reportValues = ref<any[]>([]);
    reportsService.getAllReports().then(reports => {
        reportValues.value = reports.map(r => { return { label: r.reportName, value: r.reportID }; });
      });

    const lineReportID = reportValues["Observation Severity by Student and Date"];
    const pieReportID = reportValues["Observations Grouped by Severity"];

    const lineLoadData = () => { if (!store.getters.getReportData(lineReportID)) store.dispatch('loadReportData', { reportID: lineReportID });} 
    const pieLoadData = () => { if (!store.getters.getReportData(pieReportID)) store.dispatch('loadReportData', { reportID: pieReportID });}
    
    lineLoadData();
    pieLoadData();

    
    const lineChartData = [computed(() => store.getters.getReportData(lineReportID))]; 
    const pieChartData = computed(() => store.getters.getReportData(pieChartData)); 

    onBeforeUpdate(lineLoadData);
    onBeforeUpdate(pieLoadData);

    const openStudent = (id) => {
      emit('openStudent', id);
    };

// need to get only student observations for this student (id)
    if (!store.state.observations) store.dispatch('loadAllObservations');

    const observations = computed(() => {
      if (!store.state.observations) return null;
      return store.state.observations;
    });

    const student = StudentService.getStudent(openStudent);

    return { student, lineChartData, pieChartData, openStudent , observations}
  }
});
</script>

<style lang="scss">
.element-content {
  width: 100%;
  height: 100%;
  background-color: white;
  border-radius: 10px;
  box-shadow: 5px 5px 10px 0px var(--surface-300);
}
</style>