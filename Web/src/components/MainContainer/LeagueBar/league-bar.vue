<template>
    <v-app-bar 
        color="#7cb378"
        dark
        prominent>
      <p id="title">{{ selectedLeague.name }}</p>
      <v-spacer></v-spacer>
      <template v-slot:extension>
        <v-tabs align-with-title
          :value="selectedTab">
          <v-tab @click="changeTab(0)">General</v-tab>
          <v-tab @click="changeTab(1)">Teams</v-tab>
          <v-tab @click="changeTab(2)">Games</v-tab>
        </v-tabs>
      </template>
    </v-app-bar>
</template>

<script>

import { mapGetters, mapActions } from 'vuex';

export default {
  name: "LeagueBar",
  methods: {
    ...mapActions("LeagueModule", ["selectTabAction"]),
    changeTab(value) {
      this.selectTabAction(value);
    }
  },
  computed: {
    ...mapGetters("LeagueModule", ['getSelectedLeague', 'getSelectedTab']),
    selectedLeague() {
      return this.getSelectedLeague;
    },
    selectedTab() {
      return this.getSelectedTab;
    }
  }
};

</script>

<style lang="scss" scoped>
@import "@/assets/styles/_colors.scss";

#title {
  margin-top: 20px;
  margin-left: 10px;
  color: $white;
  font-size: 40px;
  font-family: Verdana;
}

</style>