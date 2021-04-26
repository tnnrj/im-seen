<template>
  <SideNav />
  <div class="app-content">
    <router-view />
  </div>
  <div class="app-footer p-d-flex">
    <span class="spacer"></span>
    <div class="p-d-flex p-jc-center">
      <span class="text-small p-m-3">(c) Paladin BI systems informatics etc</span>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import SideNav from '@/components/SideNav.vue';

export default defineComponent({
  name: 'App',
  components: {
    SideNav
  }
})
</script>

<style lang="scss">
html {
  font-size: 17px;
  height: 100%;
}
@media (max-width: 900px) {
  html { font-size: 13px; }
}

body {
  margin: 0;
  height: 100%;
  background-color: var(--surface-200);
}

#app {
  font-family: Open Sans, Helvetica, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  height: 100%;
  text-align: center;
  color: #2c3e50;
  transition: 0.3s;
}

.app-content {
  padding: .5em .5em 3.5em 14.5em;
  height: 100%;
}

.app-footer {
  position: fixed;
  width: 100%;
  height: 3em;
  bottom: 0;
  background-color: white;

  .spacer {
    width: 14em;
  }

  :last-child {
    flex: 1;
  }
}

.text-small {
  font-size: .75em;
}
</style>
