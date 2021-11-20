<template>
  <template v-if="isAuthenticated">
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
    <Dialog header="Error" v-model:visible="showErrorDialog" :modal="true">
      <p>An error has occurred. Please try this action again later.</p>
      <template #footer>
        <Button label="OK" class="p-button-danger" @click="showErrorDialog = false" />
      </template>
    </Dialog>
  </template>
  <!-- don't show side-nav if not logged in -->
  <router-view v-else />
</template>

<script lang="ts">
import { defineComponent, computed, ref } from "vue";
import SideNav from '@/components/SideNav.vue';
import { useStore } from "@/store/index";
import http from '@/services/base-api.service';

export default defineComponent({
  name: 'App',
  components: {
    SideNav
  },
  setup () {
    const store = useStore();
    const isAuthenticated = computed(() => store.state.isAuthenticated);
    if (store.state.isAuthenticated) {
      // need to load user on every refresh
      store.dispatch('getUser');
    }
    const showErrorDialog = ref<boolean>(false);
    http.addErrorHandler(() => {
      // interceptor custom error handling
      if (isAuthenticated.value) {
        showErrorDialog.value = true;
      }
    });
    return { isAuthenticated, showErrorDialog };
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
  padding: 0 0 3em 14em;
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

.login-background {
  height: 100%;
  width: 100%;
  background: rgb(34,193,195);
  background: linear-gradient(168deg, rgba(34,193,195,1) 0%, rgba(253,187,45,1) 100%);
}

.p-toast {
  z-index: 999999;
}
</style>
