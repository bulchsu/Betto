<template>
    <div class="navbar-end">
      <div class="navbar-item">
        <div class="buttons">
          <a :href="github" target="_blank" rel="noopener noreferrer">
            <i class="fab fa-github fa-2x"></i>
          </a>
        </div>
      </div>
    </div>
</template>

<script>
export default {
  data() {
    return {
      github: 'https://www.google.com/'
    };
  },
};
</script>