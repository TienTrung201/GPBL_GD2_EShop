<template lang="">
    <div class="continer"><slot></slot></div>
</template>
<script>
export default {};
</script>
<style lang="css">
.continer {
    background-color: #e5e6eb;
    padding: 8px;
    height: 100%;
    flex: 1;
    width: calc(100% - 200px);
    display: flex;
    flex-direction: column;
    z-index: 1;
}
</style>
