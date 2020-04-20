import { API_URL } from '../config';
import apiClient from '../api-client';

const getLeagueGames = async function(leagueId) {
  try {
    var response = await apiClient.get(API_URL + "leagues/" + leagueId + "/games/");
    return response;
  } catch (error) {
    return [];
  }
}

export const gameService = {
  getLeagueGames
}