<template>
  <div>
    <RankingDialog
      v-if="rankingDialogVisibility"
      @dialogClosed="onRankingDialogClosed"
      :dialogVisibility="rankingDialogVisibility"
    />
    <v-speed-dial v-model="isOpened" bottom right direction="left" transition="slide-y-transition">
      <template v-slot:activator>
        <v-btn v-model="isOpened" color="#003304" dark fab>
          <i class="fas fa-user fa-2x"></i>
        </v-btn>
      </template>
      <v-btn fab color="primary">
        <i class="fas fa-dollar-sign fa-2x"></i>
      </v-btn>
      <v-btn fab color="primary">
        <i class="fas fa-ticket-alt fa-2x"></i>
      </v-btn>
      <v-btn fab color="primary" @click="openRankingDialog">
        <i class="fas fa-trophy fa-2x"></i>
      </v-btn>
      <v-btn fab color="primary" @click="logout">
        <i class="fas fa-door-open fa-2x"></i>
      </v-btn>
    </v-speed-dial>
  </div>
</template>

<script>
import { mapActions } from "vuex";
import vm from "@/main";
import RankingDialog from "./RankingDialog/ranking-dialog";

export default {
  name: "UserMenu",
  components: {
    RankingDialog
  },
  data: () => ({
    isOpened: false,
    rankingDialogVisibility: false
  }),
  methods: {
    ...mapActions("UserModule", ["logoutAction"]),
    logout() {
      this.logoutAction();
      if (this.loggedUser == null) {
        vm.$snotify.info("You have been signed out!");
      }
    },
    openRankingDialog() {
      this.rankingDialogVisibility = true;
    },
    onRankingDialogClosed() {
      this.rankingDialogVisibility = false;
    }
  }
};
</script>