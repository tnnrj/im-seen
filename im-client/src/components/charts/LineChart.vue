<template>
  <div :id="'chart-' + id" style="width: 100%; height: 100%"></div>
</template>

<script>
import * as d3 from "d3";
import _ from "lodash";

export default {
  name: "LineChart",
  props: ["chartData", "id", "axis1Name", "axis2Name"],
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

      /** Set margin **/
      const margin = { top: 20, right: 20, bottom: 20, left: 30 };

      /** Helper for parsing date strings **/
      const parser = d3.utcParse("%Y-%m-%dT%H:%M:%S");

      /** Color styling vars **/
      const colorscheme = d3.interpolateTurbo;

      /** Parse data from prop **/
      const columns = _.uniq(this.chartData.map((cd) => cd.date));
      const names = _.uniq(this.chartData.map((cd) => { return { name: cd.name, id: cd.id };}))

      const data = {
        y: this.axis2Name,
        series: names.map((n) => ({
          name: n.name,
          id: n.id,
          values: columns.map((d) => {
            let match = _.find(
              this.chartData,
              (cd) => cd.date == d && cd.name == n.name
            );
            return match ? match.value : 0;
          }),
          color: colorscheme(Math.random())
        })),
        dates: columns.map((d) => parser(d)),
      };

      /** Format x and y axis **/
      const x = d3
        .scaleUtc()
        .domain(d3.extent(data.dates))
        .range([margin.left, width - margin.right]);

      const y = d3
        .scaleLinear()
        .domain([0, d3.max(data.series, (d) => d3.max(d.values))+1])
        .nice()
        .range([height - margin.bottom, margin.top]);

      const xAxis = (g) =>
        g.attr("transform", `translate(0,${height - margin.bottom})`).call(
          d3
            .axisBottom(x)
            .ticks(width / 80)
            .tickSizeOuter(0)
        );

      const yAxis = (g) =>
        g
          .attr("transform", `translate(${margin.left},0)`)
          .call(d3.axisLeft(y))
          .call((g) => g.select(".domain").remove())
          .call((g) =>
            g
              .select(".tick:last-of-type text")
              .clone()
              .attr("x", 3)
              .attr("text-anchor", "start")
              .attr("font-weight", "bold")
              .text(data.y)
          );

      /** Add axis to chart **/
      svg.append("g").call(xAxis);
      svg.append("g").call(yAxis);

      /** Interactive elements! :D **/
      function hover(svg, path) {
        if ("ontouchstart" in document)
          svg
            .style("-webkit-tap-highlight-color", "transparent")
            .on("touchmove", moved)
            .on("touchstart", entered)
            .on("touchend", left)
            .on("click", click);
        else
          svg
            .on("mousemove", moved)
            .on("mouseenter", entered)
            .on("mouseleave", left)
            .on("click", click);

        const dot = svg.append("g").attr("display", "none");

        dot.append("circle").attr("r", 2.5);

        dot
          .append("text")
          .attr("font-family", "sans-serif")
          .attr("font-size", 14)
          .attr("text-anchor", "middle")
          .attr("y", -8);

        function moved(event) {
          event.preventDefault();
          const pointer = d3.pointer(event, this);
          const xm = x.invert(pointer[0]);
          const ym = y.invert(pointer[1]);
          const i = d3.bisectCenter(data.dates, xm);
          const s = d3.least(data.series, (d) => Math.abs(d.values[i] - ym));
          path
            .attr("stroke", (d) => (d === s ? "#000000" : "#ddd"))
            .filter((d) => d === s)
            .raise();
          dot.attr(
            "transform",
            `translate(${x(data.dates[i])},${y(s.values[i])})`
          );
          dot.select("text").text(s.name + ", \n" + s.values[i]);
        }

        function entered(d, i) {
          path
            .style("mix-blend-mode", null)
            .attr("stroke", (d) => d.color);
          dot.attr("display", null);       
        }

        function left() {
          path
            .style("mix-blend-mode", "multiply")
            .attr("stroke", (d) => d.color);
          dot.attr("display", "none");
        }

        function click(event) {
          const pointer = d3.pointer(event, this);
          const xm = x.invert(pointer[0]);
          const ym = y.invert(pointer[1]);
          const i = d3.bisectCenter(data.dates, xm);
          const s = d3.least(data.series, (d) => Math.abs(d.values[i] - ym));
          component.$emit('openStudent', s.id);
        }
      }

      /** Add lines for data **/
      const line = d3
        .line()
        .defined((d) => !isNaN(d))
        .x((d, i) => x(data.dates[i]))
        .y((d) => y(d));

      const path = svg
        .append("g")
        .attr("fill", "none")
        .attr("stroke-width", 1.5)
        .attr("stroke-linejoin", "round")
        .attr("stroke-linecap", "round")
        .selectAll("path")
        .data(data.series)
        .join("path")
        .style("mix-blend-mode", "multiply")
        .attr("d", (d) => line(d.values))
        .attr("stroke", (d) => d.color);

      svg.call(hover, path);
    }
  },
};
</script>

<style>
</style>