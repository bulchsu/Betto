<template>
  <v-simple-table class="general-table">
      <tbody>
        <tr class="general-table-row">
            <td class="general-table-cell">Country: </td>
            <td class="general-table-cell">{{ selectedLeague.country }}</td>
        </tr>
        <tr class="general-table-row">
            <td class="general-table-cell">Type: </td>
            <td class="general-table-cell">{{ selectedLeague.type }}</td>
        </tr>
        <tr class="general-table-row">
            <td class="general-table-cell">Season start: </td>
            <td class="general-table-cell">{{ selectedLeague.seasonStart }}</td>
        </tr>
        <tr class="general-table-row">
            <td class="general-table-cell">Season end: </td>
            <td class="general-table-cell">{{ selectedLeague.seasonEnd }}</td>
        </tr>
      </tbody>
  </v-simple-table>
</template>

<script>

import { mapGetters } from 'vuex';

export default {
    name: "GeneralInfoTable",
    computed: {
        ...mapGetters(['getSelectedLeague']),
        selectedLeague() {
            return this.getSelectedLeague;
        }
    }
}
</script>

<style lang="scss" scoped>

.general-table {
    margin-top: 5px;
    width: 100%;
}

.general-table-row {
    height: 100px;
}

.general-table-cell {
    font-size: 18px;
    text-align: center;
}

</style>