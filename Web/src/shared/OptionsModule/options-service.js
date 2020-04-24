import { API_URL } from '../config';
import apiClient from '../api-client';

const initialImport = async function () {
    try {
        let resourcePath = `${API_URL}options/initialize/`;
        var response = await apiClient.options(resourcePath);
        return response;
    } catch (error) {
        return null;
    }
}

const addLeagues = async function(amount) {
    try {
        let resourcePath = `${API_URL}options/add/next/${amount}`;
        var response = await apiClient.options(resourcePath);
        return response;
    } catch (error) {
        return null;
    }
}

export const optionsService = {
    initialImport,
    addLeagues
}