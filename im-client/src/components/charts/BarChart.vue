<template>
  <div :id="'chart-'+id" style="width:100%;height:100%;">
  </div>
</template>

<script>
import * as d3 from "d3";

export default {
 name: "BarChart",
  props: ['chartData', 'id'],
  mounted() {
    this.main();
  },
  methods: {
    main() {

    }
  }
}
</script>

<style>
</style>