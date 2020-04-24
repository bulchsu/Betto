<template>
  <div>
    <v-simple-table class="general-table">
      <tbody>
        <tr class="general-table-row">
          <td class="general-table-cell">Country:</td>
          <td class="general-table-cell">{{ selectedLeague.country }}</td>
        </tr>
        <tr class="general-table-row">
          <td class="general-table-cell">Type:</td>
          <td class="general-table-cell">{{ selectedLeague.type }}</td>
        </tr>
        <tr class="general-table-row">
          <td class="general-table-cell">Season start:</td>
          <td class="general-table-cell">{{ selectedLeague.seasonStart | formatDate }}</td>
        </tr>
        <tr class="general-table-row">
          <td class="general-table-cell">Season end:</td>
          <td class="general-table-cell">{{ selectedLeague.seasonEnd | formatDate }}</td>
        </tr>
        <tr class="general-table-row">
          <td class="general-table-cell">Teams:</td>
          <td class="general-table-cell">{{ teamsCount }}</td>
        </tr>
        <tr class="general-table-row">
          <td class="general-table-cell">Games:</td>
          <td class="general-table-cell">{{ gamesCount }}</td>
        </tr>
      </tbody>
    </v-simple-table>
  </div>
</template>

<script>
import { mapGetters } from "vuex";

export default {
  name: "LeagueTable",
  computed: {
    ...mapGetters("LeagueModule", [
      "getSelectedLeague",
      "getTeamsCount",
      "getGamesCount"
    ]),
    selectedLeague() {
      return this.getSelectedLeague;
    },
    teamsCount() {
      return this.getTeamsCount;
    },
    gamesCount() {
      return this.getGamesCount;
    }
  },
  filters: {
    formatDate : function (date) {
      return date.slice(0, 10);
    }
  }
};
</script>

<style lang="scss" scoped>
.general-table {
  margin-top: 5px;
  width: 100%;
  float: left;
}

.general-table-row {
  height: 100px;
}

.general-table-cell {
  font-size: 18px;
  text-align: center;
}
</style>