<template lang="">
    <!-- @focusout="emit('blur', optionsSearchCombobox[indexHoverDropList].value)" -->
    <div
        :class="[{ dropdown: !combobox }, { 'select-row': props.row }]"
        @focusout="focusOut"
        @focusin="firstFocusDropdown()"
        @keydown.stop="handleKeyDown"
        class="ntt-combobox"
    >
        <label class="label-input label-dropdown">
            <p>{{ label }}<span v-if="require" :style="{ color: '#E60000', marginLeft: '4px' }">*</span></p></label
        >
        <!-- @click="inputElement.focus()" -->
        <div :class="{ 'i-error': errorMessage !== '' }" @focusout="getIndexSelected" class="custom-combobox">
            <!-- , $emit('blur', optionsSearchCombobox[indexHoverDropList]?.value) -->
            <input
                @click.stop
                @click="toggleDropdown(), scrollToOption()"
                ref="inputElement"
                :placeholder="placeholder"
                class="input-text-base combobox__i-group"
                type="text"
                :value="inputSearchCombobox"
                @input="updateComboboxInput"
                :readonly="!props.combobox"
                :style="{ cursor: props.combobox ? 'text' : 'pointer' }"
            />
            <p class="error-message">{{ errorMessage }}</p>
            <MISAButton
                @click.stop
                tabindex="-1"
                @click="clickButton(), scrollToOption()"
                type="icon"
                class="combobox__i-group"
            >
                <template #icon>
                    <img class="i-img" src="../../../assets/icons/arrow-down.svg" alt="" />
                </template>
            </MISAButton>
            <div @click="props.function" @click.stop="" v-if="props.buttonAdd" class="button-add-quick center">
                <span class="center">
                    <MISAIcon width="11" height="10" icon="plus" />
                </span>
            </div>
            <div v-show="!props.combobox ? isShowDropdown : true" class="wrapper-select">
                <ul>
                    <li
                        v-if="!props.selectEmpty"
                        ref="emptyItem"
                        @click="$emit('update:value', ''), $emit('blur', '')"
                        :class="[
                            valueSelected.value === '' ? 'selected-list' : '',
                            { 'keydown-hover': indexHoverDropList === -1 },
                        ]"
                    >
                        - {{ placeholder }} -
                        <span v-if="!value" class="center">
                            <MISAIcon width="20" height="20" icon="check"></MISAIcon>
                        </span>
                    </li>
                    <li
                        ref="itemDropdown"
                        :class="[
                            valueSelected.value === option.value ? 'selected-list' : '',
                            { 'keydown-hover': indexHoverDropList === index },
                        ]"
                        @click.stop="toggleDropdown"
                        @mousedown="updateValue(option.value), scrollToOption()"
                        v-for="(option, index) in optionsSearchCombobox"
                        :key="option.value"
                    >
                        {{ option.option }}
                        <span v-if="valueSelected.value === option.value" class="center">
                            <MISAIcon width="20" height="20" icon="check"></MISAIcon>
                        </span>
                    </li>
                </ul>
            </div>
        </div>
    </div>
</template>
<script setup>
import { computed, onMounted, ref, watch, onUnmounted } from 'vue';
import Enum from '../../../common/enum';
import { removeVietnameseTones } from '../../../common/convert-data.js';
const emit = defineEmits(['update:value', 'blur', 'next-tab-enter']);
const props = defineProps({
    label: { type: String, default: '' }, //label input
    placeholder: { type: String, default: '' }, //place holder
    options: {
        //danh sách lựa chọn
        type: Array,
        default: () => [
            {
                option: 'Công nghệ thông tin',
                value: 'CNTT',
            },
            {
                option: 'Marketing',
                value: 'MK',
            },
            {
                option: 'Product manager',
                value: 'PM',
            },
        ],
    },
    errorMessage: { type: String, default: '' },
    value: { type: String, default: '' }, //value để binding
    combobox: { type: Boolean, default: () => false }, //combobox nếu là combobox thì cho phép search
    selectEmpty: { type: Boolean, default: false }, //Nếu có selectEmpty thì không được chọn rỗng
    require: { type: Boolean, default: false }, //Có thể dùng phím để không chọn trường nòa
    row: { type: Boolean, default: false },
    buttonAdd: { type: Boolean, default: false }, // nếu có thêm nhanh thì hiện thêm button add
    function: { type: Function, default: () => {} },
});
const itemDropdown = ref(null);
const emptyItem = ref(null);
const indexHoverDropList = ref(props.selectEmpty ? 0 : -1);
const inputElement = ref(null);
const inputSearchCombobox = ref('');
const optionsSearchCombobox = ref(props.options);
const isShowDropdown = ref(false);
const firstFocus = ref(true);
/**
 * Author: Tiến Trung (06/07/2023)
 * Description: hàm update select
 */
