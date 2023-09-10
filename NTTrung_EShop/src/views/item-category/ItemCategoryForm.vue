<script setup>
import { useModalForm } from '../../stores/modalform';
import { ref, watch, onMounted, onUnmounted } from 'vue';
import Enum from '../../common/enum';
import MISAModalForm from '../../components/base/modalform/MISAModalForm.vue';
import { useDialog } from '../../stores/dialog';
import { Validator } from '../../common/validatior.js';
import { useResource } from '../../stores/resource.js';
import baseApi from '../../api/base-api';
import MISAResource from '../../common/resource';
import { useToast } from '../../stores/toast';

const modalForm = useModalForm();
const dialog = useDialog();
const resource = useResource();
const formData = ref({});
const couterChangeForm = ref(0);
const oldCode = ref('');
const validateForm = ref({
    itemCategoryCode: '',
    itemCategoryName: '',
    description: '',
});
const listErrorMessage = ref([]);
//Input-------------------
const iItemCategoryCode = ref(null);
const iItemCategoryName = ref(null);
const iDescription = ref(null);

//Input-------------------

const buttonSaveAdd = ref(null);
const buttonSave = ref(null);
const buttonCancel = ref(null);
const firstFocus = ref(null);
const loadingButton = ref({ save: false, saveAdd: false, saveCopy: false });

// eslint-disable-next-line no-unused-vars
const props = defineProps({
    buttonLoad: { type: Object, default: () => ({ save: false, saveAdd: false }) },
});

/**
 * Author: Tiến Trung (12/07/2023)
 * Description: Hàm get employee theo id
 */
const getDataId = async () => {
    try {
        baseApi.method = Enum.ApiMethod.GET;
        baseApi.path = Enum.Router.ItemCategory.Api;
        if (dialog.objectData.ItemCategoryId) {
            const res = await baseApi.request(dialog.objectData.ItemCategoryId);
            return res.data;
        }
        return {};
    } catch (error) {
        console.log(error);
    }
};

/*
 **
 * Author: Tiến Trung (27/06/2023)
 * Description: hàm đóng modal
 * nếu thay đổi dữ liệu mà đóng modal thì bật dialog
 */
function closeForm() {
    if (couterChangeForm.value > 1) {
        dialog.setFunction(submitForm);
        dialog.setCloseNavigationLink(() => {
            dialog.close();
            modalForm.close();
        });
        dialog.setMethod(modalForm.method);
        dialog.open({
            title: MISAResource[resource.langCode]?.Dialog?.Warning?.ChangeForm?.Title,
            content: MISAResource[resource.langCode]?.Dialog?.Warning?.ChangeForm?.Content,
            action: MISAResource[resource.langCode]?.Button?.Yes,
            buttonSec: MISAResource[resource.langCode]?.Button?.No,
            type: Enum.ButtonType.Pri,
            icon: 'warning',
            loading: false,
            buttonThird: MISAResource[resource.langCode]?.Button?.Cancel,
        });
    } else {
        modalForm.close();
    }
}
/*
 **
 * Author: Tiến Trung (27/06/2023)
 * Description: bắt lỗi focus vào ô đầu tiên
 */
const autoFocusForm = () => {
    try {
        if (validateForm.value.itemCategoryCode) {
            iItemCategoryCode.value.autoFocus();
            return;
        }
        if (validateForm.value.itemCategoryName) {
            iItemCategoryName.value.autoFocus();
            return;
        }
        if (validateForm.value.description) {
            iDescription.value.autoFocus();
            return;
        }
    } catch (error) {
        console.log(error);
    }
};
const toast = useToast();
/**
 * Author: Tiến Trung (26/08/2023)
 * Description: Hàm save data gọi api
 */
const saveData = async (data) => {
    baseApi.path = Enum.Router.ItemCategory.Api;
    baseApi.data = data;
    baseApi.method = Enum.ApiMethod.POST;
    const res = await baseApi.request('SaveData');
    // const res = await employeeApi.create(data);
    console.log(res);
    toast.success(MISAResource[resource.langCode]?.Toast?.Success?.SaveSuccess);
};
/**
 * Author: Tiến Trung (29/06/2023)
 * Description: hàm emit báo cho cha biết click gửi nếu bấm cất và thêm thì reset form
 */
const submitForm = async () => {
    try {
        //Nếu mà có lỗi thì lấy ra danh sách lỗi
        validateCode(formData.value.itemCategoryCode);
        validateName(formData.value.itemCategoryName);
        validateDescription(formData.value.description);
        const listError = getListErrorMessage();
        //Lấy ra lỗi đầu tiên cho vào dialog
        const error = listError.find((error) => error !== '');
        if (error) {
            dialog.setErrorMessage(error);
            dialog.setMethod(Enum.EditMode.None);
            dialog.open({
                title: MISAResource[resource.langCode]?.Dialog?.Warning?.Validate?.Title,
                action: MISAResource[resource.langCode]?.Button?.Close,
                type: Enum.ButtonType.Pri,
                icon: 'danger',
                loading: false,
            });
        } else {
            await saveData([formData.value]);
            await modalForm.affterSubmitSuccess();
            modalForm.close();
        }
    } catch (error) {
        console.log(error);
    }
};

