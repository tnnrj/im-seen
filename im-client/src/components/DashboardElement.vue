<template>
  <div v-if="chartType != 'None'" class="element-content p-d-flex p-flex-column p-jc-center p-ai-center">
    <template v-if="chartData && chartData.data">
      <h4 class="p-mb-0">{{chartData.name}}</h4>
      <BubbleCloudChart v-if="chartType == ChartType.BubbleCloud" :chartData="chartData.data" :id="idx" />
      <PieChart v-else-if="chartType == ChartType.Pie" :chartData="chartData.data" :id="idx" />
      <LineChart v-else-if="chartType == ChartType.Line" :chartData="chartData.data" :axis1Name="chartData.axis1Name" 
        :axis2Name="chartData.axis2Name" :id="idx" />
      <BarChart v-else-if="chartType == ChartType.Bar" :chartData="chartData.data" :axis1Name="chartData.axis1Name" 
        :axis2Name="chartData.axis2Name" :id="idx" />
    </template>
    <template v-else>
      <Loader />
    </template>
  </div>
</template>

<script lang="ts">
import { ChartType } from "@/model/enums.model";
import { computed, defineComponent, onBeforeUpdate } from "vue";
import BubbleCloudChart from "@/components/charts/BubbleCloudChart.vue";
import LineChart from "@/components/charts/LineChart.vue";
import BarChart from "@/components/charts/BarChart.vue";
import PieChart from "@/components/charts/PieChart.vue";
import Loader from "@/components/Loader.vue";
import { useStore } from "@/store/index";

export default defineComponent({
  name: "DashboardElement",
  components: { BubbleCloudChart, LineChart, BarChart, PieChart, Loader },
  props: {
    chartType: {
      type: String,
      required: true
    },
    reportID: {
      type: Number,
      required: true
    },
    idx: Number
  },
  setup(props) {
    const store = useStore();
    const loadData = () => { if (!store.getters.getReportData(props.reportID)) store.dispatch('loadReportData', { reportID: props.reportID }); }
    loadData();
    const chartData = computed(() => store.getters.getReportData(props.reportID));

    onBeforeUpdate(loadData);

    return { chartData, ChartType }
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