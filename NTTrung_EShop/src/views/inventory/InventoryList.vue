<script setup>
import MISATable from '../../components/base/table/MISATable.vue';
import MISASettingTable from '../../components/base/table/MISASettingTable.vue';
import InventoryForm from './InventoryForm.vue';
import { convertDataTable, deepCopy } from '@/common/convert-data.js';
import { computed, ref, watch, onMounted, onUnmounted, onUpdated } from 'vue';
import employeeApi from '../../api/employee-api';
import { useModalForm } from '../../stores/modalform';
import { storeToRefs } from 'pinia';
import Enum from '../../common/enum';
import { useDialog } from '../../stores/dialog';
import { useToast } from '../../stores/toast';
import { useResource } from '../../stores/resource.js';
import MISAResource from '../../common/resource';
import { useRoute } from 'vue-router';
import router from '../../router';
import { convertDateForBE } from '../../common/convert-data';
import MISAMenuContext from '../../components/base/table/MISAMenuContext.vue';
import { useInventory } from '../../stores/inventory';
const route = useRoute();
const paging = ref({ totalPage: 1, currentPage: 1, pageSize: '10', TotalRecords: 0 });
const searchData = ref('');
const dataTable = ref([]);
const table = ref(null);
const skeleton = ref(false);
const loadingButton = ref({ save: false, saveAdd: false });
const modalForm = useModalForm();
const dialog = useDialog();
const toast = useToast();
const inventory = useInventory();
const resource = useResource();
const { objectData } = storeToRefs(dialog);
const { isShow, method, object } = storeToRefs(modalForm);
const showSettingTable = ref(false);
const columnsTable = ref();
// const isImportExcel = ref(false);
const isExportExcel = ref(false);
const loadingExport = ref(false);
const filter = ref({});
/**
 * Author: Tiến Trung (24/06/2023)
 * Description: hàm call api lấy dữ liệu gán cho dữ liệu table
 */
