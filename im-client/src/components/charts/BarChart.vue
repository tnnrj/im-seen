<template>
  <div :id="'chart-' + id" style="width: 100%; height: 100%"></div>
</template>

<script>
import * as d3 from "d3";

export default {
 name: "BarChart",
  props: ['chartData', 'id', 'axis1Name', 'axis2Name'],
  emits: ['openStudent'],
  mounted() {
    this.main();
  },
  methods: {
    main() {
      const data = this.chartData;

      // height and width should be calculated by element width
      const clientWidth = document.getElementById('chart-'+this.id).clientWidth;
      const clientHeight = document.getElementById('chart-'+this.id).clientHeight;

      // set the dimensions and margins of the graph
      let margin = {top: 30, right: 30, bottom: 50, left: 30};
      const width = clientWidth - margin.left - margin.right;
      const height = clientHeight - margin.top - margin.bottom;

      const color = d3.scaleOrdinal(data.map(d=>d.name), d3.schemeSpectral[10]);
    
      // append svg
      const svg = d3
        .select("#chart-" + this.id)
        .append("svg")
          .attr("width", "90%")
          .attr("height", "90%")
          .attr("viewBox", [0, 0, clientWidth, clientHeight])  // keeps chart within element bounds
        .append("g")
          .attr("transform", "translate(" + margin.left + "," + margin.top + ")");

      // sort data
      data.sort(function(b, a) {
        return a.value - b.value;
      });

      // add label
      svg.append("text")
        .text(this.axis2Name)
        .style("text-anchor", "left")
        .style("font-size", "12px")
        .attr('transform', 'translate(' + (-20) + ', ' + (5) + ')');

      // add X axis
      // scale for x axis
      let xScale = d3.scaleBand()
          .range([ 0, width ])
          .domain(data.map(function(d) { return d.name; }))
          .padding(0.2);
      svg.append("g")
        .attr("transform", "translate(0," + height + ")")
        .call(d3.axisBottom(xScale))
        .selectAll("text")
        .attr("transform", "translate(-10,0)rotate(-45)") // text tilts 45 degrees
        .style("text-anchor", "end"); // coordinate at the end

      // add Y axis
      // scale for y axis
      let yScale = d3.scaleLinear()
        .domain([0, d3.max(data, d => d.value)])
        .range([ height, margin.top]);
      svg.append("g")
        .call(d3.axisLeft(yScale));

      // draw bars
      svg.selectAll("barVal")
        .data(data)
        .enter()
        .append("rect")
          .attr("x", function(d) { return xScale(d.name); })         
          .attr("width", xScale.bandwidth())
          .attr("fill", d => color(d.name)) // color
          // at the begninning, no bars, so y is always 0
          .attr("y", function(d) { return yScale(0); })
          .attr("height", function(d) { return height - yScale(0); });
          
      // add animation
      svg.selectAll("rect")
        .transition()
        .duration(800)
        .attr("y", function(d) { return yScale(d.value); })
        .attr("height", function(d) { return height - yScale(d.value); });

      // Create a tooltip to show data for mouse hover
      let tooltip = d3.select("#chart-" + this.id)
        .append("div")
        .style("opacity", 0)
        .attr("class", "tooltip")
        .style("position", "absolute")
        .style("text-align", "center")
        // important for tooltip showing smoothly
        .style("pointer-events", "none")
        .style("background-color", "white")
        .style("border", "solid")
        .style("border-width", "1px")
        .style("border-radius", "5px")
        .style("padding", "5px");
          
      // add hover effect
      svg.selectAll("rect")
        .on("mouseover", function (event, d, i) {
              d3.select(this).transition()
                .duration('50')
                .attr('opacity', '.85');
              // show tooltip
              tooltip
                .html( d.name + "<br>" + d.value)
                .style("left", (event.pageX) + "px")
                .style("top", (event.pageY) + "px");
              tooltip.transition()
                .duration(200)          
                .style("opacity", .9);           
        })
        .on("mouseout", function (d, i) {
              d3.select(this).transition()
                  .duration('50')
                  .attr('opacity', '1');
              // turn off tooltip
              tooltip.transition()
                .duration(200)
                .style("opacity", 0);
        })
        .on("click", function (d, i) {
            d3.select(this).transition()
                  .duration('50')
                  .attr('opacity', '1');
            // turn off tooltip
            tooltip.transition()
              .duration(200)
              .style("opacity", 0);
            this.$emit('openStudent', d.id);
        });
    }
  }
}
</script>

<style>
</style>