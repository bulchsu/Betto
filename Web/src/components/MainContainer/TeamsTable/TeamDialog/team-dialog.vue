<template>
  <div>
    <v-dialog
      @click:outside="closeDialog"
      v-if="this.team != null"
      v-model="dialogVisibility"
      max-width="600px"
    >
      <v-card>
        <v-card-title>
          <span class="display-1 my-3 mx-3">{{ team.name }}</span>
        </v-card-title>
        <v-row justify="space-around" style="max-width: 600px">
          <v-col cols="5">
            <v-card-text>
              <div class="my-2">
                <small class="label-header">Country</small>
                <div class="label-content">{{ team.country }}</div>
              </div>
              <div class="my-2">
                <small class="label-header">National</small>
                <div class="label-content">{{ nationalLabel }}</div>
              </div>
              <div class="my-2">
                <small class="label-header">Stadium</small>
                <div class="label-content">{{ team.venue.name }}</div>
              </div>
              <div class="my-2">
                <small class="label-header">Address</small>
                <div class="label-content">{{ addressLabel }}</div>
              </div>
              <div class="my-2">
                <small class="label-header">Turf</small>
                <div class="label-content">{{ team.venue.surface }}</div>
              </div>
              <div class="my-2">
                <small class="label-header">Stadium capacity</small>
                <div class="label-content">{{ team.venue.capacity }}</div>
              </div>
            </v-card-text>
          </v-col>
          <v-col v-if="team.logo != null" class="d-flex align-center" cols="5">
            <v-img :src="team.logo" align="center" />
          </v-col>
        </v-row>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
import { leagueService } from "@/shared/LeagueModule/league-service";

export default {
  name: "TeamDialog",
  data() {
    return {
      team: null
    };
  },
  props: {
    dialogVisibility: {
      type: Boolean,
      default: false
    },
    teamId: {
      type: Number
    }
  },
  methods: {
    closeDialog() {
      this.$emit("dialogClosed");
    }
  },
  created: async function() {
    this.team = await leagueService.getTeamById(this.teamId);
  },
  computed: {
    nationalLabel() {
      return this.team.isNational == "true" ? "Yes" : "No";
    },
    addressLabel() {
      return `${this.team.venue.address}, ${this.team.venue.city}`;
    }
  }
};
</script>

<style lang="scss" scoped>
@import "@/assets/styles/_colors.scss";

.label-header {
    font-size: 13px;
}

.label-content {
    font-size: 15px;
    font-weight: bold;
}

</style>