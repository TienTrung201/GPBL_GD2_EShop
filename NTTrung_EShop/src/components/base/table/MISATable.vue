<script setup>
import MISAMenucontext from './MISAMenuContext.vue';
import MISAFilter from './MISAFilter.vue';
import { computed, onMounted, ref, watch, onUnmounted } from 'vue';
import { useDialog } from '../../../stores/dialog';
import { useResource } from '../../../stores/resource.js';
import { RouterLink } from 'vue-router';
import MISAResource from '../../../common/resource';
import Enum from '../../../common/enum';
import { useModalForm } from '../../../stores/modalform';
import { formatAlign } from '../../../common/convert-data';
const emit = defineEmits([
    'select-row',
    'select-all',
    'edit-data',
    'set-pagesize',
    'uncheck-all',
    'close-export',
    'filter-data',
]);
const props = defineProps({
    //Danh sách dữ liệu hiện thị trên bảng
    dataTable: {
        type: Array,
        require: true,
        default: () => [],
    },
    //Danh sách các tiêu đề cột
    columns: {
        type: Array,
        require: true,
    },
    //columnIdName
    propertiesIdName: {
        type: String,
        require: true,
        default: '',
    },
    routerCurrent: {
        type: String,
        require: true,
    },
    //tất cả dữ liệu khi được checked
    allDataSelected: {
        type: Array,
        require: false,
        default: () => [],
    },
    //skeleton cho table
    skeleton: {
        type: Boolean,
        default: false,
    },
    //Tổng số trang
    totalPage: {
        type: Number,
        default: 1,
    },
    //trang hiện tại
    currentPage: {
        type: Number,
        default: 1,
    },
    //data search đẻ router query
    searchData: {
        type: String,
        default: '',
    },
    //Tổng số bản ghi
    TotalRecords: {
        type: Number,
        default: 0,
    },
});
const table = ref(null);
const isShowMenu = ref(false);
const dialog = useDialog();
const modalForm = useModalForm();
const resource = useResource();
const positionMenuContext = ref({ left: 0, bottom: 0 });
const pageSize = ref('10');
const pageSizeOption = ref();
const columnsTable = ref(props.columns);
const filterValue = ref(null);
const filterBy = ref(null);
const filter = ref({
    propertySort: null,
    sortBy: null,
    typeColumn: Enum.TypeDataTable.Default,
    filter: [],
});
/**
 * Author: Tiến Trung (06/07/2023)
 * Description: Hàm lọc
 */
const emitFilterData = (filterObject) => {
    //Nếu đang có giá trị để filter thì thêm vào
    if (filterObject.isFilter) {
        var currentFilter = filter.value.filter.findIndex((filter) => filter.property === filterObject.property);
        if (currentFilter != -1) {
            filter.value.filter[currentFilter] = filterObject;
        } else {
            filter.value.filter.push(filterObject);
        }
        //Nếu không có thì xóa filter cũ đi
    } else {
        filter.value.filter = filter.value.filter.filter((filter) => filter.property != filterObject.property);
    }
    emit('filter-data', filter.value);
};
/**
 * Author: Tiến Trung (06/07/2023)
 * Description: Hàm sắp xếp
 */
const setSort = (keyName, isFilter, typeColumn) => {
    if (isFilter) {
        if (filter.value.propertySort === keyName) {
            switch (filter.value.sortBy) {
                case Enum.Sort.Desc:
                    filter.value.sortBy = Enum.Sort.Asc;
                    break;
                case Enum.Sort.Asc:
                    filter.value.sortBy = null;
                    filter.value.propertySort = '';
                    filter.value.typeColumn = Enum.TypeDataTable.Default;
                    break;
                default:
                    filter.value.sortBy = Enum.Sort.Desc;
                    break;
            }
        } else {
            filter.value.sortBy = Enum.Sort.Desc;
            filter.value.propertySort = keyName;
            filter.value.typeColumn = typeColumn;
        }
        emit('filter-data', filter.value);
    }
};
/**
 * Author: Tiến Trung (11/07/2023)
 * Description: Hàm gán pagesize cho dropdown hiển thị
 */
