<template lang="">
    <div class="wrapper-tag">
        <div class="name-tag">
            <p>{{ label }}</p>
        </div>
        <div @click="input.focus()" class="group-tag">
            <div class="wrapper__group-tag">
                <p @click="handleRemoveTag(index)" v-for="(item, index) in listTag" :key="index" class="tag-item">
                    {{ item[props.type] }}
                    <span class="center">
                        <MISAIcon width="10" height="10" icon="close-eshop" />
                    </span>
                </p>
            </div>
            <input ref="input" @keydown="handleAddTag" v-model="value" type="text" />
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
const emits = defineEmits(['update-tag']);
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
        //Nếu giá trị nhập trùng thì không thêm
        if (
            listTag.value.findIndex(
                (data) =>
                    removeVietnameseTones(data[props.type]).toLocaleLowerCase() ===
                    removeVietnameseTones(value.value).toLocaleLowerCase(),
            ) !== -1
        ) {
            setTimeout(() => {
                value.value = '';
                e.target.value = '';
            }, 2);
        } else if (value.value.trim('') !== '') {
            listTag.value.push({ [props.type]: value.value, [props.type + 'Code']: autoCode(value.value, props.type) });
            emits('update-tag');
            setTimeout(() => {
                value.value = '';
                e.target.value = '';
            }, 2);
        }
    }
    //nếu là nut BackSpace thì xóa tag cuối
    if (e.keyCode === Enum.Keyboard.BackSpace && value.value.trim('') === '') {
        listTag.value.pop();
    }
};
/**
 * Author: Tiến Trung (19/08/2023)
 * Description: Hàm tự động sinh code cho thuộc tính
 */
const autoCode = (value, type) => {
    let autoCode = '';
    const removeVietNameseTonesValue = removeVietnameseTones(value).toLocaleUpperCase();
    const lengthValue = removeVietNameseTonesValue.split(' ');
    switch (type) {
        case Enum.TypeProperties.Size:
            autoCode = removeVietNameseTonesValue.replace(' ', '');
            break;
        default:
            if (lengthValue.length > 1) {
                autoCode = lengthValue.map((item) => item.charAt(0)).join('');
            } else {
                autoCode = removeVietNameseTonesValue.substring(0, 2);
            }
            break;
    }
    return autoCode;
};
/**
 * Author: Tiến Trung (19/08/2023)
 * Description: Hàm xóa tag ở vị trí bất kỳ
 */
const handleRemoveTag = (index) => {
    listTag.value.splice(index, 1);
    emits('update-tag');
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
.wrapper-table {
    height: 300px;
    border: 1px solid var(--border-color);
}
</style>
