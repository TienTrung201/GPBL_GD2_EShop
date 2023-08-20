<template>
    <ul
        ref="listMenu"
        :style="{
            left: props.positionMenuContext?.left + 'px',
            top: props.positionMenuContext?.bottom + 'px',
            visibility: visibility ? 'visible' : 'hidden',
        }"
        class="row-list--action"
        :class="props.class"
    >
        <slot name="action"> </slot>
    </ul>
</template>
<script setup>
import { ref } from 'vue';
const props = defineProps({
    positionMenuContext: {
        //Vị trí của menu
        type: Object,
    },
    visibility: {
        //Ẩn hiện menu
        type: Boolean,
        default: false,
    },
    class: {
        //class cho menu để custom
        type: String,
        default: '',
    },
});
const listMenu = ref(null);
/**
 * Author: Tiến Trung (13/07/2023)
 * Description: Hàm lấy chiều cao của menu
 */
const getHeightMenu = () => {
    return listMenu.value.clientHeight;
};
/**
 * Author: Tiến Trung (13/07/2023)
 * Description: Hàm lấy chiều rộng của menu
 */
const getWidthMenu = () => {
    return listMenu.value.clientWidth;
};
/**
 * Author: Tiến Trung (13/07/2023)
 * Description: expose hàm lấy chiều cao và rộng ra ngoài
 */
defineExpose({ getHeightMenu, getWidthMenu });
</script>
<style lang="scss">
@import './menu-context.scss';
</style>
