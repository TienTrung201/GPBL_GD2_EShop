<script setup>
import { onMounted, onUnmounted, ref, watch } from 'vue';
import Enum from '../../common/enum';
import { useDialog } from '../../stores/dialog';
import { useResource } from '../../stores/resource.js';
import MISAResource from '../../common/resource';
import { useInventory } from '../../stores/inventory';
import { useTitleHeader } from '../../stores/title-header';
import MISATableDetail from '../../components/base/table/MISATableDetail.vue';
import baseApi from '../../api/base-api';
import { convertCurrency, convertDataTable, convertToTitleCase, deepCopy } from '../../common/convert-data';
import { useRoute } from 'vue-router';
import { useToast } from '../../stores/toast';
const dialog = useDialog();
const route = useRoute();
const inventory = useInventory();
const inventoryId = ref(route.params.id); //Biến chứa Id trên param
const itemCategories = ref([]);
const units = ref([]);
const title = useTitleHeader();
const resource = useResource();
const formData = ref({}); //Biến lưu data
const couterChangeForm = ref(0); //Biến kiểm tra số lần form thay đổi
const isEditForm = ref(Enum.EditMode.None); // Biến này nhận biết form có bị thay đổi không
const formEditMode = ref(inventoryId.value ? Enum.EditMode.Update : Enum.EditMode.Add);
const dataDelete = ref([]); //Data Detail bị delete
const inputType = ref({ SKUCode: 1, Name: 2 }); //Biến chứa loại input để biết khi blur thì thay đổi giá trị nào

// const validateForm = ref({
//     inventoryName: '',
//     skuCode: '',
//     barcode: '',
//     costPrice: '',
//     unitPrice: '',
//     description: '',
// });
//Input-------------------
const iInventoryName = ref(null);
const iSKUCode = ref(null);
const iCostPrice = ref(null);
const iUnitPrice = ref(null);
const iDescription = ref(null);
const iIsShowMenu = ref(null);
const file = ref(null);
const imgFile = ref(null);
// const buttonCancel = ref(null);
// const firstFocus = ref(null);
// eslint-disable-next-line no-unused-vars
const props = defineProps({
    buttonLoad: { type: Object, default: () => ({ save: false, saveAdd: false }) },
});
const columnTable = ref([
    {
        title: 'Tên hàng hóa',
        key: 'InventoryName',
        width: '170',
        isShow: true,
        isEdit: true,
        // pin: true,
    },
    {
        title: 'Đơn vị tính',
        key: 'UnitName',
        width: '170',
        isShow: true,
        isEdit: false,
        // pin: true,
    },
    {
        title: 'SKUCode',
        key: 'SKUCode',
        width: '170',
        isShow: true,
        isEdit: false,
        // type: 'gender',
    },
    {
        title: 'SKUCodeCustom',
        key: 'SKUCodeCustom',
        width: '170',
        isShow: true,
        isEdit: true,
        // type: 'gender',
    },
    {
        title: 'Mã vạch',
        key: 'Barcode',
        width: '170',
        isShow: true,
        isEdit: true,
    },
    {
        title: 'Giá mua',
        key: 'CostPrice',
        width: '170',
        isShow: true,
        align: Enum.AlignColumn.Right,
        type: Enum.TypeDataTable.Money,
        isEdit: true,
    },
    {
        title: 'Giá bán',
        key: 'UnitPrice',
        width: '170',
        isShow: true,
        align: Enum.AlignColumn.Right,
        type: Enum.TypeDataTable.Money,
        isEdit: true,

        // align: Enum.AlignColumn.Right,
    },
]);
const dataTable = ref([]);
const properties = ref({ color: [], size: [] });
const genDataTable = ref([]);
/*
 **
 * Author: Tiến Trung (19/08/2023)
 * Description: hàm cập nhật thuộc tính cho hàng hóa
 */
