import { ticketService } from './ticket-service';

import {
    CLEAR_TICKET,
    ADD_EVENT,
    REMOVE_EVENT
} from './mutation-types';

const state = () => ({
    ticketEvents: []
});

const mutations = {
    [CLEAR_TICKET](state) {
        state.ticketEvents = [];
    },
    [ADD_EVENT](state, event) {
        state.ticketEvents.push(event);
    },
    [REMOVE_EVENT](state, gameId) {
        state.ticketEvents = state.ticketEvents.filter(e => e.game.gameId != gameId);
    }
}

const actions = {
    async pushTicketAction({ commit }, ticket) {
        var response = await ticketService.pushTicket(ticket);
        if (response !== null) {
            commit(CLEAR_TICKET);
        }
        return response;
    },
    removeTicketEventByGameIdAction({ commit }, gameId) {
        commit(REMOVE_EVENT, gameId);
    },
    addTicketEventAction({ commit }, event) {
        commit(ADD_EVENT, event);
    },
    clearTicketAction({ commit }) {
        commit(CLEAR_TICKET);
    }
}

const getters = {
    getTicketEvents: state => state.ticketEvents
}

const module = {
    namespaced: true,
    state,
    getters,
    actions,
    mutations
}

export default module;