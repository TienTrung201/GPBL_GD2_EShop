<script setup>
import { useModalForm } from '../../stores/modalform';
import { ref, watch, onMounted, onUnmounted } from 'vue';
import departmentApi from '../../api/department-api';
import Enum from '../../common/enum';
import MISAModalForm from '../../components/base/modalform/MISAModalForm.vue';
import { useDialog } from '../../stores/dialog';
import { Validator } from '../../common/validatior.js';
import { useResource } from '../../stores/resource.js';
import MISAResource from '../../common/resource';

const emit = defineEmits(['submit-form', 'loading-button']);
const modalForm = useModalForm();
const dialog = useDialog();
const resource = useResource();
const formData = ref({});
const couterChangeForm = ref(0);
const validateForm = ref({
    departmentCode: '',
    departmentName: '',
    description: '',
});
const listErrorMessage = ref([]);
//Input-------------------
const iDepartmentCode = ref(null);
const iDepartmentName = ref(null);
const iDescription = ref(null);

//Input-------------------

const buttonSaveAdd = ref(null);
const buttonSave = ref(null);
const buttonCancel = ref(null);
const firstFocus = ref(null);
// eslint-disable-next-line no-unused-vars
const props = defineProps({
    buttonLoad: { type: Object, default: () => ({ save: false, saveAdd: false }) },
});

/**
 * Author: Tiến Trung (28/06/2023)
 * Description: Hàm get new code
 */
const getNewCode = async () => {
    try {
        departmentApi.method = Enum.ApiMethod.GET;
        const res = await departmentApi.request('NewCode');
        if (modalForm.method === Enum.EditMode.Add) {
            return res.data;
        } else {
            return modalForm.object.DepartmentCode;
        }
    } catch (error) {
        console.log(error);
    }
};
/**
 * Author: Tiến Trung (12/07/2023)
 * Description: Hàm get employee theo id
 */
