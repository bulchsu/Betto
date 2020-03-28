export class TeamDTO {
    constructor(team) {
        this.teamId = team.teamId;
        this.leagueId = team.leagueId;
        this.name = team.name;
        this.logo = team.logo;
        this.country = team.country;
        this.isNational = team.isNational;
        this.venue = team.venue;
    }
}

export default {
    TeamDTO
}