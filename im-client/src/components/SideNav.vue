<template>
  <div id="nav" class="p-d-flex p-flex-column">
    <div class="logo p-d-flex p-jc-center">
      <img src="../assets/school.png" />
    </div>
    <div v-for="item in items" :key="item.title">
      <router-link class="p-mb-5" v-bind:to="item.link">
        <i class="pi" v-bind:class="item.iconStyle"></i>
        {{ item.title }}
      </router-link>
    </div>
    <div class="account p-d-flex p-flex-column p-jc-end">
      <i class="pi pi-user p-mb-3"></i>
      <span>Test User</span>
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
  width: 14em;
  position: fixed;
  z-index: 1;
  top: 0;
  left: 0;
  background-color: white;
  overflow-x: hidden;
  text-align: left;
  box-shadow: inset -2px 0 var(--surface-300);

  a {
    display: block;
    color: var(--bluegray-300);
    text-decoration: none;
    font-weight: 600;
    padding-left: 2em;
    transition: 0.3s;

    &:hover:not(.router-link-exact-active) {
      color: var(--bluegray-500);
    }

    i {
      padding-right: 1em;
    }

    &.router-link-exact-active {
      color: var(--bluegray-800);
      border-right: 2px solid var(--blue-600);

      i {
        color: var(--blue-600);
      }
    }
  }

  .logo {
    margin-top: 10vh;
    margin-bottom: 10vh;

    img {
      border-radius: 10px;
      height: 100px;
      width: 100px;
    }
  }

  .account {
    height: 100%;
    margin-bottom: 7vh;

    i {
      font-size: 2em;
      margin: 0 auto;
      color: var(--bluegray-700);
      border: 3px solid var(--bluegray-700);
      border-radius: 2em;
      padding: .25em;
    }

    span {
      margin: 0 auto;
      color: var(--bluegray-700);
      font-weight: 600;
    }
  }
}
</style>