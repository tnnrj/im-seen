<template>
  <div class="observations-content">
    <ObservationTable :records="observations" />
  </div>
</template>

<script lang="ts">
import { computed, defineComponent } from "vue";
import { useStore } from "@/store";
import { Observation } from '@/model/observations.model';
import ObservationTable from '@/components/ObservationTable.vue';
import * as _ from 'lodash';

export default defineComponent({
  name: 'Observations',
  components: { ObservationTable },
  setup() {
    const store = useStore();
    if (!store.state.observations) store.dispatch('loadAllObservations');
    const observations = computed(() => {
      return _.filter(store.state.observations);
    })

    return { observations };
  }
})
</script>

<style lang="scss">
.observations-content {
  height: 100%;
  padding: 1em;
}
</style>