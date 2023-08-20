<template lang="">
    <span
        class="ms-icon"
        :style="{ width: props.width + 'px', height: props.height + 'px' }"
        :class="[props.class, `ms-icon-${props.icon}`]"
    >
    </span>
</template>
<script setup>
const props = defineProps({
    class: { type: String, default: '' }, //class cho icon
    icon: { type: String, default: '' }, //loại icon
    width: { type: String, default: '' }, //chiều ngang icon
    height: { type: String, default: '' }, //chiều cao icon
});
</script>
<style lang="scss">
@import './icon.scss';
</style>
