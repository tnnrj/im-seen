<template>
  <div :id="'chart-' + id" style="width: 100%; height: 100%"></div>
</template>

<script>
import * as d3 from "d3";
import _ from "lodash";

export default {
  name: "PieChart",
  props: ["chartData", "id", "axis1Name", "axis2Name"],
  mounted() {
    this.main();
  },
  methods: {
    main() {
      // height and width should be calculated by element width
      const width = document.getElementById("chart-" + this.id).clientWidth;
      const height = document.getElementById("chart-" + this.id).clientHeight;
      const minDimension = width < height ? width : height;

      //We are accessing the div with the id chart using d3's select method and appending svg
      /***** START D3.js CHART CODE *******/
      
      const svg = d3
        .select("#chart-" + this.id)
        .append("svg")
        .attr("width", "90%")
        .attr("height", "90%")
        .attr("viewBox", [0, 0, width, height]) // keeps chart within element bounds
        .attr("font-size", 0.02 * minDimension)
        .attr("font-family", "sans-serif")
        .attr("text-anchor", "middle")
        .style("overflow", "visible");

      color = d3.scaleOrdinal()
        .domain(data.map(d => d.Severity))
        .range(d3.schemeSpectral[5]);

      pie = () => {
        return d3.pie()
          .padAngle(0.005)
          .sort(null)
          .value(d => d.Total);
      }

      arc = () => {
        const radius = Math.min(width, height) / 2;
        return d3.arc()
          .innerRadius(radius * 0.36)
          .outerRadius(radius * 0.9);
      }

      const donutChart = () => {
        const arcs = pie(data);
        const svg = d3.create("svg")
          .attr("viewBox", [-width / 2, -height / 2, width, height])
        
        svg.selectAll("path")
          .data(arcs)
          .join("path")
            .attr("fill", d => color(d.data.Severity))
            .attr("d", arc)
            .on("mouseenter", onMouseEnter)
            .on("mouseleave", onMouseLeave)
        
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
              .text(d => d.data.Severity))
        // 
        //  .call(text => text.filter(d => (d.endAngle - d.startAngle) > 0.25).append("tspan")
        //      .attr("x", 0)
        //      .attr("y", "0.7em")
        //      .attr("fill-opacity", 0.7)
        //      .text(d => d.data.Total));
        
        svg.append("g")
          .attr("font-family", "'Work sans', sans-serif")
          .attr("font-size", 24)
          .attr("text-anchor", "middle")
          .append("text")
            .text("Severity");
        
        const chart = svg.node();
        
        const tooltip = d3.create("div")
          .attr("id", "tooltip")
          .attr("class", "tooltip")
          .html(`
            <div class="tooltip-name">Severity 
              <span id="category"></span>
            </div>
            <div class="tooltip-value">
              <span id="quantity"></span> Reports
            </div>
          `)
        
        function onMouseEnter(d) {
          const x = arc.centroid(d)[0] + (width / 2);
          const y = arc.centroid(d)[1] + (height / 2);
          
          tooltip.style("opacity", 1);
          tooltip.style("transform", `translate(calc( -50% + ${x}px), calc(-100% + ${y}px))`);
          tooltip.select("#category")
            .text(d.data.Severity);
          tooltip.select("#quantity")
            .text(d.data.Total);
        }

        function onMouseLeave() {
          tooltip.style("opacity", 0);
        }

        return html`
          <figure style="max-width: 100%;">
            <div id="wrapper" class="wrapper">
              ${tooltip.node()}
              ${chart}
            </div>
          </figure>
        </div>`;
      }
    }
  }
}
</script>

<style>
  .wrapper {
    position: relative;
  }

  .tooltip {
    background-color: #fff;
    box-shadow: 0 6px 8px rgba(52, 73, 94, .2), 0 1px 1px rgba(52, 73, 94, 0.1);
    font-family: "Work Sans", sans-serif;
    left: 0;
    opacity: 0;
    padding: 0.5em 1em;
    pointer-events: none;
    border-radius: 5px;
    position: absolute;
    text-align: center;
    top: -12px;
    transition: opacity 0.2s linear, transform 0.2s ease-in-out;
    z-index: 1;
  }

  .tooltip:before {
    background-color: #fff;
    border-left-color: transparent;
    border-top-color: transparent;
    bottom: 0;
    content: '';
    height: 12px;
    left: 50%;
    position: absolute;
    transform-origin: center center;
    transform: translate(-50%, 50%) rotate(45deg);
    width: 12px;
    z-index: 1;
  }

  .tooltip-name {
    margin-bottom: 0.2em;
    font-size: 1em;
    line-height: 1.4;
    font-weight: 700;
  }

  .tooltip-value {
    margin-bottom: 0.2em;
    font-size: 0.8em;
    line-height: 1.4;
    font-weight: 400;
  }
</style>