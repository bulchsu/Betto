import { API_URL } from '../config';
import vm from '@/main';
import apiClient from '../api-client';

const login = async function (username, password) {
    try {
        let resourceRoute = API_URL + 'users/authenticate';
        var response = await apiClient.post(resourceRoute, { username: username, password: password });
        if (response != null) {
            localStorage.setItem('user', JSON.stringify(response));
            vm.$snotify.success('Successfully signed in ' + response.username);
        }
        return response;
    } catch (error) {
        return null;
    }
}

const register = async function (username, password, mailAddress) {
    try {
        let resourceRoute = API_URL + 'users/register';
        var response = await apiClient.post(resourceRoute, { username: username, password: password, mailAddress: mailAddress });
        if (response != null) {
            vm.$snotify.success('Successfully signed up ' + response.username);
        }
        return response;
    } catch (error) {
        return null;
    }
}

const getUsersRanking = async function () {
    try {
        let resourceRoute = `${API_URL}users/ranking/`;
        var response = await apiClient.get(resourceRoute);
        return response;
    } catch (error) {
        return [];
    }
}

const getUserById = async function(userId, includePayments, includeTickets) {
    try {
        let resourcePath = `${API_URL}users/${userId}?includePayments=${includePayments}&includeTickets=${includeTickets}`;
        var response = await apiClient.get(resourcePath);
        return response;
    } catch (error) {
        return null;
    }
}

const createPayment = async function(userId, amount, type) {
    try {
        let resourcePath = `${API_URL}payments/`;
        var response = await apiClient.post(resourcePath, { userId, amount, type });
        return response;
    } catch (error) {
        return null;
    }
}

const logout = function() {
    localStorage.removeItem('user');
}

export const userService = {
    login,
    register,
    getUsersRanking,
    getUserById,
    createPayment,
    logout
}
