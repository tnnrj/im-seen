<template>
  <div class="element-content p-d-flex p-flex-column p-jc-center">
    <div id="chart">
      sample content
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
  name: "DashboardElement"
});
</script>

<style lang="scss">
.element-content {
  width: 100%;
  height: 100%;
  background-color: white;
  border-radius: 10px;
}
</style>