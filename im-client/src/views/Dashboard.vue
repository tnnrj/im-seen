<template>
  <div class="dashboard-content p-d-flex" v-if="!showConfigurator">
    <template v-if="pages && pages.length">
      <div class="dashboard-elements p-d-flex" :class="{'p-flex-column': pages[curPageNum].layout.indexOf('LR') !== -1}">
        <div class="element" v-for="(element, index) in pages[curPageNum].elements" :key="'p'+curPageNum+'e'+index"
          :style="{ width: element.width + '%', height: element.height + '%', visibility: element.chartType == 'None' ? 'hidden' : '' }">
          <DashboardElement :chartType="element.chartType" :reportID="element.reportID" :id="'p'+curPageNum+'e'+index" @openStudent="openStudent"></DashboardElement>
        </div>
      </div>
      <div class="sidebar p-d-flex p-flex-column p-ai-end">
        <i class="pi pi-table p-mb-3" v-tooltip="'Configure'" @click="showLayoutDialog = true" />
        <div class="page-buttons p-d-flex p-flex-column p-jc-center">
          <i class="pi" :class="[curPageNum === index ? 'pi-circle-on' : 'pi-circle-off']" 
            v-for="(page, index) in pages" :key="index" @click="switchPage(index)" />
          <i class="pi pi-plus-circle" @click="addNewPage()" />
          <div style="height:5vh"></div>
        </div>
      </div>
    </template>
    <template v-else>
      <Loader />
    </template>
  </div>
  <DashboardConfigurator v-else :page="pages[curPageNum]" @submit="onConfiguratorComplete" />
  <Dialog class="layout-dialog" header="Configure" v-model:visible="showLayoutDialog" :modal="true" :closable="false" :closeOnEscape="false" :contentStyle="{'height':'30em', 'width':'31em'}">
    <span>Choose a page preset:</span>
    <Accordion :activeIndex="dashLayoutToPanelCounts(newLayout)[0] - 1">
      <AccordionTab v-for="(opt, index) in layoutOptions" :header="(index + 1) + (index ? ' panels' : ' panel')" :key="index">
        <div class="p-d-flex p-flex-wrap">
          <div class="p-m-3" v-for="(layout, lIndex) in opt" :key="lIndex">
            <label>
              <input type="radio" :value="layout" v-model="newLayout" />
              <img :src="require('../assets/layouts/'+layout+'.png')" />
            </label>
          </div>
        </div>
      </AccordionTab>
    </Accordion>
    <template #footer>
      <Button label="Delete" icon="pi pi-trash" @click="confirmDelete($event)" class="p-button-text p-button-danger"/>
      <Button label="Cancel" icon="pi pi-times" @click="onLayoutDialogClose" class="p-button-text"/>
      <Button label="Continue" icon="pi pi-check" @click="onLayoutDialogContinue" />
    </template>
  </Dialog>
  <Dialog header="Student" v-model:visible="showStudentDialog" :modal="true" :contentStyle="{'max-height':'80vh', 'width':'45em'}">
    <StudentComponent :student="curStudent" />
  </Dialog> 
  <ConfirmPopup></ConfirmPopup>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import DashboardService from "@/services/dashboard.service";
import StudentService from "@/services/students.service";
import { DashPage } from "@/model/dashboard.model";
import { DashLayout } from "@/model/enums.model";
import DashboardElement from "@/components/DashboardElement.vue";
import DashboardConfigurator from "@/components/DashboardConfigurator.vue";
import { useConfirm } from "primevue/useconfirm";
import { Student } from "@/model/student.model";
import StudentComponent from "@/components/Student.vue";

export default defineComponent({
  name: "Dashboard",
  components: { DashboardElement, DashboardConfigurator, StudentComponent},

  setup() {
    // pages setup
    const curPageNum = ref(0);
    const pages = ref<DashPage[]>([]);
    DashboardService.getDashPages().then(p => {
      pages.value = p;
      switchPage(0);
    });
    const addNewPage = () => {
      pages.value.push({
        layout: DashLayout.None,
        elements: []
      });
      curPageNum.value = pages.value.length - 1;
      newLayout.value = DashLayout.None;
      showLayoutDialog.value = true;
    };
    const switchPage = (i: number) => {
      curPageNum.value = i;
      newLayout.value = pages.value[curPageNum.value].layout;
    }

    // layout dialog setup
    const showLayoutDialog = ref(false);
    const layoutOptions: string[][] = [[], [], [], [], []];
    const newLayout = ref(DashLayout.None);
    Object.values(DashLayout).forEach(v => {
      if (v == DashLayout.None) { return; }
      const panelNum = DashboardService.dashLayoutToPanelCounts(v)[0];
      layoutOptions[panelNum - 1].push(v);
    });
    const onLayoutDialogClose = () => {
      if (pages.value[curPageNum.value].elements.length === 0) {
        pages.value.splice(curPageNum.value, 1);
        curPageNum.value = curPageNum.value ? curPageNum.value - 1 : 0;
      }
      showLayoutDialog.value = false;
    };
    const onLayoutDialogContinue = () => {
      if (newLayout.value == DashLayout.None) {
        onLayoutDialogClose();
        return;
      }
      if (pages.value[curPageNum.value].layout != newLayout.value) {
        pages.value[curPageNum.value] = DashboardService.dashLayoutToDefaultPage(newLayout.value);
      }
      showLayoutDialog.value = false;
      showConfigurator.value = true;
    };
    const confirm = useConfirm();
    const confirmDelete = (event: Event) => {
      confirm.require({
        target: event.currentTarget ?? undefined,
        message: "Delete current dashboard page?",
        icon: 'pi pi-info-circle',
        acceptClass: 'p-button-danger',
        accept: () => {
          pages.value[curPageNum.value].elements = [];
          onLayoutDialogClose();
          DashboardService.saveDashPages(pages.value);
        }
      })
    }

    // configurator setup
    const showConfigurator = ref<boolean>(false);
    const onConfiguratorComplete = (newPage: DashPage) => {
      showConfigurator.value = false;
      if (newPage) {
        pages.value[curPageNum.value] = newPage;
        DashboardService.saveDashPages(pages.value);
      }
    };

    // student dialog
    const showStudentDialog = ref<boolean>(false);
    const curStudent = ref<Student>();
    const openStudent = async (id: string) => {
      showStudentDialog.value = true;
      curStudent.value = await StudentService.getStudent(id);
    };

    return { curPageNum, pages, addNewPage, switchPage,
      showLayoutDialog, layoutOptions, onLayoutDialogClose, onLayoutDialogContinue, confirmDelete, newLayout,
      dashLayoutToPanelCounts: DashboardService.dashLayoutToPanelCounts, 
      showConfigurator, onConfiguratorComplete,
      showStudentDialog, curStudent, openStudent };
  }
});
</script>

<style lang="scss">
.dashboard-content {
  padding: .5em;
  height: 100%;
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

.p-confirm-popup {
  z-index: 99999 !important;
}
</style>