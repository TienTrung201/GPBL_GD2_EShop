<script setup>
import Enum from '../../common/enum';
import UnitForm from './UnitForm.vue';
import MISAResource from '../../common/resource';
import { ref } from 'vue';
import { useResource } from '../../stores/resource.js';
const resource = useResource();
const columns = ref([
    {
        title: 'Tên đơn vị tính',
        dataIndex: 'name',
        key: 'UnitName',
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
    <BaseList :columns="columns" name="Unit" code="UnitName"><UnitForm></UnitForm></BaseList>
</template>

<style lang="scss" scoped>
//table
</style>
