<template>
  <div>
    <RankingDialog
      v-if="rankingDialogVisibility"
      @dialogClosed="onRankingDialogClosed"
      :dialogVisibility="rankingDialogVisibility"
    />
    <PaymentsDialog
      v-if="paymentsDialogVisibility"
      @dialogClosed="onPaymentsDialogClosed"
      :dialogVisibility="paymentsDialogVisibility"
    />
    <v-speed-dial bottom right direction="left" transition="slide-y-transition">
      <template v-slot:activator>
        <v-btn color="#003304" dark fab>
          <i class="fas fa-user fa-2x"></i>
        </v-btn>
      </template>
      <v-btn fab color="primary" @click="openPaymentsDialog">
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
import PaymentsDialog from "./PaymentsDialog/payments-dialog";

export default {
  name: "UserMenu",
  components: {
    RankingDialog,
    PaymentsDialog
  },
  data: () => ({
    rankingDialogVisibility: false,
    paymentsDialogVisibility: false,
  }),
  methods: {
    ...mapActions("UserModule", ["logoutAction"]),
    ...mapActions("TicketModule", ["clearTicketAction"]),
    logout() {
      this.logoutAction();
      if (this.loggedUser == null) {
        this.clearTicketAction();
        vm.$snotify.info("You have been signed out!");
      }
    },
    openPaymentsDialog() {
      this.paymentsDialogVisibility = true;
    },
    onPaymentsDialogClosed() {
      this.paymentsDialogVisibility = false;
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