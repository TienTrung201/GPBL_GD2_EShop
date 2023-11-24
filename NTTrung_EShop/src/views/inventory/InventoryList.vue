<script setup>
import Enum from '../../common/enum';
import MISAResource from '../../common/resource';
import { ref } from 'vue';
import { useResource } from '../../stores/resource.js';
const resource = useResource();
const columns = ref([
    {
        title: MISAResource[resource.langCode]?.Manage?.Inventory?.SKUCode,
        dataIndex: 'code',
        key: 'SKUCode',
        width: '170',
        isShow: true,
        filter: true,
        type: Enum.TypeDataTable.Code,
        // pin: true,
    },
    {
        title: MISAResource[resource.langCode]?.Manage?.Inventory?.InventoryName,
        dataIndex: 'name',
        key: 'InventoryName',
        width: '200',
        isShow: true,
        filter: true,
        // pin: true,
    },
    {
        title: MISAResource[resource.langCode]?.Manage?.Inventory?.Barcode,
        dataIndex: 'Barcode',
        key: 'BarCode',
        width: '170',
        isShow: false,
        filter: false,
        // type: 'gender',
    },
    {
        title: MISAResource[resource.langCode]?.Manage?.Inventory?.ItemCategory,
        dataIndex: 'ItemCategoryName',
        key: 'ItemCategoryName',
        width: '170',
        isShow: true,
        filter: true,
    },
    {
        title: MISAResource[resource.langCode]?.Manage?.Inventory?.Unit,
        dataIndex: 'Unit',
        key: 'UnitName',
        width: '170',
        isShow: true,
        filter: true,
    },
    {
        title: MISAResource[resource.langCode]?.Manage?.Inventory?.AvgCostPrice,
        dataIndex: 'AvgCostPrice',
        key: 'AvgCostPrice',
        type: Enum.TypeDataTable.Money,
        width: '170',
        align: Enum.AlignColumn.Right,
        isShow: true,
        filter: true,
        tooltip: MISAResource[resource.langCode]?.Tooltip?.AvgCostPrice,
    },
    {
        title: MISAResource[resource.langCode]?.Manage?.Inventory?.AvgUnitPrice,
        dataIndex: 'AvgUnitPrice',
        key: 'AvgUnitPrice',
        type: Enum.TypeDataTable.Money,
        width: '170',
        align: Enum.AlignColumn.Right,
        isShow: false,
        filter: true,
        tooltip: MISAResource[resource.langCode]?.Tooltip?.AvgUnitPrice,
    },
    {
        title: MISAResource[resource.langCode]?.Manage?.Inventory?.CostPrice,
        dataIndex: 'CostPrice',
        key: 'CostPrice',
        type: Enum.TypeDataTable.Money,
        width: '170',
        isShow: false,
        filter: false,
    },
    {
        title: MISAResource[resource.langCode]?.Manage?.Inventory?.UnitPrice,
        dataIndex: 'UnitPrice',
        key: 'UnitPrice',
        type: Enum.TypeDataTable.Money,
        width: '170',
        isShow: false,
        filter: false,
    },
    {
        title: MISAResource[resource.langCode]?.Manage?.Inventory?.Color,
        dataIndex: 'Color',
        key: 'Color',
        width: '170',
        isShow: false,
        filter: false,
    },
    {
        title: MISAResource[resource.langCode]?.Manage?.Inventory?.Size,
        dataIndex: 'Size',
        key: 'Size',
        width: '170',
        isShow: false,
        filter: false,
    },

    {
        title: MISAResource[resource.langCode]?.Manage?.Inventory?.Description,
        dataIndex: 'Description',
        key: 'Description',
        width: '300',
        // align: Enum.AlignColumn.Right,
        filter: true,
        isShow: false,
    },
    {
        title: MISAResource[resource.langCode]?.Manage?.Inventory?.IsShowMenu,
        dataIndex: 'IsShowMenu',
        key: 'IsShowMenu',
        width: '250',
        type: Enum.TypeDataTable.Show,
        filter: true,
        isShow: true,
        tooltip: MISAResource[resource.langCode]?.Tooltip?.IsShowMenu,
    },
    {
        title: MISAResource[resource.langCode]?.Manage?.Inventory?.IsActive,
        dataIndex: 'IsActive',
        key: 'IsActive',
        width: '170',
        type: Enum.TypeDataTable.Business,
        isShow: true,
        filter: true,
    },
]);
</script>
<template lang="">
    <BaseList copy routerForm :columns="columns" name="Inventory" code="SKUCode"> </BaseList>
</template>

<style lang="scss" scoped>
//table
</style>
