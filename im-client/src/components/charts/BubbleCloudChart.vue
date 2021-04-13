<template>
  <div :id="'chart-'+id" style="width:100%;height:100%;">
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
   };
 },
 mounted() {
   this.main();
 },
 methods: {
   main() { 
// these need to not be static, but this.width/this.height does not return pixel values
     const width = 250;
     const height =  250;
     //We are accessing the div with the id chart using d3's select method and appending svg
     const svg = d3
       .select("#chart-" + this.id)
       .append("svg")
       .attr("width", "90%")
       .attr("height", "90%")
       // I added these!
       .attr("viewBox", [0, 0, width, height])
       .attr("preserveAspectRatio", "xMinYMin")
       .attr("font-size", 0.05 * width)
       .attr("font-family", "sans-serif")
       .attr("text-anchor", "middle");

////////////////////////////////////
      const data = this.students;
      const color = d3.scaleOrdinal(data.map(d=>d), d3.schemeSpectral[10]);
    
      const root = d3.pack()
        .size([width-2, height-2])
        .padding(4)
        (d3.hierarchy({children: data})
        .sum(d => d.Severity));

      const leaf = svg.selectAll("g")
        .data(root.leaves())
        .join("g")
        .attr("transform", d => `translate(${d.x + 1},${d.y + 1})`);

      leaf.append("circle")
        .attr("id", d => (d.leafUid = Date.now()))
        .attr("r", d => d.r)
        .attr("fill-opacity", 0.7)
        .attr("fill", d => color(Math.random()*11));

      leaf.append("clipPath")
        .attr("id", d => (d.clipUid = Date.now()))
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
        .text(d => d.data.name === undefined ? "" : d.data.name);
   }
/////////////////////////////////////
  }
};
</script>

<style>
</style>