const updateProperties = () => {
    //Lấy thuộc tính
    properties.value = { color: [], size: [] };
    const colorData = dataTable.value.map((data) => {
        return {
            color: data.Color,
            colorCode: data.ColorCode,
        };
    });
    //Bỏ thuộc tính bị lặp
    colorData.forEach((item) => {
        const foundIndex = properties.value.color.findIndex(
            (currentItem) => currentItem.color === item.color && currentItem.colorCode === item.colorCode,
        );
        if (foundIndex === -1) {
            if (item.color) {
                properties.value.color.push(item);
            }
        }
    });
    const sizeData = dataTable.value.map((data) => {
        return {
            size: data.Size,
            sizeCode: data.SizeCode,
        };
    });
    sizeData.forEach((item) => {
        const foundIndex = properties.value.size.findIndex(
            (currentItem) => currentItem.size === item.size && currentItem.sizeCode === item.sizeCode,
        );
        if (foundIndex === -1) {
            if (item.size) {
                properties.value.size.push(item);
            }
        }
    });
};
const getPropertiesDetail = (color, size) => {
    const sizeDetail = size ? size.size : '';
    const colorDetail = color ? color.color : '';
    const sizeCodeDetail = size ? size.sizeCode : '';
    const colorCodeDetail = color ? color.colorCode : '';
    const SKUCode = formData.value.SKUCode ? formData.value.SKUCode : '';
    const SKUCodeDetail = `${SKUCode}${colorCodeDetail ? '-' + colorCodeDetail : ''}${
        sizeCodeDetail ? '-' + sizeCodeDetail : ''
    }`;
    let nameDetail = formData.value.inventoryName ? formData.value.inventoryName : '';
    if (sizeDetail && colorDetail) {
        nameDetail = `${nameDetail} (${colorDetail}/${sizeDetail})`;
    }
    if (colorDetail && sizeDetail === '') {
        nameDetail = `${nameDetail} (${colorDetail})`;
    }
    if (sizeDetail && colorDetail === '') {
        nameDetail = `${nameDetail} (${sizeDetail})`;
    }
    return {
        sizeDetail,
        colorDetail,
        sizeCodeDetail,
        colorCodeDetail,
        SKUCodeDetail,
        nameDetail,
    };
};
/*
 **
 * Author: Tiến Trung (19/08/2023)
 * Description: hàm thêm data vào bảng tự động tạo bảng detail
 */
const pushDataTable = (color, size) => {
    const propertiesDetail = getPropertiesDetail(color, size);
    genDataTable.value.push({
        InventoryName: propertiesDetail.nameDetail ? propertiesDetail.nameDetail : '',
        UnitName: formData.value.unitName ? formData.value.unitName : null,
        SKUCode: propertiesDetail.SKUCodeDetail,
        SKUCodeCustom: propertiesDetail.SKUCodeDetail,
        Color: propertiesDetail.colorDetail,
        ColorCode: propertiesDetail.colorCodeDetail,
        Size: propertiesDetail.sizeDetail,
        SizeCode: propertiesDetail.sizeCodeDetail,
        Barcode: '',
        CostPrice: '0',
        UnitPrice: '0',
        IsActive: true,
        IsShowMenu: true,
        ParentId: formData.value.inventoryId,
    });
};
/*
 **
 * Author: Tiến Trung (19/08/2023)
 * Description: hàm update thuộc tính và tự động tạo bảng detail
 */
const autoDataTable = () => {
    genDataTable.value = [];
    if (properties.value.color.length === 0) {
        properties.value.size.forEach((size) => {
            pushDataTable(null, size);
        });
    }
    if (properties.value.color.length > 0 && properties.value.size.length === 0) {
        properties.value.color.forEach((color) => {
            pushDataTable(color, null);
        });
    }
    if (properties.value.color.length > 0 && properties.value.size.length > 0) {
        properties.value.color.forEach((color) => {
            properties.value.size.forEach((size) => {
                pushDataTable(color, size);
            });
        });
    }
    //Tự động tạo bảng
    let isDeleteAllData = true;
    const dataGenAuto = [];
    genDataTable.value.forEach((dataGen) => {
        const dataOld = dataTable.value.find((dataOld) => dataOld.SKUCode === dataGen.SKUCode);
        if (dataOld) {
            isDeleteAllData = false;
            if (dataOld.EditMode === Enum.EditMode.None) {
                dataOld.InventoryName = dataGen.InventoryName;
            }
            dataGenAuto.push(dataOld);
        } else {
            dataGen.EditMode = Enum.EditMode.Add;
            dataGenAuto.push(dataGen);
        }
    });
    //Kiểm tra xem data có bị xóa hết không nếu xóa hết thì add vào mảng data xóa
    if (isDeleteAllData) {
        //Chỉ return ra data cũ có ID thì sẽ không dính vào data mới
        const findDataDelete = dataTable.value.filter((data) => {
            data.EditMode = Enum.EditMode.Delete;
            return data.InventoryId;
        });
        dataDelete.value.push(...findDataDelete);
    }
    dataTable.value = dataGenAuto;
};
/*
 **
 * Author: Tiến Trung (26/08/2023)
 * Description: Hàm xử lý khi xóa tag thì xóa tất cả data có tag đấy
 */
