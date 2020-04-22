<template>
  <div>
    <v-dialog
      @click:outside="closeDialog"
      v-if="this.game != null"
      v-model="dialogVisibility"
      max-width="700px"
    >
      <v-card>
        <v-card-title>
          <span class="display-1 my-3 mx-3">{{ gameLabel }}</span>
        </v-card-title>
        <v-row justify="space-around" style="max-width: 700px">
          <v-col cols="4">
            <v-card-text>
              <div class="my-2">
                <small class="label-header">Game date</small>
                <div class="label-content">{{ game.eventDate | formatDate }}</div>
              </div>
              <div class="my-2">
                <small class="label-header">Round</small>
                <div class="label-content">{{ game.round }}</div>
              </div>
              <div class="my-2">
                <small class="label-header">Stadium</small>
                <div class="label-content">{{ game.venue }}</div>
              </div>
              <div class="my-2">
                <small class="label-header">Referee</small>
                <div class="label-content">{{ game.referee }}</div>
              </div>
            </v-card-text>
          </v-col>
          <v-col cols="4">
            <v-card-text>
              <div class="my-2">
                <small class="label-header">{{ `${homeTeamLabel} rate` }}</small>
                <div class="label-content">{{ game.rates.homeTeamWinRate }}</div>
              </div>
              <div class="my-2">
                <small class="label-header">Tie rate</small>
                <div class="label-content">{{ game.rates.tieRate }}</div>
              </div>
              <div class="my-2">
                <small class="label-header">{{ `${awayTeamLabel} rate` }}</small>
                <div class="label-content">{{ game.rates.awayTeamWinRate }}</div>
              </div>
            </v-card-text>
          </v-col>
          <v-col cols="3">
            <v-radio-group v-model="betType" column>
              <v-radio :label="homeTeamLabel" :value="1"></v-radio>
              <v-radio label="Tie" :value="0"></v-radio>
              <v-radio :label="awayTeamLabel" :value="2"></v-radio>
              <v-btn @click="addToTicket" class="my-7" color="primary">Add to ticket</v-btn>
            </v-radio-group>
          </v-col>
        </v-row>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
import { leagueService } from "@/shared/LeagueModule/league-service";

export default {
  name: "GameDialog",
  data() {
    return {
      game: null,
      homeTeam: null,
      awayTeam: null,
      betType: null
    };
  },
  props: {
    dialogVisibility: {
      type: Boolean,
      default: false
    },
    gameId: {
      type: Number
    }
  },
  methods: {
    addToTicket() {

    },
    closeDialog() {
      this.$emit("dialogClosed");
    }
  },
  created: async function() {
    var game = await leagueService.getGameById(this.gameId);
    this.homeTeam = await leagueService.getTeamById(game.homeTeamId);
    this.awayTeam = await leagueService.getTeamById(game.awayTeamId);
    this.game = game;
  },
  computed: {
    gameLabel() {
      return `${this.homeTeam.name} - ${this.awayTeam.name}`;
    },
    homeTeamLabel() {
      return `${this.homeTeam.name} wins`;
    },
    awayTeamLabel() {
      return `${this.awayTeam.name} wins`;
    }
  },
  filters: {
    formatDate: function (date) {
      return date.replace('T', ' ');
    }
  }
};
</script>

<style lang="scss" scoped>
@import "@/assets/styles/_colors.scss";

.label-header {
    font-size: 13px;
}

.label-content {
    font-size: 15px;
    font-weight: bold;
}

</style>