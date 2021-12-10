<template>
  <div class="student-content" v-if="curStudent">
    <!-- <div>
        <h4>Statistics</h4>
        <div id="stats"> -->
            <!-- todo -->
            <!-- possible fields: X reports in last two weeks; average score increase/decrease over last two weeks -->
            <!-- Frequency < SELECT count(*) FROM Observations GROUP BY StudentID; find desired student and calculate percentile x students below out of total; do we want a date range?? -->
            <!-- Average Severity < SELECT avg(Severity) FROM Observations WHERE StudentId = id; do we want percentile or just average? -->
            <!-- Weighted Score < are weighted scores stored somewhere?? how do I use the weighted score calculator here?  -->
        <!-- </div>
    </div> -->
    <div class="p-d-flex p-flex-column p-jc-center p-ai-center student-chart">
      <template v-if="pieChartData && pieChartData.data">
          <h4 class="p-mb-0">{{pieChartData.name}}</h4>
          <PieChart :chartData="pieChartData.data" :id="curStudent.studentID+'-'+1" />
      </template> 
      <template v-else>
        <Loader />
      </template>
    </div>
    <div class="p-d-flex p-flex-column p-jc-center p-ai-center student-chart">
      <template v-if="lineChartData && lineChartData.data">
          <h4 class="p-mb-0">{{lineChartData.name}}</h4>
          <LineChart :chartData="lineChartData.data" :axis1Name="lineChartData.axis1Name" 
            :axis2Name="lineChartData.axis2Name" :id="curStudent.studentID+'-'+2" :showFilter="true" />
      </template>
      <template v-else>
        <Loader />
      </template>
    </div>
    <ObservationTable :records="observations" /> <!-- TODO: SELECT * FROM Observations WHERE studentId = student.id -->
  </div> 
</template>

<script lang="ts">
import { computed, defineComponent } from "vue";
import LineChart from "@/components/charts/LineChart.vue";
import PieChart from "@/components/charts/PieChart.vue";
import ObservationTable from "@/components/ObservationTable.vue"
import Loader from "@/components/Loader.vue";
import { useStore } from "@/store/index";
import { Student } from "@/model/student.model";

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
  setup(props) {
    const curStudent = computed(() => props.student as Student);
    const store = useStore();
    
    if (!store.state.observations) store.dispatch('loadAllObservations');

    const observations = computed(() => {
      if (!store.state.observations) return null;
      return _.filter(store.state.observations, o => o.studentID == curStudent.value.studentID);
    });

    const lineChartData = computed(() => {
      let axis1Name = "Date";
      let axis2Name = "Severity of Observations";
      let name = "Observation Severity by Date";
      let data = _.sortBy(observations.value?.map(o => { return { name: o.studentFirstName + ' ' + o.studentLastName, value: o.severity, date: o.observationDate, id: o.studentID }}), d => d.date);
      if (!data?.length)
        return null;
      return { axis1Name, axis2Name, name, data };
    });

    const pieChartData = computed(() => {
      let axis1Name =  "Severity";
      let data = Object.values(_.groupBy(observations.value, o => o.severity))?.map(olst => { return {name: (olst as any)[0].severity, value: (olst as any).length}});
      let name = "Observations Grouped by Severity";
      if (!data?.length)
        return null;
      return { name, data, axis1Name };
    });

    return { curStudent, lineChartData, pieChartData, observations }
  }
});
</script>

<style lang="scss">
.student-content {
  display: flex;
  flex-flow: row wrap;
}

.student-chart {
  height: 20em;
  width: 50%;
}

</style>