<template>
  <div>
    <GameDialog
      v-if="gameDialogVisibility"
      @dialogClosed="onGameDialogClosed"
      :gameId="selectedGameId"
      :dialogVisibility="gameDialogVisibility"
    />
    <GamesTableForm @filterGames="filterGames" />
    <v-list class="main-list overflow-y-auto" max-height="607">
      <v-list-item v-for="game in chosenGames" :key="game.GameId" @click="openGameDialog(game.gameId)">
        <v-list-item-avatar>
          <v-img :src="getTeamLogoById(game.homeTeamId)"></v-img>
        </v-list-item-avatar>

        <v-list-item-content>
          <v-list-item-title v-text="getTeamNameById(game.homeTeamId)"></v-list-item-title>
        </v-list-item-content>

        <div class="ml-auto" style="display: inline-block;">
          <v-list-item-content class="my-1" style="float: left;">
            <v-list-item-title v-text="getTeamNameById(game.awayTeamId)"></v-list-item-title>
          </v-list-item-content>

          <v-list-item-avatar>
            <v-img :src="getTeamLogoById(game.awayTeamId)"></v-img>
          </v-list-item-avatar>
        </div>
      </v-list-item>
    </v-list>
  </div>
</template>

<script>
import { mapGetters } from "vuex";
import GamesTableForm from "./GamesTableForm/games-table-form";
import GameDialog from "./GameDialog/game-dialog";

export default {
  name: "GamesTable",
  components: {
    GamesTableForm,
    GameDialog
  },
  data() {
    return {
      chosenGames: [],
      selectedGameId: null,
      gameDialogVisibility: false
    };
  },
  methods: {
    openGameDialog(gameId) {
      this.selectedGameId = gameId;
      this.gameDialogVisibility = true;
    },
    onGameDialogClosed() {
      this.selectedGameId = null;
      this.gameDialogVisibility = false;
    },
    getTeamNameById(teamId) {
      return this.teams.find(t => t.teamId == teamId).name;
    },
    getTeamLogoById(teamId) {
      return this.teams.find(t => t.teamId == teamId).logo;
    },
    filterGames(team, round) {
      if (team == null && round == null) {
        this.chosenGames = this.games.slice();
        this.sortChosenGames();
      } else if (team != null) {
        this.chosenGames = this.games.filter(
          g => g.homeTeamId == team.teamId || g.awayTeamId == team.teamId
        );
        this.sortChosenGames();
      } else {
        this.chosenGames = this.games.filter(g => g.round == round);
        this.sortChosenGames();
      }
    },
    sortChosenGames() {
      this.chosenGames.sort((a, b) => (a.eventDate > b.eventDate ? 1 : -1));
    }
  },
  mounted() {
    this.chosenGames = this.games;
  },
  computed: {
    ...mapGetters("LeagueModule", [
      "getSelectedLeagueGames",
      "getSelectedLeagueTeams"
    ]),
    games() {
      return this.getSelectedLeagueGames;
    },
    teams() {
      return this.getSelectedLeagueTeams;
    }
  }
};
</script>