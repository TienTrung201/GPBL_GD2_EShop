<script setup>
import MISATable from '../../components/base/table/MISATable.vue';
import MISASettingTable from '../../components/base/table/MISASettingTable.vue';
import PositionForm from './PositionForm.vue';
import { convertDataTable } from '@/common/convert-data.js';
import { computed, ref, watch, onMounted, onUpdated, onUnmounted } from 'vue';
import positionApi from '../../api/position-api';
import { useModalForm } from '../../stores/modalform';
import { storeToRefs } from 'pinia';
import Enum from '../../common/enum';
import { useDialog } from '../../stores/dialog';
import { useToast } from '../../stores/toast';
import { useResource } from '../../stores/resource.js';
import MISAResource from '../../common/resource';
import { useRoute } from 'vue-router';
import router from '../../router';
import MISAMenuContext from '../../components/base/table/MISAMenuContext.vue';

const route = useRoute();
const paging = ref({ totalPage: 1, currentPage: 1, pageSize: '10', TotalRecords: 0 });
const searchData = ref('');
const idsDelete = ref('');
const dataTable = ref([]);
const table = ref(null);
const skeleton = ref(false);
const loadingButton = ref({ save: false, saveAdd: false });
const modalForm = useModalForm();
const dialog = useDialog();
const toast = useToast();
const resource = useResource();
const { objectData } = storeToRefs(dialog);
const { isShow, method, object } = storeToRefs(modalForm);
const showSettingTable = ref(false);
const columnsTable = ref([]);
const isExportExcel = ref(false);
const loadingExport = ref(false);
// const isImportExcel = ref(false);

// /**
//  * Author: Tiến Trung (11/07/2023)
//  * Description: Hàm set các loại cột cho table
//  */
const setColumnTable = () => {
    columnsTable.value = [
        {
            title: MISAResource[resource.langCode]?.Manage?.PositionInfo?.PositionCode,
            dataIndex: 'id',
            key: 'PositionCode',
            width: '170',
            isShow: true,
            // pin: true,
        },
        {
            title: MISAResource[resource.langCode]?.Manage?.PositionInfo?.PositionName,
            dataIndex: 'name',
            key: 'PositionName',
            width: '200',
            isShow: true,
            // pin: true,
        },
        {
            title: MISAResource[resource.langCode]?.Manage?.PositionInfo?.Description,
            dataIndex: 'description',
            key: 'Description',
            width: '700',
            isShow: true,
            // pin: true,
        },
    ];
};
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
//  * Description: computec lưu mảng data đã chọn
//  */
const dataSelected = computed(() => {
    return dataTable.value.filter((data) => data.isChecked === true);
});

// /**
//  * Author: Tiến Trung (28/06/2023)
//  * Description: hàm click vào button thêm
//  */
const showModalAddData = () => {
    modalForm.setAction(MISAResource[resource.langCode]?.FormTitle?.Position?.Add);
    modalForm.setObjectForm({});
    modalForm.setMethod(Enum.EditMode.Add);
    // dialog.setMethod(Enum.EditMode.Add);
    modalForm.open();
};
/**
 * Author: Tiến Trung (09/07/2023)
 * Description: Hàm nhận emit khi click sửa thông tin
 */
