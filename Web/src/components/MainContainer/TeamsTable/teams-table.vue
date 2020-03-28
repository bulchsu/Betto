<template>
    <div>
        <v-list
        class="main-list overflow-y-auto" max-height="700">
      <v-list-item
        v-for="team in teams"
        :key="team.teamId"
      >

        <v-list-item-content>
          <v-list-item-title v-text="team.name"></v-list-item-title>
        </v-list-item-content>

        <v-list-item-avatar>
          <v-img :src="team.logo"></v-img>
        </v-list-item-avatar>
      </v-list-item>
    </v-list>
    </div>
</template>

<script>

import { mapGetters } from 'vuex';

export default {
    name: 'TeamsTable',
    computed: {
        ...mapGetters(['getSelectedLeagueTeams']),
        teams() {
            return this.getSelectedLeagueTeams;
        }
    }
}
</script>
