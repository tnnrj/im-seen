<template>
  <div class="dashboard-content p-d-flex" v-if="!showConfigurator">
    <div class="dashboard-elements p-d-flex" :class="{'p-flex-column': pages[curPage].layout.indexOf('LR') !== -1}">
      <div class="element" v-for="(element, index) in pages[curPage].elements"
        :style="{ width: element.width + '%', height: element.height + '%' }">
        <DashboardElement :chartType="element.chartType" :queryId="element.queryId" :idx="index"></DashboardElement>
      </div>
    </div>
    <div class="sidebar p-d-flex p-flex-column p-ai-end">
      <i class="pi pi-table p-mb-3" v-tooltip="'Configure'" @click="showLayoutDialog = true" />
      <div class="page-buttons p-d-flex p-flex-column p-jc-center">
        <i class="pi" :class="[curPage === index ? 'pi-circle-on' : 'pi-circle-off']" 
          v-for="(page, index) in pages" @click="switchPage(index)" />
        <i class="pi pi-plus-circle" @click="addNewPage()" />
        <div style="height:5vh"></div>
      </div>
    </div>
  </div>
  <DashboardConfigurator v-else :page="pages[curPage]" @submit="onConfiguratorComplete" />
  <Dialog class="layout-dialog" header="Configure" v-model:visible="showLayoutDialog" :modal="true" :contentStyle="{'height':'30em', 'width':'31em'}">
    <span>Choose a page preset:</span>
    <Accordion :activeIndex="dashLayoutToPanelCounts(newLayout)[0] - 1">
      <AccordionTab v-for="(opt, index) in layoutOptions" :header="(index + 1) + (index ? ' panels' : ' panel')" :key="index">
        <div class="p-d-flex p-flex-wrap">
          <div class="p-m-3" v-for="layout in opt">
            <label>
              <input type="radio" :value="layout" v-model="newLayout" />
              <img :src="require('../assets/layouts/'+layout+'.png')" />
            </label>
          </div>
        </div>
      </AccordionTab>
    </Accordion>
    <template #footer>
        <Button label="Cancel" icon="pi pi-times" @click="onLayoutDialogClose" class="p-button-text"/>
        <Button label="Continue" icon="pi pi-check" @click="onLayoutDialogContinue" />
    </template>
  </Dialog>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import DashboardService from "@/services/dashboard.service";
import { DashPage } from "@/model/dashboard.model";
import { DashLayout } from "@/model/enums.model";
import DashboardElement from "@/components/DashboardElement.vue";
import DashboardConfigurator from "@/components/DashboardConfigurator.vue";

export default defineComponent({
  name: "Dashboard",
  components: { DashboardElement, DashboardConfigurator },
  setup() {
    // pages setup
    const curPage = ref(0);
    const pages = ref<DashPage[]>([]);
    pages.value = DashboardService.getDashPages();
    const addNewPage = () => {
      pages.value.push({
        layout: DashLayout.None,
        elements: []
      });
      curPage.value = pages.value.length - 1;
      newLayout.value = DashLayout.None;
      showLayoutDialog.value = true;
    };
    const switchPage = (i: number) => {
      curPage.value = i;
      newLayout.value = pages.value[curPage.value].layout;
    }

    // layout dialog setup
    const showLayoutDialog = ref(false);
    const layoutOptions: string[][] = [[], [], [], [], []];
    const newLayout = ref(pages.value[curPage.value].layout);
    Object.values(DashLayout).forEach(v => {
      if (v == DashLayout.None) { return; }
      const panelNum = DashboardService.dashLayoutToPanelCounts(v)[0];
      layoutOptions[panelNum - 1].push(v);
    });
    const onLayoutDialogClose = () => {
      if (pages.value[curPage.value].elements.length === 0) {
        pages.value.splice(curPage.value, 1);
        curPage.value = curPage.value ? curPage.value - 1 : 0;
      }
      showLayoutDialog.value = false;
    };
    const onLayoutDialogContinue = () => {
      if (newLayout.value == DashLayout.None) {
        onLayoutDialogClose();
        return;
      }
      if (pages.value[curPage.value].layout != newLayout.value) {
        pages.value[curPage.value] = DashboardService.dashLayoutToDefaultPage(newLayout.value);
      }
      showLayoutDialog.value = false;
      showConfigurator.value = true;
    };

    // configurator setup
    const showConfigurator = ref(false);
    const onConfiguratorComplete = (newPage: DashPage) => {
      showConfigurator.value = false;
      if (newPage) {
        pages.value[curPage.value] = newPage;
      }
    };


    return { curPage, pages, addNewPage, switchPage,
      showLayoutDialog, layoutOptions, onLayoutDialogClose, onLayoutDialogContinue, newLayout,
      dashLayoutToPanelCounts: DashboardService.dashLayoutToPanelCounts, 
      showConfigurator, onConfiguratorComplete };
  }
});
</script>

<style lang="scss">
.dashboard-content {
  height: 100%;
  padding: .5em;
}

.dashboard-elements {
  flex-flow: row wrap;
  flex: 1;
  height: 100%;

  .element {
    padding: .5em;
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

.layout-dialog {
  input[type="radio"] {
    display: none;

    +img {
      cursor: pointer;
      width: 6em;
      height: 6em;
      border: 2px solid white;
      border-radius: 5px;
      padding: .5em;
      transition: 0.3s;

      &:hover {
        box-shadow: 0 0 5px var(--surface-600);
      }
    }

    &:checked+img {
      border: 2px solid var(--blue-600);
      box-shadow: 0 0 5px var(--surface-600);
    }
  }
}
</style>