const handleShowEditInfo = (data) => {
    modalForm.open();
    modalForm.setAction(MISAResource[resource.langCode]?.FormTitle?.Position?.Update);
    modalForm.setMethod(Enum.EditMode.Update);
    dialog.setMethod(Enum.EditMode.Update);
    modalForm.setObjectForm(data);
};
// /**
//  * Author: Tiến Trung (01/07/2023)
//  * Description: hàm nhận emit chọn hàng trên table
//  */
const selectRowTable = (isChecked, data) => {
    try {
        dataTable.value = dataTable.value.map((d) => {
            if (d.PositionId === data.PositionId) {
                d.isChecked = isChecked;
            }
            return d;
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
        dataTable.value = dataTable.value.map((data) => {
            data.isChecked = isChecked;
            return data;
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
    dataTable.value = dataTable.value.map((data) => {
        data.isChecked = false;
        return data;
    });
};

/**
 * Author: Tiến Trung (24/06/2023)
 * Description: Hàm call api lấy dữ liệu gán cho dữ liệu table
 */
async function getData() {
    try {
        //Cài đặt param và gọi API
        skeleton.value = true;
        positionApi.method = Enum.ApiMethod.GET;
        positionApi.param = {
            pageSize: paging.value.pageSize,
            pageNumber: paging.value.currentPage,
            search: searchData.value,
        };
        const response = await positionApi.request('filter');
        console.log(response);

        paging.value.totalPage = Math.ceil(response.data.TotalRecords / paging.value.pageSize);
        paging.value.currentPage = route.query.page ? Number(route.query.page) : 1;
        paging.value.TotalRecords = response.data.TotalRecords;
        //Nếu vào trang bất kỳ lớn không có dữ liệu thì trả về trang cuối cùng
        if (response.data.Data.length === 0 && paging.value.TotalRecords > 0) {
            if (paging.value.currentPage > 1) {
                router.push({
                    path: Enum.Router.Position.path,
                    query: { page: paging.value.totalPage },
                });
            }
            return;
        } else {
            dataTable.value = convertDataTable(response.data.Data, columnsTable.value, resource.langCode);
        }
        //Nếu còn dữ liệu cũ được checked sau khi xóa thì trả giữ lại
        if (idsDelete.value.length > 0) {
            dataTable.value.forEach((data) => {
                idsDelete.value.forEach((id) => {
                    if (data.PositionId === id) {
                        data.isChecked = true;
                    }
                });
            });
        }
        // dataTable.value = response?.data ? response.data : [];
    } catch (error) {
        console.log(error);
    } finally {
        skeleton.value = false;
    }
}

/**
 * Author: Tiến Trung (29/06/2023)
 * Description: Hàm hiển thị dialog xóa các nhân viên được chọn
 */
const openDialogDeleteSelected = () => {
    dialog.open({
        title: MISAResource[resource.langCode]?.Dialog?.DeletePosition?.Title,
        content: MISAResource[resource.langCode]?.Dialog?.DeletePosition?.DeleteListContent,
        action: MISAResource[resource.langCode]?.Button?.Delete,
        buttonSec: MISAResource[resource.langCode]?.Button?.No,
        type: Enum.ButtonType.Pri,
        icon: 'warning',
        loading: false,
    });
    dialog.setMethod(Enum.EditMode.Delete);
    modalForm.setMethod(Enum.EditMode.Delete);
};
/**
 * Author: Tiến Trung (28/06/2023)
 * Description: Hàm xóa nhân viên
 */
const deleteOneData = async () => {
    await positionApi.request(`${objectData.value.PositionId}`);
    if (dialog.errorResponse === false) {
        toast.success(MISAResource[resource.langCode]?.Toast?.Success?.Delete);
        //Nếu xóa ở trang cuối hết thì gọi api về trang cuối - 1
        if (dataTable.value.length === 1) {
            dataTable.value = [];
            router.push({
                path: Enum.Router.Position.path,
                query: { page: paging.value.totalPage - 1 === 0 ? 1 : paging.value.totalPage - 1 },
            });
        } else {
            dataTable.value = [];
            await getData();
        }
    }
};
/**
 * Author: Tiến Trung (29/06/2023)
 * Description: Hàm xóa nhân viên
 */
const deleteDataSelected = async () => {
    const listIds = dataSelected.value.map((data) => data.PositionId).join(',');
    idsDelete.value = listIds.split(',');
    positionApi.method = Enum.ApiMethod.DELETE;
    positionApi.data = listIds;
    await positionApi.request('DeleteMany');
    handleUnCheckedAll();
    //Nếu xóa ở trang cuối hết thì gọi api về trang cuối - 1
    if (listIds.split(',').length === dataTable.value.length && paging.value.currentPage === paging.value.totalPage) {
        dataTable.value = [];
        router.push({
            path: Enum.Router.Position.path,
            query: { page: paging.value.totalPage - 1 === 0 ? 1 : paging.value.totalPage - 1 },
        });
    } else {
        dataTable.value = [];
        await getData();
    }
    //nếu 2 mảng bằng nhau thì chưa xóa bản ghi nào
    if (dataSelected.value.length < idsDelete.value.length) {
        toast.success(MISAResource[resource.langCode]?.Toast?.Success?.Delete);
    }
    idsDelete.value = [];
};
/**
 * Author: Tiến Trung (29/06/2023)
 * Description: Hàm xóa nhân viên
 */
const deleteData = async () => {
    //khi không xóa từng đối tượng thì không
    //có đối tượng nào được chọn và object rỗng
    //sẽ chạy vào xóa tất cả
    try {
        positionApi.method = Enum.ApiMethod.DELETE;
        if (Object.keys(objectData.value).length === 0) {
            await deleteDataSelected();
        } else {
            await deleteOneData();
        }
    } catch (error) {
        console.log(error);
    }
};
/**
 * Author: Tiến Trung (28/06/2023)
 * Description: Hàm bật dialog xóa
 */
const handleDeleteData = () => {
    try {
        dialog.setMethod(Enum.EditMode.Delete);
        modalForm.setMethod(Enum.EditMode.Delete);
        dialog.open({
            title: MISAResource[resource.langCode]?.Dialog?.DeletePosition?.Title,
            content: MISAResource[resource.langCode]?.Dialog?.DeletePosition?.DeleteContent.replace(
                'PositionCode',
                dialog.objectData.PositionCode,
            ),
            action: MISAResource[resource.langCode]?.Button?.Delete,
            buttonSec: MISAResource[resource.langCode]?.Button?.No,
            type: Enum.ButtonType.Pri,
            icon: 'warning',
            loading: false,
        });
    } catch (error) {
        console.log(error);
    }
};

/**
 * Author: Tiến Trung (29/06/2023)
 * Description: set Loading cho button
 */
const setLoadingButton = (status, isAddNewForm) => {
    if (isAddNewForm) {
        loadingButton.value.saveAdd = status;
    } else {
        loadingButton.value.save = status;
    }
};
/**
 * Author: Tiến Trung (29/06/2023)
 * Description: Hàm Thêm mới
 */
async function addNewData(data) {
    positionApi.data = data;
    positionApi.method = Enum.ApiMethod.POST;
    const res = await positionApi.request();
    console.log(res);
    await getData();
    toast.success(MISAResource[resource.langCode]?.Toast?.Success?.Add);
}
/**
 * Author: Tiến Trung (29/06/2023)
 * Description: Hàm Gửi form
 */
async function submitFormData(data, isAddNewForm) {
    try {
        setLoadingButton(true, isAddNewForm);
        if (method.value === Enum.EditMode.Update) {
            await updateData(data).then(() => {
                setLoadingButton(false, isAddNewForm);
            });
        }
        if (method.value === Enum.EditMode.Add) {
            await addNewData(data).then(() => {
                setLoadingButton(false, isAddNewForm);
            });
        }
        if (isAddNewForm) {
            showModalAddData();
        } else {
            modalForm.close();
        }
    } catch (error) {
        setLoadingButton(false, isAddNewForm);
        dialog.setMethod(Enum.EditMode.None);
        console.log(error);
    }
}
/**
 * Author: Tiến Trung (29/06/2023)
 * Description: Hàm Update
 */
async function updateData(data) {
    positionApi.data = data;
    positionApi.method = Enum.ApiMethod.PUT;
    const res = await positionApi.request(`${object.value.PositionId}`);
    console.log(res);
    await getData();
    toast.success(MISAResource[resource.langCode]?.Toast?.Success?.Update);
}

/**
 * Author: Tiến Trung (10/07/2023)
 * Description:hàm reset data và bỏ filter
 */
async function reset() {
    router.push({
        path: Enum.Router.Position.path,
    });
    dialog.setObjectData({});
    dataTable.value = [];
    await getData();
}
// /**
//  * Author: Tiến Trung (12/07/2023)
//  * Description: Hàm nhận emit Search filter
//  */
const handleSearchData = () => {
    router.push({
        path: Enum.Router.Position.path,
        query: { page: 1, search: searchData.value || undefined },
    });
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
        getData();
    } else {
        router.push({
            path: Enum.Router.Position.path,
            query: { page: 1 },
        });
    }
};
/**
 * Author: Tiến Trung (10/07/2023)
 * Description:hàm xuất file excel
 */
const exportExcel = async (listIds, search) => {
    try {
        loadingExport.value = true;
        const columnTableExcel = columnsTable.value.filter((column) => column.isShow);
        positionApi.method = Enum.ApiMethod.GET;
        const excelResquest = {
            columns: columnTableExcel,
            ids: listIds === null ? null : listIds,
            search: search ? search : '',
            title: MISAResource[resource.langCode]?.Excel?.Position.Title,
            sheet: MISAResource[resource.langCode]?.Excel?.Position.Sheet,
        };
        const res = await positionApi.downloadExcel(excelResquest);
        // Tạo URL object từ dữ liệu trả về
        const url = window.URL.createObjectURL(
            new Blob([res.data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' }),
        );

        // Tạo một link ẩn và sử dụng nó để tải xuống file
        const link = document.createElement('a');
        link.href = url;
        link.download = MISAResource[resource.langCode].Excel.Position.FileName;
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
        const listIds = dataSelected.value.map((data) => data.PositionId).join(',');
        await exportExcel(listIds);
    } catch (error) {
        console.log(error);
    }
};
/**
 * Author: Tiến Trung (10/07/2023)
 * Description:hàm xuất file excel theo dữ liệu được chọn
 */
const exportExcelFromSearch = async () => {
    try {
        await exportExcel(null, searchData.value);
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
        searchData.value = route.query.page ? route.query.search : '';
        dataTable.value = [];
        getData();
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
    if (e.ctrlKey && e.keyCode === Enum.Keyboard.D) {
        e.preventDefault(); // Ngăn chặn hành động mặc định của trình duyệt
        openDialogDeleteSelected();
    }
};
/**
 * Author: Tiến Trung (30/07/2023)
 * Description: khi được update thì thêm sự kiện delete bằng phím
 */
onUpdated(() => {
    if (dataSelected.value.length > 0) {
        window.addEventListener('keydown', onKeyDownDelete);
    } else {
        window.removeEventListener('keydown', onKeyDownDelete);
    }
    if (dialog.isShow || modalForm.isShow) {
        window.removeEventListener('keydown', onKeyDownDelete);
        window.removeEventListener('keydown', onKeyDownInsertNew);
    } else {
        window.addEventListener('keydown', onKeyDownInsertNew);
    }
});
/**
 * Author: Tiến Trung (30/07/2023)
 * Description: vòng đời mount và unmount để add sự kiện và hủy sự kiện
 */
onMounted(() => {
    window.addEventListener('keydown', onKeyDownInsertNew);
    window.addEventListener('click', closeMenuExport);
});
onUnmounted(() => {
    window.removeEventListener('keydown', onKeyDownInsertNew);
    window.removeEventListener('keydown', onKeyDownDelete);
    window.removeEventListener('click', closeMenuExport);
});
</script>
<template lang="">
    <div class="tt-continer__body">
        <div class="tt-continer__header">
            <h3 class="tt-continer__title center">
                {{ MISAResource[resource.langCode]?.Manage?.PositionInfo?.Name }}
            </h3>
            <div class="tt-continer__action center">
                <!-- <MISAGroupButton></MISAGroupButton> -->

                <MISAButton
                    v-tooltip.top="Enum.KeyboardShortcuts.Ctrl1"
                    @click="showModalAddData"
                    :type="Enum.ButtonType.IconPri"
                    :action="MISAResource[resource.langCode]?.Button?.AddPosition"
                >
                    <!-- <template #icon>
                        <img class="i-img" src="../../assets/icons/whiteplus.svg" alt="" />
                    </template> -->
                </MISAButton>
            </div>
        </div>

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
            <div class="tt-continer__filter-table">
                <div class="tt-continer__filter-left">
                    <div v-if="dataSelected.length > 1" class="selectedTable info-table">
                        <p class="selectedTable__content">
                            {{ MISAResource[resource.langCode]?.Button?.Selected }} <b>{{ dataSelected.length }}</b>
                        </p>
                        <MISAButton
                            :type="Enum.ButtonType.Link"
                            @click="handleUnCheckedAll"
                            :action="MISAResource[resource.langCode]?.Button?.UnSelected"
                        >
                        </MISAButton>
                        <MISAButton
                            :type="Enum.ButtonType.Danger"
                            v-tooltip.top="Enum.KeyboardShortcuts.CtrlD"
                            @click="openDialogDeleteSelected"
                            :action="MISAResource[resource.langCode]?.Button?.Delete"
                        >
                        </MISAButton>
                    </div>
                </div>
                <div class="tt-continer__filter-right">
                    <MISAGroupInput
                        :maxLength="100"
                        @filter="handleSearchData"
                        v-model:value="searchData"
                        :placeholder="MISAResource[resource.langCode]?.Manage?.PositionInfo?.InputSearch"
                        class="width300"
                    ></MISAGroupInput>
                    <MISAButton
                        v-tooltip.top="MISAResource[resource.langCode]?.Tooltip?.ResetData"
                        @click="reset"
                        :class="'filter-btn'"
                        :type="Enum.ButtonType.Icon"
                        ><template #icon> <MISAIcon class="filter-icon" icon="reset"></MISAIcon></template>
                    </MISAButton>
                    <div class="relative">
                        <MISAButton
                            v-tooltip.top="MISAResource[resource.langCode].Tooltip.ExportExcel"
                            @click.stop
                            @click="toggleMenuExport"
                            :class="'filter-btn'"
                            type="icon"
                            :loading="loadingExport"
                            loadingPriColor
                        >
                            <template #icon>
                                <MISAIcon width="24" height="24" class="filter-icon excel" icon="excel"></MISAIcon>
                            </template>
                        </MISAButton>

                        <MISAMenuContext v-if="isExportExcel" class="custom-menu" :visibility="true">
                            <template #action>
                                <li @click="exportExcel(null, null)">
                                    {{ MISAResource[resource.langCode]?.Button?.ExportAll }}
                                </li>
                                <li @click="exportExcelFromSearch">
                                    {{ MISAResource[resource.langCode]?.Button?.ExportSearch }}
                                </li>
                                <li @click="exportExcelFromDataSelected">
                                    {{ MISAResource[resource.langCode]?.Button?.ExportSelected }}
                                </li>
                            </template>
                        </MISAMenuContext>
                    </div>
                    <MISAButton
                        v-tooltip.top="MISAResource[resource.langCode]?.Tooltip?.SettingTable"
                        @click="showSettingTable = !showSettingTable"
                        :class="'filter-btn'"
                        :type="Enum.ButtonType.Icon"
                    >
                        <template #icon>
                            <MISAIcon width="24" height="24" class="filter-icon" icon="setting"></MISAIcon>
                        </template>
                    </MISAButton>
                </div>
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
                    propertiesIdName="PositionId"
                    routerCurrent="Position"
                    @set-pagesize="setPageSize"
                    :totalPage="paging.totalPage"
                    :currentPage="paging.currentPage"
                    :TotalRecords="paging.TotalRecords"
                    @uncheck-all="handleUnCheckedAll"
                >
                    <template #listMenuContext>
                        <!-- <li>{{ MISAResource[resource.langCode].Table.Action.Duplicate }}</li> -->
                        <li @click="handleDeleteData">{{ MISAResource[resource.langCode]?.Table?.Action?.Delete }}</li>
                        <!-- <li>{{ MISAResource[resource.langCode].Table.Action.DiscontinueUse }}</li> -->
                    </template>
                </MISATable>
            </div>
        </div>
    </div>
    <Teleport to="#app">
        <!-- @loading-button="(isload) => (loadingButton = isload)" -->
        <PositionForm :buttonLoad="loadingButton" @submit-form="submitFormData" v-if="isShow"></PositionForm>
        <MISADialog v-if="dialog.isShow" @submit-form="submitFormData" @delete-data="deleteData"></MISADialog>
    </Teleport>
</template>

<style lang="scss" scoped>
// @import './position-list.scss';

//table
</style>
