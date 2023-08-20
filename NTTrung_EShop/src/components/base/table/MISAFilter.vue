<template>
    <div
        @keydown="handleKeyDown"
        @mousedown.stop
        :style="{
            left: props.positionMenuFilter?.left + 'px',
            top: props.positionMenuFilter?.bottom + 'px',
        }"
        class="wrapper-filter"
    >
        <div class="filter__by">
            <MISASelectFilter v-model:value="filterBySelected" selectEmpty :options="filterByOption"></MISASelectFilter>
        </div>
        <div class="filter__value">
            <!-- :placeholder="MISAResource[resource.langCode]?.Table?.Filter?.ValueFilter" -->
            <MISADropdown
                selectEmpty
                :options="genderSelect"
                v-model:value="filterGenderSelected"
                v-if="props.filterType === Enum.TypeDataTable.Gender"
            ></MISADropdown>
            <MISADatePicker
                :dateTime="filterDateSelect"
                v-model="filterDateSelect"
                posiiton="top"
                v-if="props.filterType === Enum.TypeDataTable.Date"
            ></MISADatePicker>
            <MISAInput
                :maxLength="50"
                ref="iInput"
                v-model:value="filterSearch"
                v-if="props.filterType === Enum.TypeDataTable.Default"
            ></MISAInput>
        </div>
    </div>
</template>
<script setup>
import { ref, onMounted } from 'vue';
import Enum from '../../../common/enum';
import MISAResource from '../../../common/resource';
import { useResource } from '../../../stores/resource.js';
import { convertDateForBE } from '../../../common/convert-data';
const resource = useResource();
const emit = defineEmits(['filter-data', 'close-menu']);
const props = defineProps({
    //Loại cột lọc là date, gender hay là bình thường
    filterType: { type: Number, default: Enum.TypeDataTable.Default },
    //Vị trí của MenuFilter
    positionMenuFilter: {
        type: Object,
    },
    //Key của đối tượng
    keyName: String,
    //Giá trị lọc
    filterValue: [Number, String],
    //Lọc theo kiểu nào
    filterBy: String,
    //Danh sách khóa đã được lọc
    listKeyFilterSelected: Array,
});
const iInput = ref(null);
const buttonUnFilter = ref(null);
const buttonFilter = ref(null);
const filterByOption = ref([
    {
        option: MISAResource[resource.langCode]?.Table?.Filter?.Equal,
        value: Enum.FilterBy.Equal,
    },
    {
        option: MISAResource[resource.langCode]?.Table?.Filter?.NotEqual,
        value: Enum.FilterBy.NotEqual,
    },
    {
        option: MISAResource[resource.langCode]?.Table?.Filter?.Contain,
        value: Enum.FilterBy.Contain,
    },
    {
        option: MISAResource[resource.langCode]?.Table?.Filter?.NotContain,
        value: Enum.FilterBy.NotContain,
    },
]);
const genderSelect = ref([
    {
        option: MISAResource[resource.langCode]?.Gender?.Male,
        value: '1',
    },
    {
        option: MISAResource[resource.langCode]?.Gender?.Female,
        value: '0',
    },
    {
        option: MISAResource[resource.langCode]?.Gender?.Other,
        value: '2',
    },
]);
const filterDateSelect = ref(null);
const filterGenderSelected = ref('1');
const filterBySelected = ref(Enum.FilterBy.Contain);
const filterSearch = ref('');

/**
 * Author: Tiến Trung (2/07/2023)
 * Description: hàm hủy tiêu chí lọc
 */
const emitUnFilter = () => {
    //Do Gender có giá trị 0 nên phải tách ra
    //0 thì là false
    if (
        props.filterValue >= 0 &&
        props.filterValue !== null &&
        props.filterValue <= 2 &&
        props.filterType === Enum.TypeDataTable.Gender
    ) {
        emit('filter-data', {
            [props.keyName + 'FilterBy']: null,
            [props.keyName]: null,
        });
    } else if (props.filterValue) {
        emit('filter-data', {
            [props.keyName + 'FilterBy']: null,
            [props.keyName]: null,
        });
        //Nếu không có gì thay đổi chỉ close menu thôi không lọc
    } else {
        emit('close-menu');
    }
};
/**
 * Author: Tiến Trung (2/08/2023)
 * Description: hàm gửi tiêu chí lọc
 */
