<template lang="">
    <label class="label-input" :class="{ 'label-input-row': props.row }"
        ><p>{{ props.label }}<span v-if="require" :style="{ color: '#E60000', marginLeft: '4px' }">*</span></p>
        <div class="wrapper-input error">
            <input
                :autocomplete="false"
                @focus="handleFocusInput"
                ref="input"
                :class="[
                    'input-text-base',
                    { 'ntt-success': props.isSuccess },
                    { readonly: readonly },
                    { validate: props.validate },
                    { 'ntt-error': errorMessage },
                    { 'text-align--right': right },
                    { noborder: noborder },
                ]"
                :type="type"
                :name="''"
                :placeholder="props.placeholder"
                :readonly="props.readonly"
                :value="value"
                @input="handleChangeInput"
                @blur="onBlur"
                @keydown.enter="onEnter"
                :title="tooltip"
            />
            <div v-if="errorMessage && errorBottom === false" class="error-wrapper">
                <div class="error-wrapper__icon center">
                    <img src="../../../assets/icons/error-icon-circle.webp" alt="" />
                </div>
                <span :class="{ 'show-message': showMessage }" class="error-message">{{ errorMessage }}</span>
            </div>
            <span v-if="props.isSuccess">
                <img class="success-icon" src="../../../assets/icons/check-circle.svg" alt="" />
            </span>
            <div v-if="false" class="wrapper-svg center">
                <svg class="circle-svg">
                    <circle cx="50%" cy="50%" r="6"></circle>
                </svg>
            </div>
        </div>
        <span v-if="errorMessage && errorBottom === true" class="error-message-bottom">{{ errorMessage }}</span>
    </label>
</template>
<script setup>
import { onMounted, ref } from 'vue';
import { convertCurrency } from '../../../common/convert-data';

const emit = defineEmits(['update:value', 'input-validation', 'blur', 'enter']);
const props = defineProps({
    name: { String, default: () => 'name' }, //name input
    right: { type: Boolean, default: false }, // text ở bên phải
    isSuccess: { Boolean, default: () => null },
    label: { String, default: () => '' }, //label input
    validate: { Boolean, default: () => false }, //có validate thì input nhập vào có màu
    errorMessage: { String, default: () => '' }, //tex cảnh báo lỗi
    placeholder: { String, default: () => '' }, //placeholder
    type: { String, default: () => 'text' }, //loại input
    value: { String, default: () => '' }, //value input
    readonly: { Boolean, default: false }, // chỉ đọc
    focus: Boolean, //focus input
    require: { type: Boolean, default: false },
    tooltip: { type: String, default: '' }, //tooltip cho input
    checkNumber: { type: Boolean, default: false }, //Props quy định nhập số
    isName: { type: Boolean, default: false }, // Nếu là kiểu nhập tên thì In Hoa chữ cái đầu
    maxLength: {
        type: Number,
    },
    row: { type: Boolean, default: false },
    autoFocusMount: { type: Boolean, default: false }, //Khi mount thì input tự động focus
    money: { type: Boolean, default: false }, //Định dạng tiền tệ
    notDelete: { type: Boolean, default: false }, //Định dạng tiền tệ
    errorBottom: { type: Boolean, default: false }, //Error message ở dưới
    noborder: { type: Boolean, default: false }, //input không có border
});
const input = ref(null);
const oldValue = ref(props.value);
const showMessage = ref(false);
/**
 * Author: Tiến Trung (29/06/2023)
 * Description: hàm sự kiện focus vào input
 */
const handleFocusInput = () => {
    if (!props.readonly) {
        input.value.select();
        showMessage.value = true;
    }
};
/**
 * Author: Tiến Trung (29/06/2023)
 * Description: hàm sự kiện blur
 */
const onBlur = (e) => {
    try {
        if (e.target.value.trim('').length === 0 && props.notDelete) {
            emit('update:value', oldValue.value);
            emit('blur', oldValue.value, oldValue.value);
        } else {
            emit('blur', e.target.value, oldValue.value);
        }

        if (props.isName) {
            const wordsUpperCase = convertToTitleCase(e.target.value);
            emit('update:value', wordsUpperCase);
        }
        showMessage.value = false;
    } catch (error) {
        console.log(error);
    }
};
/**
 * Author: Tiến Trung (29/06/2023)
 * Description: hàm sự kiện khi enter
 */
const onEnter = (e) => {
    emit('enter', e.target.value);
};
/**
 * Author: Tiến Trung (29/06/2023)
 * Description: hàm để binding Input
 */
const handleChangeInput = (e) => {
    //Nếu có max length thì không cho nhập nữa
    if (e.target.value.length > props.maxLength) {
        input.value.value = props.value;
        return;
    }
    if (props.checkNumber) {
        if (/^\d+$/.test(e.target.value) || e.target.value === '') {
            emit('update:value', e.target.value);
            emit('input-validation', e.target.value);
        } else {
            input.value.value = props.value;
        }
    } else if (props.money) {
        if (e.target.value.trim('').length === 0) {
            emit('update:value', '0');
            emit('input-validation', '0');
        }
        const value = e.target.value.replace(/\./g, '');
        if (/^\d+$/.test(value)) {
            emit('update:value', convertCurrency(Number(value)));
            emit('input-validation', convertCurrency(Number(value)), convertCurrency(Number(oldValue.value)));
        } else {
            input.value.value = props.value;
        }
    } else {
        emit('update:value', e.target.value);
        emit('input-validation', e.target.value, oldValue.value);
    }
    // Nếu là chữ thì không cho nhập khi truyền props chỉ nhập số
};
/**
 * Author: Tiến Trung (13/07/2023)
 * Description: khi component dược tạo xong thì chủ
 * động focus vào input có prop focus
 */
onMounted(() => {
    oldValue.value = props.value;
    if (props.focus) {
        autoFocus();
    }
    if (props.autoFocusMount) {
        autoFocus();
    }
});
/**
 * Author: Tiến Trung (13/07/2023)
 * Description: Hàm convert chữ thường thành hoa
 */
function convertToTitleCase(input) {
    if (!input) return '';

    // Tách các từ trong chuỗi bằng khoảng trắng hoặc dấu cách
    const words = input.toLowerCase().split(/[\s]+/);
    // Chuyển chữ cái đầu của từ thành chữ in hoa
    const capitalizedWords = words.map((word) => {
        return word.charAt(0).toUpperCase() + word.slice(1);
    });
    // Ghép các từ đã chuyển thành chuỗi mới
    return capitalizedWords.join(' ');
}
/**
 * Author: Tiến Trung (13/07/2023)
 * Description: Hàm chủ động focus vào input
 */
const autoFocus = () => {
    input.value.focus();
};
defineExpose({ autoFocus });
</script>
<style lang="scss">
@import './input.scss';
</style>