const setPageSize = () => {
    pageSizeOption.value = [
        {
            option: '10 ',
            value: '10',
        },
        {
            option: '20 ',
            value: '20',
        },
        {
            option: '30 ',
            value: '30',
        },
        {
            option: '50 ',
            value: '50',
        },
        {
            option: '100 ',
            value: '100',
        },
    ];
};
/**
 * Author: Tiến Trung (11/07/2023)
 * Description: khi component create gọi setpagesize
 */
setPageSize();
/**
 * Author: Tiến Trung (11/07/2023)
 * Description: Ngôn ngữ thay đổi thì cài đặt lại
 */
watch(
    () => resource.langCode,
    () => {
        setPageSize();
    },
);
/**
 * Author: Tiến Trung (11/07/2023)
 * Description: computed hiển thị các trang để phân trang
 */
const pages = computed(() => {
    const visibleRange = 1; // Số trang hiển thị trước và sau trang hiện tại
    const startPage = Math.max(1, props.currentPage - visibleRange);
    const endPage = Math.min(props.totalPage, props.currentPage + visibleRange);
    const pages = [];
    if (startPage > 1) {
        pages.push(1); // Thêm trang đầu tiên
        if (startPage > 2) {
            pages.push('...'); // Thêm dấu '...' nếu có khoảng cách giữa trang đầu tiên và trang hiển thị trước
        }
    }

    for (let i = startPage; i <= endPage; i++) {
        pages.push(i); // Thêm các trang hiển thị
    }

    if (endPage < props.totalPage) {
        if (endPage < props.totalPage - 1) {
            pages.push('...'); // Thêm dấu '...' nếu có khoảng cách giữa trang hiển thị sau và trang cuối cùng
        } // Thêm trang cuối cùng
        pages.push(props.totalPage);
    }
    return pages;
});
/**
 * Author: Tiến Trung (11/07/2023)
 * Description: computed hiển thị số thông tin bản ghi trên trang
 */
const pagination = computed(() => {
    const page =
        props.currentPage * Number(pageSize.value) +
        1 -
        Number(pageSize.value) +
        '-' +
        (props.currentPage * Number(pageSize.value) - Number(pageSize.value) + props.dataTable.length);

    return `${MISAResource[resource.langCode].Table?.Show} ${page} ${MISAResource[resource.langCode].Table.OutOf} ${
        props.TotalRecords
    } ${MISAResource[resource.langCode].Table.Record}`;
});
/**
 * Author: Tiến Trung (12/07/2023)
 * Description: watch theo dõi khi langcode thay đổi thì
 * columnsTable thay đổi, theo dõi columnsTable để columnsTable
 * thay đổi
 */
watch(
    () => props.columns,
    () => {
        columnsTable.value = props.columns;
        setStickyTable();
    },
    { deep: true },
);
setStickyTable();
/**
 * Author: Tiến Trung (12/07/2023)
 * Description: Set left cho từng cột sticky
 */
function setStickyTable() {
    const defaultWidth = 170;
    const checkBoxWidth = 40;
    try {
        columnsTable.value.forEach((column, i) => {
            if (!column.width) {
                column.width = defaultWidth;
            }
            const stickyLeft = columnsTable.value.reduce((left, column, currentIndex) => {
                //Nếu có ghim thì set stickyleft cho cột
                if (column.pin && currentIndex < i && column.isShow) {
                    return left + (Number(column.width) || defaultWidth);
                } else {
                    return left;
                }
            }, 0);
            //Nếu cột bị ẩn thì bỏ qua
            if (column.pin && column.isShow) {
                column.stickyLeft = stickyLeft + checkBoxWidth;
            }
        });
    } catch (error) {
        console.log(error);
    }
}
/**
 * Author: Tiến Trung (28/06/2023)
 * Description: Hàm click sửa thông tin
 */
const handleShowEditInfo = (data) => {
    emit('edit-data', data);
};

/**
 * Author: Tiến Trung (01/07/2023)
 * Description: Hàm báo cho cha biết khi chọn vào 1 hàng
 */

const handleSelectRow = (data) => {
    dialog.setObjectData({});
    if (data.isChecked) {
        emit('select-row', false, data);
    } else {
        data.isChecked === true;
        emit('select-row', true, data);
    }
};
/**
 * Author: Tiến Trung (25/06/2023)
 * Description: Hàm báo cho cha biết khi chọn tất cả
 */
