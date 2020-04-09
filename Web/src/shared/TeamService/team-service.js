import { API_URL } from '../config';
import apiClient from '../api-client';

const getLeagueTeams = async function(leagueId) {
  try {
    var response = await apiClient.get(API_URL + "leagues/" + leagueId + "/teams/");
    return response;
  } catch (error) {
    return [];
  }
}

export const teamService = {
  getLeagueTeams
}
