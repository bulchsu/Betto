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
    <UserTicketsDialog
      v-if="userTicketsDialogVisibility"
      @dialogClosed="onUserTicketsDialogClosed"
      :dialogVisibility="userTicketsDialogVisibility"
    />
    <ImportLeaguesDialog
      v-if="importLeaguesDialogVisibility"
      @dialogClosed="onImportLeaguesDialogClosed"
      :dialogVisibility="importLeaguesDialogVisibility"
    />
    <v-speed-dial bottom right direction="left" transition="slide-y-transition">
      <template v-slot:activator>
        <v-btn color="#003304" dark fab>
          <i class="fas fa-user fa-2x"></i>
        </v-btn>
      </template>
      <v-btn v-if="this.role == 'Admin'" fab color="primary" @click="importInitialData">
        <i class="fas fa-sync-alt fa-2x"></i>
      </v-btn>
      <v-btn v-if="this.role == 'Admin'" fab color="primary" @click="addLeagues">
        <i class="fas fa-plus fa-2x"></i>
      </v-btn>
      <v-btn fab color="primary" @click="openPaymentsDialog">
        <i class="fas fa-dollar-sign fa-2x"></i>
      </v-btn>
      <v-btn fab color="primary" @click="openUserTicketsDialog">
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
import { mapActions, mapGetters } from "vuex";
import vm from "@/main";
import RankingDialog from "./RankingDialog/ranking-dialog";
import PaymentsDialog from "./PaymentsDialog/payments-dialog";
import UserTicketsDialog from "./UserTicketsDialog/user-tickets-dialog";
import ImportLeaguesDialog from "./ImportLeaguesDialog/import-leagues-dialog";
import { userService } from "@/shared/UserModule/user-service";
import { optionsService } from "@/shared/OptionsModule/options-service";

export default {
  name: "UserMenu",
  components: {
    RankingDialog,
    PaymentsDialog,
    UserTicketsDialog,
    ImportLeaguesDialog
  },
  data: () => ({
    rankingDialogVisibility: false,
    paymentsDialogVisibility: false,
    userTicketsDialogVisibility: false,
    importLeaguesDialogVisibility: false,
    role: "User"
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
    },
    openUserTicketsDialog() {
      this.userTicketsDialogVisibility = true;
    },
    onUserTicketsDialogClosed() {
      this.userTicketsDialogVisibility = false;
    },
    async importInitialData() {
      var response = await optionsService.initialImport();
      if (response != null) {
        vm.$snotify.success('Successfully initialized database! Refresh the page!');
      } else {
        vm.$snotify.error('Something went wrong! Check logs!');
      }
    },
    addLeagues() {
      this.importLeaguesDialogVisibility = true;
    },
    onImportLeaguesDialogClosed() {
      this.importLeaguesDialogVisibility = false;
    }
  },
  computed: {
    ...mapGetters("UserModule", ["getLoggedUser"])
  },
  created: async function() {
    this.role = (
      await userService.getUserById(this.getLoggedUser.userId, false, false)
    ).role;
  }
};
</script>