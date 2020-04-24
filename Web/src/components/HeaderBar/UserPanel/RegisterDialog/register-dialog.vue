<template>
  <div>
    <v-dialog @click:outside="closeDialog" v-model="dialogVisibility" max-width="350px">
      <v-card class="d-flex flex-column" height="500px">
        <v-card-title>
          <span class="headline">Sign Up</span>
        </v-card-title>

        <v-col class="d-flex justify-center">
          <v-card-text>
            <v-text-field v-model="mailAddress" label="E-mail"></v-text-field>
            <v-text-field v-model="username" label="Username"></v-text-field>
            <v-text-field v-model="password" :type="'password'" label="Password"></v-text-field>
          </v-card-text>
        </v-col>

        <v-col class="d-flex justify-center">
          <v-card-actions class="card-actions">
            <v-btn :disabled="!isSigningUpPossible" text color="primary" @click="signUp">SIGN UP</v-btn>
          </v-card-actions>
        </v-col>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
export default {
  name: "RegisterDialog",
  data() {
    return {
      username: null,
      password: null,
      mailAddress: null
    };
  },
  props: {
    dialogVisibility: {
      type: Boolean,
      default: false
    }
  },
  methods: {
    signUp() {
      this.closeDialog();
      this.$emit("register", this.username, this.password, this.mailAddress);
      this.username = null;
      this.password = null;
      this.mailAddress = null;
    },
    closeDialog() {
      this.$emit("dialogClosed");
    }
  },
  computed: {
    isSigningUpPossible() {
      return (
        this.username != null &&
        this.password != null &&
        this.mailAddress != null
      );
    }
  }
};
</script>

<style lang="scss" scoped>
.v-text-field {
  width: 300px;
}
</style>