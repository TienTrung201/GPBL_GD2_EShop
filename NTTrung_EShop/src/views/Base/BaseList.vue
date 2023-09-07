<script setup>
import MISATable from '../../components/base/table/MISATable.vue';
import MISASettingTable from '../../components/base/table/MISASettingTable.vue';
import { convertDataTable } from '@/common/convert-data.js';
import { computed, ref, watch, onMounted, onUnmounted, onUpdated } from 'vue';
import { useModalForm } from '../../stores/modalform';
import { storeToRefs } from 'pinia';
import Enum from '../../common/enum';
import { useDialog } from '../../stores/dialog';
import { useToast } from '../../stores/toast';
import { useResource } from '../../stores/resource.js';
import MISAResource from '../../common/resource';
import { useRoute } from 'vue-router';
import router from '../../router';
import { useInventory } from '../../stores/inventory';
import baseApi from '../../api/base-api';
const route = useRoute();
const paging = ref({ totalPage: 1, currentPage: 1, pageSize: '10', TotalRecords: 0 });
const searchData = ref('');
const dataTable = ref([]);
const table = ref(null);
const skeleton = ref(false);
const modalForm = useModalForm();
const dialog = useDialog();
const toast = useToast();
const inventory = useInventory();
const resource = useResource();
const { objectData } = storeToRefs(dialog);
const showSettingTable = ref(false);
const columnsTable = ref();
// const isImportExcel = ref(false);
const isExportExcel = ref(false);
const loadingExport = ref(false);
const filter = ref({});
const props = defineProps({
    columns: {
        type: Array,
        default: () => [],
    },
    name: {
        type: String,
        default: '',
    },
    code: {
        type: String,
        default: 'Code',
    },
});
/**
 * Author: Tiến Trung (24/06/2023)
 * Description: hàm call api lấy dữ liệu gán cho dữ liệu table
 */
const getData = async (filterObject) => {
    try {
        if (route.query?.page === undefined) {
            router.push({ query: { page: 1 } });
            return;
        }
        skeleton.value = true;
        const filterData = {
            pageSize: paging.value.pageSize,
            currentPage: paging.value.currentPage,
            ...filterObject,
        };
        filter.value = filterObject;

        baseApi.path = Enum.Router[props.name].Api;
        baseApi.method = Enum.ApiMethod.POST;
        baseApi.data = filterData;
        const response = await baseApi.request('filter');
        console.log(response);

        paging.value.totalPage = Math.ceil(response.data.TotalRecords / paging.value.pageSize);
        paging.value.currentPage = route.query.page ? Number(route.query.page) : 1;
        paging.value.TotalRecords = response.data.TotalRecords;
        //Nếu không có data thì lấy trang cuối - 1
        if (response.data.Data.length === 0 && paging.value.TotalRecords > 0) {
            if (paging.value.currentPage > 1) {
                router.push({
                    path: Enum.Router[props.name].Path,
                    query: { page: paging.value.totalPage },
                });
            }
            return;
        } else {
            dataTable.value = convertDataTable(response.data.Data, columnsTable.value, resource.langCode);
        }
    } catch (error) {
        console.log(error);
    } finally {
        skeleton.value = false;
    }
};
// /**
//  * Author: Tiến Trung (12/07/2023)
//  * Description: Hàm nhận emit filter data
//  */
const filterData = (filterObject) => {
    dataTable.value = [];
    //Nếu ở trang khác 1 thì getdata về trang 1
    if (paging.value.currentPage === 1) {
        getData(filterObject);
    } else {
        //Nếu xóa hết filter thì không filter nữa
        filter.value = { ...filter.value, ...filterObject };
        router.push({
            path: Enum.Router[props.name].Path,
            query: { page: 1 },
        });
    }
};
// /**
//  * Author: Tiến Trung (11/07/2023)
//  * Description: Hàm nhận emit set pagesize cho bảng dữ liệu
//  */
const setPageSize = (pageSize) => {
    paging.value.pageSize = pageSize;
    paging.value.currentPage = 1;
    if (route.query.page === '1') {
        dataTable.value = [];
        getData(filter.value);
    } else {
        router.push({
            path: Enum.Router.ItemCategory.Path,
            query: { page: 1 },
        });
    }
};

