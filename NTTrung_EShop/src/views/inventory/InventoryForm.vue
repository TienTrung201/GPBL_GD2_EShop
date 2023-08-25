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
import { convertDataTable, convertToTitleCase } from '../../common/convert-data';

const dialog = useDialog();
const inventory = useInventory();
const title = useTitleHeader();
const resource = useResource();
const formData = ref({});
const departments = ref([]);
const couterChangeForm = ref(0);
const editModeForm = ref(Enum.EditMode.None);
const dataDelete = ref([]);
const isDeleteAll = ref(false);
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
const data = [
    {
        InventoryName: 'haha',
        UnitName: 'Chiếc',
        SKUCode: 'S-XA-S',
        Color: 'Xanh',
        ColorCode: 'XA',
        Size: 'S',
        SizeCode: 'S',
        Barcode: '100001',
        CostPrice: 200000,
        UnitPrice: 300000,
    },
    {
        InventoryName: 'haha',
        UnitName: 'Chiếc',
        SKUCode: 'S-XA-M',
        Color: 'Xanh',
        ColorCode: 'XA',
        Size: 'M',
        SizeCode: 'M',
        Barcode: '100001',
        CostPrice: 200000,
        UnitPrice: 300000,
    },
    {
        InventoryName: 'haha',
        UnitName: 'Chiếc',
        SKUCode: 'S-DO-S',
        Color: 'Đỏ',
        ColorCode: 'DO',
        Size: 'S',
        SizeCode: 'S',
        Barcode: '100001',
        CostPrice: 200000,
        UnitPrice: 300000,
    },
    {
        InventoryName: 'haha',
        UnitName: 'Chiếc',
        SKUCode: 'S-DO-M',
        Color: 'Đỏ',
        ColorCode: 'DO',
        Size: 'M',
        SizeCode: 'M',
        Barcode: '100001',
        CostPrice: 200000,
        UnitPrice: 300000,
    },
];
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

/*
 **
 * Author: Tiến Trung (19/08/2023)
 * Description: hàm thêm data vào bảng tự động tạo bảng detail
 */
const pushDataTable = (color, size) => {
    const sizeDetail = size ? size.size : '';
    const colorDetail = color ? color.color : '';
    const sizeCodeDetail = size ? size.sizeCode : '';
    const colorCodeDetail = color ? color.colorCode : '';
    let nameDetail = formData.value.inventoryName;
    if (sizeDetail && colorDetail) {
        nameDetail = `${nameDetail} (${colorDetail}/${sizeDetail})`;
    }
    if (colorDetail && sizeDetail === '') {
        nameDetail = `${nameDetail} (${colorDetail})`;
    }
    if (sizeDetail && colorDetail === '') {
        nameDetail = `${nameDetail} (${sizeDetail})`;
    }
    genDataTable.value.push({
        InventoryName: nameDetail,
        UnitName: formData.value.unitName,
        SKUCode: `${formData.value.SKUCode}${colorCodeDetail ? '-' + colorCodeDetail : ''}${
            sizeCodeDetail ? '-' + sizeCodeDetail : ''
        }`,
        SKUCodeCustom: `${formData.value.SKUCode}${colorCodeDetail ? '-' + colorCodeDetail : ''}${
            sizeCodeDetail ? '-' + sizeCodeDetail : ''
        }`,
        Color: colorDetail,
        ColorCode: colorCodeDetail,
        Size: sizeDetail,
        SizeCode: sizeCodeDetail,
        Barcode: '',
        CostPrice: '',
        UnitPrice: '',
    });
};
/*
 **
 * Author: Tiến Trung (19/08/2023)
 * Description: hàm update thuộc tính và tự động tạo bảng detail
 */
