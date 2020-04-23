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
                    <th class="text-center">Status</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="event in events" :key="event.eventId">
                    <td>{{ getGameLabel(event.game) }}</td>
                    <td class="text-center">{{ event.betType }}</td>
                    <td class="text-center">{{ event.confirmedRate }}</td>
                    <td class="text-center">
                      <i v-if="event.eventStatus == 0" class="fas fa-times"></i>
                      <i v-if="event.eventStatus == 1" class="fas fa-check"></i>
                      <i v-if="event.eventStatus == 2" class="fas fa-question"></i>
                    </td>
                  </tr>
                </tbody>
              </template>
            </v-simple-table>
          </v-col>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn large color="primary" @click="closeDialog">RETURN</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
import { ticketService } from "@/shared/TicketModule/ticket-service";
import { leagueService } from "@/shared/LeagueModule/league-service";

export default {
  name: "TicketDetailsDialog",
  data() {
    return {
      events: [],
      homeTeams: [],
      awayTeams: []
    };
  },
  props: {
    dialogVisibility: {
      type: Boolean,
      default: false
    },
    ticketId: {
      type: Number
    }
  },
  methods: {
    closeDialog() {
      this.$emit("dialogClosed");
    },
    getGameLabel(game) {
      var homeTeam = this.homeTeams.filter(t => t.teamId == game.homeTeamId)[0];
      var awayTeam = this.awayTeams.filter(t => t.teamId == game.awayTeamId)[0];
      var homeTeamLabel = homeTeam != null ? homeTeam.name : 'Unknown';
      var awayTeamLabel = awayTeam != null ? awayTeam.name : 'Unknown';
      return `${homeTeamLabel}-${awayTeamLabel}`;
    }
  },
  created: async function() {
    var ticket = await ticketService.getTicketById(this.ticketId);
    this.events = ticket.events;
    var homeTeamsIds = this.events.map(t => t.game.homeTeamId);
    var awayTeamsIds = this.events.map(t => t.game.awayTeamId);
    homeTeamsIds.forEach(async t => {
      var team = await leagueService.getTeamById(t);
      this.homeTeams.push(team);
    });
    awayTeamsIds.forEach(async t => {
      var team = await leagueService.getTeamById(t);
      this.awayTeams.push(team);
    });
  }
};
</script>