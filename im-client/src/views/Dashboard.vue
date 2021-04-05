<template>
  <div class="dashboard-content p-d-flex" v-if="!showConfigurator">
    <div class="dashboard-elements p-d-flex" :class="{'p-flex-column': pages[curPage].layout.indexOf('LR') !== -1}">
      <div class="element" v-for="element in pages[curPage].elements"
        :style="{ width: element.width + '%', height: element.height + '%' }">
        <DashboardElement :chartType="element.chartType" :queryId="element.queryId"></DashboardElement>
      </div>
    </div>
    <div class="sidebar p-d-flex p-flex-column p-ai-end">
      <i class="pi pi-table p-mb-3" v-tooltip="'Configure'" @click="showLayoutDialog = true" />
      <div class="page-buttons p-d-flex p-flex-column p-jc-center">
        <i class="pi" :class="[curPage === index ? 'pi-circle-on' : 'pi-circle-off']" 
          v-for="(page, index) in pages" @click="curPage = index" />
        <i class="pi pi-plus-circle" @click="addNewPage()" />
        <div style="height:5vh"></div>
      </div>
    </div>
  </div>
  <Dialog class="layout-dialog" header="Configure" v-model:visible="showLayoutDialog" :modal="true" :contentStyle="{'height':'30em', 'width':'31em'}">
    <span>Choose a page preset:</span>
    <Accordion :activeIndex="dashLayoutToPanelCount(pages[curPage].layout) - 1">
      <AccordionTab v-for="(opt, index) in layoutOptions" :header="(index + 1) + (index ? ' panels' : ' panel')" :key="index">
        <div class="p-d-flex p-flex-wrap">
          <div class="p-m-3" v-for="layout in opt">
            <label>
              <input type="radio" :value="layout" v-model="pages[curPage].layout" />
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
import { getDashPages, dashLayoutToPanelCount, dashLayoutToDefaultPage } from "@/services/dashboard.service";
import { DashPage } from "@/model/dashboard.model";
import DashboardElement from "@/components/DashboardElement.vue";
import { DashLayout } from "@/model/enums.model";

export default defineComponent({
  name: "Dashboard",
  components: { DashboardElement },
  setup() {
    // pages setup
    const curPage = ref(0);
    const pages = ref<DashPage[]>([]);
    pages.value = getDashPages();
    const addNewPage = () => {
      pages.value.push({
        layout: DashLayout.None,
        elements: []
      });
      curPage.value = pages.value.length - 1;
      showLayoutDialog.value = true;
    }

    // layout dialog setup
    const showLayoutDialog = ref(false);
    const layoutOptions: string[][] = [[], [], [], [], []];
    Object.values(DashLayout).forEach(v => {
      if (v == DashLayout.None) { return; }
      const panelNum = dashLayoutToPanelCount(v);
      layoutOptions[panelNum - 1].push(v);
    });
    const onLayoutDialogClose = () => {
      if (pages.value[curPage.value].elements.length === 0) {
        pages.value.splice(curPage.value, 1);
        curPage.value = curPage.value ? curPage.value - 1 : 0;
      }
      showLayoutDialog.value = false;
    }
    const onLayoutDialogContinue = () => {
      if (pages.value[curPage.value].layout == DashLayout.None) {
        onLayoutDialogClose();
        return;
      }
      pages.value[curPage.value] = dashLayoutToDefaultPage(pages.value[curPage.value].layout);
      showLayoutDialog.value = false;
      //showConfigurator.value = true;
    }

    // configurator setup
    const showConfigurator = ref(false);


    return { curPage, pages, addNewPage,
      showLayoutDialog, layoutOptions, dashLayoutToPanelCount, onLayoutDialogClose, onLayoutDialogContinue,
      showConfigurator };
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