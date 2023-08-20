<script setup>
import TheMain from './components/layouts/TheMain.vue';
console.log(import.meta.env.VITE_APP_BASEURL);
</script>

<template>
    <div id="app">
        <TheMain></TheMain>
        <!-- <MISALoading></MISALoading> -->
    </div>
</template>

<style lang="css" scoped></style>
