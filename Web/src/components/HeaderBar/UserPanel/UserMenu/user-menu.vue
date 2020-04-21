<template>
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
    <v-btn fab color="primary">
      <i class="fas fa-trophy fa-2x"></i>
    </v-btn>
    <v-btn fab color="primary" @click="logout">
      <i class="fas fa-door-open fa-2x"></i>
    </v-btn>
  </v-speed-dial>
</template>

<script>
import { mapActions } from "vuex";
import vm from "@/main";

export default {
  name: "UserMenu",
  data: () => ({
    isOpened: false
  }),
  methods: {
    ...mapActions("UserModule", ["logoutAction"]),
    logout() {
      this.logoutAction();
      if (this.loggedUser == null) {
        vm.$snotify.info("You have been signed out!");
      }
    }
  }
};
</script>