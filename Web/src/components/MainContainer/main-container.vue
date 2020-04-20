<template>
    <div id="main-container"> 
      <LeagueBar/>
      <GeneralInfo v-if="selectedTab == 0"/>
      <TeamsTable v-if="selectedTab == 1"/>
      <GamesTable v-if="selectedTab == 2"/>      
    </div>
</template>

<script>
import { mapActions, mapGetters } from 'vuex';
import LeagueBar from '@/components/MainContainer/LeagueBar/league-bar';
import GeneralInfo from './GeneralInfo/general-info';
import TeamsTable from './TeamsTable/teams-table';
import GamesTable from './GamesTable/games-table';

export default {
  name: "MainContainer",
    components: {
    LeagueBar,
    GeneralInfo,
    TeamsTable,
    GamesTable
  },
  methods: {
    ...mapActions(["selectTabAction"]),
    changeTab(value) {
      this.selectTabAction(value);
    }
  },
  computed: {
    ...mapGetters(["getSelectedTab"]),
    selectedTab() {
      return this.getSelectedTab;
    }
  }
};

</script>

<style lang="scss" scoped>
@import "@/assets/styles/_colors.scss";

#main-container {
    margin-top: 1px;
    margin-left: 1px;
    width: calc(100% - 301px);
    height: 879px;
    background-color: $very-light-gray;
}

</style>