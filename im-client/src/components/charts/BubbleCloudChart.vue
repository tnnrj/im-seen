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
     // height and width should be calculated by element width
     const width = document.getElementById('chart-'+this.id).clientWidth;
     const height = document.getElementById('chart-'+this.id).clientHeight;

     //We are accessing the div with the id chart using d3's select method and appending svg
     const svg = d3
       .select("#chart-" + this.id)
       .append("svg")                   
       .attr("width", "90%")
       .attr("height", "90%")
       .attr("viewBox", [0, 0, width, height])  // keeps chart within element bounds
       .attr("font-size", 0.03 * height)       // depending on component layout, this may need to be set to min(width, height)
       .attr("font-family", "sans-serif") 
       .attr("text-anchor", "middle");

      // get data from the method above
      const data = this.students;
      // initialize color scheme
      const color = d3.scaleOrdinal(data.map(d=>d), d3.schemeSpectral[10]);
    
      // this method packs the data into scaled bubbles based off of weighted severity
      const root = d3.pack()
        .size([width-2, height-2])
        .padding(3)
        (d3.hierarchy({children: data})
        .sum(d => d.Severity));

      // each leaf represents one student - this is selecting nodes from the data essentially
      const leaf = svg.selectAll("g")
        .data(root.leaves())
        .join("g")
        .attr("transform", d => `translate(${d.x + 1},${d.y + 1})`)
        // these two atti\ributes are the event handlers. They control interactive elements
        .on("mouseover", function() {
          d3.select(this)
          .attr("opacity", 0.5)
          .attr("font-size", 0.05 * height);
        })
        .on("mouseleave", function() {
          d3.select(this)
          .attr("opacity", 1)
          .attr("font-size", 0.03 * height);
        });

      // draw the circle  
      leaf.append("circle")
        .attr("stroke", "black")
        .style("stroke-width", 1)
        .attr("id", d => (d.leafUid = Date.now()))
        .attr("r", d => d.r)
        .attr("fill-opacity", 0.7)
        .attr("fill", d => color(Math.random()*11))

      leaf.append("clipPath")
        .attr("id", d => (d.clipUid = Date.now()))
        .append("use")
        .attr("xlink:href", d => d.leafUid.href);

      // draw the text
      leaf.append("text")
        .attr("clip-path", d => d.clipUid)
        .selectAll("tspan")
        .data(d => d.data.Name.split(/(?=[A-Z][a-z])|\s+/g)) //+ "\n(" + d.data.Severity + ")")
        .join("tspan")
        .attr("x", 0)
        .attr("y", (d, i, nodes) => `${i - nodes.length / 2 + 0.8}em`)
        .text(d => d);

      // title is the student's name
      leaf.append("title")
        .text(d => d.data.name === undefined ? "" : d.data.name);

//////////////////////////////////// END D3.js CODE
   }
}}
</script>

<style>
</style>