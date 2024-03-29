<template>
  <div style="height:100%;width:100%; position:relative;">
    <template v-if="showFilter">
      <Button class="p-button-rounded p-button-text p-button-plain" icon="pi pi-filter" @click="toggle" />
      <OverlayPanel ref="overlay">
        <span class="overlay-label">Show: {{count}}</span>
        <Slider v-model="count" @slideend="render" :min="1" :max="max" style="width:10em" />
      </OverlayPanel>
    </template>
    <div :id="'chart-' + id" style="width: 100%; height: 100%"></div>
  </div>
</template>

<script>
import * as d3 from "d3";

export default {
 name: "BarChart",
  props: ['chartData', 'id', 'axis1Name', 'axis2Name', 'showFilter'],
  emits: ['openStudent'],
  data() {
    return { count: 10, max: 10 }
  },
  mounted() {
    this.render();
  },
  methods: {
    render() {
      const component = this;
      this.max = this.chartData.length;
      if (this.count > this.max) {
        this.count = this.max;
      }

      // sort data
      let data = _.filter(this.chartData, cd => cd.value != 0);
      data.sort(function(b, a) {
        return a.value - b.value;
      });

      // only take certain number of bars based on slider
      data = data.slice(0, this.count);

      // if redrawing, need to ensure the canvas is blank
      d3.selectAll("#chart-" + this.id + " > *").remove();

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
          .attr("overflow", "visible")
        .append("g")
          .attr("transform", "translate(" + margin.left + "," + margin.top + ")");

      // add label
      svg.append("text")
        .text(this.axis2Name)
        .style("text-anchor", "left")
        .style("font-size", ".8em")
        .style("font-family", "Open Sans")
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
        .style("font-size", "1.3em")
        .style("font-family", "Open Sans")
        .style("text-anchor", "end"); // coordinate at the end

      // add Y axis
      // scale for y axis
      let yScale = d3.scaleLinear()
        .domain([0, d3.max(data, d => d.value)])
        .range([ height, margin.top]);
      svg.append("g")
        .style("font-size", ".8em")
        .style("font-family", "Open Sans")
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
                .style("cursor", "pointer")
                .attr('opacity', '.85');
              // show tooltip
              tooltip
                .html( d.name + "<br>" + d.value)
                .style("left", (event.layerX) + "px")
                .style("top", (event.layerY) + "px");
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
          if (i.id) {
            component.$emit('openStudent', i.id);
          }
        });
    },
    toggle(event) {
      this.$refs.overlay.toggle(event);
    }
  }
}
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