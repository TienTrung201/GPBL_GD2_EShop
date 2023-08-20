<template>
    <div class="table-setting">
        <div class="table-setting--top">
            <div class="seting-header">
                <h3 class="title">{{ MISAResource[resource.langCode]?.Table?.SettingTable }}</h3>
                <button @click="handleCloseSetting" class="table-setting__close center">
                    <MISAIcon width="14" height="16" icon="close"></MISAIcon>
                </button>
            </div>
            <div class="setting-search">
                <MISAGroupInput
                    @filter="handleSearchColumns"
                    v-model:value="searchColumn"
                    :placeholder="MISAResource[resource.langCode]?.Table?.InputSearchColumn"
                    class="width300"
                />
            </div>
        </div>
        <div class="wrapper-setting">
            <ul class="setting-list">
                <li
                    @drop="onDrop(i)"
                    @dragenter.prevent
                    @dragover.prevent
                    @dragstart="startDrag($event, i)"
                    draggable="true"
                    v-for="(column, i) in columnsSetting"
                    :key="i"
                    class="setting-item"
                >
                    <template v-if="column.isShowSetting">
                        <div class="wrapper-item">
                            <div class="setting-item--left">
                                <MISACheckbox
                                    @change-checkbox="(e) => toggleColumn(e, column)"
                                    :valueCheckbox="column.isShow"
                                />
                                <p class="item-content">{{ column.title }}</p>
                            </div>
                            <div class="setting-item--right">
                                <p
                                    v-tooltip.top="MISAResource[resource.langCode]?.Tooltip?.Pin"
                                    @click="setPinColumn(column, i)"
                                    class="center wrapper-icon"
                                >
                                    <MISAIcon
                                        :class="`setting-item__icon ${column.pin ? 'active' : ''}`"
                                        width="20"
                                        height="20"
                                        icon="pin"
                                    ></MISAIcon>
                                </p>
                                <p
                                    v-tooltip.top="MISAResource[resource.langCode]?.Tooltip?.Drag"
                                    class="center wrapper-icon"
                                >
                                    <MISAIcon class="setting-item__icon icon-drag" width="6" height="15" icon="drag">
                                    </MISAIcon>
                                    <MISAIcon class="setting-item__icon icon-drag" width="6" height="15" icon="drag">
                                    </MISAIcon>
                                </p>
                            </div>
                        </div>
                    </template>
                </li>
            </ul>
        </div>
        <div class="wrapper-save">
            <MISAButton
                @click="resetTable"
                :type="Enum.ButtonType.Sec"
                :action="MISAResource[resource.langCode]?.Button?.DefaultTable"
            ></MISAButton>
            <MISAButton
                @click="saveSetting"
                :type="Enum.ButtonType.Pri"
                :action="MISAResource[resource.langCode]?.Button?.SaveSetting"
            ></MISAButton>
        </div>
    </div>
</template>
<script setup>
import { removeVietnameseTones } from '../../../common/convert-data.js';
import { ref, watch } from 'vue';
import { useResource } from '../../../stores/resource.js';
import MISAResource from '../../../common/resource';
import Enum from '../../../common/enum';
const resource = useResource();
const emits = defineEmits(['drag-column', 'close']);
const props = defineProps({
    columns: {
        //mảng các cột để cài đặt
        type: Array,
        default: () => [],
    },
    isShow: {
        //ẩn hiện cìa đặt
        type: Boolean,
        default: false,
    },
    resetData: {
        //lấy lại cài đặt mặc định
        type: Function,
    },
});
const searchColumn = ref('');
const dragIndex = ref();
const columnsSetting = ref(JSON.parse(JSON.stringify(props.columns)));
/**
 * Author: Tiến Trung (12/07/2023)
 * Description: Hàm tìm kiếm cột
 */
const handleSearchColumns = () => {
    columnsSetting.value.map((column) => {
        if (
            removeVietnameseTones(column.title.toLowerCase()).includes(
                removeVietnameseTones(searchColumn.value.toLowerCase()),
            )
        ) {
            column.isShowSetting = true;
        } else {
            column.isShowSetting = false;
        }
    });
};
handleSearchColumns();
/**
 * Author: Tiến Trung (12/07/2023)
 * Description: Hàm kéo item
 */
