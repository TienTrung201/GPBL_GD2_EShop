<template lang="">
    <div class="input-group">
        <input
            ref="input"
            @input="handleChangeInput"
            :value="value"
            :placeholder="props.placeholder"
            type="text"
            class="input-text-base i-group"
            :class="props.class"
            @keydown.enter="handleSearch"
        />
        <MISAButton @click="handleSearch(), input.focus()" class="i-group" type="icon">
            <template #icon>
                <img class="i-img" src="../../../assets/icons/search 2-1.svg" alt="" />
            </template>
        </MISAButton>
    </div>
</template>
<script setup>
import { ref } from 'vue';

const emit = defineEmits(['update:value', 'filter']);
const props = defineProps({
    placeholder: { Type: String, default: () => '' }, //placeholder input
    value: { Type: String, default: () => '' }, //value input
    class: { Type: String, default: '' }, //class để custom input
    maxLength: {
        type: Number,
    },
});
const firstSearch = ref('');
/**
 * Author: Tiến Trung (24/06/2023)
 * Description: hàm search data nếu dữ liệu giống nhau sẽ không search lại
 */
const handleSearch = () => {
    if (firstSearch.value !== props.value.trim()) {
        // firstSearch.value = props.value.trim();
        firstSearch.value = props.value;
        emit('filter');
    }
};
/**
 * Author: Tiến Trung (29/06/2023)
 * Description: hàm để binding Input
 */
const handleChangeInput = (e) => {
    //Nếu có max length thì
    if (e.target.value.length > props.maxLength) {
        input.value.value = props.value;
    } else {
        emit('update:value', e.target.value);
    }
};
const input = ref();
</script>
<style lang="scss">
.input-group {
    display: flex;
    align-items: center;
    background-color: #fff;
    height: 36px;
    border-radius: 3px;
    border: 1px solid var(--border-gray);
    overflow: hidden;
    &:focus-within {
        border: 1px solid var(--primary-color);
    }
    .i-group {
        border: none;
        margin: 0;
        background-color: #fff;
        &:first-child {
            border-right: 1px solid var(--border-gray);
            border-top-right-radius: 0px;
            border-bottom-right-radius: 0px;
        }
        &:last-child {
            &:focus {
                background-color: var(--border-gray);
                border-top-left-radius: 0;
                border-bottom-left-radius: 0;
            }
        }
        &:focus {
            outline: none;
        }
    }
}
.width300 {
    width: 300px;
}
</style>
