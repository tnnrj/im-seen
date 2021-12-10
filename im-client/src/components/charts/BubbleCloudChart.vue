<template>
  <div style="height:100%;width:100%; position:relative;">
    <template v-if="showFilter">
      <Button class="p-button-rounded p-button-text p-button-plain" icon="pi pi-filter" @click="toggle" />
      <OverlayPanel ref="overlay">
        <span class="overlay-label">Show: {{count}}</span>
        <Slider v-model="count" @slideend="render" :min="1" :max="max" style="width:10em" />
      </OverlayPanel>
    </template>
    <div :id="'chart-'+id" style="width:100%;height:100%;"></div>
  </div>
</template>

<script>
import * as d3 from "d3";

export default {
  name: "BubbleCloudChart",
  props: ['chartData', 'id', 'showFilter'],
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
      let data = this.chartData;
      data.sort(function(b, a) {
        return a.value - b.value;
      });

      // only take certain number of bubbles based on slider
      data = data.slice(0, this.count);

      // if redrawing, need to ensure the canvas is blank
      d3.selectAll("#chart-" + this.id + " > *").remove();

      // height and width should be calculated by element width
      const width = document.getElementById('chart-'+this.id).clientWidth;
      const height = document.getElementById('chart-'+this.id).clientHeight;
      const minDimension = width < height ? width : height;

      //We are accessing the div with the id chart using d3's select method and appending svg
      const svg = d3
        .select("#chart-" + this.id)
        .append("svg")                   
        .attr("width", "90%")
        .attr("height", "90%")
        .attr("viewBox", [0, 0, width, height])  // keeps chart within element bounds
        .attr("font-size", 0.02 * minDimension)
        .attr("font-family", "sans-serif") 
        .attr("text-anchor", "middle");

      // initialize color scheme
      const color = d3.scaleOrdinal(data, d3.schemeSpectral[10]);
    
      // this method packs the data into scaled bubbles based off of weighted severity
      const root = d3.pack()
        .size([width-2, height-2])
        .padding(3)
        (d3.hierarchy({children: data})
        .sum(d => d.value));

      // each leaf represents one student - this is selecting nodes from the data essentially
      const leaf = svg.selectAll("g")
        .data(root.leaves())
        .join("g")
        .attr("transform", d => `translate(${d.x + 1},${d.y + 1})`)
        // these two atti\ributes are the event handlers. They control interactive elements
        .on("mouseover", function() {
          d3.select(this)
          .attr("opacity", 0.5)
          .attr("font-size", 0.03 * minDimension);
        })
        .on("mouseleave", function() {
          d3.select(this)
          .attr("opacity", 1)
          .attr("font-size", 0.02 * minDimension);
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
        .selectAll("tspan")
        .data(d => d)
        .join("tspan")
        .text(d => d.data.name)
        .style("font-family", "Open Sans")
        .style("font-size", d => d.r / 4 + "px")
        .attr("dy", ".35em");

        // Create a tooltip to show data for mouse hover
      let tooltip = d3.select("#chart-" + this.id)
        .append("div")
        .style("opacity", 0)
        .attr("class", "tooltip")
        .style("position", "absolute")
        .style("text-align", "center")
        .style("font-family", "Open Sans")
        // important for tooltip showing smoothly
        .style("pointer-events", "none")
        .style("background-color", "white")
        .style("border", "solid")
        .style("border-width", "1px")
        .style("border-radius", "5px")
        .style("padding", "5px");
          
      // add hover effect
      svg.selectAll("g")
        .on("mouseover", function (event, d, i) {
              d3.select(this).transition()
                .duration('50')
                .attr('opacity', '.85')
                .style('cursor', 'pointer');
              // show tooltip
              tooltip
                .html(d.data.name + "<br>" + d.data.value)
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
        .on("click", function (event, d, i) {
          if (d.data.id) {
            component.$emit('openStudent', d.data.id);
          }
        });

//////////////////////////////////// END D3.js CODE
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