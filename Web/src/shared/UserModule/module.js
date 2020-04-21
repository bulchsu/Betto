import { userService } from './user-service';

import {
    LOGIN,
    LOGOUT
} from './mutation-types';

const state = () => ({
    user: null,
});

const mutations = {
    [LOGIN](state, user) {
        state.user = user;
    },
    [LOGOUT](state) {
        state.user = null;
    }
};

const actions = {
    async loginAction({ commit }, loginData) {
        const user = await userService.login(loginData.username, loginData.password);
        commit(LOGIN, user);
    },
    logoutAction({ commit }) {
        commit(LOGOUT);
    }
};

const getters = {
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