<template>
  <div :id="'chart-' + id" style="width: 100%; height: 100%"></div>
</template>

<script>
import * as d3 from "d3";

export default {
  name: "PieChart",
  props: ['chartData', 'id'],
  emits: ['openStudent'],
  mounted() {
    this.main();
  },
  methods: {
    main() {
      const component = this;

      // height and width should be calculated by element width
      const width = document.getElementById("chart-" + this.id).clientWidth;
      const height = document.getElementById("chart-" + this.id).clientHeight;
      const minDimension = width < height ? width : height;

      //We are accessing the div with the id chart using d3's select method and appending svg
      /***** START D3.js CHART CODE *******/
      const data = this.chartData;
      // format of data: {name: 1, value: 51},

      const color = d3.scaleOrdinal()
        .domain(data.map(d => d.name))
        .range(d3.schemeSpectral[5]);

      //////////////////////////////////////////////////////////////////////////////////////////////// this right here is the buggy shit
      const arc = d3.arc()
        .innerRadius( 0.5 * height / 2 )
        .outerRadius( 0.85 * height / 2 );

      const pie = d3.pie()
        .value(d => d.value);

      const labelArcs = d3.arc()
        .innerRadius( 0.65 * height /2 )
        .outerRadius( 0.65 * height / 2 );

      const pieArcs = pie( data );

      const svg = d3
        .select("#chart-" + this.id)
        .append("svg")
        .attr("width", "90%")
        .attr("height", "90%")
        .attr("viewBox", [0, 0, width, height]) // keeps chart within element bounds
        .attr("font-family", "sans-serif")
        .attr("text-anchor", "middle");
        
      svg.append('g')
          .attr('class', 'donut-container')
          .attr('transform', `translate(${ width / 2 }, ${ height / 2})` )
          .selectAll('path')
          .data(pieArcs)
          .join('path')
            .style('stroke', 'white')
            .style('stroke-width', 2)
            .style('fill', d => color( d.data.name ))
            .attr('d', arc);
        
      const text = svg.append('g')
          .attr('class', 'lablels-container')
          .attr('transform', `translate(${width/2},${height/2})`)
          .selectAll('text')
          .data(pieArcs)
          .join('text')
            .attr('transform', d => `translate(${ labelArcs.centroid(d) })`)
            .attr('text-anchor', 'middle');
        
      text.selectAll('tspan')
        .data( d => [
          d.data.name,
        ])
        .join('tspan')
          .attr('x', 0)
          .style('font-family', 'sans-serif')
          .style('font-size', 24)
          .style('font-weight', (d,i) => i ? undefined : 'bold')
          .style('fill', '#222')
          .attr('dy', (d,i) => i ? '1.2em' : 0)
          .text(d => d);

      ///////////////////////////////////////////////////////////////////////////////////////////////

      const tooltip = d3.select("svg")
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
      svg.selectAll("path")
        .on("mouseover", function (event, d, i) {
              d3.select(this).transition()
                .duration('50')
                .attr('opacity', '.85');
              // show tooltip
              tooltip
                .html(d.value)
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
            if (d.id) component.$emit('openStudent', d.id);
        });
    }
  }
}
</script>

<style>
</style>