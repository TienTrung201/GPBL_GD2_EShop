<template lang="">
    <label :class="[{ 'label-input-row': props.row }, props.class]" class="label-input"
        ><p>{{ props.label }}<span v-if="require" :style="{ color: '#E60000', marginLeft: '4px' }">*</span></p>
        <div class="wrapper-input">
            <textarea
                @focus="input.select()"
                ref="input"
                :class="[
                    'input-text-base',
                    'input-text-area',
                    { 'ntt-success': props.isSuccess },
                    { readonly: props.readonly },
                    { validate: props.validate },
                    { 'ntt-error': errorMessage },
                ]"
                :type="type"
                :name="name"
                :placeholder="props.placeholder"
                :readonly="props.readonly"
                :value="value"
                @input="handleChangeInput"
                @blur="onBlur"
                :title="tooltip"
            ></textarea>
            <span v-if="props.isSuccess">
                <img class="success-icon" src="../../../assets/icons/check-circle.svg" alt="" />
            </span>
            <div v-if="false" class="wrapper-svg center">
                <svg class="circle-svg">
                    <circle cx="50%" cy="50%" r="6"></circle>
                </svg>
            </div>
        </div>
        <span v-if="errorMessage" class="error-message">{{ errorMessage }}</span>
    </label>
</template>
<script setup>
import { onMounted, ref } from 'vue';

const emit = defineEmits(['update:value', 'input-validation', 'blur']);
const props = defineProps({
    name: { String, default: () => 'name' }, //name input
    isSuccess: { Boolean, default: () => null },
    label: { String, default: () => '' }, //label input
    validate: { Boolean, default: () => false }, //có validate
    errorMessage: { String, default: () => '' }, //tex cảnh báo lỗi
    placeholder: { String, default: () => '' }, //placeholder
    type: { String, default: () => 'text' }, //loại input
    value: { String, default: () => '' }, //value input
    readonly: { Boolean, default: false }, // chỉ đọc
    focus: Boolean, //focus input
    require: { type: Boolean, default: false },
    tooltip: { type: String, default: '' },
    checkNumber: { type: Boolean, default: false }, //Props quy định nhập số
    isName: { type: Boolean, default: false }, // Nếu là kiểu nhập tên thì In Hoa chữ cái đầu
    row: { type: Boolean, default: false },
    class: { type: String, default: '' },
    maxLength: {
        type: Number,
    },
});
const input = ref(null);
/**
 * Author: Tiến Trung (29/06/2023)
 * Description: hàm sự kiện blur
 */
const onBlur = (e) => {
    emit('blur', e.target.value);
};

/**
 * Author: Tiến Trung (29/06/2023)
 * Description: hàm để binding Input
 */
const handleChangeInput = (e) => {
    //Nếu có max length thì
    if (e.target.value.length > props.maxLength) {
        input.value.value = props.value;
        return;
    }
    emit('update:value', e.target.value);
    emit('input-validation', e.target.value);
};
onMounted(() => {
    if (props.focus) {
        autoFocus();
    }
});
/**
 * Author: Tiến Trung (13/07/2023)
 * Description: Hàm chủ động focus vào input
 */
const autoFocus = () => {
    input.value.focus();
};
/**
 * Author: Tiến Trung (13/07/2023)
 * Description: expose hàm autofocus ra ngoài
 */
defineExpose({ autoFocus });
</script>
<style lang="scss" scoped>
@import './input.scss';

.input-text-area {
    padding: 12px;
    min-height: 74px;
    max-width: 100%;
    min-width: 100%;
    resize: none;
}

.label-input-row {
    display: flex;
    flex-direction: row;
    align-items: flex-start;

    p {
        min-width: 170px;
    }
}

.error-message {
    bottom: -20px;
}
</style>
