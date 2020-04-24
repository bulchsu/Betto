<template>
  <div>
    <TeamDialog
      v-if="teamDialogVisibility"
      @dialogClosed="onTeamDialogClosed"
      :teamId="selectedTeamId"
      :dialogVisibility="teamDialogVisibility"
    />
    <v-list class="main-list overflow-y-auto" max-height="700">
      <v-list-item v-for="team in teams" :key="team.teamId" @click="openTeamDialog(team.teamId)">
        <v-list-item-content>
          <v-list-item-title v-text="team.name"></v-list-item-title>
        </v-list-item-content>

        <v-list-item-avatar v-if="team.logo != null">
          <v-img :src="team.logo"></v-img>
        </v-list-item-avatar>
      </v-list-item>
    </v-list>
  </div>
</template>

<script>
import { mapGetters, mapActions } from "vuex";
import TeamDialog from "./TeamDialog/team-dialog";

export default {
  name: "TeamsTable",
  data() {
    return {
      selectedTeamId: null,
      teamDialogVisibility: false
    };
  },
  components: {
    TeamDialog
  },
  methods: {
    ...mapActions("LeagueModule", ["selectTeamAction"]),
    openTeamDialog(teamId) {
      this.selectedTeamId = teamId;
      this.teamDialogVisibility = true;
    },
    onTeamDialogClosed() {
      this.selectedTeamId = null;
      this.teamDialogVisibility = false;
    }
  },
  computed: {
    ...mapGetters("LeagueModule", ["getSelectedLeagueTeams"]),
    teams() {
      return this.getSelectedLeagueTeams;
    }
  }
};
</script>
