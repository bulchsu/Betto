import { API_URL } from '../config';
import apiClient from '../api-client';

const getLeagues = async function() {
  try {
    var response = await apiClient.get(`${API_URL}leagues/`);
    return response;
  } catch (error) {
    return [];
  }
}

const getLeagueTeams = async function(leagueId) {
  try {
    var response = await apiClient.get(`${API_URL}leagues/${leagueId}/teams/`);
    return response;
  } catch (error) {
    return [];
  }
}

const getLeagueGames = async function(leagueId) {
  try {
    var response = await apiClient.get(`${API_URL}leagues/${leagueId}/games/`);
    return response;
  } catch (error) {
    return [];
  }
}

const getTeamById = async function(teamId) {
  try {
    var response = await apiClient.get(`${API_URL}teams/${teamId}?includeVenue=true`);
    return response;
  } catch (error) {
    return null;
  }
}

const getGameById = async function(gameId) {
  try {
    var response = await apiClient.get(`${API_URL}games/${gameId}?includeRates=true`);
    return response;
  } catch (error) {
    return null;
  }
}

export const leagueService = {
  getLeagues,
  getLeagueTeams,
  getLeagueGames,
  getTeamById,
  getGameById
}
