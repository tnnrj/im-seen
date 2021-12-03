<template>
<div v-if="curStudent">
    <h1 class="p-mb-0">{{curStudent?.firstName + ' ' + curStudent?.lastName}}</h1>
    <div>
        <h4>Statistics</h4>
        <div id="stats">
            <!-- todo -->
            <!-- possible fields: X reports in last two weeks; average score increase/decrease over last two weeks -->
            <!-- Frequency < SELECT count(*) FROM Observations GROUP BY StudentID; find desired student and calculate percentile x students below out of total; do we want a date range?? -->
            <!-- Average Severity < SELECT avg(Severity) FROM Observations WHERE StudentId = id; do we want percentile or just average? -->
            <!-- Weighted Score < are weighted scores stored somewhere?? how do I use the weighted score calculator here?  -->
        </div>
    </div>
    <ObservationTable :records="observations" /> <!-- TODO: SELECT * FROM Observations WHERE studentId = student.id -->
  <div class="element-content p-d-flex p-flex-column p-jc-center p-ai-center student-chart">
    <template v-if="pieChartData && pieChartData.data">
        <h4 class="p-mb-0">{{pieChartData.name}}</h4>
        <PieChart :chartData="pieChartData.data" :id="idx" @openStudent="openStudent"/>
    </template> 
    <template v-else>
      <Loader />
    </template>
  </div>
  <div class="element-content p-d-flex p-flex-column p-jc-center p-ai-center student-chart">
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
//import StudentService from "@/services/students.service";
import LineChart from "@/components/charts/LineChart.vue";
import PieChart from "@/components/charts/PieChart.vue";
import ObservationTable from "@/components/ObservationTable.vue"
import Loader from "@/components/Loader.vue";
import { useStore } from "@/store/index";
import { Student } from "@/model/student.model";
import reportsService from "@/services/report-data.service";
import studentsService from "@/services/students.service";
import * as _ from "lodash";

export default defineComponent({
  name: "Student",
  components: { LineChart, PieChart, Loader, ObservationTable },
  props: {
    student : Object,
  },
  emits: {
    // event payload with validation
    openStudent: (id: string) => {
      return !!id;
    }
  },
  setup(props, { emit }) {
    const curStudent = computed(() => props.student as Student);
    const store = useStore();

    const reportValues = ref<any[]>([]);
    reportsService.getAllReports().then(reports => {
      reportValues.value = reports;
    });

    // todo: don't set like this :'(
    const lineID = 2;
    const pieID = 1;

    const loadDataLine = () => { if (!store.getters.getReportData(lineID)) store.dispatch('loadReportData', { reportID: lineID }); }
    loadDataLine();
    const lineChartData = computed(() => {
      let chartData = store.getters.getReportData(lineID);
      chartData.data = _.filter(chartData.data, d => d.id == curStudent.value.studentID);
      return chartData;
    });
    onBeforeUpdate(loadDataLine);

    if (!store.state.observations) store.dispatch('loadAllObservations');

    const observations = computed(() => {
      if (!store.state.observations) return null;
      return _.filter(store.state.observations, o => o.studentID == curStudent.value.studentID);
    });

    const pieChartData = computed(() => {
      let axis1Name =  "Severity";
      let data = Object.values(_.groupBy(observations.value, o => o.severity)).map(olst => { return {name: (olst as any)[0].severity, value: (olst as any).length}});
      let name = "Observations Grouped by Severity";
      console.log(data);
      if(!data.length)
        return null;
      return {name, data, axis1Name};
      });

    return { curStudent, lineChartData, pieChartData, observations }
  }
});
</script>

<style lang="scss">
.element-content {
  width: 100%;
  background-color: white;
  border-radius: 10px;
  box-shadow: 5px 5px 10px 0px var(--surface-300);
}

.student-chart {
  height: 200px;
}

</style>