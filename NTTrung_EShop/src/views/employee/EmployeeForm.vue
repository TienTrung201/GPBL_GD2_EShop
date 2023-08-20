<script setup>
import { useModalForm } from '../../stores/modalform';
import { onMounted, onUnmounted, ref, watch } from 'vue';
import positionApi from '../../api/position-api';
import departmentApi from '../../api/department-api';
import Enum from '../../common/enum';
import MISAModalForm from '../../components/base/modalform/MISAModalForm.vue';
import employeeApi from '../../api/employee-api';
import { useDialog } from '../../stores/dialog';
import { Validator } from '../../common/validatior.js';
import { useResource } from '../../stores/resource.js';
import MISAResource from '../../common/resource';

const emit = defineEmits(['submit-form', 'loading-button']);
const modalForm = useModalForm();
const dialog = useDialog();
const resource = useResource();
const formData = ref({});
const positions = ref([]);
const departments = ref([]);
const couterChangeForm = ref(0);
const validateForm = ref({
    employeeCode: '',
    fullName: '',
    department: '',
    identityNumber: '',
    identityPlace: '',
    address: '',
    phoneNumber: '',
    landlinePhone: '',
    email: '',
    bankAccount: '',
    bankName: '',
    bankBranch: '',
});
const listErrorMessage = ref([]);
//Input-------------------
const iEmployeeCode = ref(null);
const iFullName = ref(null);
const iEmail = ref(null);
const iIdentity = ref(null);
const iPhoneNumber = ref(null);
const iDepartment = ref(null);
const iBankAccount = ref(null);
const iLandLinePhone = ref(null);
const iIdentityPlace = ref(null);
const iAddress = ref(null);
const iBankName = ref(null);
const iBankBranch = ref(null);
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
 * Description: Hàm get api vị trí làm việc
 */
const getDataPosition = async () => {
    try {
        positionApi.method = Enum.ApiMethod.GET;
        const res = await positionApi.request();
        positions.value = res.data.map((data) => {
            return {
                option: data.PositionName,
                value: data.PositionId,
            };
        });
    } catch (error) {
        console.log(error);
    }
};
/**
 * Author: Tiến Trung (28/06/2023)
 * Description: Hàm get api bộ phận làm việc
 */
const getDataDepartment = async () => {
    try {
        departmentApi.method = Enum.ApiMethod.GET;
        const res = await departmentApi.request();
        departments.value = res.data.map((data) => {
            return {
                option: data.DepartmentName,
                value: data.DepartmentId,
            };
        });
    } catch (error) {
        console.log(error);
    }
};
/**
 * Author: Tiến Trung (28/06/2023)
 * Description: Hàm get new code
 */
const getNewEmployeeCode = async () => {
    try {
        employeeApi.method = Enum.ApiMethod.GET;
        const res = await employeeApi.request('NewCode');
        if (modalForm.method === Enum.EditMode.Add || modalForm.method === Enum.EditMode.Copy) {
            return res.data;
        } else {
            return modalForm.object.EmployeeCode;
        }
    } catch (error) {
        console.log(error);
    }
};
/**
 * Author: Tiến Trung (12/07/2023)
 * Description: Hàm get employee theo id
 */