const updateValue = (data) => {
    emit('update:value', data);
    emit('blur', data);
    inputElement.value.blur();
};
// @click="itemDropdown[indexHoverDropList]?.scrollIntoView()"
/**
 * Author: Tiến Trung (06/07/2023)
 * Description: hàm khi lần đầu tiên input focus thì mở dropdown
 */
const firstFocusDropdown = () => {
    if (firstFocus.value) {
        isShowDropdown.value = true;
    } else {
        optionsSearchCombobox.value = props.options;
    }
};
/**
 * Author: Tiến Trung (06/07/2023)
 * Description: hàm khi focusout đóng dropdown
 */
const focusOut = () => {
    setTimeout(() => {
        closeDropdown();
    }, 10);
};
/**
 * Author: Tiến Trung (06/07/2023)
 * Description: hàm đóng mở dropdown
 */
const toggleDropdown = () => {
    if (!props.combobox) {
        if (!firstFocus.value) {
            isShowDropdown.value = !isShowDropdown.value;
        }
        firstFocus.value = false;
    }
};
/**
 * Author: Tiến Trung (06/07/2023)
 * Description: hàm đóng dropdown
 */
const closeDropdown = () => {
    if (!firstFocus.value) {
        isShowDropdown.value = false;
        firstFocus.value = true;
    }
};
/**
 * Author: Tiến Trung (06/07/2023)
 * Description: hàm khi click vào button dropdown
 */
const clickButton = () => {
    if (!props.combobox) {
        toggleDropdown();
    } else {
        optionsSearchCombobox.value = props.options;
    }
    inputElement.value.focus();
};

/**
 * Author: Tiến Trung (06/07/2023)
 * Description: hàm update inputSearch, mỗi khi nhập thì
 * tìm kiếm
 */
const updateComboboxInput = (e) => {
    try {
        inputSearchCombobox.value = e.target.value;
        if (e.target.value === '') {
            if (props.combobox) {
                // emit('update:value', '');
                // emit('blur', '');
            }

            optionsSearchCombobox.value = props.options;
        } else {
            filterOption();
        }
    } catch (error) {
        console.log(error);
    }
};
/**
 * Author: Tiến Trung (06/07/2023)
 * Description: hàm tìm kiếm option của combobox
 */
const filterOption = () => {
    if (props.combobox) {
        optionsSearchCombobox.value = props.options.filter((option) => {
            return removeVietnameseTones(option.option.toLowerCase()).includes(
                removeVietnameseTones(inputSearchCombobox.value.toLowerCase()),
            );
        });
        if (Object.keys(optionsSearchCombobox.value).length === 0) {
            optionsSearchCombobox.value = props.options;
        }
    } else {
        optionsSearchCombobox.value = props.options;
    }
};
/**
 * Author: Tiến Trung (04/07/2023)
 * Description: lấy vị trí của option được chọn hiện tại
 */
const getIndexSelected = () => {
    const index = optionsSearchCombobox.value.findIndex((option) => option.value === valueSelected.value.value);
    if (index === -1) {
        indexHoverDropList.value = props.selectEmpty ? 0 : -1;
    } else {
        indexHoverDropList.value = index;
    }
};
watch(
    () => optionsSearchCombobox.value,
    () => {
        try {
            getIndexSelected();
        } catch (error) {
            console.log(error);
        }
    },
    { deep: true },
);
/**
 * Author: Tiến Trung (04/07/2023)
 * Description: khi nhấn phím lên xuống thì di chuyển đến các lựa chọn
 * Enter thì chọn
 */
