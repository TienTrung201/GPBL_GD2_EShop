<template>
    <div class="item-select">
        <p class="item-content">
            <b>{{ MISAResource[resource.langCode]?.KeyName[props.keySelected] }}</b>
            {{ MISAResource[resource.langCode]?.Table?.Filter?.[props.filterBy]?.toLowerCase() }}
            <b v-if="typeof props.value === 'number'">'{{ convertGender(props.value, resource.langCode) }}'</b>
            <b v-else>'{{ props.value }}'</b>
        </p>
        <p @click="$emit('delete-filter', keySelected)" class="center item-icon">
            <MISAIcon width="9" height="8" icon="close-small"></MISAIcon>
        </p>
    </div>
</template>
<script setup>
import MISAResource from '../../../common/resource';
import { convertGender } from '../../../common/convert-data';
import { useResource } from '../../../stores/resource.js';
const resource = useResource();

const props = defineProps({
    filterBy: String, //Lọc theo kiểu nào
    value: [String, Number], //Giá trị lọc
    keySelected: String, //Khóa được chọn
});
defineEmits(['delete-filter']);
</script>
<style lang="scss" scoped>
.item-select {
    display: flex;
    height: 28px;
    align-items: center;
    background-color: #f2f2f2;
    padding: 5px 12px;
    padding-right: 0;
    border-radius: 4px;
    margin-right: 12px;
    margin-bottom: 12px;

    .item-icon {
        padding: 12px;
        cursor: pointer;
    }

    .ms-icon {
        background-color: var(--text-sec-color);
    }
}
</style>