const handleSelectAll = (isChecked) => {
    emit('select-all', isChecked);
};
/**
 * Author: Tiến Trung (20/06/2023)
 * Description: Hàm tăng giảm kích thước cột trong table
 */

function startResize(e, columnKey) {
    const parrentElementResize = e.target.parentElement;
    const element = parrentElementResize.getBoundingClientRect();
    try {
        window.addEventListener('mousemove', mouseMove);
    } catch (error) {
        console.log(error);
    }
    //Khi di chuyển chuột
    function mouseMove(e) {
        // Thêm class để sửa cursor chuột
        table.value.classList.add('resize-mouse');
        //Lấy vị trí bắt đầu element
        const leftPositionElement = element.left;
        //Chiều ngang mới = vị trí chuột hiện tại trừ đi vị trí  element
        const newWidth = e.clientX - leftPositionElement;
        const columnResize = columnsTable.value.find((col) => col.key === columnKey);
        if (columnResize) {
            if (newWidth >= 170) {
                columnResize.width = newWidth;
            }
        }
        window.addEventListener('mouseup', clearMouseMove);
    }
    // Khi bỏ giữ chuột
    function clearMouseMove() {
        // Bỏ class để sửa cursor chuột
        table.value.classList.remove('resize-mouse');
        window.removeEventListener('mousemove', mouseMove);
    }
}
/**
 * Author: Tiến Trung (2/07/2023)
 * Description: Hàm closse menucontext
 */
const closeMenu = () => {
    isShowMenu.value = false;
    // dialog.setObjectData = {};
};
const curentMenucontext = ref(null); // Dòng hiện tại được bật menu context
const menuContext = ref(null);
/**
 * Author: Tiến Trung (2/07/2023)
 * Description: Hàm hiện menucontext và set vị
 * trí cho menucontext theo icon dropdown
 */
const openMenuContext = (e, index, data) => {
    try {
        //Khi mở menu thì dialog và form nhận được đối tượng chọn
        //để thực hiện hành vi
        e.stopPropagation();
        dialog.setObjectData(data);
        modalForm.setObjectForm(data);
        emit('uncheck-all');
        emit('close-export');
        //Khối này để kiểm tra đóng mở menu khi click mở
        //nhiều đối tượng trong bảng biết menu nào
        if (curentMenucontext.value === index) {
            isShowMenu.value = !isShowMenu.value;
        } else {
            isShowMenu.value = true;
            curentMenucontext.value = index;
        }
        //Cài đặt vị trí cho menu ở icon được click
        const positionIcon = e.target.getBoundingClientRect();
        const left = positionIcon.right - 150;
        let bottom = 0;
        if (positionIcon.bottom > 500) {
            //Nếu vị trí ở cuối thì set cho ngược lên
            bottom = positionIcon.bottom - (menuContext.value.getHeightMenu() + 20);
        } else {
            bottom = positionIcon.bottom + 10;
        }
        positionMenuContext.value = { left: left, bottom: bottom };
    } catch (error) {
        console.log(error);
    }
};

/**
 * Author: Tiến Trung (2/07/2023)
 * Description: mount và unmount thêm và hủy sự kiện
 */
onMounted(() => {
    window.addEventListener('click', closeMenu);
});
onUnmounted(() => {
    window.removeEventListener('click', closeMenu);
});

