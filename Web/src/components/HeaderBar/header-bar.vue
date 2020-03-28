<template>
  <div class="main-header d-flex p-2">
    <HeaderBarBrand/>
    <HeaderBarLinks/>
  </div>
</template>

<script>
import HeaderBarBrand from '@/components/HeaderBar/HeaderBarBrand/header-bar-brand';
import HeaderBarLinks from '@/components/HeaderBar/HeaderBarLinks/header-bar-links';

export default {
  name: "HeaderBar",
  components: { HeaderBarBrand, HeaderBarLinks }
};

</script>

<style lang="scss" scoped>
@import "../../assets/styles/_colors.scss";

.main-header {
  background-color: $green-background;
  width: 100%;
  height: 89px;
}

</style>

