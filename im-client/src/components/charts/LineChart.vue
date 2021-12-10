<template>
  <div style="height:100%;width:100%; position:relative;">
    <template v-if="showFilter">
      <Button class="p-button-rounded p-button-text p-button-plain" icon="pi pi-filter" @click="toggle" />
      <OverlayPanel ref="overlay">
        <span class="overlay-label">Range: {{new Date(range[0]*1000).toLocaleDateString()}} to {{new Date(range[1]*1000).toLocaleDateString()}}</span>
        <Slider v-model="range" @slideend="render" :min="min" :max="max" :range="true" style="width:10em" />
      </OverlayPanel>
    </template>
    <div :id="'chart-' + id" style="width: 100%; height: 100%"></div>
  </div>
</template>

<script>
import * as d3 from "d3";
import * as moment from 'moment';
import _ from "lodash";

export default {
  name: "LineChart",
  props: ["chartData", "id", "axis1Name", "axis2Name", "showFilter"],
  emits: ['openStudent'],
  data() {
    return { range: [0,0], min: 0, max: Math.floor(moment().toDate().getTime() / 1000) }
  },
  mounted() {
    this.render();
  },
  methods: {
    render() {
      const component = this;
      let cdata = this.chartData;

      if (this.showFilter) {
        const dates = cdata.map((cd) => new Date(cd.date).getTime());
        this.min = Math.floor(_.min(dates) / 1000);

        if (this.range[0] == 0 && this.range[1] == 0) {
          let ordered = _.orderBy(dates, d => d, 'desc');
          this.range[0] = Math.floor(ordered[ordered.length > 20 ? 20 : ordered.length-1] / 1000);
          this.range[1] = Math.floor(_.max(dates) / 1000);

          if (this.range[1] > this.max) {
            // shouldn't happen, quick fix for timezone bugs
            this.max = this.range[1];
          }
        }

        cdata = _.filter(cdata, cd => {
          let val = Math.floor(new Date(cd.date).getTime() / 1000);
          return val >= this.range[0] && val <= this.range[1];
        });
      }
      
      // if redrawing, need to ensure the canvas is blank
      d3.selectAll("#chart-" + this.id + " > *").remove();

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
        .style("cursor", "pointer")
        .attr("font-family", "sans-serif")
        .attr("text-anchor", "middle")
        .style("overflow", "visible");

      /** Set margin **/
      const margin = { top: 20, right: 20, bottom: 20, left: 30 };

      /** Helper for parsing date strings **/
      const parser = d3.utcParse("%Y-%m-%dT%H:%M:%S");

      /** Parse data from prop **/
      const columns = _.uniq(cdata.map((cd) => cd.date));
      const names = _.uniqBy(cdata.map((cd) => { return { name: cd.name, id: cd.id };}), cd => cd.id);
      const color = d3.scaleOrdinal(names, d3.schemeSpectral[10]);
      const data = {
        y: this.axis2Name,
        series: names.map((n) => ({
          name: n.name,
          id: n.id,
          values: columns.map((d) => {
            let match = _.find(
              cdata,
              (cd) => cd.date == d && cd.name == n.name
            );
            return match ? match.value : 0;
          }),
          color: color(Math.random()*11)
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
        g.attr("transform", `translate(0,${height - margin.bottom})`)
          .style("font-size", "12px")
          .call(
            d3
              .axisBottom(x)
              .ticks(width / 80)
              .tickSizeOuter(0)
        );

      const yAxis = (g) =>
        g
          .attr("transform", `translate(${margin.left},0)`)
          .style("font-size", "14px")
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
        if ("ontouchstart" in document) svg
            .style("-webkit-tap-highlight-color", "transparent")
            .on("touchmove", moved)
            .on("touchstart", entered)
            .on("touchend", left)
            .on("click", click);
        else svg
            .on("mousemove", moved)
            .on("mouseenter", entered)
            .on("mouseleave", left)
            .on("click", click);

        const dot = svg.append("g").attr("display", "none");

        dot.append("circle").attr("r", 2.5);

        dot
          .append("text")
          .attr("font-family", "Open Sans")
          .attr("font-size", "14px")
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
          if (s.id) {
            component.$emit('openStudent', s.id);
          }
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
    },    
    toggle(event) {
      this.$refs.overlay.toggle(event);
    }
  },
};
</script>

<style scoped>
.p-button {
  position: absolute;
  top: 10px;
  right: 20px;
}
.overlay-label {
  display: block;
  margin-bottom: .5em;
  font-size: .8em;
}
</style>