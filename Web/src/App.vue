<template>
  <v-app>
    <vue-snotify></vue-snotify>
    <HeaderBar />
    <div>
      <SideBar style="float :left;" />
      <MainContainer v-if="selectedLeague" style="float :left;" />
    </div>
  </v-app>
</template>

<script>
import HeaderBar from "@/components/HeaderBar/header-bar";
import SideBar from "@/components/SideBar/side-bar";
import MainContainer from "@/components/MainContainer/main-container";
import { mapGetters } from "vuex";

export default {
  name: "app",
  components: {
    HeaderBar,
    SideBar,
    MainContainer
  },
  computed: {
    ...mapGetters(["getSelectedLeague"]),
    selectedLeague() {
      return this.getSelectedLeague;
    }
  }
};
</script>