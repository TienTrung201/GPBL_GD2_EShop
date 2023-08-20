<template>
    <div :class="{ 'wrapper-radio-row': props.row }" class="wrapper-radio">
        <p class="ntt-radio--title">{{ props.label }}</p>
        <div class="groupRadio">
            <slot></slot>
        </div>
    </div>
</template>
<script setup>
const props = defineProps({
    //Danh sách dữ liệu hiện thị trên bảng
    label: {
        //label của nhóm radio
        type: String,
        require: true,
        default: '',
    },
    row: {
        type: Boolean,
        default: false,
    },
});
</script>
<style lang="scss">
.wrapper-radio {
    .ntt-radio--title {
        color: var(--text-primary-color);
        margin-bottom: 8px;
    }
    .groupRadio {
        display: flex;
        align-items: center;
        height: 36px;
        column-gap: 16px;
    }
    .ntt-radio {
        // margin-bottom: 12px;
        display: flex;
        align-items: center;
    }
    input {
        height: 20px;
        margin-right: 8px;
        width: 20px;
        cursor: pointer;
    }
}
.wrapper-radio-row {
    display: flex;
    align-items: center;
    .ntt-radio--title {
        margin-bottom: 0px;
        min-width: 170px;
    }
    .groupRadio {
        height: 32px;
    }
}
</style>
