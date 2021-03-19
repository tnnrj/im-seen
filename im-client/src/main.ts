import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import PrimeVue from 'primevue/config';
import Dialog from 'primevue/dialog';
import Button from 'primevue/button';
import InputText from 'primevue/inputtext';

// primevue theming
import 'primevue/resources/themes/saga-blue/theme.css';
import 'primevue/resources/primevue.min.css';
import 'primeicons/primeicons.css';
import 'primeflex/primeflex.css';

const app = createApp(App);

// vue-router
app.use(router);

// primefaces
app.use(PrimeVue);
app.component('Dialog', Dialog);
app.component('Button', Button);
app.component('InputText', InputText);

app.mount('#app');