// /**
//  * Author: Tiến Trung (11/07/2023)
//  * Description: Hàm set các loại cột cho table
//  */
const setColumnTable = () => {
    columnsTable.value = props.columns;
};
//Gọi khi created
setColumnTable();
// /**
//  * Author: Tiến Trung (11/07/2023)
//  * Description: Khi langCode thay đổi thì chạy lại
//  */
watch(
    () => resource.langCode,
    () => {
        setColumnTable();
    },
);
// /**
//  * Author: Tiến Trung (01/07/2023)
//  * Description: computec lưu mảng Inventory đã chọn
//  */
const dataSelected = computed(() => {
    return dataTable.value.filter((data) => data.isChecked === true);
});

// /**
//  * Author: Tiến Trung (28/06/2023)
//  * Description: hàm click vào button thêm
//  */
const showModalAddData = () => {
    modalForm.setAction(MISAResource[resource.langCode]?.FormTitle?.[props.name]?.Add);
    modalForm.setObjectForm({});
    dialog.setObjectData({});
    modalForm.setMethod(Enum.EditMode.Add);
    modalForm.open();
};
/**
 * Author: Tiến Trung (09/07/2023)
 * Description: Hàm nhận emit khi click sửa thông tin
 */
const handleShowEditInfo = (data) => {
    try {
        let dataEdit = data;
        if (!dialog.objectData[props.name + 'Id']) {
            dataEdit = dataSelected.value[0];
        }
        modalForm.open();
        modalForm.setAction(MISAResource[resource.langCode]?.FormTitle[props.name]?.Update);
        modalForm.setObjectForm(dataEdit);
        dialog.setObjectData(dataEdit);
        modalForm.setMethod(Enum.EditMode.Update);

        //inventory.openForm(dataEdit.InventoryId, Enum.EditMode.Update);
    } catch (error) {
        console.log(error);
    }
};

/**
 * Author: Tiến Trung (03/09/2023)
 * Description: Hàm mở form nhân bản
 */
// const handleReplication = (data) => {
//     try {
//         let dataEdit = data;
//         if (!dialog.objectData?.InventoryId) {
//             dataEdit = dataSelected.value[0];
//         }
//         dialog.setMethod(Enum.EditMode.Add);
//         //inventory.openForm(dataEdit.InventoryId, Enum.EditMode.Copy);
//     } catch (error) {
//         console.log(error);
//     }
//     // dialog.setMethod(Enum.EditMode.Copy);
// };
// /**
//  * Author: Tiến Trung (01/07/2023)
//  * Description: hàm nhận emit chọn hàng trên table
//  */
const selectRowTable = (isChecked, data) => {
    try {
        dataTable.value = dataTable.value.map((dataMap) => {
            if (dataMap[props.name + 'Id'] === data[props.name + 'Id']) {
                dataMap.isChecked = isChecked;
            }
            return dataMap;
        });
    } catch (error) {
        console.log(error);
    }
};
// /**
//  * Author: Tiến Trung (01/07/2023)
//  * Description: hàm nhận emit tất cả trên table
//  */
const selectAllTable = (isChecked) => {
    try {
        dataTable.value = dataTable.value.map((entity) => {
            entity.isChecked = isChecked;
            return entity;
        });
    } catch (error) {
        console.log(error);
    }
};
/**
 * Author: Tiến Trung (28/06/2023)
 * Description: hàm click unCheck tất cả data báo cho component table con để bỏ check
 */
const handleUnCheckedAll = () => {
    dataTable.value = dataTable.value.map((entity) => {
        entity.isChecked = false;
        return entity;
    });
};

/**
 * Author: Tiến Trung (29/06/2023)
 * Description: Hàm hiển thị dialog xóa các nhân viên được chọn
 */
