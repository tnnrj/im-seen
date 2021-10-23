<template>
  <div>
    <Loader *ngIf="!obs || saving" />
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

</style>