import { createRouter, createWebHistory, RouteRecordRaw } from "vue-router";
import Dashboard from "../views/Dashboard.vue";

const routes: Array<RouteRecordRaw> = [
  {
    path: "/",
    name: "Dashboard",
    component: Dashboard
  },
  {
    path: "/reports",
    name: "Reports",
    // route level code-splitting
    // this generates a separate chunk (reports.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () =>
      import(/* webpackChunkName: "reports" */ "@/views/Reports.vue")
  }
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
});

export default router;
