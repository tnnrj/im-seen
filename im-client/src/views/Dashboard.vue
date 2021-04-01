<template>
  <div class="dashboard-content p-d-flex">
    <div class="dashboard-elements p-d-flex">
      <div class="element" v-for="element in pages[curPage]" :key="element.elementId"
        :style="{ width: element.width + '%', height: element.height + '%' }">
        <DashboardElement></DashboardElement>
      </div>
    </div>
    <div class="sidebar p-d-flex p-flex-column p-ai-end">
      <i class="pi pi-cog p-mb-3" v-tooltip="'Configure'" />
      <i class="pi pi-table p-mb-3" v-tooltip="'Layout'" />
      <div class="page-buttons p-d-flex p-flex-column p-jc-center">
        <i class="pi" :class="[curPage === index ? 'pi-circle-on' : 'pi-circle-off']" 
          v-for="(page, index) in pages" @click="curPage = index"/>
        <i class="pi pi-plus-circle" />
        <div style="height:9.5vh"></div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import { ChartType } from "@/model/enums";
import { DashPages } from "@/model/dashboard";
import DashboardElement from "@/components/DashboardElement.vue";

export default defineComponent({
  name: "Dashboard",
  components: {
    DashboardElement
  },
  setup() {
    const curPage = ref(0);
    const pages = ref<DashPages>([]);
    pages.value = [
      [
        {
        elementId: 1,
        chartType: ChartType.Bar,
        width: 100,
        height: 50
      },
      {
        elementId: 2,
        chartType: ChartType.Line,
        width: 50,
        height: 50
      },
      {
        elementId: 3,
        chartType: ChartType.Pie,
        width: 50,
        height: 50
      }
      ],
      [
        {
        elementId: 1,
        chartType: ChartType.Bar,
        width: 60,
        height: 60
      },
      {
        elementId: 2,
        chartType: ChartType.Line,
        width: 40,
        height: 60
      },
      {
        elementId: 3,
        chartType: ChartType.Pie,
        width: 100,
        height: 40
      }
      ],
      [
        {
        elementId: 1,
        chartType: ChartType.Bar,
        width: 50,
        height: 50
      },
      {
        elementId: 2,
        chartType: ChartType.Line,
        width: 50,
        height: 50
      },
      {
        elementId: 3,
        chartType: ChartType.Pie,
        width: 50,
        height: 50
      },
      {
        elementId: 4,
        chartType: ChartType.Pie,
        width: 50,
        height: 50
      }
      ]
    ];
    return { curPage, pages };
  }
});
</script>

<style lang="scss">
.dashboard-content {
  height: 100%;
}

.dashboard-elements {
  flex-flow: row wrap;
  flex: 1;
  height: 100%;
  margin-top: -.75em;
  margin-left: -.75em;  

  .element {
    padding: .75em;
  }
}

.sidebar {
  width: 1.5em;

  i {
    cursor: pointer;
    padding: .25em;
    color: var(--surface-600);
    transition: 0.3s;

    &:hover:not(.pi-circle-on) {
      color: var(--surface-900);
    }
  }

  .page-buttons {
    flex: 1;
  }
}
</style>