const openDialogDeleteSelected = () => {
    try {
        if (dataSelected.value.length > 1) {
            dialog.setMethod(Enum.EditMode.Delete);
            dialog.setFunction(deleteDataSelected);
            modalForm.setMethod(Enum.EditMode.Delete);
            dialog.open({
                title: MISAResource[resource.langCode]?.Dialog['Delete' + props.name]?.Title,
                content: MISAResource[resource.langCode]?.Dialog['Delete' + props.name]?.DeleteListContent,
                action: MISAResource[resource.langCode]?.Button?.Delete,
                buttonSec: MISAResource[resource.langCode]?.Button?.No,
                type: Enum.ButtonType.Danger,
                icon: 'question-dialog',
                loading: false,
            });
        } else {
            let dataDelete = dialog.objectData;
            if (!dialog.objectData[props.name + 'Id']) {
                dataDelete = dataSelected.value[0];
            }
            dialog.setObjectData(dataDelete);
            dialog.setMethod(Enum.EditMode.Delete);
            modalForm.setMethod(Enum.EditMode.Delete);
            dialog.setFunction(deleteData);
            debugger;
            dialog.open({
                title: MISAResource[resource.langCode]?.Dialog['Delete' + props.name]?.Title,
                content: MISAResource[resource.langCode]?.Dialog['Delete' + props.name]?.DeleteContent.replace(
                    'code',
                    dialog.objectData[props.name + props.code],
                ),
                action: MISAResource[resource.langCode]?.Button?.Delete,
                buttonSec: MISAResource[resource.langCode]?.Button?.No,
                type: Enum.ButtonType.Danger,
                icon: 'question-dialog',
                loading: false,
            });
        }
    } catch (error) {
        console.log(error);
    }
};
/**
 * Author: Tiến Trung (28/06/2023)
 * Description: Hàm xóa nhân viên
 */
const deleteData = async () => {
    baseApi.path = Enum.Router[props.name]?.Api;
    baseApi.method = Enum.ApiMethod.DELETE;
    await baseApi.request(`${objectData.value[props.name + 'Id']}`);
    toast.success(MISAResource[resource.langCode]?.Toast?.Success?.Delete);
    //Nếu đang xóa trang cuối còn 1 bản ghi thì quay vể trang cuối -1
    if (dataTable.value.length === 1) {
        dataTable.value = [];
        router.push({
            path: Enum.Router[props.name]?.Path,
            query: { page: paging.value.totalPage - 1 === 0 ? 1 : paging.value.totalPage - 1 },
        });
    } else {
        dataTable.value = [];
        await getData(filter.value);
    }
};
/**
 * Author: Tiến Trung (29/06/2023)
 * Description: Hàm xóa nhân viên
 */
const deleteDataSelected = async () => {
    const listIds = dataSelected.value.map((data) => data[props.name + 'Id']).join(',');
    baseApi.path = Enum.Router[props.name].Api;
    baseApi.method = Enum.ApiMethod.DELETE;
    baseApi.data = listIds;
    await baseApi.request('DeleteMany');
    handleUnCheckedAll();
    toast.success(MISAResource[resource.langCode]?.Toast?.Success?.Delete);
    //Nếu đang xóa trang cuối hết bản ghi quay về trang cuối -1
    if (listIds.split(',').length === dataTable.value.length && paging.value.currentPage === paging.value.totalPage) {
        dataTable.value = [];
        router.push({
            path: Enum.Router[props.name].Path,
            query: { page: paging.value.totalPage - 1 === 0 ? 1 : paging.value.totalPage - 1 },
        });
    } else {
        dataTable.value = [];
        await getData(filter.value);
    }
};

/**
 * Author: Tiến Trung (10/07/2023)
 * Description:hàm reset data và bỏ filter
 */
