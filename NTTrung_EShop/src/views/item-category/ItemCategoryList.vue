<script setup>
import Enum from '../../common/enum';
import MISAResource from '../../common/resource';
import { ref } from 'vue';
import { useResource } from '../../stores/resource.js';
const resource = useResource();
const columns = ref([
    {
        title: 'Mã nhóm hàng hóa',
        dataIndex: 'ItemCategoryCode',
        key: 'ItemCategoryCode',
        width: '170',
        isShow: true,
        filter: true,
        type: Enum.TypeDataTable.Code,
        // pin: true,
    },
    {
        title: 'Tên nhóm hàng hóa',
        dataIndex: 'name',
        key: 'ItemCategoryName',
        width: '200',
        isShow: true,
        filter: true,
        // pin: true,
    },

    {
        title: 'Mô tả',
        dataIndex: 'Description',
        key: 'Description',
        width: '300',
        // align: Enum.AlignColumn.Right,
        filter: true,
        isShow: true,
    },
]);
</script>
<template lang="">
    <BaseList :columns="columns" name="ItemCategory"></BaseList>
</template>

<style lang="scss" scoped>
//table
</style>
