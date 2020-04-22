<template>
  <div v-if="user != null">
    <v-dialog @click:outside="closeDialog" v-model="dialogVisibility" max-width="900px">
      <v-card>
        <v-card-title>
          <span class="display-1 my-3 mx-3">{{ getHeaderLabel }}</span>
        </v-card-title>
        <v-row justify="space-around" style="max-width: 900px">
          <v-col cols="2">
            <div>
              <div class="my-5">
                <small class="label-header">Account Balance</small>
                <div class="label-content">{{ round(user.accountBalance) }}</div>
              </div>
              <div class="my-5">
                <small class="label-header">Deposit Sum</small>
                <div class="label-content">{{ round(depositSum) }}</div>
              </div>
              <div class="my-5">
                <small class="label-header">Withdraw Sum</small>
                <div class="label-content">{{ round(withdrawSum) }}</div>
              </div>
            </div>
          </v-col>
          <v-col cols="3">
            <v-text-field
              v-model="amount"
              @keypress="isNumber($event)"
              class="mt-7"
              label="Amount"
              outlined
            ></v-text-field>
            <v-switch v-model="paymentType" :label="paymentTypeLabel(this.paymentType)"></v-switch>
            <v-btn
              :disabled="!isCreatingPaymentPossible"
              @click="createPayment"
              class="mx-4 my-3"
              color="primary"
            >CREATE PAYMENT</v-btn>
          </v-col>
          <v-col cols="5">
            <v-simple-table>
              <template v-slot:default>
                <thead>
                  <tr>
                    <th class="text-center">Amount</th>
                    <th class="text-center">Type</th>
                    <th class="text-center">Date</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="payment in selectedPayments" :key="payment.paymentId">
                    <td class="text-center">{{ payment.amount }}</td>
                    <td class="text-center">{{ paymentTypeLabel(payment.type) }}</td>
                    <td class="text-center">{{ payment.paymentDateTime }}</td>
                  </tr>
                </tbody>
              </template>
            </v-simple-table>
          </v-col>
        </v-row>
        <v-col class="d-flex justify-end">
          <v-card-actions>
            <v-pagination
              circle
              @input="choosePayments"
              v-model="selectedPage"
              :length="pageAmount"
              :total-visible="7"
            ></v-pagination>
          </v-card-actions>
        </v-col>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
import { mapGetters } from "vuex";
import { userService } from "@/shared/UserModule/user-service";
import vm from "@/main";

export default {
  name: "PaymentsDialog",
  data() {
    return {
      paymentType: false,
      user: null,
      amount: "",
      pageAmount: 1,
      selectedPage: 1,
      selectedPayments: []
    };
  },
  props: {
    dialogVisibility: {
      type: Boolean,
      default: false
    }
  },
  methods: {
    closeDialog() {
      this.$emit("dialogClosed");
    },
    async createPayment() {
      let type = this.paymentType ? 1 : 0;
      var response = await userService.createPayment(
        this.getLoggedUser.userId,
        this.amount,
        type
      );
      if (response != null) {
        vm.$snotify.success("Successfully created payment!");
      }
      await this.refresh();
    },
    isNumber: function(evt) {
      evt = evt ? evt : window.event;
      var charCode = evt.which ? evt.which : evt.keyCode;
      if (
        (charCode > 31 &&
          (charCode < 48 || charCode > 57) &&
          charCode !== 46) ||
        this.amount.length > 3
      ) {
        evt.preventDefault();
      } else {
        return true;
      }
    },
    async refresh() {
      this.user = await userService.getUserById(
        this.getLoggedUser.userId,
        true,
        false
      );
      this.user.payments = this.user.payments.slice().reverse();
      this.pageAmount = this.getPageAmount(this.user.payments.length);
      this.choosePayments();
      this.selectedPage = 1;
    },
    getPageAmount(length) {
      return Math.floor(length / 4) + 1;
    },
    choosePayments() {
      var begin = 4 * (this.selectedPage - 1);
      this.selectedPayments = this.user.payments.slice(begin, begin + 4);
    },
    paymentTypeLabel(paymentType) {
      return paymentType == 0 ? "Deposit" : "Withdraw";
    },
    round(number) {
      return Math.round(number * 1000) / 1000;
    }
  },
  computed: {
    ...mapGetters("UserModule", ["getLoggedUser"]),
    getHeaderLabel() {
      return `${this.getLoggedUser.username} finances`;
    },
    depositSum() {
      return this.user.payments
        .filter(p => p.type == 0)
        .map(p => p.amount)
        .reduce((a, b) => a + b, 0);
    },
    withdrawSum() {
      return this.user.payments
        .filter(p => p.type == 1)
        .map(p => p.amount)
        .reduce((a, b) => a + b, 0);
    },
    isCreatingPaymentPossible() {
      return (
        this.amount.length != 0 &&
        !this.amount.startsWith("0") &&
        (this.paymentType == 0 || this.amount > this.user.accountBalance)
      );
    }
  },
  created: async function() {
    await this.refresh();
  }
};
</script>

<style lang="scss" scoped>
@import "@/assets/styles/_colors.scss";

.label-header {
  font-size: 16px;
}

.label-content {
  font-size: 20px;
  font-weight: bold;
}

</style>