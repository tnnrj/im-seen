<template>
<div>
    <h1 class="p-mb-0">{{Student_Name}}</h1>
    <div>
        <h4>Percentiles</h4>
        <div id="percentile-stats">
            <!-- todo -->
            Frequency 
            <br/>
            Severity 
            <br/>
            Weighted Score
        </div>
    </div>
    <ObservationTable :records="observations" />
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
import { computed, defineComponent, onBeforeUpdate } from "vue";
import StudentService from "@/services/students.service";
import LineChart from "@/components/charts/LineChart.vue";
import PieChart from "@/components/charts/PieChart.vue";
import ObservationTable from "@/components/ObservationTable.vue"
import Loader from "@/components/Loader.vue";
import { useStore } from "@/store/index";

export default defineComponent({
  name: "Student",
  components: { LineChart, PieChart, Loader, ObservationTable },
  props: {
    pieReportID: {
      type: Number,
      required: true
    },
    lineReportID: {
      type: Number,
      required: true
    },
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

    const lineLoadData = () => { if (!store.getters.getReportData(props.lineReportID)) store.dispatch('loadReportData', { reportID: props.lineReportID });} 
    const pieLoadData = () => { if (!store.getters.getReportData(props.pieReportID)) store.dispatch('loadReportData', { reportID: props.pieReportID });}
    
    lineLoadData();
    pieLoadData();

    const lineChartData = computed(() => store.getters.getReportData(props.lineReportID));
    const pieChartData = computed(() => store.getters.getReportData(props.pieReportID));

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

    //const student = StudentService.getStudent(openStudent);

    return { lineChartData, pieChartData, openStudent , observations}
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