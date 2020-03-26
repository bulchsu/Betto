<template>
    <div id="main-container"> 
      <LeagueBar @tabChanged="changeTab"/>
      <GeneralInfo v-if="selectedTab == 0"/>
    </div>
</template>

<script>

import LeagueBar from '@/components/MainContainer/LeagueBar/league-bar';
import GeneralInfo from './GeneralInfo/general-info';

export default {
  name: "MainContainer",
  data(){
    return {
      selectedTab: 0
    }
  },
  methods: {
    changeTab(value) {
      this.selectedTab = value;
    }
  },
  components: {
    LeagueBar,
    GeneralInfo
  }
};

</script>

<style lang="scss" scoped>
@import "@/assets/styles/_colors.scss";

#main-container {
    margin-top: 1px;
    margin-left: 1px;
    width: calc(100% - 301px);
    height: 876px;
    background-color: $very-light-gray;
}

</style>