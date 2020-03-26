import Vue from 'vue';
import Vuex from 'vuex';
import { leagueService } from '../shared/LeagueService/league-service';

import {
    GET_LEAGUES,
    SELECT_LEAGUE
} from './mutation-types';

Vue.use(Vuex);

const state = () => ({
    leagues: [],
    selectedLeague: null
});

const mutations = {
    [GET_LEAGUES](state, leagues){
        state.leagues = leagues;
    },
    [SELECT_LEAGUE](state, league){
        state.selectedLeague = league;
    }
};

const actions = {
    async getLeaguesAction({ commit }) {
        const leagues = await leagueService.getLeagues();
        commit(GET_LEAGUES, leagues);
    },
    selectLeagueAction({ commit }, league) {
        commit(SELECT_LEAGUE, league);
    } 
};

const getters = {
    getLeagueById: state => id => state.leagues.find(league => league.leagueId === id),
    getSelectedLeague: state => state.selectedLeague
};

export default new Vuex.Store({
    strict: process.env.NODE_ENV !== 'production',
    state,
    mutations,
    actions,
    getters
});