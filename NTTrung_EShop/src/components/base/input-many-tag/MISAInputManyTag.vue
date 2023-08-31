<template lang="">
    <div @focusout="handleAddTag" class="wrapper-tag">
        <div class="name-tag">
            <p>{{ label }}</p>
        </div>
        <div @click="input.focus()" class="group-tag">
            <div class="wrapper__group-tag">
                <p @click="handleRemoveTag(index, item)" v-for="(item, index) in listTag" :key="index" class="tag-item">
                    {{ item[props.type] }}
                    <span class="center">
                        <MISAIcon width="10" height="10" icon="close-eshop" />
                    </span>
                </p>
                <input :maxLength="50" @blur="addTag" ref="input" @keydown="handleAddTag" v-model="value" type="text" />
            </div>
        </div>
    </div>
</template>
<script setup>
import { ref, watch, onMounted } from 'vue';
import Enum from '../../../common/enum';
import { removeVietnameseTones } from '../../../common/convert-data';
const listTag = ref([]);
const value = ref('');
const input = ref(null);
const props = defineProps({
    label: { type: String, default: '' },
    properties: { type: Array, default: () => [{ Color: 'Xanh', ColorCode: 'XA' }] },
    type: { type: String, default: Enum.TypeProperties.Color },
});
const emits = defineEmits(['update-tag', 'remove-tag']);
/**
 * Author: Tiến Trung (19/08/2023)
 * Description: Hàm xử lý tag
 */
const handleAddTag = (e) => {
    //nếu là giá trị , thì bỏ đi
    if (e.target.value === ',') {
        value.value = '';
        e.target.value = '';
    }
    //nếu là enter hoặc , thì add tag
    if (e.keyCode === Enum.Keyboard.Enter || e.keyCode === Enum.Keyboard.Comma) {
        addTag(e);
    }
    //nếu là nut BackSpace thì xóa tag cuối
    if (e.keyCode === Enum.Keyboard.BackSpace && value.value.trim('') === '') {
        listTag.value.pop();
        emits('update-tag');
    }
};
/**
 * Author: Tiến Trung (19/08/2023)
 * Description: Hàm thêm tag
 */
const addTag = (e) => {
    //Nếu giá trị nhập trùng thì không thêm
    const findTag = listTag.value.findIndex(
        (data) => data[props.type].toLocaleLowerCase() === value.value.toLocaleLowerCase(),
    );
    if (findTag !== -1) {
        setTimeout(() => {
            value.value = '';
            e.target.value = '';
        }, 2);
        return;
    }
    if (value.value.trim('') !== '') {
        const valueTag = value.value.trim().replace(/\s+/g, ' '); // biểu thức chính quy xóa các khoảng trắng đầu tiên
        listTag.value.push({ [props.type]: valueTag, [props.type + 'Code']: autoCode(valueTag, props.type) });
        emits('update-tag');
        setTimeout(() => {
            value.value = '';
            e.target.value = '';
        }, 2);
    }
};
/**
 * Author: Tiến Trung (19/08/2023)
 * Description: Hàm tự động sinh code cho thuộc tính
 */
const autoCode = (value, type) => {
    let autoCode = '';
    const removeVietNameseTonesValue = removeVietnameseTones(value).toLocaleUpperCase();
    const listWord = removeVietNameseTonesValue.split(' '); //từ trong chuỗi
    //color và size có kiểu auto code khác nhau
    switch (type) {
        case Enum.TypeProperties.Size:
            autoCode = validateAutoCode(removeVietNameseTonesValue.replace(' ', ''), true);
            break;
        default:
            if (listWord.length > 1) {
                autoCode = validateAutoCode(
                    listWord.map((item) => item.charAt(0)).join(''),
                    false,
                    listWord[listWord.length - 1],
                );
            } else {
                autoCode = validateAutoCode(removeVietNameseTonesValue.substring(0, 2), true);
            }
            break;
    }
    return autoCode;
};
/**
 * Author: Tiến Trung (21/08/2023)
 * Description: Hàm validate color code
 */
const validateAutoCode = (currentCode, upNumber, lastWord) => {
    let newCode = currentCode;
    let autoNumber = 0;
    let findCode = listTag.value.findIndex((tag) => tag[props.type + 'Code'] === newCode);
    if (upNumber) {
        while (findCode !== -1) {
            autoNumber++;
            newCode = currentCode + `${autoNumber < 10 ? '0' + autoNumber : autoNumber}`;
            findCode = listTag.value.findIndex((tag) => tag[props.type + 'Code'] === newCode);
        }
    } else {
        let currentWordIndex = 0;
        const arrayLastWord = lastWord.split('');
        while (findCode !== -1) {
            if (currentWordIndex < arrayLastWord.length - 1) {
                currentWordIndex++;
                newCode = currentCode + arrayLastWord[currentWordIndex];
                currentCode = newCode;
                findCode = listTag.value.findIndex((tag) => tag[props.type + 'Code'] === newCode);
                if (findCode !== -1) {
                    continue;
                } else {
                    break;
                }
            }
            autoNumber++;
            newCode = currentCode + `${autoNumber < 10 ? '0' + autoNumber : autoNumber}`;
            findCode = listTag.value.findIndex((tag) => tag[props.type + 'Code'] === newCode);
        }
    }
    return newCode;
};
/**
 * Author: Tiến Trung (19/08/2023)
 * Description: Hàm xóa tag ở vị trí bất kỳ
 */
const handleRemoveTag = (index, tag) => {
    emits('remove-tag', index, props.type, tag[props.type + 'Code']);
};
watch(
    () => props.properties,
    () => {
        listTag.value = props.properties;
    },
);
onMounted(() => {
    listTag.value = props.properties;
});
</script>
<style lang="scss">
@import './input-tag.scss';
</style>
