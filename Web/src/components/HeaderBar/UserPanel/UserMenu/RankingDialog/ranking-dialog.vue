<template>
  <div>
    <v-dialog @click:outside="closeDialog" v-model="dialogVisibility" max-width="600px" scrollable>
      <v-card class="d-flex flex-column">
        <v-card-title>
          <span class="display-1 my-3 mx-3">Users Ranking</span>
        </v-card-title>
        <v-card-text style="height: 400px;">
        <v-col class="d-flex justify-center">
        <v-simple-table style="width: 90%;">
          <template v-slot:default>
            <thead>
              <tr>
                <th>Username</th>
                <th class="text-center">Highest price</th>
                <th class="text-center">Total won amount</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="position in ranking" :key="position.rankingPosition">
                <td>{{ getLabel(position.rankingPosition, position.username) }}</td>
                <td class="text-center">{{ position.highestPrice }}</td>
                <td class="text-center">{{ position.totalWonAmount }}</td>
              </tr>
            </tbody>
          </template>
        </v-simple-table>
        </v-col>
        </v-card-text>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
import { userService } from "@/shared/UserModule/user-service";

export default {
  name: "RankingDialog",
  data() {
    return {
      ranking: []
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
    getLabel(position, username) {
      return `${position}. ${username}`;
    }
  },
  created: async function() {
    this.ranking = await userService.getUsersRanking();
  }
};
</script>