const handleKeyDown = (e) => {
    // console.log(e.keyCode);
    try {
        if (e.keyCode === Enum.Keyboard.Down) {
            if (indexHoverDropList.value === optionsSearchCombobox.value.length - 1) {
                indexHoverDropList.value = props.selectEmpty ? 0 : -1;
                scrollToOption();
            } else {
                indexHoverDropList.value = indexHoverDropList.value + 1;
                scrollToOption();
            }
        }
        if (e.keyCode === Enum.Keyboard.Up) {
            if (indexHoverDropList.value < 0) {
                indexHoverDropList.value = optionsSearchCombobox.value.length - 1;
                scrollToOption();
            } else {
                indexHoverDropList.value = indexHoverDropList.value - 1;
                scrollToOption();
            }
        }
        if (e.keyCode === Enum.Keyboard.Enter) {
            if (indexHoverDropList.value === (props.selectEmpty ? 0 : -1)) {
                inputSearchCombobox.value = '';
                emit('update:value', '');
                emit('blur', '');
                emit('next-tab-enter');
                e.target.blur();
                return;
            }
            if (indexHoverDropList.value > (props.selectEmpty ? 0 : -1)) {
                inputSearchCombobox.value = optionsSearchCombobox.value[indexHoverDropList.value].option;
                emit('update:value', optionsSearchCombobox.value[indexHoverDropList.value].value);
                emit('blur', optionsSearchCombobox.value[indexHoverDropList.value].value);
                e.target.blur();
                emit('next-tab-enter');
            }
        }
    } catch (error) {
        console.log(error);
    }
};
/**
 * Author: Tiến Trung (04/07/2023)
 * Description: computed lưu option cho dropdown được chọn
 */
const valueSelected = computed(() => {
    if (props.options.length > 0 && props.value) {
        return props.options.find((option) => option.value === props.value);
    }
    return { value: '', option: '' };
});
/**
 * Author: Tiến Trung (13/07/2023)
 * Description: Hàm chủ động focus vào input
 */
const autoFocus = () => {
    inputElement.value.focus();
};

/**
 * Author: Tiến Trung (10/07/2023)
 * Description:Hàm scroll đến vị trí option được chọn
 */
const scrollToOption = () => {
    if (props.selectEmpty) {
        return;
    }
    if (indexHoverDropList.value === -1) {
        emptyItem.value.scrollIntoView({
            block: 'nearest',
            behavior: 'smooth',
        });
    } else if (itemDropdown.value?.length > 0) {
        itemDropdown.value[indexHoverDropList.value].scrollIntoView({
            block: 'nearest',
            behavior: 'smooth',
        });
    }
};
/**
 * Author: Tiến Trung (06/07/2023)
 * Description: Watch theo dõi mỗi khi chọn thì update vị trí
 * index để dùng bàn phím hover
 * và mỗi khi component mouted xong thì tìm kiếm những option
 */
watch(
    () => valueSelected.value,
    () => {
        // console.log(valueSelected.value.option);
        try {
            inputSearchCombobox.value = valueSelected.value.option;
            filterOption();
            getIndexSelected();
            scrollToOption(true);
        } catch (error) {
            console.log(error);
        }
    },
    { deep: true },
);
// window.addEventListener('click', closeDropdown);
/**
 * Author: Tiến Trung (10/07/2023)
 * Description:mounted thì thêm sự kiện
 */
onMounted(() => {
    window.addEventListener('click', closeDropdown);
    inputSearchCombobox.value = valueSelected.value.option;
});
/**
 * Author: Tiến Trung (10/07/2023)
 * Description:unmounted thì hủy sự kiện
 */
onUnmounted(() => {
    window.removeEventListener('click', closeDropdown);
});
/**
 * Author: Tiến Trung (10/07/2023)
 * Description:expose hàm autoFocus ra ngoài
 */
defineExpose({ autoFocus });
</script>
<style lang="scss" scoped>
@import './dropdown.scss';

label {
    margin-bottom: 8px;
}
</style>
