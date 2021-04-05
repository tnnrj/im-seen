<template>
  <div :id="'chart-'+id">
  </div>
</template>

<script>
import * as d3 from "d3";

export default {
  name: "BubbleCloudChart",
  props: ['chartData', 'id'],
  data() {
   return {
     width: '90%',
     height: '90%',
     margin: {
       top: 1,
       right: 1,
       left: 1,
       bottom: 1,
     },
   };
 },
 mounted() {
   this.init();
 },
 methods: {
   init() {
     //We are accessing the div with the id chart using d3's select method and appending svg
     const svg = d3
       .select("#chart")
       .append("svg")
         .attr("width", this.width)
         .attr("height", this.height)
       .append("g")
       .style(
         "transform",
         `translate(${this.margin.left}em, ${this.margin.top}em)`
       );
   }
 }
}
</script>

<style>
</style>