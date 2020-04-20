<template>
  <v-card outlined height="100">
    <v-form>
      <v-container>
        <v-row align="start" justify="center">
          <v-col cols="4">
            <v-autocomplete
              :disabled="this.selectedRound != null"
              v-model="selectedTeam"
              :items="teams"
              chips
              color="blue-grey lighten-2"
              label="Select team"
            >
              <template v-slot:selection="data">
                <v-chip
                  :input-value="data.selected"
                  close
                  @click="data.select"
                  @click:close="removeSelectedTeam"
                >
                  <v-avatar left>
                    <v-img :src="data.item.logo"></v-img>
                  </v-avatar>
                  {{ data.item.name }}
                </v-chip>
              </template>
              <template v-slot:item="data">
                <template>
                  <v-list-item-avatar>
                    <img :src="data.item.logo" />
                  </v-list-item-avatar>
                  <v-list-item-content>
                    <v-list-item-title v-html="data.item.name"></v-list-item-title>
                  </v-list-item-content>
                </template>
              </template>
            </v-autocomplete>
          </v-col>
          <v-col cols="4">
            <v-autocomplete
              :disabled="this.selectedTeam != null"
              v-model="selectedRound"
              :items="rounds"
              chips
              color="blue-grey lighten-2"
              label="Select round"
            >
              <template v-slot:selection="data">
                <v-chip
                  :input-value="data.selected"
                  close
                  @click="data.select"
                  @click:close="removeSelectedRound"
                >{{ data.item }}</v-chip>
              </template>
              <template v-slot:item="data">
                <template>
                  <v-list-item-content v-text="data.item"></v-list-item-content>
                </template>
              </template>
            </v-autocomplete>
          </v-col>
        </v-row>
      </v-container>
    </v-form>
  </v-card>
</template>

<script>
import { mapGetters } from "vuex";

export default {
  name: "GamesTableForm",
  data() {
    return {
      selectedTeam: null,
      selectedRound: null
    };
  },
  methods: {
    removeSelectedTeam() {
      this.selectedTeam = null;
    },
    removeSelectedRound() {
      this.selectedRound = null;
    },
    isTeamSelectDisabled() {
      return this.selectedRound != null;
    },
    isRoundSelectDisabled() {
      return this.selectedTeam != null;
    },
    sortRounds(rounds) {
      var roundsWithNumbers = rounds.filter(r => /\d/.test(r));
      roundsWithNumbers.sort((a, b) => {
        var firstNumber = Number(a.match(/\d+/g)[0]);
        var secondNumber = Number(b.match(/\d+/g)[0]);

        return firstNumber > secondNumber ? 1 : -1;
      });

      var roundsWithoutNumbers = rounds
        .filter(r => !/\d/.test(r))
        .sort((a, b) => (a > b ? 1 : -1));

      return roundsWithNumbers.concat(roundsWithoutNumbers);
    }
  },
  mounted() {
    if (this.getSelectedLeague.type != "Cup") {
      this.selectedRound = this.rounds[0];
    }
  },
  computed: {
    ...mapGetters([
      "getSelectedLeague",
      "getSelectedLeagueGames",
      "getSelectedLeagueTeams"
    ]),
    rounds() {
      var rounds = this.getSelectedLeagueGames
        .map(g => g.round)
        .filter((value, index, array) => array.indexOf(value) === index);

      return this.sortRounds(rounds);
    },
    teams() {
      return this.getSelectedLeagueTeams;
    }
  },
  watch: {
    selectedTeam: function(value) {
      this.$emit("filterGames", value, this.selectedRound);
    },
    selectedRound: function(value) {
      this.$emit("filterGames", this.selectedTeam, value);
    }
  }
};
</script>