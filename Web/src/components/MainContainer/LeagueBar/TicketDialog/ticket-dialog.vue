<template>
  <div>
    <v-dialog @click:outside="closeDialog" v-model="dialogVisibility" max-width="800px" scrollable>
      <v-card>
        <v-card-title>
          <span class="display-1 my-3 mx-3">Ticket</span>
        </v-card-title>
        <v-card-text style="height: 400px;">
          <v-col class="d-flex justify-center">
            <v-simple-table style="width: 90%;">
              <template v-slot:default>
                <thead>
                  <tr>
                    <th>Game</th>
                    <th class="text-center">Type</th>
                    <th class="text-center">Rate</th>
                    <th class="text-center"></th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="event in events" :key="event.game.gameId">
                    <td>{{ getLabel(event) }}</td>
                    <td class="text-center">{{ event.betType }}</td>
                    <td class="text-center">{{ getRate(event) }}</td>
                    <td class="text-center">
                      <v-btn icon @click="removeEvent(event.game.gameId)">
                        <i class="fas fa-ban"></i>
                      </v-btn>
                    </td>
                  </tr>
                </tbody>
              </template>
            </v-simple-table>
          </v-col>
        </v-card-text>
        <v-card-actions>
          <v-row justify="center" style="max-width: 800px">
            <v-col cols="8">
              <div class="ml-10" style="display: inline-block;">
                <div style="float: left;" v-if="user != null">
                  <small class="label-header">Account Balance</small>
                  <div class="label-content">{{ round(user.accountBalance) }}</div>
                </div>
                <div class="ml-7" style="float: left;">
                  <small class="label-header">Total rate</small>
                  <div class="label-content">{{ totalRate }}</div>
                </div>
                <div class="mx-7" style="float: left;">
                  <small class="label-header">Possible price</small>
                  <div class="label-content">{{ possiblePrice }}</div>
                </div>
                <v-text-field v-model="stake" @keypress="isNumber($event)" label="Stake" outlined></v-text-field>
              </div>
            </v-col>
            <div class="mt-4 mx-10">
              <v-btn
                :disabled="!isTicketPushPossible"
                large
                color="primary"
                @click="pushTicket"
              >PUSH TICKET</v-btn>
            </div>
          </v-row>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
import vm from "@/main";
import { mapGetters, mapActions } from "vuex";
import { leagueService } from "@/shared/LeagueModule/league-service";
import { userService } from "@/shared/UserModule/user-service";

export default {
  name: "TicketDialog",
  data() {
    return {
      events: [],
      homeTeams: [],
      awayTeams: [],
      user: null,
      stake: ""
    };
  },
  props: {
    dialogVisibility: {
      type: Boolean,
      default: false
    }
  },
  computed: {
    ...mapGetters("LeagueModule", ["getSelectedLeagueGames"]),
    ...mapGetters("TicketModule", ["getTicketEvents"]),
    ...mapGetters("UserModule", ["getLoggedUser"]),
    ticketEvents() {
      return this.getTicketEvents;
    },
    totalRate() {
      var rate = 1.0;
      this.events.forEach(e => {
        var nextRate = this.getRate(e);
        rate *= nextRate;
      });
      return this.round(rate);
    },
    possiblePrice() {
      var stakeFactor = this.stake != null ? this.stake : 0;
      return this.round(stakeFactor * this.totalRate);
    },
    isTicketPushPossible() {
      return (
        Number(this.stake) > 2 &&
        Number(this.stake) < 5000 &&
        this.totalRate < 5000 &&
        this.stake <= this.user.accountBalance
      );
    }
  },
  methods: {
    ...mapActions("TicketModule", [
      "removeTicketEventByGameIdAction",
      "pushTicketAction"
    ]),
    closeDialog() {
      this.$emit("dialogClosed");
    },
    getLabel(event) {
      var homeTeam = this.homeTeams.filter(
        t => t.teamId === event.game.homeTeamId
      )[0];
      var awayTeam = this.awayTeams.filter(
        t => t.teamId === event.game.awayTeamId
      )[0];
      return homeTeam != null && awayTeam != null
        ? `${homeTeam.name} - ${awayTeam.name}`
        : "";
    },
    async removeEvent(gameId) {
      this.removeTicketEventByGameIdAction(gameId);
      await this.refresh();
      if (!Array.isArray(this.ticketEvents) || !this.ticketEvents.length) {
        this.closeDialog();
      }
    },
    getRate(event) {
      var rate = null;
      switch (event.betType) {
        case 0:
          rate = event.game.rates.tieRate;
          break;
        case 1:
          rate = event.game.rates.homeTeamWinRate;
          break;
        case 2:
          rate = event.game.rates.awayTeamWinRate;
          break;
      }
      return rate;
    },
    async refresh() {
      var homeTeamsIds = this.getTicketEvents.map(t => t.game.homeTeamId);
      var awayTeamsIds = this.getTicketEvents.map(t => t.game.awayTeamId);
      var homeTeams = [];
      var awayTeams = [];
      homeTeamsIds.forEach(async t => {
        var team = await leagueService.getTeamById(t);
        homeTeams.push(team);
      });
      awayTeamsIds.forEach(async t => {
        var team = await leagueService.getTeamById(t);
        awayTeams.push(team);
      });
      this.events = this.getTicketEvents.slice();
      this.homeTeams = homeTeams;
      this.awayTeams = awayTeams;
      this.user = await userService.getUserById(
        this.getLoggedUser.userId,
        true,
        false
      );
    },
    isNumber: function(evt) {
      evt = evt ? evt : window.event;
      var charCode = evt.which ? evt.which : evt.keyCode;
      if (
        (charCode > 31 &&
          (charCode < 48 || charCode > 57) &&
          charCode !== 46) ||
        this.stake.length > 3
      ) {
        evt.preventDefault();
      } else {
        return true;
      }
    },
    round(number) {
      return Math.round(number * 100) / 100;
    },
    async pushTicket() {
      var userId = this.getLoggedUser.userId;
      var stake = this.stake;
      var events = this.events.map(e => ({
        gameId: e.game.gameId,
        betType: e.betType
      }));
      var response = await this.pushTicketAction({
        userId: userId,
        stake: stake,
        events: events
      });
      if (response != null) {
        vm.$snotify.success("Ticket saved");
        this.closeDialog();
      } else {
        vm.$snotify.error("Something went wrong! Try again later!");
      }
    }
  },
  created: async function() {
    await this.refresh();
  }
};
</script>
