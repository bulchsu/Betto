import { API_URL } from '../config';
import vm from '@/main';
import apiClient from '../api-client';

const login = async function (username, password) {
    let resourceRoute = API_URL + 'users/authenticate';
    var response = await apiClient.post(resourceRoute, { username: username, password: password }, null);
    if (response != null) {
        vm.$snotify.success('Successfully signed in ' + response.username);
    }
    return response;
}

const register = async function (username, password, mailAddress) {
    let resourceRoute = API_URL + 'users/register';
    var response = await apiClient.post(resourceRoute, { username: username, password: password, mailAddress: mailAddress }, null);
    if (response != null) {
        vm.$snotify.success('Successfully signed up ' + response.username);
    }
    return response;
}

export const userService = {
    login,
    register
}
