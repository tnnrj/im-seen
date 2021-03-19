<template>
  <div id="nav" class="p-d-flex p-flex-column">
    <div class="p-mb-6 p-d-flex p-jc-center">
      <img src="./assets/school.png" class="logo" />
    </div>
    <router-link class="p-mb-3" to="/">
      <div class="link">
        <i class="pi pi-chart-bar"></i>
        Dashboard
      </div>
    </router-link>
    <router-link class="p-mb-3" to="/reports">
      <div class="link">
        <i class="pi pi-list"></i>
        Reports
      </div>
    </router-link>
  </div>
  <router-view />
</template>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  background-color: var(--surface-200); 
  margin: 0;
  height: 100vh;
}

.logo {
  border-radius: 10px;
  height: 100px;
  width: 100px;
  margin-top: 100px;
  margin-bottom: 150px;
}

#nav {
  height: 100%;
  width: 250px;
  position: fixed;
  z-index: 1;
  top: 0;
  left: 0;
  background-color: var(--blue-600);
  overflow-x: hidden;
  padding-top: 20px;
  text-align: left;
}

#nav a {
  display: block;
  color: var(--blue-100);
  text-decoration: none;
}

#nav .link {  
  font-weight: bold;
  padding-left: 20px;
  border-radius: 5px;
  transition: 0.3s;
  padding: 10px;
  margin: 0 10px 0 10px;
}

#nav .link:hover {
  background-color: var(--blue-500);
}

#nav a i {
  padding: 0 15px 0 10px;
}

#nav a.router-link-exact-active {
  color: var(--bluegray-800);
  background-color: var(--surface-200);
}
</style>
