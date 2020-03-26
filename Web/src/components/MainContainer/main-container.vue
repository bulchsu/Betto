<template>
  <v-card id="main-container">
    <LeagueBar />
  </v-card>
</template>

<script>

import LeagueBar from '@/components/MainContainer/LeagueBar/league-bar';

export default {
  name: "MainContainer",
  components: {
    LeagueBar
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