watch(
    () => pageSize.value,
    () => {
        emit('set-pagesize', pageSize.value);
    },
    { deep: true },
);
defineExpose({ closeMenu });
</script>
<template lang="">
    <div class="wrapper-table">
        <table ref="table" class="table">
            <thead class="table-header">
                <tr>
                    <th class="checkbox" scope="col">
                        <p v-if="skeleton" class="center">
                            <span class="skeleton skeleton-checkbox"></span>
                        </p>
                        <MISACheckbox
                            v-else
                            @change-checkbox="handleSelectAll"
                            :valueCheckbox="allDataSelected.length === dataTable.length && dataTable.length !== 0"
                        />
                    </th>
                    <template :key="item.dataIndex" v-for="item in columnsTable">
                        <th
                            v-if="item.isShow"
                            :style="{
                                left: item.pin ? item.stickyLeft + 'px' : '',
                            }"
                            v-tooltip-tippy="{
                                content: item.tooltip || item.title,
                                placement: 'top',
                                hideOnClick: true,
                                placement: item.width > 400 ? 'left' : 'top',
                            }"
                            scope="col"
                            :class="[
                                { 'td-control': item.dataIndex === 'action' },
                                { sticky: item.pin },
                                { 'filter-table': item.filter },
                            ]"
                        >
                            <div class="th__wrapper">
                                <p
                                    :style="{
                                        textAlign: 'center',
                                        width: item.width + 'px',
                                        maxWidth: item.width + 'px',
                                    }"
                                    @mousedown="setSort(item.key, item.filter, item.type)"
                                    class="th__content"
                                >
                                    {{ item.title }}
                                    <span v-if="item.filter && filter.propertySort === item.key" class="icon-sort">
                                        <MISAIcon
                                            :class="{ asc: filter.sortBy === Enum.Sort.Asc }"
                                            width="11"
                                            height="12"
                                            icon="desc"
                                        >
                                        </MISAIcon>
                                    </span>
                                </p>
                                <div class="th__filter">
                                    <div class="th__filter-type">
                                        <MISAFilter
                                            :filterBy="filterBy"
                                            :filterValue="filterValue"
                                            :filterType="item.type"
                                            :keyName="item.key"
                                            @filter-data="emitFilterData"
                                        ></MISAFilter>
                                    </div>
                                </div>
                            </div>

                            <p
                                @mousedown.stop
                                @mousedown="
                                    (e) => {
                                        startResize(e, item.key);
                                    }
                                "
                                class="table-col--resize"
                            ></p>
                        </th>
                    </template>
                    <!-- 
                    <th class="text-align--center td-control">
                        {{ MISAResource[resource.langCode]?.Table?.Action?.Name }}
                    </th> -->
                </tr>
            </thead>

            <tbody class="table-body">
                <!-- dataTable -->
                <tr
                    :class="{
                        'table-row--action': rowData[props.propertiesIdName] === dialog.objectData[propertiesIdName],
                    }"
                    v-for="rowData in dataTable"
                    :key="rowData[propertiesIdName]"
                    @click="dialog.setObjectData(rowData)"
                >
                    <td :class="[{ 'table-row--checked': rowData.isChecked }, 'checkbox']" scope="row">
                        <MISACheckbox @change-checkbox="handleSelectRow(rowData)" :valueCheckbox="rowData.isChecked" />
                    </td>

                    <template :key="item.dataIndex" v-for="item in columnsTable">
                        <td
                            v-if="item.isShow"
                            :class="[
                                { 'table-row--checked': rowData.isChecked },
                                { 'td-control': item.dataIndex === 'action' },
                                { sticky: item.pin },
                            ]"
                            @dblclick="handleShowEditInfo(rowData)"
                            :style="{
                                textAlign: formatAlign(item.align),
                                width: item.width + 'px',
                                maxWidth: item.width + 'px',
                                left: item.pin ? item.stickyLeft + 'px' : '',
                            }"
                            v-tooltip-tippy="{
                                content: rowData[item.key],
                                hideOnClick: false,
                                theme: 'custom',
                                placement: item.width > 400 ? 'left' : 'top',
                            }"
                        >
                            <slot :name="item.key" v-bind="rowData">
                                {{ rowData[item.key] }}
                            </slot>
                        </td>
                    </template>

                    <!-- <td
                        :class="{ 'table-row--checked': rowData.isChecked }"
                        class="text-align--center td-control"
                        scope="row"
                    >
                        <div class="wrapper-control center">
                            <button @click="handleShowEditInfo(rowData)" class="controll-title">
                                {{ MISAResource[resource.langCode]?.Table?.Action?.Edit }}
                            </button>
                            <button @click="openMenuContext($event, index, rowData)" class="controll-icon center">
                                <MISAIcon width="8" height="8" icon="down-triagle"></MISAIcon>
                            </button>
                        </div>
                    </td> -->
                </tr>

                <template v-if="skeleton">
                    <tr v-for="item in columnsTable" :key="item.key">
                        <td scope="row" class="checkbox">
                            <p class="center">
                                <span class="skeleton skeleton-checkbox"></span>
                            </p>
                        </td>
                        <td
                            @click="console.log(index)"
                            :class="item.type === 'action' ? 'td-control' : ''"
                            v-for="(item, index) in columns"
                            :key="index"
                        >
                            <p class="skeleton skeleton-text"></p>
                        </td>
                        <td class="td-control">
                            <p class="skeleton skeleton-text"></p>
                        </td>
                    </tr>
                </template>
            </tbody>
        </table>
        <!-- v-if="isShowMenu" -->
        <Teleport to="#app">
            <MISAMenucontext :visibility="isShowMenu" ref="menuContext" :positionMenuContext="positionMenuContext">
                <template #action>
                    <slot name="listMenuContext"></slot>
                </template>
            </MISAMenucontext>
        </Teleport>
    </div>

    <div class="tt-continer__footer">
        <div class="table-footer">
            <div v-if="dataTable.length" class="table-paging">
                <div class="table-page">
                    <ul class="pagination-list">
                        <li class="pagination-item">
                            <RouterLink
                                :class="{ 'no-select': currentPage === 1 }"
                                :to="{
                                    path: Enum.Router[routerCurrent].Path,
                                    query: {
                                        page: currentPage === 1 ? currentPage : currentPage - 1,
                                    },
                                }"
                                class="pagination-link"
                                ><MISAIcon width="5" height="8" icon="prev"></MISAIcon
                            ></RouterLink>
                        </li>
                        <li v-if="currentPage !== 1 || true" class="pagination-item">
                            <RouterLink
                                :to="{
                                    path: Enum.Router[routerCurrent].Path,
                                    query: {
                                        page: currentPage === 1 || currentPage === 2 ? currentPage : currentPage - 2,
                                    },
                                }"
                                :class="{ 'no-select': currentPage === 1 || currentPage === 2 }"
                                class="pagination-link"
                                ><MISAIcon width="8" height="8" icon="prev2"></MISAIcon
                            ></RouterLink>
                        </li>

                        <template v-for="(page, index) in pages" :key="index">
                            <li :class="{ active: page === currentPage }" class="pagination-item">
                                <RouterLink
                                    :to="{
                                        path: Enum.Router[routerCurrent].Path,
                                        query: { page: page, search: searchData || undefined },
                                    }"
                                    v-if="page !== '...'"
                                    class="pagination-link"
                                    >{{ page }}</RouterLink
                                >
                                <span class="pagination-ellipsis" v-else>...</span>
                            </li>
                        </template>
                        <li v-if="currentPage !== totalPage || true" class="pagination-item">
                            <RouterLink
                                :to="{
                                    path: Enum.Router[routerCurrent].Path,
                                    query: {
                                        page: currentPage >= totalPage - 1 ? currentPage : currentPage + 2,
                                    },
                                }"
                                :class="{ 'no-select': currentPage >= totalPage - 1 }"
                                class="pagination-link"
                                ><MISAIcon width="8" height="8" icon="next2"></MISAIcon
                            ></RouterLink>
                        </li>
                        <li class="pagination-item">
                            <RouterLink
                                :to="{
                                    path: Enum.Router[routerCurrent].Path,
                                    query: {
                                        page: currentPage === totalPage ? currentPage : currentPage + 1,
                                    },
                                }"
                                :class="{ 'no-select': currentPage === totalPage }"
                                class="pagination-link"
                                ><MISAIcon width="5" height="8" icon="next"></MISAIcon
                            ></RouterLink>
                        </li>
                    </ul>
                </div>
                <p class="table-page-content">
                    <!-- {{ MISAResource[resource.langCode].Table.Page }} <span>10</span> -->
                    <MISADropdown v-model:value="pageSize" selectEmpty :options="pageSizeOption"></MISADropdown>
                </p>
            </div>
            <p v-if="dataTable.length > 0" class="table-total">
                {{ pagination }}
            </p>
        </div>
    </div>
    <div v-if="dataTable.length === 0 && !skeleton" class="wrapper-no-data">
        <div class="no-data">
            <img src="@/assets//img//nodata.svg" alt="" />
            <p>{{ MISAResource[resource.langCode]?.Table?.NoData }}</p>
        </div>
    </div>
</template>

<style lang="scss">
@import './misa-table.scss';
</style>
