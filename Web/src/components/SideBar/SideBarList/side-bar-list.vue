<template>
  <v-list :color="'#DDDDDD'" class="main-list overflow-y-auto" max-height="880">
    <v-list-item
      v-for="league in leagues"
      :key="league.leagueId"
      @click="selectLeague(league)"
      class="tile"
    >
      <v-list-item-content class="mr-a/*uto">
        <v-list-item-title v-text="league.name"></v-list-item-title>
      </v-list-item-content>

      <v-list-item-avatar v-if="league.logo != null">
        <v-img :src="league.logo"></v-img>
      </v-list-item-avatar>
    </v-list-item>
  </v-list>
</template>

<script>
import { mapActions, mapGetters } from "vuex";

export default {
  name: "SideBarList",
  methods: {
    ...mapActions("LeagueModule", [
      "getLeaguesAction",
      "selectLeagueAction",
      "getLeagueTeamsAction",
      "getLeagueGamesAction",
      "selectTabAction"
    ]),
    async selectLeague(league) {
      this.selectTabAction(0);
      await this.selectLeagueAction(league);
      await this.getLeagueTeamsAction(league);
      await this.getLeagueGamesAction(league);
    }
  },
  async created() {
    await this.getLeaguesAction();
    await this.selectLeague(this.leagues[0]);
  },
  computed: {
    ...mapGetters("LeagueModule", ["getLeagues"]),
    leagues() {
      return this.getLeagues;
    }
  }
};
</script>

<style lang="scss" scoped>
@import "@/assets/styles/_colors.scss";

.tile {
  background: $light-gray;
}
</style>