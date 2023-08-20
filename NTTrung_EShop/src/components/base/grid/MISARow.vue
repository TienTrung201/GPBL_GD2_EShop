<template>
    <div
        :class="['ntt-flex', props.class]"
        :style="{
            display: props.type,
            justifyContent: props.justify,
            alignItems: props.align,
            columnGap: props.colGap,
            marginLeft: '-' + props.gutter + 'px',
            marginRight: '-' + props.gutter + 'px',
        }"
    >
        <slot></slot>
    </div>
</template>
<script setup>
const props = defineProps({
    align: String, //Kiểu display
    justify: String, //xác định căn theo kiểu gì
    colGap: String, //khoảng cách phần tử trong hàng
    gutter: String, //khoảng cách lề
    type: {
        //kiểu display
        type: String,
        default: () => 'flex',
    },
    class: String, //custom cho cột
});
</script>
<style>
.ntt-flex {
    display: flex;
}
</style>
