import { API_URL } from '../config';
import apiClient from '../api-client';

const getLeagues = async function() {
  try {
    var response = await apiClient.get(API_URL + 'leagues/');
    return response;
  } catch (error) {
    return [];
  }
}

export const leagueService = {
  getLeagues
}
