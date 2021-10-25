<template>
  <div :id="'chart-' + id" style="width: 100%; height: 100%"></div>
</template>

<script>
import * as d3 from "d3";

export default {
  name: "PieChart",
  props: ['chartData', 'id'],
  mounted() {
    this.main();
  },
  methods: {
    main() {
      // height and width should be calculated by element width
      const clientWidth = document.getElementById("chart-" + this.id).clientWidth;
      const clientHeight = document.getElementById("chart-" + this.id).clientHeight;
      const minDimension = width < height ? width : height;

      let margin = {top: 30, right: 30, bottom: 70, left: 60};
      const width = clientWidth - margin.left - margin.right;
      const height = clientHeight - margin.top - margin.bottom;

      //We are accessing the div with the id chart using d3's select method and appending svg
      /***** START D3.js CHART CODE *******/
      const data = this.chartData;
      //  {severity: 1, value: 51},


      const svg = d3
        .select("#chart-" + this.id)
        .append("svg")
        .attr("width", "90%")
        .attr("height", "90%")
        .attr("viewBox", [0, 0, width, height]) // keeps chart within element bounds
        .attr("font-size", 0.02 * minDimension)
        .attr("font-family", "sans-serif")
        .attr("text-anchor", "middle");

      color = d3.scaleOrdinal()
        .domain(data.map(d => d.name))
        .range(d3.schemeSpectral[5]);

      pie = (d) => {
        return d3.pie()
          .padAngle(0.005)
          .sort(null)
          .value(d.value);
      }

      arc = () => {
        const radius = Math.min(width, height) / 2;
        return d3.arc()
          .innerRadius(radius * 0.36)
          .outerRadius(radius * 0.9);
      }

      arcs = pie(data);

      svg.append("g")
        .attr("font-family", "'Work sans', sans-serif")
        .attr("font-size", 24)
        .attr("text-anchor", "middle")
        .append("text")
          .text("Severity");
      
      svg.append("g")
        .attr("font-family", "'Work sans', sans-serif")
        .attr("font-size", 16)
        .attr("text-anchor", "middle")
        .selectAll("text")
        .data(arcs)
        .join("text")
          .attr("transform", d => `translate(${arc.centroid(d)})`)
          .call(text => text.filter(d => (d.endAngle - d.startAngle) > 0.25).append("tspan")
            .attr("y", "-1.2em")
            .attr("font-weight", "400")
            .text(d => d.data.name));

      svg.selectAll("path")
        .data(arcs)
        .join("path")
          .attr("fill", d => color(6-d.data.name))
          .attr("d", arc);

      tooltip = d3.select("svg")
        .append("div")
        .style("opacity", 0)
        .attr("class", "tooltip")
        .style("position", "absolute")
        .style("text-align", "left")
        // important for tooltip showing smoothly
        .style("pointer-events", "none")
        .style("background-color", "white")
        .style("border", "solid")
        .style("border-width", "1px")
        .style("border-radius", "5px")
        .style("padding", "5px");

      // add hover effect
      svg.selectAll("path")
        .on("mouseover", function (event, d, i) {
              d3.select(this).transition()
                .duration('50')
                .attr('opacity', '.85');
              // show tooltip
              tooltip
                .html("Severity: " + d.value)
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
        });
    }
  }
}
</script>

<style>
</style>