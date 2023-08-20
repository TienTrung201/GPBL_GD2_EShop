<script setup>
import { onMounted, onUnmounted, ref } from 'vue';
import Enum from '../../common/enum';
import { useDialog } from '../../stores/dialog';
import { useResource } from '../../stores/resource.js';
import MISAResource from '../../common/resource';
import { useInventory } from '../../stores/inventory';
import { useTitleHeader } from '../../stores/title-header';
import MISATableDetail from '../../components/base/table/MISATableDetail.vue';

const dialog = useDialog();
const inventory = useInventory();
const title = useTitleHeader();
const resource = useResource();
const formData = ref({});
const departments = ref([]);
const couterChangeForm = ref(0);
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
        // pin: true,
    },
    {
        title: 'Đơn vị tính',
        key: 'UnitName',
        width: '170',
        isShow: true,
        // pin: true,
    },
    {
        title: 'SKUCode',
        key: 'SKUCode',
        width: '170',
        isShow: true,
        // type: 'gender',
    },
    {
        title: 'Mã vạch',
        key: 'Barcode',
        width: '170',
        isShow: true,
    },
    {
        title: 'Giá mua',
        key: 'CostPrice',
        width: '170',
        isShow: true,
        align: Enum.AlignColumn.Right,
    },
    {
        title: 'Giá bán',
        key: 'UnitPrice',
        width: '170',
        isShow: true,
        align: Enum.AlignColumn.Right,

        // align: Enum.AlignColumn.Right,
    },
]);
const dataTable = ref([]);
const properties = ref({ color: [], size: [] });
const data = [
    {
        InventoryName: 'haha',
        UnitName: 'Nhóm hh',
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
        UnitName: 'Nhóm hh',
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
        UnitName: 'Nhóm hh',
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
        UnitName: 'Nhóm hh',
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
dataTable.value = data;
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
            properties.value.color.push(item);
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
            properties.value.size.push(item);
        }
    });
};
updateProperties();
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
    dataTable.value = genDataTable.value.map((dataGen) => {
        const dataOld = dataTable.value.find((dataOld) => dataOld.SKUCode === dataGen.SKUCode);
        if (dataOld) {
            return dataOld;
        } else {
            return dataGen;
        }
    });
};
/*
 **
 * Author: Tiến Trung (19/08/2023)
 * Description: hàm nhận sự kiện emit xóa detail và cập nhật lại thuộc tính
 */
const deleteDetail = (index) => {
    dataTable.value.splice(index, 1);
    updateProperties();
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
    let nameDetail = 'name';
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
        UnitName: 'Chai',
        SKUCode: `S${colorCodeDetail ? '-' + colorCodeDetail : ''}${sizeCodeDetail ? '-' + sizeCodeDetail : ''}`,
        Color: colorDetail,
        ColorCode: colorCodeDetail,
        Size: sizeDetail,
        SizeCode: sizeCodeDetail,
        Barcode: 'test',
        CostPrice: '',
        UnitPrice: '',
    });
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
    if (couterChangeForm.value > 1) {
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
/**
 * Author: Tiến Trung (05/07/2023)
 * Description: Hàm update form nếu form mới thì call api lấy newCode
 */
const updateForm = async () => {
    const newCode = 'CODE'; // await getNewEmployeeCode();
    const data = {}; //await getEmployee();

    couterChangeForm.value = 0;
    formData.value = {
        inventoryName: data.InventoryName,
        itemCategoryId: data.ItemCategoryId,
        SKUCode: newCode,
        costPrice: data.CostPrice,
        unitPrice: data.UnitPrice,
        unitId: data.UnitId,
        discription: data.Discription,
        image: data.Image,
        isActive: data.IsActive ? data.IsActive : 1,
        isShowMenu: data.IsShowMenu,
    };
    iInventoryName.value.autoFocus();
};
/**
 * Author: Tiến Trung (2/07/2023)
 * Description: khi component được tạo thì get data
 * mỗi lần cất và thêm thì lại tạo form mới
 */

onMounted(() => {
    updateForm();
    title.setTitle(
        MISAResource[resource.langCode]?.SideBar?.Inventory,
        title.editMode === Enum.EditMode.Add
            ? MISAResource[resource.langCode]?.FormTitle?.Inventory?.Add
            : MISAResource[resource.langCode]?.FormTitle?.Inventory?.Update,
    );
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
                <MISARow gutter="12" justify="flex-start">
                    <MISACol gutter="12" display="flex" direction="column" rowGap="12" span="11">
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
                            ></MISAInput> </MISARow
                        ><MISARow>
                            <MISADropdown
                                @next-tab-enter="iSKUCode.autoFocus()"
                                combobox
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
                            <MISADropdown
                                @next-tab-enter="iIsShowMenu.autoFocus()"
                                combobox
                                :options="departments"
                                v-model:value="formData.departmentId"
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
                    <MISACol display="flex" direction="column" rowGap="12" gutter="12" span="13">
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
            <div sty class="ntt-form__header" :style="{ marginTop: '30px' }">
                <h3 class="form__title">THÔNG TIN THUỘC TÍNH</h3>
            </div>
            <div class="wrapper-form">
                <MISAInputManyTag
                    @update-tag="updateTag"
                    :properties="properties.color"
                    :type="Enum.TypeProperties.Color"
                    label="Màu sắc"
                ></MISAInputManyTag>
                <MISAInputManyTag
                    @update-tag="updateTag"
                    :properties="properties.size"
                    :type="Enum.TypeProperties.Size"
                    label="Size"
                ></MISAInputManyTag>
            </div>
            <div class="form-table">
                <MISATableDetail
                    @delete-detail="deleteDetail"
                    :columns="columnTable"
                    :dataTable="dataTable"
                ></MISATableDetail>
            </div>
        </div>

        <div class="ntt-form__footer">
            <MISAButton :type="Enum.ButtonType.IconPri" :action="MISAResource[resource.langCode]?.Button?.Save">
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
</style>
