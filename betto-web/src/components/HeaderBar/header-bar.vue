<template>
  <nav id="navbar">
      <HeaderBarBrand></HeaderBarBrand>
      <HeaderBarLinks></HeaderBarLinks>
  </nav>
</template>

<script>
import HeaderBarLinks from '@/components/HeaderBar/HeaderBarLinks/header-bar-links';
import HeaderBarBrand from '@/components/HeaderBar/HeaderBarBrand/header-bar-brand';

export default {
  name: 'HeaderBar',
  components: { HeaderBarBrand, HeaderBarLinks },
};
</script>

<style lang="scss" scoped>
@import "../../assets/styles/_colors.scss";

#navbar {
  background-color: $green-background;
  width: 100%;
  height: 265px;
}

</style>
