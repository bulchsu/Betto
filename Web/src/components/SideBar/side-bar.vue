<template>
  <div class="side-container">
    <SideBarList />
  </div>
</template>

<script>

import SideBarList from '@/components/SideBar/SideBarList/side-bar-list'; 

export default {
  name: "SideBar",
  components: {
    SideBarList
  }
};

</script>

<style lang="scss" scoped>

@import "@/assets/styles/_colors.scss";

.side-container {
  width: 300px;
  height: 100%;
  background-color: $light-gray;
}

.tile {
  background: $light-gray;
}

</style>