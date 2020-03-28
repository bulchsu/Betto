<template>
    <div class="d-flex align-self-center">
        <a href="https://github.com/bulchsu/Betto" target="_blank"> <!-- TODO make configurable -->
            <i class="fab fa-github fa-4x" aria-hidden="true"></i>
        </a>
    </div>
</template>

<style lang="scss" scoped>
@import "@/assets/styles/_colors.scss";

.fa-github {
    color: $white;
   &:hover {
      color: $light-gray;
   }
}

</style>