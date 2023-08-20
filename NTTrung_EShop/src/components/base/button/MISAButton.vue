<template lang="">
    <button
        @click="handleClickButton"
        @mouseup="handleMouseUp"
        ref="myButton"
        class="btn-base"
        :class="[
            'btn-' + props.type,
            { disable: props.disable },
            props.class ? props.class : '',
            { 'no-focus': menu },
            { 'icon-eshop': iconEShop },
            { 'btn-sec-icon': sec },
            { 'btn-link-icon': link },
        ]"
        :style="{ height: form ? '40px' : '' }"
    >
        <p :class="{ 'pri-loading': loadingPriColor }" v-if="isLoading || loading" class="wrapper-loading center">
            <svg class="circle-loading">
                <circle cx="50%" cy="50%" r="6"></circle>
            </svg>
        </p>

        <p class="center" :style="{ opacity: isLoading || loading ? 0 : 1 }">
            <template v-if="props.type === 'icon' || props.type === 'i-pri'">
                <span class="center"> <slot name="icon"></slot></span>
            </template>

            {{ props.action }}
        </p>
        <span v-if="iconSec" class="icon-down-btn">
            <MISAIcon width="8" height="6" icon="down-triagle"></MISAIcon>
        </span>
    </button>
</template>
<script setup>
import { ref } from 'vue';
const props = defineProps({
    type: { type: String, default: '' }, //loại button: sec, pri, danger, icon, link
    action: { type: String, default: '' }, //Hành động chữ trong button
    disable: { type: Boolean, default: false }, //disable
    loading: { type: Boolean, default: false }, //loading này phụ thuộc vào prop bên ngoài set true false
    class: { type: String, default: '' }, //class của button
    clickLoading: { type: Boolean, default: false }, //Loading này chỉ cần đặt true thì tự động loading khi bấm
    menu: { type: Boolean, default: false },
    loadingPriColor: { type: Boolean, default: false },
    form: { type: Boolean, default: false },
    iconEShop: {
        type: Boolean,
        default: false,
    },
    iconSec: { type: Boolean, default: false }, // icon show menu
    sec: { type: Boolean, default: false }, //button có icon và là nút phụ
    link: { type: Boolean, default: false }, //button có icon và là nút link
});
const isLoading = ref(false);
const myButton = ref(null);

/**
 * Author: Tiến Trung (13/07/2023)
 * Description: Hàm khi bỏ click thì blur button
 */
const handleMouseUp = () => {
    myButton.value.blur();
};
/**
 * Author: Tiến Trung (13/07/2023)
 * Description: Hàm click button thì loading
 */
const handleClickButton = () => {
    if (props.clickLoading === true) {
        isLoading.value = true;
    }
};
//Được chạy khi button phụ thuộc vào ref từ cha truyền vào
// watch(
//     () => props.loading,
//     () => {
//         // isLoading.value = false;
//     },
// );
/**
 * Author: Tiến Trung (13/07/2023)
 * Description: Hàm chủ động focus vào input
 */
const autoFocus = () => {
    myButton.value.focus();
};
defineExpose({ autoFocus });
</script>
<style lang="scss">
@import './button.scss';
</style>
