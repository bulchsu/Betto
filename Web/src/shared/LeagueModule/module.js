import { leagueService } from './league-service';

import {
    GET_LEAGUES,
    SELECT_LEAGUE,
    GET_LEAGUE_TEAMS,
    GET_LEAGUE_GAMES,
    CHANGE_TAB,
} from './mutation-types';

const state = () => ({
    leagues: [],
    leagueTeams: [],
    leagueGames: [],
    selectedLeague: null,
    selectedTab: 0
});

const mutations = {
    [GET_LEAGUES](state, leagues) {
        state.leagues = leagues;
    },
    [SELECT_LEAGUE](state, league, teams, games) {
        state.leagueTeams = teams;
        state.leagueGames = games;
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
    }
};

const actions = {
    async getLeaguesAction({ commit }) {
        const leagues = await leagueService.getLeagues();
        commit(GET_LEAGUES, leagues);
    },
    async selectLeagueAction({ commit }, league) {
        commit(SELECT_LEAGUE, league, null, null);
    },
    async getLeagueTeamsAction({ commit }, league) {
        const leagueTeams = await leagueService.getLeagueTeams(league.leagueId);
        commit(GET_LEAGUE_TEAMS, leagueTeams);
    },
    async getLeagueGamesAction({ commit }, league) {
        const leagueGames = await leagueService.getLeagueGames(league.leagueId);
        commit(GET_LEAGUE_GAMES, leagueGames);
    },
    selectTabAction({ commit }, tab) {
        commit(CHANGE_TAB, tab);
    }
};

const getters = {
    getLeagues: state => state.leagues,
    getSelectedLeague: state => state.selectedLeague,
    getSelectedLeagueTeams: state => state.leagueTeams,
    getSelectedLeagueGames: state => state.leagueGames,
    getSelectedTab: state => state.selectedTab,
    getTeamsCount: state => Object(state.leagueTeams).length,
    getGamesCount: state => Object(state.leagueGames).length,
    getLoggedUser: state => state.user
};

const module = {
    namespaced: true,
    state,
    getters,
    actions,
    mutations,
};

export default module;