const handleRemoveTag = (index, type, tagCode) => {
    const findDataDelete = dataTable.value.filter((data) => {
        return data[convertToTitleCase(type) + 'Code'] === tagCode && data.InventoryId;
    });
    dataDelete.value.push(...findDataDelete);
    findDataDelete.forEach((dataDelete) => {
        dataDelete.EditMode = Enum.EditMode.Delete;
        dataTable.value = dataTable.value.filter((data) => data.SKUCode !== dataDelete.SKUCode);
    });
    properties.value[type].splice(index, 1);
    autoDataTable();
};
/*
 **
 * Author: Tiến Trung (19/08/2023)
 * Description: hàm khi blur tag thì update
 */
const onBlurInputFormUpdateData = (value, nameForm) => {
    // autoDataTable();
    if (isEditForm.value === Enum.EditMode.None) {
        return;
    }
    dataTable.value.forEach((data) => {
        const SKUCodeDetail = `${formData.value.SKUCode}${data.ColorCode ? '-' + data.ColorCode : ''}${
            data.SizeCode ? '-' + data.SizeCode : ''
        }`;
        let nameDetail = formData.value.inventoryName;
        if (data.Size && data.Color) {
            nameDetail = `${nameDetail} (${data.Color}/${data.Size})`;
        }
        if (data.Color && data.Size === '') {
            nameDetail = `${data.Size} (${data.Color})`;
        }
        if (data.Size && data.Color === '') {
            nameDetail = `${nameDetail} (${data.Size})`;
        }
        switch (nameForm) {
            case inputType.value.Name:
                data.InventoryName = nameDetail;
                break;
            case inputType.value.SKUCode:
                data.SKUCode = SKUCodeDetail;
                data.SKUCodeCustom = SKUCodeDetail;
                break;
            default:
                break;
        }
    });
    //Chỉ cần lặp qua tất cả data rồi update lại mã là xong
    //Cập nhật lại hết detail thành update
    //Nếu data mới gen check editmode create không dc update
};
/*
 **
 * Author: Tiến Trung (19/08/2023)
 * Description: hàm nhận sự kiện emit xóa detail và cập nhật lại thuộc tính
 */
const deleteDetail = (index) => {
    try {
        if (dataTable.value[index].InventoryId) {
            dataTable.value[index].EditMode = Enum.EditMode.Delete;
            dataDelete.value.push(dataTable.value[index]);
        }

        dataTable.value.splice(index, 1);
        updateProperties();
    } catch (error) {
        console.log(error);
    }
};

/*
 **
 * Author: Tiến Trung (19/08/2023)
 * Description: hàm lấy đường dẫn ảnh
 */
const handleChangeImg = (image) => {
    imgFile.value = URL.createObjectURL(image);
    formData.value.image = imgFile.value;
};
/*
 **
 * Author: Tiến Trung (27/06/2023)
 * Description: hàm đóng modal
 * nếu thay đổi dữ liệu mà đóng modal thì bật dialog
 */
function closeForm() {
    if (isEditForm.value === Enum.EditMode.Update) {
        dialog.setObjectData(formData.value);
        // dialog.setMethod(modalForm.method);
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
        // modalForm.close();
        inventory.closeForm();
    }
}
/*
 **
 * Author: Tiến Trung (27/06/2023)
 * Description: bắt lỗi focus vào ô đầu tiên
 */
// const autoFocusForm = () => {
//     try {
//         //Nếu là lỗi trùng mã chủ động cho input bị lỗi để báo
//         if (dialog.errorCode === Enum.ErorCode.DuplicateCode) {
//             dialog.setErrorCode(0);
//             validateForm.value.skuCode = MISAResource[resource.langCode].EmployeeInvalidError.EmployeeDuplicateCode;
//             iSKUCode.value.autoFocus();
//             return;
//         }
//         if (validateForm.value.inventoryName) {
//             iInventoryName.value.autoFocus();
//             return;
//         }
//         if (validateForm.value.skuCode) {
//             iSKUCode.value.autoFocus();
//             return;
//         }
//         if (validateForm.value.costPrice) {
//             iCostPrice.value.autoFocus();
//             return;
//         }
//         if (validateForm.value.unitPrice) {
//             iUnitPrice.value.autoFocus();
//             return;
//         }
//     } catch (error) {
//         console.log(error);
//     }
// };
const getDetail = async () => {
    if (formEditMode.value === Enum.EditMode.Update) {
        baseApi.path = Enum.Router.Inventory.Api;
        baseApi.method = Enum.ApiMethod.GET;
        const response = await baseApi.request(inventoryId.value);
        console.log(response);
        return response.data;
    } else {
        return {};
    }
};
/**
 * Author: Tiến Trung (05/07/2023)
 * Description: Hàm update form nếu form mới thì call api lấy newCode
 */
