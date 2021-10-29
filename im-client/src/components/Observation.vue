<template>
  <div class="observation-content">
    <template v-if="obs && !saving">
      <div class="p-d-flex observation-panel">
        <Panel header="Student" class="p-mb-1" style="width:100%">
          <span>{{(obs.studentFirstName + ' ' + obs.studentLastName).trim()}}</span>
        </Panel>
        <Panel header="Description" class="p-mb-1" style="width:100%" :toggleable="true">
          <span>{{obs.description}}</span>
        </Panel>
        <Panel header="Severity" class="p-mb-1" style="width:20%">
          <span>{{obs.severity}}</span>
        </Panel>
        <Panel header="Status" class="p-mb-1 p-pl-1 p-pr-1" style="width:40%">
          <span>{{obs.status ?? 'None'}}</span>
        </Panel>
        <Panel header="Severity" class="p-mb-1" style="width:40%">
          <span>{{obs.action ?? 'None'}}</span>
        </Panel>
      </div>
    </template>
    <template v-else>
      <Loader />
    </template>
  </div>
</template>

<script lang="ts">
import { defineComponent, onUpdated, ref } from "vue";
import Loader from "@/components/Loader.vue";
import { Observation } from "@/model/observations.model";
import observationService from "@/services/observations.service";

export default defineComponent({
  name: "Observation",
  components: { Loader },
  props: {
    observation: Object
  },
  setup(props) {
    const obs = ref<Observation>();
    obs.value = props.observation as Observation;
    onUpdated(() => {
      obs.value = props.observation as Observation;
    });

    const saving = ref<boolean>(false);
    const save = async function () {
      if (obs.value) {
        saving.value = true;
        await observationService.saveObservation(obs.value);
        saving.value = false;
      }
    };
    return { obs, save };
  }
});
</script>

<style lang="scss">
.observation-content {
  height: 100%;
}
.observation-panel {  
  width: 100%;
  height: 100%;  
  flex-flow: row wrap;
}
</style>