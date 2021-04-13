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
     ],
     
     width: '90%',
     height: '90%',
     margin: {
       top: 1,
       right: 1,
       left: 1,
       bottom: 1,
     }
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
       .attr("width", width)
       .attr("height", height)
       // I added these!
       //.attr("viewBox", [0, 0, width, height])
       .attr("font-size", 0.05 * width)
       .attr("font-family", "sans-serif")
       .attr("text-anchor", "middle");

////////////////////////////////////
  // Three function that change the tooltip when user hover / move / leave a cell
  var mouseover = function(d) {
    Tooltip
      .style("opacity", 1)
  }
  var mousemove = function(d) {
    Tooltip
      .html('<u>' + d.key + '</u>' + "<br>" + d.value + " inhabitants")
      .style("left", (d3.mouse(this)[0]+20) + "px")
      .style("top", (d3.mouse(this)[1]) + "px")
  }
  var mouseleave = function(d) {
    Tooltip
      .style("opacity", 0)
  }


      const data = this.students;
      const color = d3.scaleOrdinal(data.map(d=>d), d3.schemeSpectral[10]);
    
    var node = svg.append("g")
    .selectAll("circle")
    .data(data)
    .enter()
    .append("circle")
      .attr("class", "node")
      .attr("r", function(d){ return size(d.value)})
      .attr("cx", width / 2)
      .attr("cy", height / 2)
      .style("fill", d => color(Math.random()*11))
      .style("fill-opacity", 0.8)
      .attr("stroke", "black")
      .style("stroke-width", 1)
      .on("mouseover", mouseover) // What to do when hovered
      .on("mousemove", mousemove)
      .on("mouseleave", mouseleave)
      .call(d3.drag() // call specific function when circle is dragged
           .on("start", dragstarted)
           .on("drag", dragged)
           .on("end", dragended));


    /*
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
        .enter()
        .attr("stroke", "blue")
        .style("stroke-width", 1)
        .attr("id", d => (d.leafUid = Date.now()))
        .attr("r", d => d.r)
        .attr("fill-opacity", 0.7)
        .attr("fill", d => color(Math.random()*11))
        .on("mouseover", mouseover)
        .on("mousemove", mousemove)
        .on("mouseleave", mouseleave)
        .call(d3.drag()
        .on("start", dragstarted)
        .on("drag", dragged)
        .on("end", dragended));

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
   */

///////////////////// MOVEMENT CODE ////////////
  // Features of the forces applied to the nodes:
  var simulation = d3.forceSimulation()
      .force("center", d3.forceCenter().x(width / 2).y(height / 2)) // Attraction to the center of the svg area
      .force("charge", d3.forceManyBody().strength(.1)) // Nodes are attracted one each other of value is > 0
      .force("collide", d3.forceCollide().strength(.2).radius(function(d){ return (size(d.value)+3) }).iterations(1)) // Force that avoids circle overlapping

  // Apply these forces to the nodes and update their positions.
  // Once the force algorithm is happy with positions ('alpha' value is low enough), simulations will stop.
  simulation
      .nodes(data)
      .on("tick", function(d){
        node
            .attr("cx", function(d){ return d.x; })
            .attr("cy", function(d){ return d.y; })
      });

  // What happens when a circle is dragged?
  function dragstarted(d) {
    if (!d3.event.active) simulation.alphaTarget(.03).restart();
    d.fx = d.x;
    d.fy = d.y;
  }
  function dragged(d) {
    d.fx = d3.event.x;
    d.fy = d3.event.y;
  }
  function dragended(d) {
    if (!d3.event.active) simulation.alphaTarget(.03);
    d.fx = null;
    d.fy = null;
  }

   }
/////////////////////////////////////
  }
};
</script>

<style>
</style>