const updateTag = () => {
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
    genDataTable.value.forEach((dataGen, index) => {
        const dataOld = dataTable.value.find((dataOld) => dataOld.SKUCode === dataGen.SKUCode);
        if (dataOld) {
            isDeleteAllData = false;
            // dataOld.InventoryName = dataGen.InventoryName;
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
    updateTag();
};
/*
 **
 * Author: Tiến Trung (19/08/2023)
 * Description: hàm khi blur tag thì update
 */
const onBlurInputFormUpdateData = (propertyName, SKUCode) => {
    updateTag();
    //Chỉ cần lặp qua tất cả data rồi update lại mã là xong
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
    if (editModeForm.value === Enum.EditMode.Update) {
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
    if (inventory.editMode === Enum.EditMode.Update) {
        baseApi.path = Enum.Router.Inventory.Api;
        baseApi.method = Enum.ApiMethod.GET;
        const response = await baseApi.request(inventory.uid);
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
        costPrice: data.CostPrice,
        unitPrice: data.UnitPrice,
        unitId: data.UnitId,
        unitName: data.UnitName,
        description: data.Description,
        image: data.Image,
        isActive: isActive,
        isShowMenu: data.IsShowMenu,
        editMode: Enum.EditMode.None,
    };
    dataTable.value = convertDataTable(data.Detail ? data.Detail : [], columnTable.value, resource.langCode) || [];
    updateProperties();
    iInventoryName.value.autoFocus();
};
const submitForm = () => {
    const dataDetail = [...dataTable.value, ...dataDelete.value];
    formData.value.detail = dataDetail;
    console.log(formData.value);
};

watch(
    () => formData.value,
    () => {
        if (couterChangeForm.value !== 0) {
            editModeForm.value = Enum.EditMode.Update;
            couterChangeForm.value++;
        } else {
            couterChangeForm.value++;
        }
    },
    { deep: true },
);
/**
 * Author: Tiến Trung (2/07/2023)
 * Description: khi component được tạo thì get data
 * mỗi lần cất và thêm thì lại tạo form mới
 */

onMounted(() => {
    try {
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
 * Author: Tiến Trung (2/07/2023)
 * Description: khi component hủy thì xóa sự kiện phím
 */
onUnmounted(() => {
    title.setTitle(MISAResource[resource.langCode]?.SideBar?.Inventory);
});
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
                                @blur="onBlurInputFormUpdateData"
                            ></MISAInput>
                        </MISARow>
                        <MISARow>
                            <!-- <MISADropdown
                                @next-tab-enter="iSKUCode.autoFocus()"
                                combobox
                                v-model:value="formData.itemCategoryId"
                                :placeholder="MISAResource[resource.langCode]?.Manage?.EmployeeInfo?.Department?.Empty"
                                :label="'Nhóm hàng hóa'"
                                row
                            ></MISADropdown> -->
                        </MISARow>
                        <MISARow>
                            <MISAInput
                                v-model:value="formData.SKUCode"
                                ref="iSKUCode"
                                :label="'Mã SKU'"
                                type="text"
                                validate="true"
                                row
                                @blur="onBlurInputFormUpdateData"
                            ></MISAInput>
                        </MISARow>
                        <MISARow>
                            <MISAInput
                                v-model:value="formData.costPrice"
                                ref="iCostPrice"
                                :label="'Giá mua'"
                                type="text"
                                validate="true"
                                checkNumber
                                row
                            ></MISAInput>
                        </MISARow>
                        <MISARow>
                            <MISAInput
                                v-model:value="formData.unitPrice"
                                ref="iUnitPrice"
                                :label="'Giá Bán'"
                                type="text"
                                validate="true"
                                checkNumber
                                row
                            ></MISAInput>
                        </MISARow>
                        <MISARow>
                            <!-- <MISADropdown
                                @next-tab-enter="iIsShowMenu.autoFocus()"
                                combobox
                                :options="departments"
                                v-model:value="formData.departmentId"
                                :placeholder="MISAResource[resource.langCode]?.Manage?.EmployeeInfo?.Department?.Empty"
                                :label="'Đơn vị tính'"
                                row
                            ></MISADropdown> -->
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
                    @update-tag="updateTag"
                    @remove-tag="handleRemoveTag"
                    :properties="properties.color"
                    :type="Enum.TypeProperties.Color"
                    label="Màu sắc"
                ></MISAInputManyTag>
                <MISAInputManyTag
                    @update-tag="updateTag"
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
