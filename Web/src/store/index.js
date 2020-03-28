import Vue from 'vue';
import Vuex from 'vuex';
import { leagueService } from '../shared/LeagueService/league-service';
import { teamService } from '../shared/TeamService/team-service';

import {
    GET_LEAGUES,
    SELECT_LEAGUE,
    GET_LEAGUE_TEAMS
} from './mutation-types';

Vue.use(Vuex);

const state = () => ({
    leagues: [],
    selectedLeague: null,
    leagueTeams: []
});

const mutations = {
    [GET_LEAGUES](state, leagues){
        state.leagues = leagues;
    },
    [SELECT_LEAGUE](state, league){
        state.selectedLeague = league;
    },
    [GET_LEAGUE_TEAMS](state, teams){
        state.leagueTeams = teams;
    }
};

const actions = {
    async getLeaguesAction({ commit }) {
        const leagues = await leagueService.getLeagues();
        commit(GET_LEAGUES, leagues);
    },
    async selectLeagueAction({ commit }, league) {
        commit(SELECT_LEAGUE, league);

        const leagueTeams = await teamService.getLeagueTeams(league.leagueId);
        commit(GET_LEAGUE_TEAMS, leagueTeams);
    } 
};

const getters = {
    getLeagueById: state => id => state.leagues.find(league => league.leagueId === id),
    getSelectedLeague: state => state.selectedLeague,
    getSelectedLeagueTeams: state => state.leagueTeams
};

export default new Vuex.Store({
    strict: process.env.NODE_ENV !== 'production',
    state,
    mutations,
    actions,
    getters
});