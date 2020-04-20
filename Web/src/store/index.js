import Vue from 'vue';
import Vuex from 'vuex';
import { leagueService } from '../shared/LeagueService/league-service';
import { teamService } from '../shared/TeamService/team-service';
import { userService } from '../shared/UserService/user-service';
import { gameService } from '../shared/GameService/game-service';

import {
    GET_LEAGUES,
    SELECT_LEAGUE,
    GET_LEAGUE_TEAMS,
    GET_LEAGUE_GAMES,
    CHANGE_TAB,
    LOGIN,
    LOGOUT
} from './mutation-types';

Vue.use(Vuex);

const state = () => ({
    leagues: [],
    selectedLeague: null,
    leagueTeams: [],
    leagueGames: [],
    user: null,
    selectedTab: 0
});

const mutations = {
    [GET_LEAGUES](state, leagues) {
        state.leagues = leagues;
    },
    [SELECT_LEAGUE](state, league) {
        state.selectedLeague = league;
    },
    [GET_LEAGUE_TEAMS](state, teams) {
        state.leagueTeams = teams;
    },
    [GET_LEAGUE_GAMES](state, games) {
        state.leagueGames = games;
    },
    [CHANGE_TAB](state, tab) {
        state.selectedTab = tab;
    },
    [LOGIN](state, user) {
        state.user = user;
    },
    [LOGOUT](state) {
        state.user = null;
    }
};

const actions = {
    async getLeaguesAction({ commit }) {
        const leagues = await leagueService.getLeagues();
        commit(GET_LEAGUES, leagues);
    },
    async selectLeagueAction({ commit }, league) {
        commit(GET_LEAGUE_TEAMS, null);
        commit(GET_LEAGUE_GAMES, null);
        commit(SELECT_LEAGUE, league);
    },
    async getLeagueTeamsAction({ commit }, league) {
        const leagueTeams = await teamService.getLeagueTeams(league.leagueId);
        commit(GET_LEAGUE_TEAMS, leagueTeams);
    },
    async getLeagueGamesAction({ commit }, league) {
        const leagueGames = await gameService.getLeagueGames(league.leagueId);
        commit(GET_LEAGUE_GAMES, leagueGames);
    },
    selectTabAction({ commit }, tab) {
        commit(CHANGE_TAB, tab);
    },
    async loginAction({ commit }, loginData) {
        const user = await userService.login(loginData.username, loginData.password);
        commit(LOGIN, user);
    },
    logoutAction({ commit }) {
        commit(LOGOUT);
    }
};

const getters = {
    getSelectedLeague: state => state.selectedLeague,
    getSelectedLeagueTeams: state => state.leagueTeams,
    getSelectedLeagueGames: state => state.leagueGames,
    getSelectedTab: state => state.selectedTab,
    getLoggedUser: state => state.user
};

export default new Vuex.Store({
    strict: process.env.NODE_ENV !== 'production',
    state,
    mutations,
    actions,
    getters
});