const getEmployee = async () => {
    try {
        employeeApi.method = Enum.ApiMethod.GET;
        if (modalForm.object.EmployeeId) {
            const res = await employeeApi.request(modalForm.object.EmployeeId);
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
        //Nếu là lỗi trùng mã chủ động cho input bị lỗi để báo
        if (dialog.errorCode === Enum.ErorCode.DuplicateCode) {
            dialog.setErrorCode(0);
            validateForm.value.employeeCode =
                MISAResource[resource.langCode].EmployeeInvalidError.EmployeeDuplicateCode;
            iEmployeeCode.value.autoFocus();
            return;
        }
        if (validateForm.value.employeeCode) {
            iEmployeeCode.value.autoFocus();
            return;
        }
        if (validateForm.value.fullName) {
            iFullName.value.autoFocus();
            return;
        }
        if (validateForm.value.department) {
            iDepartment.value.autoFocus();
            return;
        }
        if (validateForm.value.identityNumber) {
            iIdentity.value.autoFocus();
            return;
        }
        if (validateForm.value.identityPlace) {
            iIdentityPlace.value.autoFocus();
            return;
        }
        if (validateForm.value.address) {
            iAddress.value.autoFocus();
            return;
        }
        if (validateForm.value.phoneNumber) {
            iPhoneNumber.value.autoFocus();
            return;
        }
        if (validateForm.value.landlinePhone) {
            iLandLinePhone.value.autoFocus();
            return;
        }
        if (validateForm.value.email) {
            iEmail.value.autoFocus();
            return;
        }
        if (validateForm.value.bankAccount) {
            iBankAccount.value.autoFocus();
            return;
        }
        if (validateForm.value.bankName) {
            iBankName.value.autoFocus();
            return;
        }
        if (validateForm.value.bankBranch) {
            iBankBranch.value.autoFocus();
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
        validateEmployeeCode(formData.value.employeeCode);
        validateFullName(formData.value.fullName);
        validateDepartment(formData.value.departmentId);
        validateIdentity(formData.value.identityNumber);
        validateIdentityPlace(formData.value.identityPlace);
        validateAddress(formData.value.address);
        validatePhoneNumber(formData.value.phoneNumber);
        validateLandlinePhone(formData.value.landlinePhone);
        validateEmail(formData.value.email);
        validateBankAccount(formData.value.bankAccount);
        validateBankName(formData.value.bankName);
        validateBankBranch(formData.value.bankBranch);
        const listError = getListErrorMessage();
        //Lấy ra lỗi đầu tiên để hiển thị dialog
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
            emit('submit-form', formData.value, isAddNewForm);
        }
        // !validateBankAccount(formData.value.BankAccount)
        //Không có lỗi gửi emit cho Cha update
    } catch (error) {
        console.log(error);
    }
};
///Validator-------------------------------------------------------
///Validator-------------------------------------------------------
///Validator-------------------------------------------------------
/**
 * Author: Tiến Trung (03/07/2023)
 * Description: Hàm validate form
 */
//Hàm validate mã
function validateEmployeeCode(value) {
    try {
        const errorMessage = Validator(value, [
            Validator.isRequired(MISAResource[resource.langCode]?.EmployeeInvalidError?.EmployeeCodeEmpty),
            Validator.isCode(MISAResource[resource.langCode]?.EmployeeInvalidError?.EmployeeCodeFormat),
            Validator.maxLength(20, MISAResource[resource.langCode]?.EmployeeInvalidError?.EmployeeCodeMaxLength),
        ]);
        validateForm.value.employeeCode = errorMessage;
        return errorMessage;
    } catch (error) {
        console.log(error);
    }
}
//Hàm validate tên
function validateFullName(value) {
    try {
        const errorMessage = Validator(value, [
            Validator.isRequired(MISAResource[resource.langCode]?.EmployeeInvalidError?.FullNameEmpty),
            Validator.maxLength(50, MISAResource[resource.langCode]?.EmployeeInvalidError?.FullNameMaxLength),
        ]);
        validateForm.value.fullName = errorMessage;
        return errorMessage;
    } catch (error) {
        console.log(error);
    }
}
//Hàm validate email
function validateEmail(value) {
    try {
        const errorMessage = Validator(value, [
            Validator.isEmail(MISAResource[resource.langCode]?.EmployeeInvalidError?.EmailFormat),
            Validator.maxLength(50, MISAResource[resource.langCode]?.EmployeeInvalidError?.EmailMaxLength),
        ]);
        validateForm.value.email = errorMessage;
        return errorMessage;
    } catch (error) {
        console.log(error);
    }
}
//Hàm validate CMND
function validateIdentity(value) {
    try {
        const errorMessage = Validator(value, [
            Validator.maxLength(15, MISAResource[resource.langCode]?.EmployeeInvalidError?.IdentityNumberMaxLength),
        ]);
        validateForm.value.identityNumber = errorMessage;
        return errorMessage;
    } catch (error) {
        console.log(error);
    }
}
//Hàm validate Số điện thoại
function validatePhoneNumber(value) {
    try {
        const errorMessage = Validator(value, [
            Validator.maxLength(15, MISAResource[resource.langCode]?.EmployeeInvalidError?.PhoneNumberMaxLength),
        ]);
        validateForm.value.phoneNumber = errorMessage;
        return errorMessage;
    } catch (error) {
        console.log(error);
    }
}
//Hàm validate DT cố định
function validateLandlinePhone(value) {
    try {
        const errorMessage = Validator(value, [
            Validator.maxLength(15, MISAResource[resource.langCode]?.EmployeeInvalidError?.LandLinePhoneMaxLength),
        ]);
        validateForm.value.landlinePhone = errorMessage;
        return errorMessage;
    } catch (error) {
        console.log(error);
    }
}
//Hàm validate số tài khoản
function validateBankAccount(value) {
    try {
        const errorMessage = Validator(value, [
            Validator.maxLength(12, MISAResource[resource.langCode].EmployeeInvalidError.BankAccountMaxLength),
        ]);
        validateForm.value.bankAccount = errorMessage;
        return errorMessage;
    } catch (error) {
        console.log(error);
    }
}
//Hàm validate phòng ban
function validateDepartment(value) {
    try {
        const errorMessage = Validator(value, [
            Validator.isRequired(MISAResource[resource.langCode]?.EmployeeInvalidError?.DepartmentEmpty),
        ]);
        validateForm.value.department = errorMessage;
        return errorMessage;
    } catch (error) {
        console.log(error);
    }
}
//Hàm validate nơi cấp CMND
function validateIdentityPlace(value) {
    try {
        const errorMessage = Validator(value, [
            Validator.maxLength(20, MISAResource[resource.langCode]?.EmployeeInvalidError?.IdentityPlaceMaxLength),
        ]);
        validateForm.value.identityPlace = errorMessage;
        return errorMessage;
    } catch (error) {
        console.log(error);
    }
}
//Hàm validate địa chỉ
function validateAddress(value) {
    try {
        const errorMessage = Validator(value, [
            Validator.maxLength(150, MISAResource[resource.langCode]?.EmployeeInvalidError?.AddressMaxLength),
        ]);
        validateForm.value.address = errorMessage;
        return errorMessage;
    } catch (error) {
        console.log(error);
    }
}
//Hàm validate tên ngân hàng
function validateBankName(value) {
    try {
        const errorMessage = Validator(value, [
            Validator.maxLength(100, MISAResource[resource.langCode]?.EmployeeInvalidError?.BankNameMaxLength),
        ]);
        validateForm.value.bankName = errorMessage;
        return errorMessage;
    } catch (error) {
        console.log(error);
    }
}
//Hàm validate chi nhanh ngân hàng
function validateBankBranch(value) {
    try {
        const errorMessage = Validator(value, [
            Validator.maxLength(50, MISAResource[resource.langCode]?.EmployeeInvalidError?.BankBranchMaxLength),
        ]);
        validateForm.value.bankBranch = errorMessage;
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
    const newCode = await getNewEmployeeCode();
    const employee = await getEmployee();
    const dateOfBirth = employee.DateOfBirth ? new Date(employee.DateOfBirth) : null;
    const identityDate = employee.IdentityDate ? new Date(employee.IdentityDate) : null;
    let gender;
    if (employee.Gender >= 0 && employee.Gender <= 2) {
        gender = employee.Gender;
    } else {
        gender = Enum.Gender.Male;
    }
    couterChangeForm.value = 0;
    formData.value = {
        employeeCode: newCode,
        fullName: employee.FullName,
        email: employee.Email,
        gender: gender,
        dateOfBirth: dateOfBirth,
        salary: employee.Salary,
        address: employee.Address,
        identityNumber: employee.IdentityNumber,
        identityDate: identityDate,
        identityPlace: employee.IdentityPlace,
        departmentName: employee.DepartmentName,
        departmentId: employee.DepartmentId,
        positionId: employee.PositionId,
        positionName: employee.PositionName,
        phoneNumber: employee.PhoneNumber,
        landlinePhone: employee.LandlinePhone,
        bankAccount: employee.BankAccount,
        bankName: employee.BankName,
        bankBranch: employee.BankBranch,
        isCustomer: employee.IsCustomer ? true : false,
        isProvider: employee.IsProvider ? true : false,
    };
    iEmployeeCode.value.autoFocus();
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
            debugger;
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
        iEmployeeCode.value.autoFocus();
        //Báo cho button biết có sự kiện click thì gửi form
        buttonSaveAdd.value.$emit('click');
        iEmployeeCode.value.autoFocus();
        return;
        // handleSubmitForm(false);
    }
    if (e.ctrlKey && e.keyCode === Enum.Keyboard.S) {
        e.preventDefault();
        buttonSave.value.$emit('click');
        // handleSubmitForm(true);
    }
    if (e.keyCode === Enum.Keyboard.ESC) {
        closeForm();
    }
};
/**
 * Author: Tiến Trung (11/07/2023)
 * Description: theo dõi nếu validateForm thay đổi
 * gọi hàm lấy danh sách lỗi
 */
watch(
    () => validateForm.value,
    () => {
        getListErrorMessage();
    },
    { deep: true },
);

/**
 * Author: Tiến Trung (03/07/2023)
 * Description: watch kiểm tra khi form thay đổi lưu
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
 * Description: watch focus form khi có lỗi
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
 * Description: khi component được tạo thì get data
 * mỗi lần cất và thêm thì lại tạo form mới
 */

onMounted(() => {
    getDataDepartment();
    getDataPosition();
    window.addEventListener('keydown', handleKeyDown);
    dialog.setFunction(handleSubmitForm);
});
/**
 * Author: Tiến Trung (2/07/2023)
 * Description: khi component hủy thì xóa sự kiện phím
 */
onUnmounted(() => {
    window.removeEventListener('keydown', handleKeyDown);
});
</script>
<template>
    <MISAModalForm @close-modal="closeForm">
        <div class="ntt-form">
            <!-- <MISARow> -->
            <!-- <MISAInput isSuccess title="input"></MISAInput>
        <MISAInput title="input"></MISAInput>
        <MISAInput :validate="true" isSuccesstype="success" title="input"></MISAInput>
        <MISAInput :validate="true" title="input"></MISAInput> -->
            <!-- </MISARow> -->
            <MISARow gutter="12" justify="space-between">
                <MISACol gutter="12" span="12">
                    <MISARow gutter="4" justify="space-between">
                        <MISACol gutter="4" span="10">
                            <MISAInput
                                ref="iEmployeeCode"
                                require
                                @input-validation="validateEmployeeCode"
                                focus
                                v-model:value="formData.employeeCode"
                                name="id"
                                placeholder="NV-0000"
                                :label="MISAResource[resource.langCode].Manage.EmployeeInfo.Id"
                                validate="true"
                                :errorMessage="validateForm.employeeCode"
                            ></MISAInput>
                        </MISACol>
                        <MISACol gutter="4" span="14">
                            <MISAInput
                                ref="iFullName"
                                @input-validation="validateFullName"
                                isName
                                :errorMessage="validateForm.fullName"
                                require
                                v-model:value="formData.fullName"
                                name="name"
                                :label="MISAResource[resource.langCode]?.Manage?.EmployeeInfo?.FullName"
                                validate="true"
                            >
                            </MISAInput>
                        </MISACol>
                    </MISARow>
                </MISACol>
                <MISACol gutter="12" span="12">
                    <MISARow gutter="4" justify="space-between">
                        <MISACol gutter="4" span="10">
                            <MISADatePicker
                                :label="MISAResource[resource.langCode]?.Manage?.EmployeeInfo?.DateOfBirth"
                                :dateTime="formData.dateOfBirth"
                                v-model="formData.dateOfBirth"
                                position="left"
                                :startDate="new Date('12/31/2005')"
                                :maxDate="new Date('12/31/2005')"
                                noSelectToDay
                            >
                            </MISADatePicker>
                        </MISACol>
                        <MISACol gutter="4" span="13">
                            <MISARadioGroup :label="MISAResource[resource.langCode]?.Manage?.EmployeeInfo?.Gender">
                                <MISARadio
                                    v-model:value="formData.gender"
                                    :valueRadio="1"
                                    :optionName="MISAResource[resource.langCode]?.Gender?.Male"
                                    name="gender"
                                ></MISARadio>
                                <MISARadio
                                    v-model:value="formData.gender"
                                    :valueRadio="0"
                                    :optionName="MISAResource[resource.langCode]?.Gender?.Female"
                                    name="gender"
                                ></MISARadio>
                                <MISARadio
                                    v-model:value="formData.gender"
                                    :valueRadio="2"
                                    :optionName="MISAResource[resource.langCode]?.Gender?.Other"
                                    name="gender"
                                ></MISARadio>
                            </MISARadioGroup>
                        </MISACol>
                    </MISARow>
                </MISACol>
            </MISARow>
            <MISARow gutter="12" justify="space-between">
                <MISACol gutter="12" span="12">
                    <MISADropdown
                        @keydown="handleKeyDown"
                        require
                        :errorMessage="validateForm.department"
                        ref="iDepartment"
                        @blur="validateDepartment"
                        @next-tab-enter="iIdentity.autoFocus()"
                        combobox
                        :options="departments"
                        v-model:value="formData.departmentId"
                        :placeholder="MISAResource[resource.langCode]?.Manage?.EmployeeInfo?.Department?.Empty"
                        :label="MISAResource[resource.langCode]?.Manage?.EmployeeInfo?.Department?.Name"
                    ></MISADropdown>
                </MISACol>
                <MISACol gutter="12" span="12">
                    <MISARow gutter="4">
                        <MISACol gutter="4" span="14">
                            <MISAInput
                                v-model:value="formData.identityNumber"
                                :errorMessage="validateForm.identityNumber"
                                @input-validation="validateIdentity"
                                ref="iIdentity"
                                name="identity"
                                :label="MISAResource[resource.langCode]?.Manage?.EmployeeInfo?.IdentityNumber"
                                validate="true"
                                type="text"
                                checkNumber
                                v-tooltip.absoluteTop="MISAResource[resource.langCode]?.Tooltip?.IdentityNumber"
                            >
                            </MISAInput>
                        </MISACol>
                        <MISACol gutter="4" span="10">
                            <MISADatePicker
                                :label="MISAResource[resource.langCode]?.Manage?.EmployeeInfo?.IdentityDate"
                                :dateTime="formData.identityDate"
                                v-model="formData.identityDate"
                                position="right"
                            >
                            </MISADatePicker>
                        </MISACol>
                    </MISARow>
                </MISACol>
            </MISARow>
            <MISARow gutter="12" justify="space-between">
                <MISACol gutter="12" span="12">
                    <MISADropdown
                        @next-tab-enter="iIdentityPlace.autoFocus()"
                        :options="positions"
                        v-model:value="formData.positionId"
                        :placeholder="MISAResource[resource.langCode]?.Manage?.EmployeeInfo?.Position?.Empty"
                        :label="MISAResource[resource.langCode]?.Manage?.EmployeeInfo?.Position?.Name"
                    ></MISADropdown>
                </MISACol>
                <MISACol gutter="12" span="12">
                    <MISAInput
                        @input-validation="validateIdentityPlace"
                        ref="iIdentityPlace"
                        :errorMessage="validateForm.identityPlace"
                        v-model:value="formData.identityPlace"
                        name="identityplace"
                        :label="MISAResource[resource.langCode]?.Manage?.EmployeeInfo?.IdentityPlace"
                        validate="true"
                        isName
                    >
                    </MISAInput>
                </MISACol>
            </MISARow>
            <MISARow justify="space-between">
                <MISACol span="24">
                    <MISARow justify="space-between">
                        <MISACol span="24">
                            <MISAInput
                                @input-validation="validateAddress"
                                ref="iAddress"
                                :errorMessage="validateForm.address"
                                v-model:value="formData.address"
                                name="address"
                                :label="MISAResource[resource.langCode]?.Manage?.EmployeeInfo?.Address"
                                validate="true"
                            >
                            </MISAInput>
                        </MISACol>
                    </MISARow>
                </MISACol>
            </MISARow>
            <MISARow gutter="4" justify="flex-start">
                <MISACol gutter="4" span="6">
                    <MISAInput
                        v-model:value="formData.phoneNumber"
                        @input-validation="validatePhoneNumber"
                        ref="iPhoneNumber"
                        :errorMessage="validateForm.phoneNumber"
                        :label="MISAResource[resource.langCode]?.Manage?.EmployeeInfo?.PhoneNumberForm"
                        name="phone"
                        type="text"
                        validate="true"
                        checkNumber
                    ></MISAInput>
                </MISACol>
                <MISACol gutter="4" span="6">
                    <MISAInput
                        v-model:value="formData.landlinePhone"
                        @input-validation="validateLandlinePhone"
                        ref="iLandLinePhone"
                        :errorMessage="validateForm.landlinePhone"
                        :label="MISAResource[resource.langCode]?.Manage?.EmployeeInfo?.LandlinePhone"
                        name="phone"
                        type="text"
                        validate="true"
                        checkNumber
                    ></MISAInput>
                </MISACol>
                <MISACol gutter="4" span="6">
                    <MISAInput
                        ref="iEmail"
                        @blur="validateEmail"
                        :errorMessage="validateForm.email"
                        v-model:value="formData.email"
                        :label="MISAResource[resource.langCode]?.Manage?.EmployeeInfo?.Email"
                        name="phone"
                        type="text"
                        validate="true"
                    ></MISAInput>
                </MISACol>
            </MISARow>
            <MISARow gutter="4" justify="flex-start">
                <MISACol gutter="4" span="6">
                    <MISAInput
                        v-model:value="formData.bankAccount"
                        @input-validation="validateBankAccount"
                        :errorMessage="validateForm.bankAccount"
                        ref="iBankAccount"
                        :label="MISAResource[resource.langCode]?.Manage?.EmployeeInfo?.BankAccount"
                        name="bank"
                        type="text"
                        validate="true"
                        checkNumber
                    ></MISAInput>
                </MISACol>
                <MISACol gutter="4" span="6">
                    <MISAInput
                        @input-validation="validateBankName"
                        ref="iBankName"
                        :errorMessage="validateForm.bankName"
                        v-model:value="formData.bankName"
                        :label="MISAResource[resource.langCode]?.Manage?.EmployeeInfo?.BankName"
                        name="bankName"
                        type="text"
                        validate="true"
                    ></MISAInput>
                </MISACol>
                <MISACol gutter="4" span="6">
                    <MISAInput
                        @input-validation="validateBankBranch"
                        ref="iBankBranch"
                        :errorMessage="validateForm.bankBranch"
                        v-model:value="formData.bankBranch"
                        :label="MISAResource[resource.langCode]?.Manage?.EmployeeInfo?.BankBranch"
                        name="bankAddress"
                        type="text"
                        validate="true"
                        @keydown.tab="buttonCancel.autoFocus()"
                    ></MISAInput>
                </MISACol>
            </MISARow>
        </div>

        <template #action>
            <MISARow justify="space-between" colGap="8px">
                <MISACol>
                    <MISAButton
                        ref="buttonCancel"
                        @click="closeForm"
                        :type="Enum.ButtonType.Sec"
                        :action="MISAResource[resource.langCode]?.Button?.Cancel"
                        v-tooltip.absoluteTop="Enum.KeyboardShortcuts.Esc"
                        @keydown.tab="firstFocus.focus()"
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
                        <!-- @keydown.tab="buttonCancel.autoFocus()" -->
                        <MISAButton
                            ref="buttonSaveAdd"
                            @click="handleSubmitForm(true)"
                            :loading="buttonLoad.saveAdd"
                            :type="Enum.ButtonType.Pri"
                            :action="MISAResource[resource.langCode]?.Button?.SaveAdd"
                            v-tooltip.absoluteTop="Enum.KeyboardShortcuts.CtrlShiftS"
                        ></MISAButton>
                    </MISARow>
                </MISACol>
            </MISARow>
            <button class="focusFirst" @focus="buttonCancel.autoFocus()"></button>
        </template>

        <template #checkboxs>
            <button ref="firstFocus" class="focusFirst"></button>
            <MISACheckbox
                :valueCheckbox="formData.isCustomer"
                v-model:value="formData.isCustomer"
                :lable="MISAResource[resource.langCode]?.Manage?.EmployeeInfo?.Customer"
            >
            </MISACheckbox>
            <MISACheckbox
                :valueCheckbox="formData.isProvider"
                v-model:value="formData.isProvider"
                :lable="MISAResource[resource.langCode]?.Manage?.EmployeeInfo?.Provider"
            ></MISACheckbox>
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

.focusFirst {
    position: absolute;
}
</style>
