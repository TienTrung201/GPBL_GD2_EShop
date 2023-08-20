<template lang="">
    <label :class="{ 'custom-checkbox': props.custom }" class="container-input center">
        {{ lable }}
        <input
            @change="changeCheckbox"
            class="input--checkbox"
            type="checkbox"
            name="table"
            id=""
            :checked="props.valueCheckbox"
            :value="props.valueCheckbox"
        />
        <div class="checkmark"></div>
        <slot></slot>
    </label>
</template>
<script setup>
const emits = defineEmits(['update:value', 'change-checkbox']);
const props = defineProps({
    lable: {
        // lable checkbox
        type: String,
        default: '',
    },
    value: {
        //value của checkbox
        type: Boolean,
        default: false,
    },
    valueCheckbox: {
        //value của checkbox
        type: Boolean,
        default: false,
    },
    custom: { type: Boolean, default: false },
});
/**
 * Author: Tiến Trung (12/07/2023)
 * Description: Emit binding
 */
const changeCheckbox = (e) => {
    emits('update:value', e.target.checked);
    emits('change-checkbox', e.target.checked);
};
</script>
<style lang="scss">
@import './checkbox.scss';
</style>
