<template>
    <div
        :class="[`ntt-col-${props.span}`, 'ntt-col']"
        :style="{
            order: props.order,
            display: props.display,
            flexDirection: props.direction,
            flex: props.flex,
            rowGap: props.rowGap + 'px',
            paddingLeft: props.gutter + 'px',
            paddingRight: props.gutter + 'px',
        }"
    >
        <slot></slot>
    </div>
</template>
<script setup>
const props = defineProps({
    span: String, //Xác định số lượng cột lưới
    order: String, //Các định thứ tự của phần tử
    flex: [String, Number], //điều chỉnh mức độ co giãn thu nhỏ của phần tử
    gutter: String, //khoảng lề của cột, giữa các cột
    direction: { String, default: () => 'row' }, //thay đổi ngang hoặc dọc
    rowGap: { String, default: () => '' }, //Định nghĩa khoảng các dọc giữa các hàng
    display: { String, default: () => '' }, //xác định kiểu display nào
});
</script>
<style>
@import url('./MISACol.css');
</style>
