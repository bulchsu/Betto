import '@babel/polyfill'
import 'mutationobserver-shim'
import Vue from 'vue'
import './plugins/bootstrap-vue'
import App from './App.vue'
import '@fortawesome/fontawesome-free/css/all.css'
import '@fortawesome/fontawesome-free/js/all.js'
import vuetify from './plugins/vuetify';
import store from './store';
import Snotify from 'vue-snotify';
import 'vue-snotify/styles/material.css';

Vue.config.productionTip = false;
Vue.use(Snotify, {
  toast: {
    timeout: 3000
  }
});

const vm = new Vue({
  store,
  vuetify,
  render: h => h(App)
}).$mount('#app')

export default vm;