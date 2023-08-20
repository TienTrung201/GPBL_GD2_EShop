<script setup>
// import { RouterView } from 'vue-router';
import { RouterView } from 'vue-router';
import TheHeader from './TheHeader.vue';
import TheSidebar from './TheSidebar.vue';
import TheContiner from './TheContiner.vue';
</script>
<template lang="">
    <div class="main">
        <TheHeader></TheHeader>
        <div class="main__content">
            <TheSidebar></TheSidebar>
            <TheContiner>
                <RouterView />
            </TheContiner>
        </div>
    </div>
    <!-- <MISADialog :hidden="true" /> -->
    <!-- <MISAToast :hidden="true" /> -->
    <MISAToast></MISAToast>
</template>

<style lang="scss">
.main {
    display: flex;
    flex-direction: column;
    height: 100vh;
    overflow: hidden;
}

.main__content {
    display: flex;
    /* flex: 1; */
    overflow: hidden;
    height: calc(100% - 56px);
}
</style>
