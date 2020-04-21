import { userService } from './user-service';

import {
    LOGIN,
    LOGOUT
} from './mutation-types';

const loggedUser = JSON.parse(localStorage.getItem('user'));

const state = () => ({
    user: loggedUser,
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
        userService.logout();
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