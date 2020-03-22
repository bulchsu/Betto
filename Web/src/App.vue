<template>
  <v-app>
    <HeaderBar />
  </v-app>
</template>

<script>
import HeaderBar from '@/components/HeaderBar/header-bar';

export default {
  name: "app",
  components: {
    HeaderBar
  }
};
</script>
