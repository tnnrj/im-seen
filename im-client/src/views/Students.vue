<template>
  <div class="content p-d-flex">
    <div class="file-upload-section">
      <file-upload-component/>
    </div>
  </div>
</template>

<script lang="ts">
import { computed, defineComponent, ref } from "vue";
import FileUploadComponent from "@/components/FileUploadComponent.vue"
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
.content {
  height: 45%;
  padding: 1em;
}

.file-upload-section {
  background: white;
  flex: 1;
}
</style>