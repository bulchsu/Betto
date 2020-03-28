<template>
    <v-list :color="'#DDDDDD'" class="main-list overflow-y-auto" max-height="880">
      <v-list-item
        v-for="league in leagues"
        :key="league.leagueId"
        @click="selectLeague(league)"
        class="tile">

        <v-list-item-content class="mr-a/*uto">
          <v-list-item-title v-text="league.name"></v-list-item-title>
        </v-list-item-content>

        <v-list-item-avatar>
          <v-img :src="league.logo"></v-img>
        </v-list-item-avatar>

      </v-list-item>
    </v-list>
</template>

<script>

import { mapActions, mapState } from 'vuex';

export default {
  name: "SideBarList",
  methods:{
    ...mapActions(['getLeaguesAction', 'selectLeagueAction']),
    selectLeague (league){
      this.selectLeagueAction(league);
    }
  },
  async created() {
      await this.getLeaguesAction();
  },
  computed: {
    ...mapState(['leagues'])
  }
};

</script>

<style lang="scss" scoped>
@import "@/assets/styles/_colors.scss";

.tile {
  background: $light-gray;
}

</style>