const startDrag = (e, index) => {
    dragIndex.value = index;
    e.dataTransfer.dropEffect = 'move';
    e.dataTransfer.effectAllowed = 'move';
};
/**
 * Author: Tiến Trung (12/07/2023)
 * Description: Hàm thả item
 */
// const onDrop = (dropIndex) => {
//     if (dragIndex.value !== -1 && dropIndex !== -1) {
//         // Tạo một bản sao của mảng columnsSetting.value để tránh tham chiếu
//         const newColumnsSetting = JSON.parse(JSON.stringify(columnsSetting.value));
//         // Swap vị trí giữa phần tử bị kéo (itemIndex) và phần tử thả (dropItemIndex) trong bản sao mới
//         [newColumnsSetting[dragIndex.value], newColumnsSetting[dropIndex]] = [
//             newColumnsSetting[dropIndex],
//             newColumnsSetting[dragIndex.value],
//         ];
//         // Cập nhật lại mảng columnsSetting.value bằng bản sao mới
//         columnsSetting.value = newColumnsSetting;
//     }
// };
const onDrop = (dropIndex) => {
    if (dragIndex.value !== -1 && dropIndex !== -1) {
        // Tạo một bản sao của mảng columnsSetting.value để tránh tham chiếu
        const newColumnsSetting = JSON.parse(JSON.stringify(columnsSetting.value));
        // Lấy phần tử bị kéo
        const draggedItem = newColumnsSetting[dragIndex.value];

        // Xóa phần tử bị kéo khỏi mảng
        newColumnsSetting.splice(dragIndex.value, 1);
        // Chèn phần tử bị kéo vào vị trí mới
        newColumnsSetting.splice(dropIndex, 0, draggedItem);

        // Cập nhật lại mảng columnsSetting.value bằng bản sao mới
        columnsSetting.value = newColumnsSetting;
    }
};
/**
 * Author: Tiến Trung (12/07/2023)
 * Description: Hàm ghim item
 */
const setPinColumn = (column, i) => {
    columnsSetting.value[i].pin = !column.pin;
};
/**
 * Author: Tiến Trung (12/07/2023)
 * Description: Hàm lưu cài đặt
 */
const saveSetting = () => {
    const columnsPin = columnsSetting.value.filter((column) => column.pin);
    const columnsNoPin = columnsSetting.value.filter((column) => !column.pin);

    // columnsSetting.value = [...columnsPin, ...columnsNoPin];
    emits('drag-column', [...columnsPin, ...columnsNoPin]);
    handleCloseSetting();
};
/**
 * Author: Tiến Trung (12/07/2023)
 * Description: Hàm đóng cài đặt
 */
function handleCloseSetting() {
    searchColumn.value = '';
    handleSearchColumns();
    emits('close');
}
/**
 * Author: Tiến Trung (12/07/2023)
 * Description: Hàm reset cài đặt ban đầu
 */
const resetTable = () => {
    props.resetData();
    emits('close');
};
/**
 * Author: Tiến Trung (12/07/2023)
 * Description: Hàm ẩn hiện cột
 */
function toggleColumn(value, column) {
    columnsSetting.value = columnsSetting.value.map((columnSet) => {
        if (columnSet.title === column.title) {
            columnSet.isShow = value;
        }
        return columnSet;
    });
}

/**
 * Author: Tiến Trung (12/07/2023)
 * Description: theo dõi cột thay đổi thì gán lại
 * columnsSetting để cài đặt sắp xếp cột
 */
watch(
    () => props.columns,
    () => {
        columnsSetting.value = JSON.parse(JSON.stringify(props.columns));
    },
);
/**
 * Author: Tiến Trung (12/07/2023)
 * Description: Nếu search thay đổi thì gọi hàm searchColumns
 */
watch(searchColumn, () => {
    handleSearchColumns();
});
</script>
<style lang="scss">
@import './setting-table.scss';
</style>