const getDataId = async () => {
    try {
        departmentApi.method = Enum.ApiMethod.GET;
        if (modalForm.object.DepartmentId) {
            const res = await departmentApi.request(modalForm.object.DepartmentId);
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
        dialog.setObjectData(formData.value);
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
        if (validateForm.value.departmentCode) {
            iDepartmentCode.value.autoFocus();
            return;
        }
        if (validateForm.value.departmentName) {
            iDepartmentName.value.autoFocus();
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
/**
 * Author: Tiến Trung (29/06/2023)
 * Description: hàm emit báo cho cha biết click gửi nếu bấm cất và thêm thì reset form
 */
const handleSubmitForm = (isAddNewForm) => {
    try {
        //Nếu mà có lỗi thì lấy ra danh sách lỗi
        validateCode(formData.value.departmentCode);
        validateName(formData.value.departmentName);
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
            //Không có lỗi gửi emit cho Cha update
        } else {
            emit('submit-form', formData.value, isAddNewForm);
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
//Validate mã
function validateCode(value) {
    try {
        const errorMessage = Validator(value, [
            Validator.isRequired(MISAResource[resource.langCode]?.DepartmentInvalidError?.DepartmentCodeEmpty),
            Validator.isCode(MISAResource[resource.langCode]?.DepartmentInvalidError?.DepartmentCodeFormat),
            Validator.maxLength(20, MISAResource[resource.langCode]?.DepartmentInvalidError?.DepartmentCodeMaxLength),
        ]);
        validateForm.value.departmentCode = errorMessage;
        return errorMessage;
    } catch (error) {
        console.log(error);
    }
}
//Validate tên
function validateName(value) {
    try {
        const errorMessage = Validator(value, [
            Validator.isRequired(MISAResource[resource.langCode]?.DepartmentInvalidError?.DepartmentNameEmpty),
            Validator.maxLength(100, MISAResource[resource.langCode]?.DepartmentInvalidError?.DepartmentNameMaxLength),
        ]);
        validateForm.value.departmentName = errorMessage;
        return errorMessage;
    } catch (error) {
        console.log(error);
    }
}
//Validate mô tả
function validateDescription(value) {
    try {
        const errorMessage = Validator(value, [
            Validator.maxLength(250, MISAResource[resource.langCode]?.DepartmentInvalidError?.DescriptionMaxLength),
        ]);
        validateForm.value.description = errorMessage;
        return errorMessage;
    } catch (error) {
        console.log(error);
    }
}
///Validator-------------------------------------------------------
///Validator-------------------------------------------------------
///Validator-------------------------------------------------------

/**
 * Author: Tiến Trung (05/07/2023)
 * Description: Hàm update form nếu form mới thì call api lấy newCode
 */
const updateForm = async () => {
    const newCode = await getNewCode();
    const dataId = await getDataId();
    couterChangeForm.value = 0;
    formData.value = {
        departmentCode: newCode,
        departmentName: dataId.DepartmentName,
        description: dataId.Description,
    };
    iDepartmentCode.value.autoFocus();
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
        iDepartmentCode.value.autoFocus();
        return;
    }
    if (e.ctrlKey && e.keyCode === Enum.Keyboard.S) {
        e.preventDefault();
        //Báo cho button biết có sự kiện click thì gửi form
        buttonSave.value.$emit('click');
    }
    if (e.keyCode === Enum.Keyboard.ESC) {
        closeForm();
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
    dialog.setFunction(handleSubmitForm);
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
        <div class="ntt-form">
            <button ref="firstFocus" class="focusFirst"></button>
            <MISARow gutter="12" justify="space-between">
                <MISACol gutter="4" span="10">
                    <MISAInput
                        ref="iDepartmentCode"
                        require
                        @input-validation="validateCode"
                        focus
                        v-model:value="formData.departmentCode"
                        name="id"
                        placeholder="NV-0000"
                        :label="MISAResource[resource.langCode]?.Manage?.DepartmentInfo?.DepartmentCode"
                        validate="true"
                        :errorMessage="validateForm.departmentCode"
                    ></MISAInput>
                </MISACol>
                <MISACol gutter="4" span="14">
                    <MISAInput
                        ref="iDepartmentName"
                        @input-validation="validateName"
                        :errorMessage="validateForm.departmentName"
                        require
                        v-model:value="formData.departmentName"
                        name="name"
                        :label="MISAResource[resource.langCode]?.Manage?.DepartmentInfo?.DepartmentName"
                        validate="true"
                    ></MISAInput>
                </MISACol>
            </MISARow>
            <MISARow gutter="12" justify="space-between">
                <MISACol gutter="4" span="24">
                    <MISATextArea
                        ref="iDescription"
                        @input-validation="validateDescription"
                        isName
                        :errorMessage="validateForm.description"
                        v-model:value="formData.description"
                        name="name"
                        :label="MISAResource[resource.langCode]?.Manage?.DepartmentInfo?.Description"
                        validate="true"
                        @keydown.tab="buttonCancel.autoFocus()"
                    >
                    </MISATextArea>
                </MISACol>
            </MISARow>
        </div>

        <template #action>
            <MISARow justify="space-between" colGap="8px">
                <MISACol>
                    <MISAButton
                        ref="buttonCancel"
                        @click="closeForm"
                        @keydown.tab="firstFocus.focus()"
                        :type="Enum.ButtonType.Sec"
                        :action="MISAResource[resource.langCode]?.Button?.Cancel"
                        v-tooltip.absoluteTop="Enum.KeyboardShortcuts.Esc"
                    ></MISAButton>
                </MISACol>
                <MISACol>
                    <MISARow colGap="8px">
                        <!-- @keydown.enter="handleSubmitForm(false)" -->
                        <MISAButton
                            ref="buttonSave"
                            @click="handleSubmitForm(false)"
                            :loading="buttonLoad.save"
                            :type="Enum.ButtonType.Sec"
                            :action="MISAResource[resource.langCode]?.Button?.Save"
                            v-tooltip.absoluteTop="Enum.KeyboardShortcuts.CtrlS"
                        ></MISAButton>
                        <MISAButton
                            ref="buttonSaveAdd"
                            @click="handleSubmitForm(true)"
                            :loading="buttonLoad.saveAdd"
                            :type="Enum.ButtonType.Pri"
                            :action="MISAResource[resource.langCode].Button.SaveAdd"
                            v-tooltip.absoluteTop="Enum.KeyboardShortcuts.CtrlShiftS"
                        ></MISAButton>
                    </MISARow>
                </MISACol>
            </MISARow>
            <button class="focusFirst" @focus="buttonCancel.autoFocus()"></button>
        </template>
    </MISAModalForm>
</template>

<style lang="scss">
.ntt-form {
    display: flex;
    flex-direction: column;
    // column-gap: 24px;
    row-gap: 24px;
    justify-content: flex-start;

    // justify-content: space-between;
}
</style>
