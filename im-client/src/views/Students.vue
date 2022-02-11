<template>
  <div class="file-upload-section">
    <file-upload-component />
  </div>
</template>

<script lang="ts">
import { computed, defineComponent, ref } from "vue";
import FileUploadComponent from "@/components/FileUploadComponent.vue";
import * as _ from "lodash";

export default defineComponent({
  name: "Students",
  components: { FileUploadComponent },
  setup() {
    return {};
  },
});
</script>

<style lang="scss">
.file-upload-section {
  background: white;
  margin: 1em;
  padding: 1em;
}
</style>