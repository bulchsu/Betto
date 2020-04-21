<template>
  <div>
    <div v-if="loggedUser == null" class="my-4 mx-10">
      <v-btn class="mx-7" color="#004a04" @click="showRegisterDialog">
        <p class="my-2">SIGN UP</p>
      </v-btn>
      <v-btn color="#004a04" @click="showLoginDialog">
        <p class="my-2">SIGN IN</p>
      </v-btn>
    </div>
    <div v-if="loggedUser != null" class="mx-5" style="display: inline-block;">
      <UserMenu class="my-6" style="float: left" />
    </div>
    <LoginDialog
      @dialogClosed="onLoginDialogClosed"
      @login="login"
      :dialogVisibility="loginDialogVisibility"
    />
    <RegisterDialog
      @dialogClosed="onRegisterDialogClosed"
      @register="register"
      :dialogVisibility="registerDialogVisibility"
    />
  </div>
</template>
<script>
import RegisterDialog from "./RegisterDialog/register-dialog";
import LoginDialog from "./LoginDialog/login-dialog";
import { mapActions, mapGetters } from "vuex";
import { userService } from "@/shared/UserModule/user-service";
import UserMenu from "./UserMenu/user-menu";

export default {
  name: "UserPanel",
  components: { LoginDialog, RegisterDialog, UserMenu },
  data() {
    return {
      loginDialogVisibility: false,
      registerDialogVisibility: false
    };
  },
  methods: {
    ...mapActions("UserModule", ["loginAction", "logoutAction"]),
    async login(username, password) {
      await this.loginAction({ username, password });
    },
    async register(username, password, mailAddress) {
      await userService.register(username, password, mailAddress);
    },
    showLoginDialog() {
      this.loginDialogVisibility = true;
    },
    showRegisterDialog() {
      this.registerDialogVisibility = true;
    },
    onLoginDialogClosed() {
      this.loginDialogVisibility = false;
    },
    onRegisterDialogClosed() {
      this.registerDialogVisibility = false;
    }
  },
  computed: {
    ...mapGetters("UserModule", ["getLoggedUser"]),
    loggedUser() {
      return this.getLoggedUser;
    }
  }
};
</script>

<style lang="scss" scoped>
@import "@/assets/styles/_colors.scss";

p {
  color: $white;
  white-space: nowrap;
}
</style>