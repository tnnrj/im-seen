<template>
  <div class="configurator-window">
    <Splitter class="configurator-content" :layout="isMainAxisVertical ? 'vertical' : 'horizontal'">
      <SplitterPanel v-for="(splitter, si) in elementGrid" :key="'sp-'+si" :size="mainAxisSizes[si]" :ref="el => { mainAxisRefs[si] = el }">
        <Splitter :layout="isMainAxisVertical ? 'horizontal' : 'vertical'">
          <SplitterPanel class="p-d-flex p-ai-center p-jc-center" :ref="el => { splitterRefs[si][pi] = el }"
            v-for="(panel, pi) in splitter" :key="'p-'+pi" :size="isMainAxisVertical ? panel.width : panel.height">
            sample content
          </SplitterPanel>
        </Splitter>
      </SplitterPanel>
    </Splitter>
    <div class="config-overlay">
      <Button icon="pi pi-times" @click="confirmCancel($event)" class="p-button p-button-rounded p-button-danger"/>
      <Button icon="pi pi-check" @click="onSubmit" class="p-button p-button-rounded"/>
    </div>
  </div>
  <ConfirmPopup></ConfirmPopup>
</template>

<script lang="ts">
import { defineComponent, PropType, ref } from "vue";
import DashboardService from "@/services/dashboard.service";
import { DashElement, DashPage } from "@/model/dashboard.model";
import { useConfirm } from "primevue/useconfirm";

export default defineComponent({
  name: 'DashboardConfigurator',
  props: {
    page: {
      type: Object as PropType<DashPage>,
      required: true
    }
  },
  emits: ['submit'],
  setup(props, context) {
    // load info about layout
    const isMainAxisVertical = props.page.layout.indexOf('TB') != -1;
    const mainAxisFirstSize = isMainAxisVertical ? props.page.elements[0].height : props.page.elements[0].width;
    const mainAxisSizes = [mainAxisFirstSize, 100 - mainAxisFirstSize];
    const counts = DashboardService.dashLayoutToPanelCounts(props.page.layout);

    // used to set initial value of splitter sizes and bind to element chart types/queries
    const elementGrid = ref<DashElement[][]>([]);
    for (let i = 0; i < 2; i++) {
      elementGrid.value.push([]);
      for (let ii = 0; ii < counts[i + 1]; ii++) {
        elementGrid.value[i].push(props.page.elements[i * counts[1] + ii]);
      }
    }

    // used to bind reactive values of splitters
    const mainAxisRefs = ref<any[]>([]);
    const splitterRefs = ref<any[]>([[], []]);

    // reconstruct DashElement array and emit
    const onSubmit = () => {
      const newEls: DashElement[] = [];
      let mainAxisSum = 0;
      for (let i = 0; i < 2; i++) {
        let mainAxisSize: number;
        if (i === 1) {
          // ensure final size adds up to 100 to account for rounding issues when extracting
          mainAxisSize = 100 - mainAxisSum;
        }
        else {
          // this size extraction is highly dependent on internal implementation of Prime's Splitter, which could change with updates
          // look here first if there's a problem with building DashPages
          mainAxisSize = Math.round(mainAxisRefs.value[i].$el.style["flex-basis"].split('%')[0].substr(5) as number * 100) / 100;
          mainAxisSum += mainAxisSize;
        }        
        // same process as above, for secondary axis (rows/cols)
        let sizeSum = 0;
        for (let ii = 0; ii < counts[i + 1]; ii++) {
          let size: number;
          if (ii === counts[i + 1] - 1) {
            size = 100 - sizeSum;
          }
          else {
            size = Math.round(splitterRefs.value[i][ii].$el.style["flex-basis"].split('%')[0].substr(5) as number * 100) / 100;
            sizeSum += size;
          }
          // assign w/h based on extracted sizes and orientation
          elementGrid.value[i][ii].width = isMainAxisVertical ? size : mainAxisSize;
          elementGrid.value[i][ii].height = isMainAxisVertical ? mainAxisSize : size;
          newEls.push(elementGrid.value[i][ii]);
        }
      }
      context.emit('submit', { layout: props.page.layout, elements: newEls });
    }

    // confirmation dialog callback
    const confirm = useConfirm();
    const confirmCancel = (event: Event) => {
      confirm.require({
        target: event.currentTarget ?? undefined,
        message: "Discard changes?",
        icon: 'pi pi-info-circle',
        acceptClass: 'p-button-danger',
        accept: () => {
          context.emit('submit', null);
        }
      })
    }

    return { isMainAxisVertical, mainAxisSizes, elementGrid, mainAxisRefs, splitterRefs, onSubmit, confirmCancel };
  }
});
</script>

<style lang="scss">
.configurator-window {
  width: 100%;
  height: 100%;
  position: relative;
}
.configurator-content {
  width: 100%;
  height: 100%;
  background-color: white;
}

.config-overlay {
  font-size: .8rem;
  position: absolute;
  padding: 4em 0 0 8em;
  bottom: 0;
  right: 0;
  background: transparent;
  transition: background-image .3s ease-in-out;

  .p-button {
    margin: 0 .5em .5em 0;
  }

  &:not(:hover) .p-button {    
    color: var(--surface-500);
    border: 1px solid var(--surface-500);
    background-color: rgba(0,0,0,0);
  }

  &:hover {
    background-image: radial-gradient(ellipse at 90% 100%, rgba(0,0,0,.7), rgba(0,0,0,0) 70%);
  }
}
</style>