const updateForm = async () => {
    const newCode = 'CODE'; // await getNewEmployeeCode();
    const data = await getDetail();
    let isActive = 1;
    if (data.IsActive === undefined) {
        isActive = 1;
    } else {
        isActive = data.IsActive ? 1 : 0;
    }
    couterChangeForm.value = 0;
    formData.value = {
        inventoryName: data.InventoryName,
        itemCategoryId: data.ItemCategoryId,
        SKUCode: data.SKUCode,
        SKUCodeCustom: data.SKUCodeCustom,
        costPrice: data.CostPrice ? convertCurrency(data.CostPrice) : '0',
        unitPrice: data.UnitPrice ? convertCurrency(data.UnitPrice) : '0',
        unitId: data.UnitId,
        unitName: data.UnitName,
        description: data.Description,
        image: data.Image,
        isActive: isActive,
        isShowMenu: data.IsShowMenu ? true : false,
        editMode: formEditMode.value,
        inventoryId: data.InventoryId,
    };
    dataTable.value = convertDataTable(data.Detail ? data.Detail : [], columnTable.value, resource.langCode) || [];
    updateProperties();
    iInventoryName.value.autoFocus();
};
/**
 * Author: Tiến Trung (26/08/2023)
 * Description: Hàm save data gọi api
 */
const toast = useToast();
const saveData = async (data) => {
    baseApi.path = Enum.Router.Inventory.Api;
    baseApi.data = data;
    baseApi.method = Enum.ApiMethod.POST;
    const res = await baseApi.request('SaveData');
    // const res = await employeeApi.create(data);
    console.log(res);
    toast.success(MISAResource[resource.langCode]?.Toast?.Success?.Add);
};
/**
 * Author: Tiến Trung (26/08/2023)
 * Description: Hàm gửi form
 */
const submitForm = async () => {
    try {
        //vadidate nhập
        const copyObj = deepCopy(formData.value);
        const dataCreateUpdate = dataTable.value.map((data) => {
            const costPrice = Number(data.CostPrice?.replace('.', ''));
            const unitPrice = Number(data.CostPrice?.replace('.', ''));
            return {
                ...data,
                CostPrice: costPrice,
                UnitPrice: unitPrice,
                UnitId: formData.value.unitId,
                ItemCategoryId: formData.value.itemCategoryId,
            };
        });
        copyObj.detail = [...dataCreateUpdate, ...dataDelete.value];
        copyObj.isActive = copyObj.isActive ? true : false;
        copyObj.costPrice = Number(copyObj.costPrice?.replace('.', ''));
        copyObj.unitPrice = Number(copyObj.unitPrice?.replace('.', ''));
        await saveData(copyObj);
    } catch (error) {
        console.log(error);
    }
};

/**
 * Author: Tiến Trung (26/08/2023)
 * Description: hàm lấy nhóm hàng hóa
 */
const getItemCategory = async () => {
    baseApi.method = Enum.ApiMethod.GET;
    baseApi.path = Enum.Router.ItemCategory.Api;
    const response = await baseApi.request();
    itemCategories.value = response.data.map((data) => {
        return {
            option: data.ItemCategoryName,
            value: data.ItemCategoryId,
        };
    });
};
/**
 * Author: Tiến Trung (26/08/2023)
 * Description: hàm lấy đơn vị tính
 */
const getUnit = async () => {
    baseApi.method = Enum.ApiMethod.GET;
    baseApi.path = Enum.Router.Unit.Api;
    const response = await baseApi.request();
    console.log(response);
    units.value = response.data.map((data) => {
        return {
            option: data.UnitName,
            value: data.UnitId,
        };
    });
};
/**
 * Author: Tiến Trung (2/07/2023)
 * Description: khi component được tạo thì get data
 * mỗi lần cất và thêm thì lại tạo form mới
 */
