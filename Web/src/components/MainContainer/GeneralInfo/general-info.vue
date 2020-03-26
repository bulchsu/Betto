<template>
    <div>
        <GeneralInfoTable class="content" />
    </div>
</template>

<script>

import GeneralInfoTable from './GeneralInfoTable/general-info-table';

export default {
    name: "GeneralInfo",
    components: {
        GeneralInfoTable
    }
}

</script>

<style lang="scss" scoped>

.content {
    float: left;
}

</style>