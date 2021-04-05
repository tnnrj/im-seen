<template>
  <div class="element-content p-d-flex p-flex-column p-jc-center">
    <template v-if="data.length">
      <BubbleCloudChart v-if="chartType == ChartType.BubbleCloud" :chartData="data" :id="idx"/>
    </template>
  </div>
</template>

<script lang="ts">
import { ChartType } from "@/model/enums.model";
import { defineComponent } from "vue";
import { getChartData } from "@/services/chart-data.service";
import BubbleCloudChart from "@/components/charts/BubbleCloudChart.vue";

export default defineComponent({
  name: "DashboardElement",
  components: { BubbleCloudChart },
  props: {
    chartType: {
      type: String,
      required: true
    },
    queryId: {
      type: String,
      required: true
    },
    idx: Number
  },
  setup(props) {
    const data = getChartData(props.queryId);

    return { data, ChartType }
  }
});
</script>

<style lang="scss">
.element-content {
  width: 100%;
  height: 100%;
  background-color: white;
  border-radius: 10px;
}
</style>