const emitFilter = () => {
    let value = '';
    //Thiết lập giá trị theo từng loại lọc
    switch (props.filterType) {
        case Enum.TypeDataTable.Gender:
            value = Number(filterGenderSelected.value);
            break;
        case Enum.TypeDataTable.Date:
            value = convertDateForBE(filterDateSelect.value);
            break;
        default:
            value = filterSearch.value;
            break;
    }
    //Giới tính có giá trị 0 nên tách ra
    if (props.filterType === Enum.TypeDataTable.Gender) {
        emit('filter-data', {
            [props.keyName + 'FilterBy']: filterBySelected.value,
            [props.keyName]: value,
        });
    } else if (value !== null && value !== undefined && value.trim().length > 0) {
        emit('filter-data', {
            [props.keyName + 'FilterBy']: filterBySelected.value,
            [props.keyName]: value,
        });
    } else {
        emitUnFilter();
    }
};
/**
 * Author: Tiến Trung (02/08/2023)
 * Description: khi component được hiển thị cập nhật dữ liệu cần thiết
 */
onMounted(() => {
    try {
        let newDate = null;
        if (props.filterType === Enum.TypeDataTable.Date) {
            newDate = props.filterValue ? new Date(props.filterValue) : null;
        }
        //Mỗi cột lọc có từng tiêu chí khác nhau
        switch (props.filterType) {
            case Enum.TypeDataTable.Gender:
                filterByOption.value = [
                    {
                        option: MISAResource[resource.langCode]?.Table?.Filter?.Equal,
                        value: Enum.FilterBy.Equal,
                    },
                    {
                        option: MISAResource[resource.langCode]?.Table?.Filter?.NotEqual,
                        value: Enum.FilterBy.NotEqual,
                    },
                ];
                filterBySelected.value = props.filterBy ? props.filterBy : Enum.FilterBy.Equal;
                filterGenderSelected.value = props.filterValue
                    ? props.filterValue.toString()
                    : filterGenderSelected.value;
                break;
            case Enum.TypeDataTable.Date:
                filterByOption.value = [
                    {
                        option: MISAResource[resource.langCode]?.Table?.Filter?.Equal,
                        value: Enum.FilterBy.Equal,
                    },
                    {
                        option: MISAResource[resource.langCode]?.Table?.Filter?.NotEqual,
                        value: Enum.FilterBy.NotEqual,
                    },
                    {
                        option: MISAResource[resource.langCode]?.Table?.Filter?.Greater,
                        value: Enum.FilterBy.Greater,
                    },
                    {
                        option: MISAResource[resource.langCode]?.Table?.Filter?.Smaller,
                        value: Enum.FilterBy.Smaller,
                    },
                ];
                filterBySelected.value = props.filterBy ? props.filterBy : Enum.FilterBy.Equal;
                filterDateSelect.value = newDate;
                break;
            default:
                filterBySelected.value = props.filterBy ? props.filterBy : Enum.FilterBy.Contain;
                filterSearch.value = props.filterValue ? props.filterValue : '';
                // iInput.value.autoFocus();
                break;
        }
    } catch (error) {
        console.log(error);
    }
});
/**
 * Author: Tiến Trung (02/08/2023)
 * Description: hàm click vào button filter
 */
const handleClickFilter = (isFilter) => {
    try {
        if (isFilter) {
            emitFilter();
        } else {
            emitUnFilter();
        }
    } catch (error) {
        console.log(error);
    }
};
/**
 * Author: Tiến Trung (2/08/2023)
 * Description: hàm khi sử dụng phím để lọc và không lọc
 */
const handleKeyDown = (e) => {
    try {
        if (e.keyCode === Enum.Keyboard.ESC) {
            emit('close-menu');
        }
        if (e.keyCode === Enum.Keyboard.Enter) {
            // e.preventDefault();
            buttonFilter.value.$emit('click');
        }
    } catch (error) {
        console.log(error);
    }
};
</script>
<style lang="scss">
@import './filter-table.scss';
</style>