const getData = async (filterObject) => {
    try {
        skeleton.value = true;
        employeeApi.method = Enum.ApiMethod.POST;
        const filterData = {
            pageSize: paging.value.pageSize,
            currentPage: paging.value.currentPage,
            search: searchData.value,
            ...filterObject,
        };
        employeeApi.data = filterData;
        filter.value = filterObject;
        const response = await employeeApi.request('filter');
        console.log(response);

        paging.value.totalPage = Math.ceil(response.data.TotalRecords / paging.value.pageSize);
        paging.value.currentPage = route.query.page ? Number(route.query.page) : 1;
        paging.value.TotalRecords = response.data.TotalRecords;
        //Nếu không có data thì lấy trang cuối - 1
        if (response.data.Data.length === 0 && paging.value.TotalRecords > 0) {
            if (paging.value.currentPage > 1) {
                router.push({
                    path: Enum.Router.Employee.path,
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
const filterData = (filterObject, isDeleteAll) => {
    dataTable.value = [];
    //Nếu ở trang khác 1 thì getdata về trang 1
    if (paging.value.currentPage === 1) {
        getData(filterObject);
    } else {
        //Nếu xóa hết filter thì không filter nữa
        if (isDeleteAll) {
            filter.value = {};
        } else {
            filter.value = { ...filter.value, ...filterObject };
        }
        router.push({
            path: Enum.Router.Employee.path,
            query: { page: 1 },
        });
    }
};
// /**
//  * Author: Tiến Trung (12/07/2023)
//  * Description: Hàm nhận emit Search filter
//  */
const handleSearchData = () => {
    router.push({
        path: Enum.Router.Employee.path,
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
        getData(filter.value);
    } else {
        router.push({
            path: Enum.Router.Inventory.path,
            query: { page: 1 },
        });
    }
};

// /**
//  * Author: Tiến Trung (11/07/2023)
//  * Description: Hàm set các loại cột cho table
//  */
const setColumnTable = () => {
    columnsTable.value = [
        {
            title: MISAResource[resource.langCode]?.Manage?.EmployeeInfo?.EmployeeCode,
            dataIndex: 'id',
            key: 'EmployeeCode',
            width: '170',
            isShow: true,
            filter: true,
            // pin: true,
        },
        {
            title: MISAResource[resource.langCode]?.Manage?.EmployeeInfo?.FullName,
            dataIndex: 'name',
            key: 'FullName',
            width: '200',
            isShow: true,
            filter: true,
            // pin: true,
        },
        {
            title: MISAResource[resource.langCode]?.Manage?.EmployeeInfo?.Gender,
            dataIndex: 'sex',
            type: Enum.TypeDataTable.Gender,
            key: 'Gender',
            width: '170',
            isShow: true,
            filter: true,
            // type: 'gender',
        },
        {
            title: MISAResource[resource.langCode]?.Manage?.EmployeeInfo?.DateOfBirth,
            dataIndex: 'DateOfBirth',
            key: 'DateOfBirth',
            type: Enum.TypeDataTable.Date,
            width: '170',
            align: Enum.AlignColumn.Center,
            isShow: true,
            filter: true,
        },
        {
            title: MISAResource[resource.langCode]?.Manage?.EmployeeInfo?.PhoneNumber,
            dataIndex: 'phoneNumber',
            key: 'PhoneNumber',
            tooltip: MISAResource[resource.langCode]?.Tooltip?.PhoneNumber,
            width: '170',
            // align: Enum.AlignColumn.Right,
            isShow: true,
        },
        {
            title: MISAResource[resource.langCode]?.Manage?.EmployeeInfo?.LandlinePhone,
            dataIndex: 'landlinePhone',
            key: 'LandlinePhone',
            width: '170',
            // align: Enum.AlignColumn.Right,
        },
        {
            title: MISAResource[resource.langCode]?.Manage?.EmployeeInfo?.IdentityNumber,
            dataIndex: 'identitynumber',
            key: 'IdentityNumber',
            width: '170',
            // align: Enum.AlignColumn.Right,
            filter: true,
            isShow: true,
        },
        {
            title: MISAResource[resource.langCode]?.Manage?.EmployeeInfo?.IdentityDate,
            dataIndex: 'identitydate',
            type: Enum.TypeDataTable.Date,
            key: 'IdentityDate',
            width: '170',
            align: Enum.AlignColumn.Center,
        },
        {
            title: MISAResource[resource.langCode]?.Manage?.EmployeeInfo?.IdentityPlace,
            dataIndex: 'identityPlace',
            key: 'IdentityPlace',
            width: '170',
            isShow: true,
        },
        {
            title: MISAResource[resource.langCode]?.Manage?.EmployeeInfo?.Department?.Name,
            dataIndex: 'DepartmentName',
            key: 'DepartmentName',
            width: '250',
            isShow: true,
            filter: true,
        },
        {
            title: MISAResource[resource.langCode]?.Manage?.EmployeeInfo?.Position?.Name,
            dataIndex: 'PositionName',
            key: 'PositionName',
            width: '250',
            isShow: true,
            filter: true,
        },
        {
            title: MISAResource[resource.langCode]?.Manage?.EmployeeInfo?.BankAccount,
            dataIndex: 'bankaccount',
            key: 'BankAccount',
            width: '230',
            align: Enum.AlignColumn.Right,
            filter: true,
        },
        {
            title: MISAResource[resource.langCode]?.Manage?.EmployeeInfo?.BankBranch,
            dataIndex: 'bankbranch',
            key: 'BankBranch',
            width: '230',
            filter: true,
        },
        {
            title: MISAResource[resource.langCode]?.Manage?.EmployeeInfo?.BankName,
            dataIndex: 'bankname',
            key: 'BankName',
            width: '230',
            filter: true,
        },
    ];
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
//  * Description: computec lưu mảng employee đã chọn
//  */
const employeeSelected = computed(() => {
    return dataTable.value.filter((data) => data.isChecked === true);
});

// /**
//  * Author: Tiến Trung (28/06/2023)
//  * Description: hàm click vào button thêm
//  */
const showModalAddEmployee = () => {
    // modalForm.setAction(MISAResource[resource.langCode]?.FormTitle?.Employee?.Add);
    // modalForm.setObjectForm({});
    // modalForm.setMethod(Enum.EditMode.Add);
    // // dialog.setMethod(Enum.EditMode.Add);
    // modalForm.open();
    inventory.openForm('uid');
};
/**
 * Author: Tiến Trung (09/07/2023)
 * Description: Hàm nhận emit khi click sửa thông tin
 */
const handleShowEditInfo = (data) => {
    modalForm.open();
    modalForm.setAction(MISAResource[resource.langCode]?.FormTitle?.Employee?.Update);
    modalForm.setMethod(Enum.EditMode.Update);
    dialog.setMethod(Enum.EditMode.Update);
    modalForm.setObjectForm(data);
};
/**
 * Author: Tiến Trung (09/07/2023)
 * Description: Hàm nhận emit khi click nhân bản
 */
const handleReplication = () => {
    modalForm.open();
    modalForm.setAction(MISAResource[resource.langCode]?.FormTitle?.Employee?.Add);
    modalForm.setMethod(Enum.EditMode.Copy);
    // dialog.setMethod(Enum.EditMode.Copy);
};
// /**
//  * Author: Tiến Trung (01/07/2023)
//  * Description: hàm nhận emit chọn hàng trên table
//  */
const selectRowTable = (isChecked, data) => {
    try {
        dataTable.value = dataTable.value.map((employee) => {
            if (employee.EmployeeId === data.EmployeeId) {
                employee.isChecked = isChecked;
            }
            return employee;
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
        dataTable.value = dataTable.value.map((employee) => {
            employee.isChecked = isChecked;
            return employee;
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
    dataTable.value = dataTable.value.map((employee) => {
        employee.isChecked = false;
        return employee;
    });
};

/**
 * Author: Tiến Trung (29/06/2023)
 * Description: Hàm hiển thị dialog xóa các nhân viên được chọn
 */
const openDialogDeleteSelected = () => {
    dialog.open({
        title: MISAResource[resource.langCode]?.Dialog?.DeleteEmployee?.Title,
        content: MISAResource[resource.langCode]?.Dialog?.DeleteEmployee?.DeleteListContent,
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
const deleteData = async () => {
    await employeeApi.request(`${objectData.value.EmployeeId}`);
    toast.success(MISAResource[resource.langCode]?.Toast?.Success?.Delete);
    //Nếu đang xóa trang cuối còn 1 bản ghi thì quay vể trang cuối -1
    if (dataTable.value.length === 1) {
        dataTable.value = [];
        router.push({
            path: Enum.Router.Employee.path,
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
    const listIds = employeeSelected.value.map((data) => data.EmployeeId).join(',');
    employeeApi.method = Enum.ApiMethod.DELETE;
    employeeApi.data = listIds;
    await employeeApi.request('DeleteMany');
    handleUnCheckedAll();
    toast.success(MISAResource[resource.langCode]?.Toast?.Success?.Delete);
    //Nếu đang xóa trang cuối hết bản ghi quay về trang cuối -1
    if (listIds.split(',').length === dataTable.value.length && paging.value.currentPage === paging.value.totalPage) {
        dataTable.value = [];
        router.push({
            path: Enum.Router.Employee.path,
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
const deleteEmployee = async () => {
    try {
        //khi không xóa từng đối tượng thì không
        //có đối tượng nào được chọn và object rỗng
        //sẽ chạy vào xóa tất cả
        employeeApi.method = Enum.ApiMethod.DELETE;
        if (Object.keys(objectData.value).length === 0) {
            await deleteDataSelected();
        } else {
            await deleteData();
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
            title: MISAResource[resource.langCode]?.Dialog?.DeleteEmployee?.Title,
            content: MISAResource[resource.langCode]?.Dialog?.DeleteEmployee?.DeleteContent.replace(
                'EmployeeCode',
                dialog.objectData.EmployeeCode,
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
const addNewData = async (data) => {
    employeeApi.data = data;
    employeeApi.method = Enum.ApiMethod.POST;
    const res = await employeeApi.request();
    // const res = await employeeApi.create(data);
    console.log(res);
    toast.success(MISAResource[resource.langCode]?.Toast?.Success?.Add);
    await getData(filter.value);
};
/**
 * Author: Tiến Trung (29/06/2023)
 * Description: Hàm Update
 */
const updateData = async (data) => {
    employeeApi.data = data;
    employeeApi.method = Enum.ApiMethod.PUT;
    const res = await employeeApi.request(`${object.value.EmployeeId}`);
    console.log(res);
    toast.success(MISAResource[resource.langCode]?.Toast?.Success?.Update);
    await getData(filter.value);
};

/**
 * Author: Tiến Trung (29/06/2023)
 * Description: Hàm Gửi form
 */
const submitFormData = async (data, isAddNewForm) => {
    try {
        //Copy đối tượng để thay đổi định dạng date gửi đến BE
        const copyObj = deepCopy(data);
        setLoadingButton(true, isAddNewForm);
        copyObj.dateOfBirth = data.dateOfBirth ? convertDateForBE(data.dateOfBirth) : null;
        copyObj.identityDate = data.identityDate ? convertDateForBE(data.identityDate) : null;
        if (method.value === Enum.EditMode.Update) {
            await updateData(copyObj).then(() => {
                setLoadingButton(false, isAddNewForm);
            });
        }
        if (method.value === Enum.EditMode.Add || method.value === Enum.EditMode.Copy) {
            await addNewData(copyObj).then(() => {
                setLoadingButton(false, isAddNewForm);
            });
        }
        if (isAddNewForm) {
            showModalAddEmployee();
        } else {
            modalForm.close();
        }
    } catch (error) {
        setLoadingButton(false, isAddNewForm);
        console.log(error);
    }
};

/**
 * Author: Tiến Trung (10/07/2023)
 * Description:hàm reset data và bỏ filter
 */
async function reset() {
    router.push({
        path: Enum.Router.Employee.path,
    });
    dialog.setObjectData({});
    dataTable.value = [];
    await getData();
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
        employeeApi.method = Enum.ApiMethod.GET;
        const excelResquest = {
            columns: columnTableExcel,
            ids: listIds === null ? null : listIds,
            search: search ? search : '',
            title: MISAResource[resource.langCode]?.Excel?.Employee.Title,
            sheet: MISAResource[resource.langCode]?.Excel?.Employee.Sheet,
        };
        const res = await employeeApi.downloadExcel(excelResquest);
        // Tạo URL object từ dữ liệu trả về
        const url = window.URL.createObjectURL(
            new Blob([res.data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' }),
        );

        // Tạo một link ẩn và sử dụng nó để tải xuống file
        const link = document.createElement('a');
        link.href = url;
        link.download = MISAResource[resource.langCode].Excel.Employee.FileName;
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
        const listIds = employeeSelected.value.map((data) => data.EmployeeId).join(',');
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
    table.value.closeMenuFilter();
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
        showModalAddEmployee();
    }
};
/**
 * Author: Tiến Trung (30/07/2023)
 * Description: Hàm sự kiện phím ctrl + D để xóa danh sách được chọn
 */
const onKeyDownDelete = (e) => {
    if (employeeSelected.value.length > 1) {
        if (e.ctrlKey && e.keyCode === Enum.Keyboard.D) {
            e.preventDefault(); // Ngăn chặn hành động mặc định của trình duyệt
            openDialogDeleteSelected();
        }
    }
};
/**
 * Author: Tiến Trung (30/07/2023)
 * Description: khi được update thì thêm sự kiện delete bằng phím
 */
onUpdated(() => {
    if (employeeSelected.value.length > 0) {
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
                        @click="showModalAddEmployee"
                        :type="Enum.ButtonType.IconPri"
                        :action="MISAResource[resource.langCode]?.Button?.Add"
                    >
                        <template #icon>
                            <MISAIcon width="21" height="10" icon="plus" />
                        </template>
                    </MISAButton>
                    <MISAButton
                        :type="Enum.ButtonType.IconPri"
                        :action="MISAResource[resource.langCode]?.Button?.Replication"
                    >
                        <template #icon>
                            <MISAIcon width="25" height="15" icon="replication" />
                        </template>
                    </MISAButton>
                    <MISAButton :type="Enum.ButtonType.IconPri" :action="MISAResource[resource.langCode]?.Button?.Edit">
                        <template #icon>
                            <MISAIcon width="22" height="12" icon="edit" />
                        </template>
                    </MISAButton>
                    <MISAButton
                        :type="Enum.ButtonType.IconPri"
                        :action="MISAResource[resource.langCode]?.Button?.Delete"
                    >
                        <template #icon>
                            <MISAIcon width="20" height="12" icon="delete" />
                        </template>
                    </MISAButton>
                    <MISAButton
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

                    <MISAButton
                        iconSec
                        :type="Enum.ButtonType.IconPri"
                        :action="MISAResource[resource.langCode]?.Button?.ExportExcel"
                    >
                        <template #icon>
                            <MISAIcon width="26" height="10" icon="export2 " />
                        </template>
                    </MISAButton>
                </ul>
            </div>
            <div class="tt-continer__table-body">
                <MISATable
                    ref="table"
                    @select-row="selectRowTable"
                    @select-all="selectAllTable"
                    @edit-data="handleShowEditInfo"
                    @close-export="closeMenuExport"
                    :allDataSelected="employeeSelected"
                    :dataTable="dataTable"
                    :columns="columnsTable"
                    :skeleton="skeleton"
                    :searchData="searchData"
                    propertiesIdName="EmployeeId"
                    routerCurrent="Inventory"
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
    </div>
    <Teleport to="#app">
        <!-- @loading-button="(isload) => (loadingButton = isload)" -->
        <InventoryForm :buttonLoad="loadingButton" @submit-form="submitFormData" v-if="isShow"></InventoryForm>
        <MISADialog v-if="dialog.isShow" @submit-form="submitFormData" @delete-data="deleteEmployee"></MISADialog>
    </Teleport>
</template>

<style lang="scss" scoped>
@import './inventory.scss';

//table
</style>