///Validator-------------------------------------------------------
/**
 * Author: Tiến Trung (03/07/2023)
 * Description: Hàm validate form
 */
//Hàm validate mã
function validateCode(value) {
    try {
        const errorMessage = Validator(value, [
            Validator.isRequired(MISAResource[resource.langCode]?.ItemCategoryInvalidError?.ItemCategoryCodeEmpty),
            Validator.isCode(MISAResource[resource.langCode]?.ItemCategoryInvalidError?.ItemCategoryCodeFormat),
            Validator.maxLength(
                20,
                MISAResource[resource.langCode]?.ItemCategoryInvalidError?.ItemCategoryCodeMaxLength,
            ),
        ]);
        validateForm.value.itemCategoryCode = errorMessage;
        return errorMessage;
    } catch (error) {
        console.log(error);
    }
}
//Hàm validate name
function validateName(value) {
    try {
        const errorMessage = Validator(value, [
            Validator.isRequired(MISAResource[resource.langCode]?.ItemCategoryInvalidError?.ItemCategoryNameEmpty),
            Validator.maxLength(
                100,
                MISAResource[resource.langCode]?.ItemCategoryInvalidError?.ItemCategoryNameMaxLength,
            ),
        ]);
        validateForm.value.itemCategoryName = errorMessage;
        return errorMessage;
    } catch (error) {
        console.log(error);
    }
}
//Hàm validate mô tả
function validateDescription(value) {
    try {
        const errorMessage = Validator(value, [
            Validator.maxLength(255, MISAResource[resource.langCode]?.ItemCategoryInvalidError?.DescriptionMaxLength),
        ]);
        validateForm.value.description = errorMessage;
        return errorMessage;
    } catch (error) {
        console.log(error);
    }
}
///Validator-------------------------------------------------------

/**
 * Author: Tiến Trung (05/07/2023)
 * Description: Hàm update form nếu form mới thì call api lấy newCode
 */
const updateForm = async () => {
    try {
        const dataId = await getDataId();
        couterChangeForm.value = 0;
        formData.value = {
            itemCategoryId: dataId.ItemCategoryId,
            itemCategoryCode: dataId.ItemCategoryCode,
            itemCategoryName: dataId.ItemCategoryName,
            description: dataId.Description,
            editmode: modalForm.method,
            createdDate: dataId.CreatedDate,
        };
        (oldCode.value = dataId.ItemCategoryCode), iItemCategoryCode.value.autoFocus();
    } catch (error) {
        console.log(error);
    }
};
updateForm();
/**
 * Author: Tiến Trung (05/07/2023)
 * Description: Watch khi submit cất và thêm thì lấy form mới
 * mỗi lần cất và thêm thì lại tạo form mới
 */
watch(
    () => modalForm.object,
    async () => {
        if (modalForm.isShow) {
            updateForm();
        }
    },
    { deep: true },
    { immediate: true },
);
/**
 * Author: Tiến Trung (03/07/2023)
 * Description: hàm để lấy ra danh sách lỗi khi có lỗi thì add list lỗi vào mảng
 */
const getListErrorMessage = () => {
    const listError = Object.keys(validateForm.value).map((key) => validateForm.value[key]);
    listErrorMessage.value = listError;
    return listError;
};
/**
 * Author: Tiến Trung (11/07/2023)
 * Description: hàm xử lý khi gõ phím gửi form
 */
const handleKeyDown = (e) => {
    if (e.ctrlKey && e.shiftKey && e.keyCode === Enum.Keyboard.S) {
        e.preventDefault();
        //Báo cho button biết có sự kiện click thì gửi form
        buttonSaveAdd.value.$emit('click');
        iItemCategoryCode.value.autoFocus();
        return;
    }
    if (e.ctrlKey && e.keyCode === Enum.Keyboard.S) {
        e.preventDefault();
        buttonSave.value.$emit('click');
    }
    if (e.keyCode === Enum.Keyboard.ESC) {
        closeForm();
    }
};
/**
 * Author: Tiến Trung (07/09/2023)
 * Description: Hàm update data nếu là code mới thì isupdatecode true
 */
const setDataEditMode = (value) => {
    if (formData.value.EditMode !== Enum.EditMode.Add) {
        formData.value.IsUpdateCode = oldCode.value !== value;
    }
};
watch(
    () => validateForm.value,
    () => {
        getListErrorMessage();
    },
    { deep: true },
);

