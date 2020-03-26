<template>
    <v-app-bar 
        color="#7cb378"
        dark
        prominent>
      <p id="title">{{ selectedLeague.name }}</p>
      <v-spacer></v-spacer>
      <v-btn icon>
        <v-icon>mdi-heart</v-icon>
      </v-btn>
      <template v-slot:extension>
        <v-tabs align-with-title>
          <v-tab @click="changeTab(0)">General</v-tab>
          <v-tab @click="changeTab(1)">Teams</v-tab>
        </v-tabs>
      </template>
    </v-app-bar>
</template>

<script>

import { mapGetters } from 'vuex';

export default {
  name: "LeagueBar",
  methods: {
    changeTab(value) {
      this.$emit('tabChanged', value);
    }
  },
  computed: {
    ...mapGetters(['getSelectedLeague']),
    selectedLeague() {
      return this.getSelectedLeague;
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