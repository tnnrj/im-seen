<template>
  <div id="nav" class="p-d-flex p-flex-column">
    <div class="logo p-d-flex p-jc-center">
      <img src="../assets/school.png" />
    </div>
    <div v-for="item in navItems" :key="item.title">
      <router-link v-if="item.show" class="p-mb-5" v-bind:to="item.link">
        <i class="pi" v-bind:class="item.iconStyle"></i>
        {{ item.title }}
      </router-link>
    </div>
    <div class="account p-d-flex p-flex-column p-jc-end" @click="toggleAccountMenu">
      <i class="pi pi-user p-mb-3"></i>
      <span>{{ userName }}</span>
      <Menu ref="accountMenu" :model="accountMenuItems" :popup="true" />
    </div>
  </div>
</template>

<script lang="ts">
import { computed, defineComponent, ref } from "vue";
import { useStore } from "@/store/index";
import router from "@/router";
import { UserRole } from "@/model/enums.model";

interface NavItem {
  title: string;
  link: string;
  iconStyle: string;
  show: boolean;
};

export default defineComponent({
  name: "SideNav",
  setup() {
    const store = useStore();

    const navItems = computed(() => {
      let isAdmin = store.state.user?.role === UserRole.Administrator;
      return [{
        title: 'Dashboard',
        link: '/',
        iconStyle: 'pi-chart-bar',
        show: true
      },
      {
        title: 'Observations',
        link: '/observations',
        iconStyle: 'pi-list',
        show: true
      },
      {
        title: 'Users',
        link: '/users',
        iconStyle: 'pi-users',
        show: isAdmin
      },
      {
        title: 'Groups',
        link: '/groups',
        iconStyle: 'pi-th-large',
        show: isAdmin
      }];
    });

    const userName = computed(() => {
      let u = store.state.user;
      if (u) {
        return u?.firstName + " " + u?.lastName;
      }
      return "";
    });
    const accountMenu = ref();
    const toggleAccountMenu = function (event) {
      accountMenu.value.toggle(event);
    };
    const accountMenuItems = ref<any[]>([]);
    accountMenuItems.value = [{
      label: 'Sign Out',
      icon: 'pi pi-sign-out', 
      command: () => {
        store.dispatch('logOut');
        router.push('/login');
      }
    }];

    return { navItems, userName, accountMenu, toggleAccountMenu, accountMenuItems };
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
    cursor: pointer;

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

  .p-menu {
    width: 80%;
  }
}
</style>