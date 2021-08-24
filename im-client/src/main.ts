import { createApp } from 'vue';
import App from './App.vue';
import router from './router';
import { store, key } from "@/store";

// primevue components
import PrimeVue from 'primevue/config';
import Dialog from 'primevue/dialog';
import Button from 'primevue/button';
import InputText from 'primevue/inputtext';
import Tooltip from 'primevue/tooltip';
import Accordion from 'primevue/accordion';
import AccordionTab from 'primevue/accordiontab';
import RadioButton from 'primevue/radiobutton';
import Splitter from 'primevue/splitter';
import SplitterPanel from 'primevue/splitterpanel';
import ConfirmPopup from 'primevue/confirmpopup';
import ConfirmationService from 'primevue/confirmationservice';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Password from 'primevue/password';
import Menu from 'primevue/menu';

// primevue theming
import 'primevue/resources/themes/saga-blue/theme.css';
import 'primevue/resources/primevue.min.css';
import 'primeicons/primeicons.css';
import 'primeflex/primeflex.css';

const app = createApp(App);

// vue-router
app.use(router);

// vuex
app.use(store, key);

// prime
app.use(PrimeVue);
app.component('Dialog', Dialog);
app.component('Button', Button);
app.component('InputText', InputText);
app.component('Accordion', Accordion);
app.component('AccordionTab', AccordionTab);
app.component('RadioButton', RadioButton);
app.component('Splitter', Splitter);
app.component('SplitterPanel', SplitterPanel);
app.component('ConfirmPopup', ConfirmPopup);
app.component('DataTable', DataTable);
app.component('Column', Column);
app.component('Password', Password);
app.component('Menu', Menu);
app.directive('tooltip', Tooltip);
app.use(ConfirmationService);

app.mount('#app');
