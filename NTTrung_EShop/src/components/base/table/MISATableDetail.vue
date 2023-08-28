<script setup>
import { useDialog } from '../../../stores/dialog';
import { useResource } from '../../../stores/resource.js';
// import MISAResource from '../../../common/resource';
import Enum from '../../../common/enum';
import { formatAlign } from '../../../common/convert-data';
import { ref, watch, onMounted } from 'vue';
const emit = defineEmits([
    'select-row',
    'select-all',
    'edit-data',
    'set-pagesize',
    'uncheck-all',
    'close-export',
    'filter-data',
    'delete-detail',
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
const editIndex = ref('');
const table = ref(null);
const dialog = useDialog();
const resource = useResource();
const columnsTable = ref(props.columns);
const filter = ref({
    sortColumn: '',
    sortOrder: null,
});
const dataTableRefence = ref([]);
/**
 * Author: Tiến Trung (19/08/2023)
 * Description: Hàm xóa detail
 */
const deleteDetail = (index) => {
    // const dataTableCopy=props.dataTable
    // dataTableCopy.splice(index,1)
    emit('delete-detail', index);
};
/**
 * Author: Tiến Trung (06/07/2023)
 * Description: Hàm sắp xếp
 */
const setSort = (keyName, isFilter) => {
    if (isFilter) {
        if (filter.value.sortColumn === keyName) {
            switch (filter.value.sortOrder) {
                case Enum.Sort.Desc:
                    filter.value.sortOrder = Enum.Sort.Asc;
                    break;
                case Enum.Sort.Asc:
                    filter.value.sortOrder = null;
                    filter.value.sortColumn = '';
                    break;
                default:
                    filter.value.sortOrder = Enum.Sort.Desc;
                    break;
            }
        } else {
            filter.value.sortOrder = Enum.Sort.Desc;
            filter.value.sortColumn = keyName;
        }
        emit('filter-data', filter.value);
    }
};
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
    const checkBoxWidth = 0;
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
 * Author: Tiến Trung (25/08/2023)
 * Description: hàm gán tên cho editindex để mở input sửa
 */
const handleOpenEditColumn = (item, index) => {
    if (item.isEdit) {
        editIndex.value = index;
    }
};
/**
 * Author: Tiến Trung (25/08/2023)
 * Description: Hàm update data
 */
const setDataEditMode = (value, oldValue, data, isCode, isBarcode) => {
    if (data.EditMode !== Enum.EditMode.Add) {
        data.EditMode = Enum.EditMode.Update;
        if (isCode) {
            data.IsUpdateCode = oldValue !== value;
        }
        if (isBarcode) {
            data.IsUpdateBarcode = oldValue !== value;
        }
    }
};
/**
 * Author: Tiến Trung (25/08/2023)
 * Description: khi component dc hiển thị thì gán dữ liệu  dataTableRefence
 */
onMounted(() => {
    dataTableRefence.value = props.dataTable;
});
watch(
    () => props.dataTable,
    () => {
        dataTableRefence.value = props.dataTable;
    },
);
</script>
<template lang="">
    <div class="wrapper-table">
        <table ref="table" class="table">
            <thead class="table-header">
                <tr>
                    <template :key="item.dataIndex" v-for="item in columnsTable">
                        <th
                            v-if="item.isShow"
                            :style="{
                                textAlign: 'center',
                                width: item.width + 'px',
                                maxWidth: item.width + 'px',
                                left: item.pin ? item.stickyLeft + 'px' : '',
                            }"
                            v-tooltip-tippy="{
                                content: item.tooltip || item.title,
                                placement: 'top',
                                hideOnClick: true,
                                placement: item.width > 400 ? 'left' : 'top',
                            }"
                            scope="col"
                            :class="[{ sticky: item.pin }, { 'filter-table': item.filter }]"
                        >
                            <div class="th__wrapper">
                                <p @mousedown="setSort(item.key, item.filter)" class="th__content">
                                    {{ item.title }}
                                    <span
                                        v-if="
                                            item.filter &&
                                            filter.sortColumn === item.key &&
                                            item.align !== Enum.AlignColumn.Right
                                        "
                                        class="icon-sort"
                                    >
                                        <MISAIcon
                                            :class="{ asc: filter.sortOrder === Enum.Sort.Asc }"
                                            width="11"
                                            height="12"
                                            icon="desc"
                                        >
                                        </MISAIcon>
                                    </span>
                                </p>
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

                    <th class="text-align--center td-control"></th>
                </tr>
            </thead>

            <tbody class="table-body">
                <!-- dataTable -->
                <tr v-for="(rowData, index) in dataTableRefence" :key="rowData[propertiesIdName]">
                    <template :key="item.dataIndex" v-for="(item, indexCol) in columnsTable">
                        <td
                            v-if="item.isShow"
                            :class="[{ sticky: item.pin }, { 'no-edit': !item.isEdit }]"
                            :style="{
                                left: item.pin ? item.stickyLeft + 'px' : '',
                            }"
                            v-tooltip-tippy="{
                                content: rowData[item.key],
                                hideOnClick: false,
                                theme: 'custom',
                                placement: item.width > 400 ? 'left' : 'top',
                            }"
                        >
                            <div @click="handleOpenEditColumn(item, index + '' + indexCol)" class="edit-data">
                                <div
                                    class="data-show"
                                    v-if="editIndex !== index + '' + indexCol"
                                    :style="{
                                        textAlign: formatAlign(item.align),
                                        width: item.width + 'px',
                                        maxWidth: item.width + 'px',
                                    }"
                                >
                                    <slot :name="item.key" v-bind="rowData">
                                        {{ rowData[item.key] }}
                                    </slot>
                                </div>
                                <MISAInput
                                    @blur="editIndex = ''"
                                    @input-validation="
                                        (value, oldValue) =>
                                            setDataEditMode(value, oldValue, rowData, item.isCode, item.isBarcode)
                                    "
                                    :money="item.type === Enum.TypeDataTable.Money"
                                    autoFocusMount
                                    v-if="editIndex === index + '' + indexCol"
                                    validate="true"
                                    v-model:value="rowData[item.key]"
                                ></MISAInput>
                            </div>
                        </td>
                    </template>
                    <td class="text-align--center td-control">
                        <div class="center">
                            <span @click="deleteDetail(index)" class="center">
                                <MISAIcon width="11" height="14" icon="delete-table" />
                            </span>
                        </div>
                    </td>
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
    </div>
</template>

<style lang="scss" scoped>
@import './misa-table.scss';
.td-control {
}
td {
    padding: 0 1px !important;
    padding-top: 1px !important;
    padding-right: 2px !important;
}
</style>