onMounted(async () => {
    try {
        await getItemCategory();
        await getUnit();
        updateForm();
        title.setTitle(
            MISAResource[resource.langCode]?.SideBar?.Inventory,
            title.editMode === Enum.EditMode.Add
                ? MISAResource[resource.langCode]?.FormTitle?.Inventory?.Add
                : MISAResource[resource.langCode]?.FormTitle?.Inventory?.Update,
        );
    } catch (e) {
        console.log(e);
    }
});
/**
 * Author: Tiến Trung (26/08/2023)
 * Description: khi component hủy thì xóa sự kiện phím
 */
onUnmounted(() => {
    title.setTitle(MISAResource[resource.langCode]?.SideBar?.Inventory);
});
/**
 * Author: Tiến Trung (26/08/2023)
 * Description: theo dõi khi form thay đổi
 */
watch(
    () => formData.value,
    () => {
        if (couterChangeForm.value !== 0) {
            switch (formEditMode.value) {
                case Enum.EditMode.Update:
                    formData.value.editMode = Enum.EditMode.Update;
                    break;
                case Enum.EditMode.Add:
                    formData.value.editMode = Enum.EditMode.Add;
                    break;
                default:
                    break;
            }
            isEditForm.value = Enum.EditMode.Update;
            couterChangeForm.value++;
        } else {
            couterChangeForm.value++;
        }
    },
    { deep: true },
);
</script>
<template>
    <div class="ntt-form">
        <div class="ntt-form__body">
            <div class="ntt-form__header">
                <h3 class="form__title">THÔNG TIN CƠ BẢN</h3>
            </div>
            <div class="wrapper-form">
                <MISARow justify="flex-start">
                    <MISACol display="flex" direction="column" rowGap="12">
                        <MISARow>
                            <MISARadioGroup :label="'Trạng thái kinh doanh'" row>
                                <MISARadio
                                    v-model:value="formData.isActive"
                                    :valueRadio="1"
                                    :optionName="'Đang kinh doanh'"
                                    name="active"
                                ></MISARadio>
                                <MISARadio
                                    v-model:value="formData.isActive"
                                    :valueRadio="0"
                                    :optionName="'Ngừng kinh doanh'"
                                    name="active"
                                ></MISARadio>
                            </MISARadioGroup>
                        </MISARow>
                        <MISARow>
                            <MISAInput
                                v-model:value="formData.inventoryName"
                                ref="iInventoryName"
                                :label="'Tên hàng hóa'"
                                type="text"
                                validate="true"
                                row
                                require
                                @blur="(value) => onBlurInputFormUpdateData(value, inputType.Name)"
                            ></MISAInput>
                        </MISARow>
                        <MISARow>
                            <MISADropdown
                                @next-tab-enter="iSKUCode.autoFocus()"
                                combobox
                                :options="itemCategories"
                                v-model:value="formData.itemCategoryId"
                                :placeholder="MISAResource[resource.langCode]?.Manage?.EmployeeInfo?.Department?.Empty"
                                :label="'Nhóm hàng hóa'"
                                row
                            ></MISADropdown>
                        </MISARow>
                        <MISARow>
                            <MISAInput
                                v-model:value="formData.SKUCode"
                                ref="iSKUCode"
                                :label="'Mã SKU'"
                                type="text"
                                validate="true"
                                row
                                @blur="(value) => onBlurInputFormUpdateData(value, inputType.SKUCode)"
                            ></MISAInput>
                        </MISARow>
                        <MISARow>
                            <MISAInput
                                v-model:value="formData.costPrice"
                                ref="iCostPrice"
                                :label="'Giá mua'"
                                type="text"
                                validate="true"
                                row
                                money
                            ></MISAInput>
                        </MISARow>
                        <MISARow>
                            <MISAInput
                                v-model:value="formData.unitPrice"
                                ref="iUnitPrice"
                                :label="'Giá Bán'"
                                type="text"
                                validate="true"
                                row
                                money
                            ></MISAInput>
                        </MISARow>
                        <MISARow>
                            <MISADropdown
                                @next-tab-enter="iIsShowMenu.autoFocus()"
                                combobox
                                :options="units"
                                v-model:value="formData.unitId"
                                :placeholder="MISAResource[resource.langCode]?.Manage?.EmployeeInfo?.Department?.Empty"
                                :label="'Đơn vị tính'"
                                row
                            ></MISADropdown>
                        </MISARow>
                        <MISARow></MISARow>
                        <MISARow>
                            <MISACheckbox
                                custom
                                :valueCheckbox="formData.isShowMenu"
                                v-model:value="formData.isShowMenu"
                                :lable="'Hiển thị trên màn hình bán hàng'"
                            >
                            </MISACheckbox>
                        </MISARow>
                    </MISACol>
                </MISARow>
            </div>
            <div sty class="ntt-form__header" :style="{ marginTop: '30px' }">
                <h3 class="form__title">THÔNG TIN THUỘC TÍNH</h3>
            </div>
            <div class="wrapper-form">
                <MISAInputManyTag
                    @update-tag="autoDataTable"
                    @remove-tag="handleRemoveTag"
                    :properties="properties.color"
                    :type="Enum.TypeProperties.Color"
                    label="Màu sắc"
                ></MISAInputManyTag>
                <MISAInputManyTag
                    @update-tag="autoDataTable"
                    @remove-tag="handleRemoveTag"
                    :properties="properties.size"
                    :type="Enum.TypeProperties.Size"
                    label="Size"
                ></MISAInputManyTag>
            </div>
            <div v-if="dataTable.length" class="form-table">
                <MISATableDetail @delete-detail="deleteDetail" :columns="columnTable" :dataTable="dataTable">
                </MISATableDetail>
            </div>
            <div sty class="ntt-form__header" :style="{ marginTop: '10px' }">
                <h3 class="form__title">THÔNG TIN BỔ SUNG</h3>
            </div>
            <div class="wrapper-form">
                <MISARow>
                    <MISACol display="flex" direction="column" rowGap="12">
                        <MISARow>
                            <MISATextArea
                                ref="iDescription"
                                isName
                                v-model:value="formData.description"
                                :label="'Mô tả'"
                                validate="true"
                                row
                                class="custom-label"
                            >
                            </MISATextArea>
                        </MISARow>
                        <MISARow>
                            <div class="form-image">
                                <div class="wrapper__label-image">
                                    <p class="label">Ảnh hàng hóa</p>
                                    <div class="wrapper-image">
                                        <input
                                            @change="handleChangeImg($event.target.files[0])"
                                            ref="file"
                                            type="file"
                                            name="inventory"
                                            id=""
                                        />
                                        <img class="image-file" v-if="imgFile" :src="imgFile" alt="" />

                                        <img
                                            v-else
                                            class="image-file"
                                            src="../../assets/img/img-inventory-default.jpg"
                                            alt=""
                                        />
                                        <button @click="file.click()" class="btn-file">...</button>
                                        <button class="btn-select__list-image">
                                            <p class="icon center">
                                                <MISAIcon width="22" height="12" icon="edit" />
                                            </p>
                                            <p>Biểu tượng</p>
                                        </button>
                                    </div>
                                </div>
                                <div class="wrapper__content">
                                    <p class="image__content">- Lựa chọn biểu tượng để thay thế nếu không có ảnh</p>
                                    <p class="image__content">
                                        - Định dạng ảnh (.jpg, .jpeg, .png, .gif) và dung lượng {{ '>' }} 5MB
                                    </p>
                                </div>
                            </div>
                        </MISARow>
                    </MISACol>
                </MISARow>
            </div>
        </div>

        <div class="ntt-form__footer">
            <MISAButton
                @click="submitForm"
                :type="Enum.ButtonType.IconPri"
                :action="MISAResource[resource.langCode]?.Button?.Save"
            >
                <template #icon>
                    <MISAIcon width="21" height="11" icon="save" />
                </template>
            </MISAButton>
            <MISAButton
                :type="Enum.ButtonType.IconPri"
                :action="MISAResource[resource.langCode]?.Button?.SaveAndReplication"
                sec
            >
                <template #icon>
                    <MISAIcon width="25" height="15" icon="replication" />
                </template>
            </MISAButton>
            <MISAButton :type="Enum.ButtonType.IconPri" :action="MISAResource[resource.langCode]?.Button?.SaveAdd" sec>
                <template #icon>
                    <MISAIcon width="21" height="10" icon="plus" />
                </template>
            </MISAButton>
            <MISAButton
                @click="closeForm"
                :type="Enum.ButtonType.IconPri"
                :action="MISAResource[resource.langCode]?.Button?.Cancel"
                link
            >
                <template #icon>
                    <MISAIcon width="20" height="10" icon="close-eshop" />
                </template>
            </MISAButton>
        </div>
    </div>
</template>

<style lang="scss">
@import './inventory-form.scss';

.custom-checkbox {
    margin-left: 10px;
}

.custom-label {
    .wrapper-input {
        max-width: 500px;
    }
}
</style>
