import { API_URL } from '../config';

import * as axios from 'axios';

const getLeagueTeams = async function(leagueId) {
  try {
    let response = await axios.get(API_URL + "leagues/" + leagueId + "/teams/");
    let leagueTeams = parseLeagueTeams(response);
    return leagueTeams;
  } catch (error) {
    console.error(error);
    return [];
  }
}

const parseLeagueTeams = response => {
  if (response.status !== 200) {
    throw Error(response.message);
  }
  return response.data;
}

export const teamService = {
  getLeagueTeams
}
