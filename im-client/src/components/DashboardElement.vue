<template>
  <div class="element-content p-d-flex p-flex-column p-jc-center p-ai-center">
    <template v-if="chartData && chartData.data">
      <h4 class="p-mb-0">{{chartData.name}}</h4>
      <BubbleCloudChart v-if="chartType == ChartType.BubbleCloud" :chartData="chartData.data" :id="idx"/>
    </template>
    <template v-else>
      <Loader />
    </template>
  </div>
</template>

<script lang="ts">
import { ChartType } from "@/model/enums.model";
import { computed, defineComponent } from "vue";
import BubbleCloudChart from "@/components/charts/BubbleCloudChart.vue";
import Loader from "@/components/Loader.vue";
import { useStore } from "@/store/index";

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
    const store = useStore();
    if (!store.getters.getChartData(props.queryId)) store.dispatch('loadChartData', { queryId: props.queryId });

    const chartData = computed(() => store.getters.getChartData(props.queryId));

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