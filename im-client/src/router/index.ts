import { createRouter, createWebHistory, RouteRecordRaw } from "vue-router";
import Login from "../views/Login.vue";
import Dashboard from "../views/Dashboard.vue";
import { store } from "@/store";
import { UserRole } from "@/model/enums.model";

const routes: Array<RouteRecordRaw> = [  
  {
    path: "/login",
    name: "Login",
    component: Login,
    meta: { guest: true }
  },
  {
    path: "/register",
    name: "Register",
    component: () => import("@/views/Register.vue"),
    props: route => ({ username: route.query.username, token: route.query.token })
  },
  {
    path: "/",
    name: "Dashboard",
    component: Dashboard,
    meta: { requiresAuth: true }
  },
  {
    path: "/observations",
    name: "Observations",
    // route level code-splitting
    // this generates a separate chunk (observations.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "observations" */ "@/views/Observations.vue"),
    meta: { requiresAuth: true }
  },
  {
    path: "/users",
    name: "Users",
    component: () => import("@/views/Users.vue"),
    meta: { requiresAuth: true, adminOnly: true }
  },
  {
    path: "/groups",
    name: "Groups",
    component: () => import("@/views/Groups.vue"),
    meta: { requiresAuth: true, adminOnly: true }
  },
  {
    path: "/students",
    name: "Students",
    component: () => import("@/views/Students.vue"),
    meta: { requiresAuth: true, adminOnly: true }
  },
];

// create router singleton
const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
});

router.beforeEach((to, from, next) => {
  // auth guard
  if (to.matched.some(record => record.meta.requiresAuth) && !store.state.isAuthenticated) {
    next('/login');
    return;
  }
  // login redirect
  if (to.matched.some(record => record.meta.guest) && store.state.isAuthenticated) {
    next('/');
    return;
  }
  // role check
  if (to.matched.some(record => record.meta.adminOnly)) {
    let checkAdminStatus = () => {      
      if (store.state.user?.role !== UserRole.Administrator) {
        next('/');
        return;
      }
      next();
    };

    if (!store.state.user) {
      store.dispatch('getUser').then(checkAdminStatus);
    }
    else {
      checkAdminStatus();
    }
  }
  else {
    next();
  }
})

export default router;
