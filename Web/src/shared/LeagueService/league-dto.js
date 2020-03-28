export class LeagueDTO {
    constructor(league) {
        this.leagueId = league.leagueId;
        this.name = league.name;
        this.type = league.type;
        this.country = league.country;
        this.season = leagues.season;
        this.seasonStart = leagues.seasonStart;
        this.seasonEnd = leagues.seasonEnd;
        this.logo = leagues.logo;
        this.flag = leagues.flag;
        this.standings = leagues.standings;
    }
}

export default {
    LeagueDTO
}