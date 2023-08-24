<template>
    <div @mousedown.stop :style="{
        left: props.positionMenuFilter?.left + 'px',
        top: props.positionMenuFilter?.bottom + 'px',
    }" class="wrapper-filter"
        :class="{ 'filter-select': props.filterType === Enum.TypeDataTable.Show || props.filterType === Enum.TypeDataTable.Business }">
        <div v-if="props.filterType !== Enum.TypeDataTable.Show && props.filterType !== Enum.TypeDataTable.Business"
            class="filter__by">
            <MISASelectFilter v-model:value="filterBySelected" selectEmpty :options="filterByOption"></MISASelectFilter>
        </div>
        <div class="filter__value">
            <MISADropdown selectEmpty :options="isShowMenu" v-model:value="filterShow"
                v-if="props.filterType === Enum.TypeDataTable.Show"></MISADropdown>
            <MISADropdown selectEmpty :options="business" v-model:value="filterBusiness"
                v-if="props.filterType === Enum.TypeDataTable.Business"></MISADropdown>
            <!-- :placeholder="MISAResource[resource.langCode]?.Table?.Filter?.ValueFilter" -->
            <MISADropdown selectEmpty :options="genderSelect" v-model:value="filterGenderSelected"
                v-if="props.filterType === Enum.TypeDataTable.Gender"></MISADropdown>
            <MISADatePicker :dateTime="filterDateSelect" v-model="filterDateSelect" posiiton="top"
                v-if="props.filterType === Enum.TypeDataTable.Date"></MISADatePicker>
            <MISAInput @enter="handleFilterData" @blur="handleFilterData" :maxLength="50" ref="iInput"
                v-model:value="filterSearch"
                v-if="props.filterType === Enum.TypeDataTable.Default || props.filterType === Enum.TypeDataTable.Money">
            </MISAInput>
        </div>
    </div>
</template>
<script setup>
import { ref, onMounted, watch } from 'vue';
import Enum from '../../../common/enum';
import MISAResource from '../../../common/resource';
import { useResource } from '../../../stores/resource.js';
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
const inputSearchOld = ref('')
const iInput = ref(null);
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
const isShowMenu = ref([
    {
        option: MISAResource[resource.langCode]?.Table?.Filter.All,
        value: Enum.FilterBy.All,
    },
    {
        option: MISAResource[resource.langCode]?.Table?.Filter.Yes,
        value: Enum.FilterBy.True,
    },
    {
        option: MISAResource[resource.langCode]?.Table?.Filter.No,
        value: Enum.FilterBy.False,
    },
]);
const business = ref([
    {
        option: MISAResource[resource.langCode]?.Table?.Filter.All,
        value: Enum.FilterBy.All,
    },
    {
        option: MISAResource[resource.langCode]?.Table?.Filter.Business,
        value: Enum.FilterBy.True,
    },
    {
        option: MISAResource[resource.langCode]?.Table?.Filter.StopBusiness,
        value: Enum.FilterBy.False,
    },
]);
const filterDateSelect = ref(null);
const filterGenderSelected = ref('1');
const filterShow = ref(Enum.FilterBy.All)
const filterBusiness = ref(Enum.FilterBy.All)
const filterBySelected = ref(Enum.FilterBy.Contain);
const filterSearch = ref('');

/**
 * Author: Tiến Trung (2/08/2023)
 * Description: hàm gửi tiêu chí lọc
 */
const emitFilter = (value) => {
    const isFilter = value.length === 0 ? false : true
    emit('filter-data', {
        property: props.keyName,
        ['value']: value,
        operator: filterBySelected.value,
        isFilter: isFilter
    });

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
            case Enum.TypeDataTable.Show:
            case Enum.TypeDataTable.Business:
                filterBySelected.value = props.filterBy ? props.filterBy : Enum.FilterBy.Equal;
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
            case Enum.TypeDataTable.Money:
                filterByOption.value = [
                    {
                        option: MISAResource[resource.langCode]?.Table?.Filter?.Contain,
                        value: Enum.FilterBy.Contain,
                    },
                    {
                        option: MISAResource[resource.langCode]?.Table?.Filter?.NotContain,
                        value: Enum.FilterBy.NotContain,
                    },
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
                filterBySelected.value = props.filterBy ? props.filterBy : Enum.FilterBy.Contain;
                filterSearch.value = props.filterValue ? props.filterValue : '';
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
 * Author: Tiến Trung (24/08/2023)
 * Description: hàm khi input blur thì filter
 */
const handleFilterData = (value) => {
    if (value !== inputSearchOld.value) {
        inputSearchOld.value = value
        emitFilter(value);
    }
}
/**
 * Author: Tiến Trung (24/08/2023)
 * Description: watch theo dõi khi lựa chọn lọc thay đổi thì emit
 */
watch(() => filterBySelected.value, () => {
    if (inputSearchOld.value.trim('').length !== 0) {
        emitFilter(inputSearchOld.value);
    }
})
/**
 * Author: Tiến Trung (24/08/2023)
 * Description: watch theo dõi khi lựa chọn lọc hiển thị thay đổi
 */
watch(() => filterShow.value, () => {
    if (filterShow.value === Enum.FilterBy.All) {
        emit('filter-data', {
            property: props.keyName,
            ['value']: 'helo',
            operator: Enum.FilterBy.AllData,
            isFilter: true
        });
    } else {
        emit('filter-data', {
            property: props.keyName,
            ['value']: filterShow.value,
            operator: Enum.FilterBy.Equal,
            isFilter: true
        });
    }

})
/**
 * Author: Tiến Trung (24/08/2023)
 * Description: watch theo dõi khi lựa chọn lọc trạng thái kinh doanh thay đổi
 */
watch(() => filterBusiness.value, () => {
    if (filterBusiness.value === Enum.FilterBy.All) {
        emit('filter-data', {
            property: props.keyName,
            ['value']: '',
            operator: Enum.FilterBy.AllData,
            isFilter: true
        });
    } else {
        emit('filter-data', {
            property: props.keyName,
            ['value']: filterBusiness.value,
            operator: Enum.FilterBy.Equal,
            isFilter: true
        });
    }

})
</script>
<style lang="scss">
@import './filter-table.scss';
</style>
