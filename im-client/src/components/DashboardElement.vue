<template>
  <div class="element-content p-d-flex p-flex-column p-jc-center p-ai-center">
    <template v-if="true || (data && data.length)">
      <BubbleCloudChart v-if="chartType == ChartType.BubbleCloud" :chartData="data" :id="idx"/>
    </template>
    <template v-else>
      <Loader />
    </template>
  </div>
</template>

<script lang="ts">
import { ChartType } from "@/model/enums.model";
import { defineComponent, ref } from "vue";
import { getChartData } from "@/services/chart-data.service";
import BubbleCloudChart from "@/components/charts/BubbleCloudChart.vue";
import Loader from "@/components/Loader.vue";

export default defineComponent({
  name: "DashboardElement",
  components: { BubbleCloudChart, Loader },
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
    const data = ref<any[]>();
    getChartData(props.queryId).then((response: any) => data.value = response.data);

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