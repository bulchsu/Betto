<template>
  <v-app>
    <HeaderBar />
    <SideBar />
  </v-app>
</template>

<script>
import HeaderBar from '@/components/HeaderBar/header-bar';
import SideBar from '@/components/SideBar/side-bar'

export default {
  name: "app",
  components: {
    HeaderBar, SideBar
  }
};
</script>