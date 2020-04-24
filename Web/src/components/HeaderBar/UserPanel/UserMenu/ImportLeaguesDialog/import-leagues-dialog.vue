<template>
  <div>
    <v-dialog @click:outside="closeDialog" v-model="dialogVisibility" max-width="350px">
      <v-card class="d-flex flex-column">
        <v-card-title>
          <span class="display-1 my-3 mx-3">Import leagues</span>
        </v-card-title>
        <v-row justify="space-around" style="width: 350px;">
          <v-col cols="6">
            <v-text-field v-model="amount" @keypress="isNumber($event)" label="Amount" outlined></v-text-field>
          </v-col>
        </v-row>
        <v-btn
          :disabled="!isImportPossible"
          @click="importLeagues"
          class="mx-4 my-3"
          color="primary"
        >IMPORT</v-btn>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
import { optionsService } from "@/shared/OptionsModule/options-service";
import vm from "@/main";

export default {
  name: "ImportLeaguesDialog",
  data() {
    return {
      amount: "1"
    };
  },
  props: {
    dialogVisibility: {
      type: Boolean,
      default: false
    }
  },
  computed: {
    isImportPossible() {
      return this.amount.length > 0;
    }
  },
  methods: {
    closeDialog() {
      this.$emit("dialogClosed");
    },
    isNumber: function(evt) {
      evt = evt ? evt : window.event;
      var charCode = evt.which ? evt.which : evt.keyCode;
      if (
        (charCode > 31 &&
          (charCode < 48 || charCode > 57) &&
          charCode !== 46) ||
        this.amount.length > 1
      ) {
        evt.preventDefault();
      } else {
        return true;
      }
    },
    async importLeagues() {
      var response = await optionsService.addLeagues(this.amount);
      if (response != null) {
        vm.$snotify.success("Successfully imported additional leagues");
      } else {
        vm.$snotify.error("Something went wrong!");
      }
      this.closeDialog();
    }
  }
};
</script>