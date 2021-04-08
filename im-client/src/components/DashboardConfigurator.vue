<template>
  <Splitter class="configurator-content" :layout="isMainAxisVertical ? 'vertical' : 'horizontal'">
    <SplitterPanel v-for="(splitter, si) in elementGrid" :key="'sp-'+si" :size="mainAxisSizes[index]" :ref="el => { mainAxisRefs[si] = el }">
      <Splitter :layout="isMainAxisVertical ? 'horizontal' : 'vertical'">
        <SplitterPanel class="p-d-flex p-ai-center p-jc-center" :ref="el => { splitterRefs[si][pi] = el }"
          v-for="(panel, pi) in splitter" :key="'p-'+pi" :size="isMainAxisVertical ? panel.width : panel.height">
          sample content
        </SplitterPanel>
      </Splitter>
    </SplitterPanel>
  </Splitter>
  <Button @click="submit" />
</template>

<script lang="ts">
import { defineComponent, PropType, ref } from "vue";
import DashboardService from "@/services/dashboard.service";
import { DashElement, DashPage } from "@/model/dashboard.model";

export default defineComponent({
  name: 'DashboardConfigurator',
  props: {
    page: {
      type: Object as PropType<DashPage>,
      required: true
    }
  },
  emits: ['submit'],
  setup(props, { emit }) {
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
    const submit = () => {
      const newEls: DashElement[] = [];
      let mainAxisSum = 0;
      for (let i = 0; i < 2; i++) {
        let mainAxisSize: number;
        if (i === 1) {
          mainAxisSize = 100 - mainAxisSum;
        }
        else {
          // this size extraction is highly dependent on internal implementation of Prime's Splitter, which could change with updates
          // look here first if there's a problem with building DashPages
          mainAxisSize = Math.round(mainAxisRefs.value[i].$el.style["flex-basis"].split('%')[0].substr(5) as number * 100) / 100;
          mainAxisSum += mainAxisSize;
        }
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
          elementGrid.value[i][ii].width = isMainAxisVertical ? size : mainAxisSize;
          elementGrid.value[i][ii].height = isMainAxisVertical ? mainAxisSize : size;
          newEls.push(elementGrid.value[i][ii]);
        }
      }
      emit('submit', { layout: props.page.layout, elements: newEls });
    }

    return { isMainAxisVertical, mainAxisSizes, elementGrid, mainAxisRefs, splitterRefs, submit };
  }
});
</script>

<style lang="scss">
.configurator-content {
  width: 100%;
  height: 100%;
  background-color: white;
}
</style>