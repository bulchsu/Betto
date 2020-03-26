import { API_URL } from '../config';

import * as axios from 'axios';

const getLeagues = async function() {
  try {
    let response = await axios.get(API_URL + 'leagues/');
    let leagues = parseLeagues(response);
    return leagues;
  } catch (error) {
    console.error(error);
    return [];
  }
}

const parseLeagues = response => {
  if (response.status !== 200) {
    throw Error(response.message);
  }
  return response.data;
}

export const leagueService = {
  getLeagues
}
