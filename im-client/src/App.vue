<template>
  <SideNav />
  <div class="content">
    <router-view />
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

<style>
body {
  margin: 0;
  background-color: var(--surface-200);
}

#app {
  font-family: Exo, Open Sans, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  transition: 0.3s;
}

.content {
  margin: 30px;
  margin-left:280px;
}
</style>
