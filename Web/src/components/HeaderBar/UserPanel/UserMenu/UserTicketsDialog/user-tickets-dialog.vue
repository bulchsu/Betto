<template>
  <div v-if="this.user != null">
    <TicketDetailsDialog
      :ticketId="ticketDetailsId"
      :dialogVisibility="ticketDetailsDialogVisibility"
      @dialogClosed="onTicketDetailsDialogClosed"
      v-if="ticketDetailsDialogVisibility"
    />
    <v-dialog @click:outside="closeDialog" v-model="dialogVisibility" max-width="900px">
      <v-card>
        <v-card-title>
          <span class="display-1 my-3 mx-3">{{ getHeaderLabel }}</span>
        </v-card-title>
        <v-row justify="space-around" style="max-width: 900px">
          <v-col cols="2">
            <div>
              <div class="my-5">
                <small class="label-header">Pending tickets</small>
                <div class="label-content">{{ pendingTickets }}</div>
              </div>
              <div class="my-5">
                <small class="label-header">Won tickets</small>
                <div class="label-content">{{ wonTickets }}</div>
              </div>
              <div class="my-5">
                <small class="label-header">Lost tickets</small>
                <div class="label-content">{{ lostTickets }}</div>
              </div>
            </div>
          </v-col>
          <v-col cols="8">
            <v-simple-table>
              <template v-slot:default>
                <thead>
                  <tr>
                    <th class="text-center">Stake</th>
                    <th class="text-center">Rate</th>
                    <th class="text-center">Status</th>
                    <th class="text-center">Creation time</th>
                    <th class="text-center">Reveal time</th>
                    <th class="text-center">Reveal/Inspect</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="ticket in selectedTickets" :key="ticket.ticketId">
                    <td class="text-center">{{ ticket.stake }}</td>
                    <td class="text-center">{{ ticket.totalConfirmedRate }}</td>
                    <td class="text-center">
                      <i v-if="ticket.status == 0" class="fas fa-times"></i>
                      <i v-if="ticket.status == 1" class="fas fa-check"></i>
                      <i v-if="ticket.status == 2" class="fas fa-question"></i>
                    </td>
                    <td class="text-center">{{ ticket.creationDateTime | formatDate }}</td>
                    <td class="text-center">{{ ticket.revealDateTime | formatDate }}</td>
                    <td class="text-center">
                      <v-btn icon @click="inspect(ticket)">
                        <i class="fas fa-eye"></i>
                      </v-btn>
                    </td>
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
              @input="chooseTickets"
              v-model="selectedPage"
              :length="pageAmount"
              :total-visible="5"
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
import { ticketService } from "@/shared/TicketModule/ticket-service";
import TicketDetailsDialog from "./TicketDetailsDialog/ticket-details-dialog";
import vm from "@/main";

export default {
  name: "UserTicketsDialog",
  components: {
    TicketDetailsDialog
  },
  data() {
    return {
      user: null,
      pageAmount: 1,
      selectedPage: 1,
      selectedTickets: [],
      ticketDetailsDialogVisibility: false,
      ticketDetailsId: null
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
    async refresh() {
      this.user = await userService.getUserById(
        this.getLoggedUser.userId,
        false,
        true
      );
      this.user.tickets = this.user.tickets.slice().reverse();
      this.pageAmount = this.getPageAmount(this.user.tickets.length);
      this.selectedPage = 1;
      this.chooseTickets();
    },
    getPageAmount(length) {
      return Math.floor(length / 4) + 1;
    },
    chooseTickets() {
      var begin = 4 * (this.selectedPage - 1);
      this.selectedTickets = this.user.tickets.slice(begin, begin + 4);
    },
    round(number) {
      return Math.round(number * 1000) / 1000;
    },
    async inspect(ticket) {
      if (ticket.status == 2) {
        await this.reveal(ticket);
      } else {
        this.ticketDetailsId = ticket.ticketId;
        this.openTicketDetailsDialog();
      }
    },
    async reveal(ticket) {
      var response = await ticketService.revealTicket(ticket.ticketId);
      if (response != null) {
        if (response.status == 1) {
          vm.$snotify.success("Congratulations, a ticket is won!");
        } else {
          vm.$snotify.error("You have lost! Try once again.");
        }
        await this.refresh();
      } else {
        vm.$snotify.error("Something went wrong! Try again later.");
      }
    },
    openTicketDetailsDialog() {
      this.ticketDetailsDialogVisibility = true;
    },
    onTicketDetailsDialogClosed() {
      this.ticketDetailsDialogVisibility = false;
    }
  },
  computed: {
    ...mapGetters("UserModule", ["getLoggedUser"]),
    getHeaderLabel() {
      return `${this.getLoggedUser.username} tickets`;
    },
    pendingTickets() {
      return this.user.tickets.filter(t => t.status == 2).length;
    },
    wonTickets() {
      return this.user.tickets.filter(t => t.status == 1).length;
    },
    lostTickets() {
      return this.user.tickets.filter(t => t.status == 0).length;
    }
  },
  created: async function() {
    await this.refresh();
  },
  filters: {
    formatDate: function(date) {
      return date != null
        ? date.replace("T", " ").slice(0, 19)
        : "It's not revealed yet";
    }
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