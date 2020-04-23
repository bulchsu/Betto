import Vue from 'vue';
import Vuex from 'vuex';
import UserModule from '@/shared/UserModule/module';
import LeagueModule from '@/shared/LeagueModule/module';
import TicketModule from '@/shared/TicketModule/module';

Vue.use(Vuex);

export default new Vuex.Store({
    modules: {
        LeagueModule,
        UserModule,
        TicketModule
    }
});