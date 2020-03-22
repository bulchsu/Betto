<template>
  <div>
    <v-list>
      <v-list-item
        v-for="item in leagues"
        :key="item.id">
        <v-list-item-title>{{ item.name }}</v-list-item-title>
      </v-list-item>
    </v-list>
  </div>
</template>

<script>

export default {
  name: "SideBar",
  data() {
      return { 
          leagues: undefined
      }
  },
  methods:{
      async initLeagues(){
          this.leagues = [
    {
        "leagueId": 2024,
        "name": "World Cup",
        "type": "Cup",
        "country": "World",
        "season": 2018,
        "seasonStart": "2018-06-14T00:00:00",
        "seasonEnd": "2018-07-15T00:00:00",
        "logo": "https://media.api-sports.io/leagues/1.png",
        "flag": null,
        "standings": true
    },
    {
        "leagueId": 2025,
        "name": "Premier League",
        "type": "League",
        "country": "England",
        "season": 2018,
        "seasonStart": "2018-08-10T00:00:00",
        "seasonEnd": "2019-05-12T00:00:00",
        "logo": "https://media.api-sports.io/leagues/2.png",
        "flag": "https://media.api-sports.io/flags/gb.svg",
        "standings": true
    },
    {
        "leagueId": 2026,
        "name": "Championship",
        "type": "League",
        "country": "England",
        "season": 2018,
        "seasonStart": "2018-08-03T00:00:00",
        "seasonEnd": "2019-05-27T00:00:00",
        "logo": "https://media.api-sports.io/leagues/3.png",
        "flag": "https://media.api-sports.io/flags/gb.svg",
        "standings": true
    },
    {
        "leagueId": 2027,
        "name": "Ligue 1",
        "type": "League",
        "country": "France",
        "season": 2018,
        "seasonStart": "2018-08-10T00:00:00",
        "seasonEnd": "2019-05-24T00:00:00",
        "logo": "https://media.api-sports.io/leagues/4.svg",
        "flag": "https://media.api-sports.io/flags/fr.svg",
        "standings": true
    },
    {
        "leagueId": 2028,
        "name": "Ligue 2",
        "type": "League",
        "country": "France",
        "season": 2018,
        "seasonStart": "2018-07-27T00:00:00",
        "seasonEnd": "2019-05-17T00:00:00",
        "logo": "https://media.api-sports.io/leagues/5.png",
        "flag": "https://media.api-sports.io/flags/fr.svg",
        "standings": true
    }
]
      }
  },
  async mounted(){
      await this.initLeagues();
  }
};

</script>