<template lang="">
    <label class="ntt-radio">
        <input
            @change="$emit('update:value', Number($event.target.value))"
            :value="props.valueRadio"
            class="ntt-radio--input"
            type="radio"
            :name="props.name"
            :checked="props.valueRadio === props.value"
        />
        <div class="radio-check"></div>
        <slot>{{ optionName }}</slot>
    </label>
</template>
<script setup>
defineEmits(['update:value']);
const props = defineProps({
    name: { String, default: () => '' }, //name radio
    optionName: { String, default: () => '' }, //tên lựa chọn
    valueRadio: { String, default: () => '' }, //giá trị raddio
    value: { String, default: () => '' }, //giá trị raddio binding
});
</script>
<style lang="scss">
.ntt-radio {
    display: flex;
    flex-direction: row;
    cursor: pointer;
    .radio-check {
        position: relative;
        box-shadow: var(--grey-color5) 0px 0px 0px 2px;
        height: 14px;
        width: 14px;
        // transition: all 0.2s ease 0s;
        cursor: pointer;
        border-radius: 50%;
        margin-right: 8px;
        transform: translateY(1px);
        &:after {
            content: '';
            position: absolute;
            border-radius: 50%;
            background-color: var(--primary-color);
            transition: all 500ms ease-in-out;
            width: 8px;
            height: 8px;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            display: none;
        }
    }
    .ntt-radio--input {
        position: absolute;
        opacity: 0;
        &:checked ~ .radio-check {
            box-shadow: var(--primary-color) 0px 0px 0px 2px;
            transition: all 0.2s ease 0s;
            cursor: pointer;
            &:after {
                display: block;
            }
        }
        &:focus ~ .radio-check {
            box-shadow: var(--primary-color) 0px 0px 3px 2px;
            // background-color: var(--blue-color2);
        }
    }
}
</style>