/**
 * Author: Tiến Trung (03/07/2023)
 * Description: watch kiểm tra khi form thay đổi
 * tăng số lần thay đổi form
 */
watch(
    () => formData.value,
    () => {
        couterChangeForm.value++;
    },
    { deep: true },
);
/**
 * Author: Tiến Trung (03/07/2023)
 * Description: watch kiểm tra dialog được đóng
 * gọi autofocus nếu có lỗi form
 */
watch(
    () => dialog.isShow,
    () => {
        if (dialog.isShow === false) {
            autoFocusForm();
        }
    },
);
/**
 * Author: Tiến Trung (2/07/2023)
 * Description: khi component được tạo gán sự kiến phím cho windown
 */
onMounted(() => {
    window.addEventListener('keydown', handleKeyDown);
    dialog.setFunction(submitForm);
});
/**
 * Author: Tiến Trung (2/07/2023)
 * Description: khi component ẩn hủy sự kiện phím
 */
onUnmounted(() => {
    window.removeEventListener('keydown', handleKeyDown);
});
</script>
<template>
    <MISAModalForm @close-modal="closeForm">
        <div class="ntt-form custom-form">
            <button ref="firstFocus" class="focusFirst"></button>
            <MISARow justify="space-between">
                <MISACol display="flex" direction="column" rowGap="12">
                    <MISARow>
                        <MISAInput
                            ref="iItemCategoryCode"
                            require
                            @input-validation="
                                (value, oldValue) => {
                                    validateCode(value);
                                    setDataEditMode(value);
                                }
                            "
                            focus
                            v-model:value="formData.itemCategoryCode"
                            name="id"
                            :label="MISAResource[resource.langCode]?.Manage?.ItemCategory?.ItemCategoryCode"
                            validate="true"
                            :errorMessage="validateForm.itemCategoryCode"
                            row
                        ></MISAInput>
                    </MISARow>
                    <MISARow>
                        <MISAInput
                            ref="iPositionName"
                            @input-validation="validateName"
                            :errorMessage="validateForm.itemCategoryName"
                            require
                            v-model:value="formData.itemCategoryName"
                            name="name"
                            :label="MISAResource[resource.langCode]?.Manage?.ItemCategory?.ItemCategoryName"
                            validate="true"
                            row
                        >
                        </MISAInput>
                    </MISARow>
                    <MISARow>
                        <MISATextArea
                            ref="iDescription"
                            @input-validation="validateDescription"
                            isName
                            :errorMessage="validateForm.description"
                            v-model:value="formData.description"
                            name="name"
                            :label="MISAResource[resource.langCode]?.Manage?.ItemCategory?.Description"
                            validate="true"
                            @keydown.tab="buttonCancel.autoFocus()"
                            :maxLength="255"
                            row
                        >
                        </MISATextArea>
                    </MISARow>
                </MISACol>
            </MISARow>
        </div>

        <template #action>
            <MISARow justify="space-between" colGap="8px">
                <MISACol> </MISACol>
                <MISACol>
                    <MISARow colGap="8px">
                        <MISAButton
                            ref="buttonSave"
                            :loading="loadingButton.save"
                            @click="submitForm"
                            :type="Enum.ButtonType.IconPri"
                            :action="MISAResource[resource.langCode]?.Button?.Save"
                            v-tooltip.absoluteTop="Enum.KeyboardShortcuts.CtrlS"
                        >
                            <template #icon>
                                <MISAIcon width="21" height="11" icon="save" />
                            </template>
                        </MISAButton>

                        <MISAButton
                            ref="buttonCancel"
                            @click="closeForm"
                            :type="Enum.ButtonType.IconPri"
                            :action="MISAResource[resource.langCode]?.Button?.Cancel"
                            link
                            v-tooltip.absoluteTop="Enum.KeyboardShortcuts.Esc"
                        >
                            <template #icon>
                                <MISAIcon width="20" height="10" icon="close-eshop" />
                            </template>
                        </MISAButton>
                    </MISARow>
                </MISACol>
            </MISARow>
            <button class="focusFirst" @focus="buttonCancel.autoFocus()"></button>
        </template>
    </MISAModalForm>
</template>

<style lang="scss">
@import './item-category-form.scss';
.custom-form {
    overflow: unset !important;
    .label-input-row {
        width: 100%;
        p {
            min-width: 150px !important;
        }
    }
    .error-message-bottom {
        left: 150px;
        bottom: -20px;
    }
    .input-text-base {
        max-width: none !important;
    }
    .wrapper-input {
        max-width: none !important;
    }
    .ntt-col {
        width: 100%;
    }
    .label-input {
        width: 100%;
    }
}
</style>