async function reset() {
    router.push({
        path: Enum.Router[props.name].Path,
    });
    dialog.setObjectData({});
    dataTable.value = [];
    // filter.value={}
    await getData(filter.value);
}
/**
 * Author: Tiến Trung (10/07/2023)
 * Description:hàm xuất file excel
 */
const exportExcel = async (listIds, search) => {
    try {
        loadingExport.value = true;
        //Lấy ra mảng cột được hiển thị
        const columnTableExcel = columnsTable.value.filter((column) => column.isShow);
        baseApi.path = Enum.Router[props.name].Api;
        baseApi.method = Enum.ApiMethod.GET;
        const excelResquest = {
            columns: columnTableExcel,
            ids: listIds === null ? null : listIds,
            search: search ? search : '',
            title: MISAResource[resource.langCode]?.Excel[props.name]?.Title,
            sheet: MISAResource[resource.langCode]?.Excel[props.name]?.Sheet,
        };
        baseApi.path = Enum.Router[props.name].Api;
        const res = await baseApi.downloadExcel(excelResquest);
        // Tạo URL object từ dữ liệu trả về
        const url = window.URL.createObjectURL(
            new Blob([res.data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' }),
        );

        // Tạo một link ẩn và sử dụng nó để tải xuống file
        const link = document.createElement('a');
        link.href = url;
        link.download = MISAResource[resource.langCode].Excel[props.name].FileName;
        link.click();

        // Giải phóng tài nguyên
        window.URL.revokeObjectURL(url);
    } catch (error) {
        console.log(error);
    } finally {
        loadingExport.value = false;
    }
};
/**
 * Author: Tiến Trung (10/07/2023)
 * Description:hàm xuất file excel theo dữ liệu được chọn
 */
const exportExcelFromDataSelected = async () => {
    try {
        const listIds = dataSelected.value.map((data) => data[props.name + 'Id']).join(',');
        await exportExcel(listIds);
    } catch (error) {
        console.log(error);
    }
};

/**
 * Author: Tiến Trung (10/07/2023)
 * Description:hàm đóng menuExprot
 */
const closeMenuExport = () => {
    isExportExcel.value = false;
};
/**
 * Author: Tiến Trung (10/07/2023)
 * Description:hàm đóng mở menu export
 */
const toggleMenuExport = () => {
    isExportExcel.value = !isExportExcel.value;
    table.value.closeMenu();
};
/**
 * Author: Tiến Trung (11/07/2023)
 * Description: Watch khi đường dẫn query url thay đổi thì tự động
 * chuyển page mới
 */
watch(
    () => route.query,
    () => {
        paging.value.currentPage = route.query.page ? Number(route.query.page) : 1;
        // searchData.value = route.query.page ? route.query.search : '';
        dataTable.value = [];
        getData(filter.value);
    },
    { immediate: true },
);
/**
 * Author: Tiến Trung (30/07/2023)
 * Description: Hàm sự kiện phím ctrl + 1 để thêm mới
 */
const onKeyDownInsertNew = (e) => {
    if (e.ctrlKey && e.keyCode === Enum.Keyboard.Ctrl1) {
        e.preventDefault(); // Ngăn chặn hành động mặc định của trình duyệt
        showModalAddData();
    }
};
/**
 * Author: Tiến Trung (30/07/2023)
 * Description: Hàm sự kiện phím ctrl + D để xóa danh sách được chọn
 */
const onKeyDownDelete = (e) => {
    if (dataSelected.value.length > 0 || dialog.objectData[props.name + 'Id']) {
        if (e.ctrlKey && e.keyCode === Enum.Keyboard.D) {
            e.preventDefault(); // Ngăn chặn hành động mặc định của trình duyệt
            openDialogDeleteSelected();
        }
    }
};
/**
 * Author: Tiến Trung (30/07/2023)
 * Description: Hàm sự kiện phím sửa hoặc nhân bản
 */
const handleKeyDown = (e) => {
    if (dataSelected.value.length === 1 || dialog.objectData[props.name + 'Id']) {
        // if (e.ctrlKey && e.keyCode === Enum.Keyboard.C) {
        //     e.preventDefault(); // Ngăn chặn hành động mặc định của trình duyệt
        //     handleReplication(dialog.objectData);
        // }
        if (e.ctrlKey && e.keyCode === Enum.Keyboard.E) {
            e.preventDefault(); // Ngăn chặn hành động mặc định của trình duyệt
            handleShowEditInfo(dialog.objectData);
        }
    }
};
/**
 * Author: Tiến Trung (30/07/2023)
 * Description: khi được update thì thêm sự kiện delete bằng phím
 */
onUpdated(() => {
    if (dataSelected.value.length > 0 || dialog.objectData[props.name + 'Id']) {
        window.addEventListener('keydown', onKeyDownDelete);
    } else {
        window.removeEventListener('keydown', onKeyDownDelete);
    }
    // khi sửa hoặc nhân bản
    if (dataSelected.value.length === 1 || dialog.objectData[props.name + 'Id']) {
        window.addEventListener('keydown', handleKeyDown);
    } else {
        window.removeEventListener('keydown', handleKeyDown);
    }
    // Khi mở form thì xóa sự kiện cũ
    if (dialog.isShow || modalForm.isShow) {
        window.removeEventListener('keydown', onKeyDownDelete);
        window.removeEventListener('keydown', onKeyDownInsertNew);
    } else {
        window.addEventListener('keydown', onKeyDownInsertNew);
    }
    // Khi mở form thì xóa sự kiện cũ
    // if (inventory.editMode !== Enum.EditMode.None) {
    //     window.removeEventListener('keydown', handleKeyDown);
    // } else {
    //     window.addEventListener('keydown', handleKeyDown);
    // }
});
/**
 * Author: Tiến Trung (30/07/2023)
 * Description: vòng đời mount và unmount để add sự kiện và hủy sự kiện
 */
onMounted(() => {
    window.addEventListener('keydown', onKeyDownInsertNew);
    window.addEventListener('click', closeMenuExport);
});
/**
 * Author: Tiến Trung (30/07/2023)
 * Description: Component bị hủy thì xóa sự kiện
 */
onUnmounted(() => {
    window.removeEventListener('keydown', onKeyDownInsertNew);
    window.removeEventListener('keydown', onKeyDownDelete);
    window.removeEventListener('click', closeMenuExport);
});
</script>
<template lang="">
    <div class="tt-continer__body">
        <div class="tt-continer__table">
            <template v-if="showSettingTable">
                <MISASettingTable
                    :resetData="setColumnTable"
                    :isShow="showSettingTable"
                    @close="showSettingTable = false"
                    @drag-column="(swapColumn) => (columnsTable = swapColumn)"
                    :columns="columnsTable"
                ></MISASettingTable>
            </template>
            <div class="tt-continer__controll-table">
                <ul class="tt-continer__controll-list">
                    <MISAButton
                        @click="showModalAddData"
                        :type="Enum.ButtonType.IconPri"
                        :action="MISAResource[resource.langCode]?.Button?.Add"
                        v-tooltip-tippy="Enum.KeyboardShortcuts.Ctrl1"
                    >
                        <template #icon>
                            <MISAIcon width="21" height="10" icon="plus" />
                        </template>
                    </MISAButton>
                    <!-- <MISAButton
                        @click="handleReplication(dialog.objectData)"
                        :disable="
                            !(dataSelected.length > 0 && dataSelected.length < 2) &&
                            !dialog.objectData[props.name + 'Id']
                        "
                        :type="Enum.ButtonType.IconPri"
                        :action="MISAResource[resource.langCode]?.Button?.Replication"
                        v-tooltip-tippy="Enum.KeyboardShortcuts.CtrlC"
                    >
                        <template #icon>
                            <MISAIcon width="25" height="15" icon="replication" />
                        </template>
                    </MISAButton> -->
                    <MISAButton
                        @click="handleShowEditInfo(dialog.objectData)"
                        :disable="
                            !(dataSelected.length > 0 && dataSelected.length < 2) &&
                            !dialog.objectData[props.name + 'Id']
                        "
                        :type="Enum.ButtonType.IconPri"
                        :action="MISAResource[resource.langCode]?.Button?.Edit"
                        v-tooltip-tippy="Enum.KeyboardShortcuts.CtrlE"
                    >
                        <template #icon>
                            <MISAIcon width="22" height="12" icon="edit" />
                        </template>
                    </MISAButton>
                    <MISAButton
                        :disable="!dialog.objectData[props.name + 'Id'] && dataSelected.length < 1"
                        @click="openDialogDeleteSelected"
                        :type="Enum.ButtonType.IconPri"
                        :action="MISAResource[resource.langCode]?.Button?.Delete"
                        v-tooltip-tippy="Enum.KeyboardShortcuts.CtrlD"
                    >
                        <template #icon>
                            <MISAIcon width="20" height="12" icon="delete" />
                        </template>
                    </MISAButton>
                    <MISAButton
                        @click="reset"
                        :type="Enum.ButtonType.IconPri"
                        :action="MISAResource[resource.langCode]?.Button?.Reset"
                    >
                        <template #icon>
                            <MISAIcon width="20" height="14" icon="reset2" />
                        </template>
                    </MISAButton>
                    <MISAButton
                        @click="showSettingTable = true"
                        :type="Enum.ButtonType.IconPri"
                        :action="MISAResource[resource.langCode]?.Button?.Setting"
                    >
                        <template #icon>
                            <MISAIcon width="27" height="15" icon="setting2" />
                        </template>
                    </MISAButton>

                    <div class="relative">
                        <MISAButton
                            iconSec
                            :type="Enum.ButtonType.IconPri"
                            :action="MISAResource[resource.langCode]?.Button?.ExportExcel"
                            @click="toggleMenuExport"
                            @click.stop
                            :loading="loadingExport"
                        >
                            <template #icon>
                                <MISAIcon width="26" height="10" icon="export2 " />
                            </template>
                        </MISAButton>

                        <MISAMenuContext v-if="isExportExcel" class="custom-menu" :visibility="true">
                            <template #action>
                                <li @click="exportExcel(null, null)">
                                    {{ MISAResource[resource.langCode]?.Excel[props.name]?.All }}
                                </li>
                                <li @click="exportExcelFromDataSelected">
                                    {{ MISAResource[resource.langCode]?.Excel[props.name]?.Select }}
                                </li>
                            </template>
                        </MISAMenuContext>
                    </div>
                </ul>
            </div>
            <div class="tt-continer__table-body">
                <MISATable
                    ref="table"
                    @select-row="selectRowTable"
                    @select-all="selectAllTable"
                    @edit-data="handleShowEditInfo"
                    @close-export="closeMenuExport"
                    :allDataSelected="dataSelected"
                    :dataTable="dataTable"
                    :columns="columnsTable"
                    :skeleton="skeleton"
                    :searchData="searchData"
                    :propertiesIdName="props.name + 'Id'"
                    :routerCurrent="props.name"
                    @set-pagesize="setPageSize"
                    :totalPage="paging.totalPage"
                    :currentPage="paging.currentPage"
                    :TotalRecords="paging.TotalRecords"
                    @uncheck-all="handleUnCheckedAll"
                    @filter-data="filterData"
                >
                    <template #listMenuContext>
                        <li @click="handleReplication">
                            {{ MISAResource[resource.langCode]?.Table?.Action?.Duplicate }}
                        </li>
                        <li @click="handleDeleteData">{{ MISAResource[resource.langCode]?.Table?.Action?.Delete }}</li>
                        <!-- <li>{{ MISAResource[resource.langCode].Table.Action.DiscontinueUse }}</li> -->
                    </template>
                </MISATable>
            </div>
        </div>
        <slot v-if="modalForm.isShow"></slot>
    </div>
</template>

<style lang="scss" scoped>
//table
@import './base-list.scss';
</style>
