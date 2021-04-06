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
     // we will use an actual method to get data rather than having this static input
     students : [
       {Name: "John Doe", Severity: 125},
       {Name: "Casey Smith", Severity: 30},
       {Name: "Adam Jenkins", Severity: 24},
       {Name: "Sarah Hopkins", Severity: 35},
       {Name: "Taylor Write", Severity: 40},
       {Name: "Sam Johnson", Severity: 10},
       {Name: "Porter Weatherly", Severity: 50},
       {Name: "Michaela Brown", Severity: 49},
       {Name: "Ryan Huntsman", Severity: 17},
       {Name: "Tanya Merril", Severity: 28},
       {Name: "Brandon Welker", Severity: 36}
     ]
     /*
     width: '90%',
     height: '90%',
     margin: {
       top: 1,
       right: 1,
       left: 1,
       bottom: 1,
     },*/
   };
 },
 mounted() {
   this.init();
 },
 methods: {
   init() {
     //We are accessing the div with the id chart using d3's select method and appending svg
     const svg = d3.create("svg")
       .select("#chart-" + this.id)
       .append("svg")
         .attr("width", this.width)
         .attr("height", this.height)
       .append("g")
       .style(
         "transform",
         `translate(${this.margin.left}em, ${this.margin.top}em)`
       )
      .attr("viewBox", [0, 0, width, height])
      .attr("font-size", 10)
      .attr("font-family", "sans-serif")
      .attr("text-anchor", "middle");

    const root = pack(totalReports);

    const leaf = svg.selectAll("g")
      .data(root.leaves())
      .join("g")
      .attr("transform", d => `translate(${d.x + 1},${d.y + 1})`);

    leaf.append("circle")
      .attr("id", d => (d.leafUid = DOM.uid("leaf")).id)
      .attr("r", d => d.r)
      .attr("fill-opacity", 0.7)
      .attr("fill", d => color(Math.random()*11));

    leaf.append("clipPath")
      .attr("id", d => (d.clipUid = DOM.uid("clip")).id)
      .append("use")
      .attr("xlink:href", d => d.leafUid.href);

    leaf.append("text")
      .attr("clip-path", d => d.clipUid)
      .selectAll("tspan")
      .data(d => d.data.Name.split(/(?=[A-Z][a-z])|\s+/g))
      .join("tspan")
      .attr("x", 0)
      .attr("y", (d, i, nodes) => `${i - nodes.length / 2 + 0.8}em`)
      .text(d => d);

    leaf.append("title")
      .text(d => `${d.data.name === undefined ? "" : `${d.data.name}
                `}${format(d.value)}${d.data.date}`);
   
    return svg.node();
   }
  }
}
</script>

<style>
</style>