<template>
  <div>
    <v-dialog @click:outside="closeDialog" v-model="dialogVisibility" max-width="300px">
      <v-card class="d-flex flex-column" height="400px">
        <v-card-title>
          <span class="headline">Sign In</span>
        </v-card-title>

        <v-col class="d-flex justify-center">
          <v-card-text>
            <v-text-field v-model="username" label="Username"></v-text-field>
            <v-text-field v-model="password" :type="'password'" label="Password"></v-text-field>
          </v-card-text>
        </v-col>

        <v-col class="d-flex justify-center">
          <v-card-actions class="card-actions">
            <v-btn :disabled="!isSigningInPossible" text color="primary" @click="login">SIGN IN</v-btn>
          </v-card-actions>
        </v-col>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
export default {
  name: "LoginDialog",
  data() {
    return {
      username: null,
      password: null
    };
  },
  props: {
    dialogVisibility: {
      type: Boolean,
      default: false
    }
  },
  methods: {
    login() {
      this.closeDialog();
      this.$emit("login", this.username, this.password);
      this.username = null;
      this.password = null;
    },
    closeDialog() {
      this.$emit("dialogClosed");
    }
  },
  computed: {
    isSigningInPossible() {
      return this.username != null && this.password != null;
    }
  }
};
</script>

<style lang="scss" scoped>
.v-text-field {
  width: 260px;
}
</style>