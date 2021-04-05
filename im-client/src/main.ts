import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
//import PrimeVue from 'primevue/config';
import Dialog from 'primevue/dialog';
import Button from 'primevue/button';
import InputText from 'primevue/inputtext';
import Tooltip from 'primevue/tooltip';
import Accordion from 'primevue/accordion';
import AccordionTab from 'primevue/accordiontab';
import RadioButton from 'primevue/radiobutton';

// primevue theming
import 'primevue/resources/themes/saga-blue/theme.css';
import 'primevue/resources/primevue.min.css';
import 'primeicons/primeicons.css';
import 'primeflex/primeflex.css';

const app = createApp(App);

// vue-router
app.use(router);

// prime
//app.use(PrimeVue);
app.component('Dialog', Dialog);
app.component('Button', Button);
app.component('InputText', InputText);
app.component('Accordion', Accordion);
app.component('AccordionTab', AccordionTab);
app.component('RadioButton', RadioButton);
app.directive('tooltip', Tooltip);

app.mount('#app');
