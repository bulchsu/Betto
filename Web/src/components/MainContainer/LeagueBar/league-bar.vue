<template>
  <v-app-bar color="#7cb378" dark prominent>
    <template>
      </template>
    <TicketDialog v-if="ticketDialogVisibility" :dialogVisibility="ticketDialogVisibility" @dialogClosed="onTicketDialogClosed" />
    <p id="title">{{ selectedLeague.name }}</p>
    <v-spacer></v-spacer>
    <v-btn @click="openTicketDialog" v-if="doesTicketExist" class="my-3" color="#005A04" dark fab>
      <i class="fas fa-ticket-alt fa-2x"></i>
    </v-btn>
    <template v-slot:extension>
      <v-tabs align-with-title :value="selectedTab">
        <v-tab @click="changeTab(0)">General</v-tab>
        <v-tab @click="changeTab(1)">Teams</v-tab>
        <v-tab @click="changeTab(2)">Games</v-tab>
      </v-tabs>
    </template>
  </v-app-bar>
</template>

<script>
import { mapGetters, mapActions } from "vuex";
import TicketDialog from "./TicketDialog/ticket-dialog";

export default {
  name: "LeagueBar",
  components: {
    TicketDialog
  },
  data() {
    return {
      ticketDialogVisibility: false
    };
  },
  methods: {
    ...mapActions("LeagueModule", ["selectTabAction"]),
    changeTab(value) {
      this.selectTabAction(value);
    },
    openTicketDialog() {
      this.ticketDialogVisibility = true;
    },
    onTicketDialogClosed() {
      this.ticketDialogVisibility = false;
    }
  },
  computed: {
    ...mapGetters("LeagueModule", ["getSelectedLeague", "getSelectedTab"]),
    ...mapGetters("TicketModule", ["getTicketEvents"]),
    selectedLeague() {
      return this.getSelectedLeague;
    },
    selectedTab() {
      return this.getSelectedTab;
    },
    doesTicketExist() {
      return Array.isArray(this.getTicketEvents) && this.getTicketEvents.length;
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