<template>
  <div id="nav" class="p-d-flex p-flex-column">
    <div class="p-mb-6 p-d-flex p-jc-center">
      <img src="../assets/school.png" class="logo" />
    </div>
    <div class="link" v-for="item in items" :key="item.title">
      <router-link class="p-mb-3" v-bind:to="item.link">
        <i class="pi" v-bind:class="item.iconStyle"></i>
        {{ item.title }}
      </router-link>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";

interface NavItem {
    title: string;
    link: string;
    iconStyle: string;
    show: boolean;
};

export default defineComponent({
  name: "SideNav",
  setup() {
    const isAdmin = true;

    const items = ref<NavItem[]>([]);
    items.value = [{
      title: 'Dashboard',
      link: '/',
      iconStyle: 'pi-chart-bar',
      show: true
    },
    {
      title: 'Reports',
      link: '/reports',
      iconStyle: 'pi-list',
      show: true
    },
    {
      title: 'Users',
      link: '/users',
      iconStyle: 'pi-users',
      show: isAdmin
    }];

    return { items };
  }
});
</script>

<style lang="scss">
#nav {
  height: 100%;
  width: 250px;
  position: fixed;
  z-index: 1;
  top: 0;
  left: 0;
  background-color: white;
  overflow-x: hidden;
  padding-top: 20px;
  text-align: left;
}

#nav a {
  display: block;
  color: var(--bluegray-300);
  text-decoration: none;
  font-weight: 600;
  padding-left: 20px;
  border-radius: 5px;
  padding: 10px;
  margin: 0 30px 0 30px;
  transition: 0.3s;
}

#nav a:hover:not(.router-link-exact-active) {
  color: var(--bluegray-500);
}

#nav a i {
  padding: 0 30px 0 10px;
}

#nav a.router-link-exact-active {
  color: var(--bluegray-800);

  i {
    color: var(--blue-800);
  }
}

.logo {
  border-radius: 10px;
  height: 100px;
  width: 100px;
  margin-top: 100px;
  margin-bottom: 150px;
}
</style>