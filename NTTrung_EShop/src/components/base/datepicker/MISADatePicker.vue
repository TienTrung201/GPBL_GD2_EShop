<template lang="">
    <div>
        <p class="label-dapepicker">{{ label }}</p>
        <div class="wrapper-input">
            <VueDatePicker
                v-model="data"
                :change="$emit('update:modelValue', data)"
                input-class-name="dp-custom-input"
                placeholder="DD/MM/YY"
                :position="position"
                auto-apply
                arrow-navigation
                keep-action-row
                :enable-time-picker="false"
                :max-date="maxDate"
                :start-date="startDate"
                :format="format"
                calendar-cell-class-name="dp-custom-cell"
                menu-class-name="dp-custom-menu"
                :day-names="days"
                class="custom-date-picker"
            >
                <template #action-row>
                    <p
                        :class="{ 'nttrung-no-select-today': noSelectToDay === true }"
                        class="nttrung_action-row center"
                        @click="selectToday('DateOfBirth')"
                    >
                        {{ MISAResource[resource.langCode]?.DatePicker?.Today }}
                    </p>
                </template>
                <template #month-year="{ month, year, months, years, updateMonthYear, handleMonthYearChange }">
                    <div class="custom-month-year-component">
                        <div class="wrapper-select">
                            <select
                                class="select-input"
                                :value="month"
                                @change="updateMonth($event, updateMonthYear, year)"
                            >
                                <option v-for="m in months" :key="m.value" :value="m.value">
                                    {{ MISAResource[resource.langCode]?.DatePicker[m.text] + ',' }}
                                </option>
                            </select>
                            <select
                                class="select-input select-input-custom"
                                :value="year"
                                @change="updateYear($event, updateMonthYear, month)"
                            >
                                <option v-for="y in years" :key="y.value" :value="y.value">{{ y.text }}</option>
                            </select>
                            <p class="center datepicker-select">
                                <MISAIcon class="icon-datepicker--down" width="8" height="14" icon="right"></MISAIcon>
                            </p>
                        </div>
                    </div>
                    <div class="wrapper-change-month">
                        <MISAButton
                            :type="Enum.ButtonType.Icon"
                            class="custom-icon"
                            @click="handleMonthYearChange(false)"
                        >
                            <template #icon>
                                <MISAIcon class="icon-datepicker--left" width="8" height="14" icon="left"></MISAIcon>
                            </template>
                        </MISAButton>
                        <MISAButton
                            :type="Enum.ButtonType.Icon"
                            class="custom-icon"
                            @click="handleMonthYearChange(true)"
                        >
                            <template #icon>
                                <MISAIcon class="icon-datepicker--left" width="8" height="14" icon="right"></MISAIcon>
                            </template>
                        </MISAButton>
                    </div>
                </template>
            </VueDatePicker>
        </div>
    </div>
</template>
<script setup>
import { ref, watch, onMounted } from 'vue';
import { convertDate } from '../../../common/convert-data';
import VueDatePicker from '@vuepic/vue-datepicker';
import '@vuepic/vue-datepicker/dist/main.css';
import { useResource } from '../../../stores/resource.js';
import MISAResource from '../../../common/resource';
import Enum from '../../../common/enum';
const props = defineProps({
    dateTime: { type: Date, default: null },
    label: {
        //label cho date picker
        type: String,
        default: '',
    }, // vị trí datrpicker
    position: {
        type: String,
        default: '',
    }, //Max ngày được chọn
    maxDate: {
        type: Date,
        default: new Date(),
    }, //Ngày bắt đầu hiển thị trên datePicker
    startDate: {
        type: Date,
        default: new Date(),
    }, //Không được phép chọn ngày hôm nay
    noSelectToDay: {
        type: Boolean,
        default: false,
    },
});
const resource = useResource();
const days = ref([]);
const daysDefault = ref(['Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa', 'Su']);
/**
 * Author: Tiến Trung 26/07/2023)
 * Description: khi component được tạo thì chuyển các ngày
 * có giá trị tương ứng tiếng anh tiếng việt
 */
onMounted(() => {
    days.value = daysDefault.value.map((day) => MISAResource[resource.langCode].DatePicker[day]);
});
defineEmits(['update:modelValue']);
const data = ref(props.dateTime);
/**
 * Author: Tiến Trung 26/07/2023)
 * Description: theo dõi date time thay đổi thì thay đổi biến data
 * từ dateTime cha chuyền vào
 */
watch(
    () => props.dateTime,
    () => {
        data.value = props.dateTime;
    },
);

// Date picker
/**
 * Author: Tiến Trung 26/07/2023)
 * Description: fomart định dạng ngày tháng năm cho input
 */
function format(date) {
    return convertDate(date);
}
/**
 * Author: Tiến Trung 26/07/2023)
 * Description: Chọn ngày hôm nay
 */
const selectToday = () => {
    if (!props.noSelectToDay) {
        const dateNow = new Date();
        data.value = dateNow;
    }
};
//Custom month year
/**
 * Author: Tiến Trung 26/07/2023)
 * Description: Hàm update tháng
 */
const updateMonth = (event, updateMonthYear, year) => {
    updateMonthYear(+event.target.value, year);
};
/**
 * Author: Tiến Trung 26/07/2023)
 * Description: Hàm update năm
 */
const updateYear = (event, updateMonthYear, month) => {
    updateMonthYear(month, +event.target.value);
};
</script>
<style lang="scss">
@import './datepicker.scss';
//datepicker
</style>
