<script setup>
import { onMounted, onUnmounted, onUpdated, ref, watch } from 'vue';
import Enum from '../../common/enum';
import { useDialog } from '../../stores/dialog';
import { useResource } from '../../stores/resource.js';
import MISAResource from '../../common/resource';
import { useInventory } from '../../stores/inventory';
import { useTitleHeader } from '../../stores/title-header';
import MISATableDetail from '../../components/base/table/MISATableDetail.vue';
import baseApi from '../../api/base-api';
import {
    convertCurrency,
    convertDataTable,
    convertToTitleCase,
    deepCopy,
    removeVietnameseTones,
} from '../../common/convert-data';
import { useRoute } from 'vue-router';
import { useToast } from '../../stores/toast';
import { Validator } from '../../common/validatior';
import ItemCategoryForm from '../item-category/ItemCategoryForm.vue';
import UnitForm from '../unit/UnitForm.vue';
import { useModalForm } from '../../stores/modalform';
import { useBaseEntity } from '../../stores/base-entity';
const dialog = useDialog();
const toast = useToast();
const route = useRoute();
const baseEntity = useBaseEntity();
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
const inputType = ref({ SKUCode: 1, Name: 2, Unit: 3, ItemCategory: 4 }); //Biến chứa loại input để biết khi blur thì thay đổi giá trị nào
const buttonTypeSave = ref({ save: 1, saveAdd: 2, saveCopy: 3 });
const typeForm = ref({ unit: 1, itemCategory: 2 });
const openForm = ref(0);
const file = ref(null);
const imgUrl = ref(null);
const loadingButton = ref({ save: false, saveAdd: false, saveCopy: false });
const inventoryOldName = ref('');
const SKUCodeOld = ref('');
const validateForm = ref({
    inventoryName: '',
    SKUCode: '',
    barcode: '',
    costPrice: '',
    unitPrice: '',
    description: '',
});
const listErrorMessage = ref([]);
//Input------------------- ref
const iInventoryName = ref(null);
const iSKUCode = ref(null);
const iCostPrice = ref(null);
const iUnitPrice = ref(null);
const iDescription = ref(null);
const iIsShowMenu = ref(null);

//Button------------------- ref
const buttonSave = ref(null);
const buttonCancel = ref(null);
const buttonSaveAdd = ref(null);
const buttonReplication = ref(null);
// const firstFocus = ref(null);
// eslint-disable-next-line no-unused-vars
const columnTable = ref([
    {
        title: MISAResource[resource.langCode]?.Manage?.Inventory?.InventoryName,
        key: 'InventoryName',
        width: '170',
        isShow: true,
        isEdit: true,
        // pin: true,
    },
    {
        title: MISAResource[resource.langCode]?.Manage?.Inventory?.Unit,
        key: 'UnitName',
        width: '170',
        isShow: true,
        isEdit: false,
        // pin: true,
    },
    {
        title: MISAResource[resource.langCode]?.Manage?.Inventory?.SKUCode,
        key: 'SKUCodeCustom',
        width: '170',
        isShow: true,
        isEdit: true,
        isCode: true,
        type: Enum.TypeDataTable.Code,
        // type: 'gender',
    },
    {
        title: MISAResource[resource.langCode]?.Manage?.Inventory?.Barcode,
        key: 'Barcode',
        width: '170',
        isShow: true,
        isEdit: true,
        isBarcode: true,
    },
    {
        title: MISAResource[resource.langCode]?.Manage?.Inventory?.CostPrice,
        key: 'CostPrice',
        width: '170',
        isShow: true,
        align: Enum.AlignColumn.Right,
        type: Enum.TypeDataTable.Money,
        isEdit: true,
    },
    {
        title: MISAResource[resource.langCode]?.Manage?.Inventory?.UnitPrice,
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
/*
 **
 * Author: Tiến Trung (19/08/2023)
 * Description: lấy thông tin thuộc tính cho data
 */
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
            //Hình như đoạn này bị thừa :))
            if (dataOld.EditMode === Enum.EditMode.None) {
                dataOld.InventoryName = dataGen.InventoryName;
            }
            dataGenAuto.push(dataOld);
        } else {
            dataGen.EditMode = Enum.EditMode.Add;
            dataGen.UnitPrice = formData.value.unitPrice;
            dataGen.CostPrice = formData.value.costPrice;
            dataGenAuto.push(dataGen);
        }
    });
    //Kiểm tra xem data có bị xóa hết không nếu xóa hết thì add vào mảng data xóa
    //Nếu xóa tag mà data cũ bị xóa hết
    if (isDeleteAllData) {
        //Chỉ return ra data cũ có ID thì sẽ không dính vào data mới
        const findDataDelete = dataTable.value.filter((data) => {
            data.EditMode = Enum.EditMode.Delete;
            data.UnitPrice = Number(data.UnitPrice?.replace(/\./g, ''));
            data.CostPrice = Number(data.CostPrice?.replace(/\./g, ''));
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
        dataDelete.CostPrice = Number(dataDelete.CostPrice?.replace(/\./g, ''));
        dataDelete.UnitPrice = Number(dataDelete.UnitPrice?.replace(/\./g, ''));
        dataTable.value = dataTable.value.filter((data) => data.SKUCode !== dataDelete.SKUCode);
    });
    properties.value[type].splice(index, 1);
    autoDataTable();
};
/*
 **
 * Author: Tiến Trung (03/09/2023)
 * Description: Hàm lấy mã mới
 */
const getNewCode = async () => {
    const parts = removeVietnameseTones(formData.value.inventoryName).split(' '); //các bộ phận
    const abbreviation = parts
        .map((part) => part[0])
        .join('')
        .toUpperCase(); //Viết tắt

    baseApi.method = Enum.ApiMethod.GET;
    baseApi.path = Enum.Router.Inventory.Api;
    const result = await baseApi.request('NewCode/' + abbreviation);
    formData.value.SKUCode = result.data;
    validateForm.value.SKUCode = '';
};
/*
 **
 * Author: Tiến Trung (19/08/2023)
 * Description: hàm khi blur tag thì update
 */
const onBlurInputFormUpdateData = async (value, nameForm) => {
    // autoDataTable();
    dataTable.value.forEach((data) => {
        const SKUCodeDetail = `${formData.value.SKUCode}${data.ColorCode ? '-' + data.ColorCode : ''}${
            data.SizeCode ? '-' + data.SizeCode : ''
        }`;
        let nameDetail = formData.value.inventoryName;
        if (data.Size && data.Color) {
            nameDetail = `${nameDetail} (${data.Color}/${data.Size})`;
        }
        if (data.Color && data.Size === '') {
            nameDetail = `${nameDetail} (${data.Color})`;
        }
        if (data.Size && data.Color === '') {
            nameDetail = `${nameDetail} (${data.Size})`;
        }
        data.EditMode = Enum.EditMode.Update;
        switch (nameForm) {
            case inputType.value.Name:
                if (isEditForm.value === Enum.EditMode.Update) {
                    data.InventoryName = nameDetail;
                }
                break;
            case inputType.value.SKUCode:
                if (isEditForm.value === Enum.EditMode.Update) {
                    data.SKUCode = SKUCodeDetail;
                    data.SKUCodeCustom = SKUCodeDetail;
                }
                break;
            case inputType.value.Unit:
                data.UnitId = formData.value.unitId;
                data.UnitName = units.value.find((unit) => unit.value === value).option;
                break;
            case inputType.value.ItemCategory:
                data.ItemCategoryId = value;
                break;
            default:
                break;
        }
    });
    // Nếu là tên cũ thì không gọi mã mới nếu là tên mới thì gọi mã mới
    if (
        (nameForm === inputType.value.Name || nameForm === inputType.value.SKUCode) &&
        formData.value.inventoryName.trim('').length > 0
    ) {
        if (inventoryOldName.value === formData.value.inventoryName && formData.value.SKUCode.trim('').length === 0) {
            formData.value.SKUCode = SKUCodeOld.value;
        } else {
            if (formData.value.SKUCode.trim('').length === 0) await getNewCode();
        }
    }
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
        const data = dataTable.value[index];
        if (data.InventoryId) {
            data.EditMode = Enum.EditMode.Delete;
            data.UnitPrice = Number(data.UnitPrice?.replace(/\./g, ''));
            data.CostPrice = Number(data.CostPrice?.replace(/\./g, ''));
            dataDelete.value.push(data);
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
const handleChangeImg = async (image) => {
    try {
        // Kiểm tra định dạng ảnh
        const validExtensions = ['jpg', 'jpeg', 'png', 'gif'];
        const maxSizeBytes = 5 * 1024 * 1024; // 5MB
        const extension = image.name.split('.').pop().toLowerCase();
        if (!validExtensions.includes(extension)) {
            toast.error(MISAResource[resource.langCode]?.Toast?.Error?.Picture?.ErrorExtension);
            return;
        }

        if (image.size > maxSizeBytes) {
            toast.error(MISAResource[resource.langCode]?.Toast?.Error?.Picture?.ErrorSize);
            return;
        }
        const formData2 = new FormData(); // Khởi tạo formData ở đây
        formData2.append('image', image);
        baseApi.path = Enum.Router.Picture.Api;
        const response = await baseApi.uploadPirture(formData2);
        imgUrl.value = response.data.PictureUrl;
        console.log(response);
        formData.value.pictureId = response.data.Picture.PictureId;
    } catch (error) {
        imgUrl.value = '';
    }
};
/*
 **
 * Author: Tiến Trung (27/06/2023)
 * Description: hàm đóng modal
 * nếu thay đổi dữ liệu mà đóng modal thì bật dialog
 */
function closeForm() {
    if (isEditForm.value === Enum.EditMode.Update) {
        dialog.setFunction(submitForm);
        dialog.setMethod(baseEntity.editMode);
        dialog.setCloseNavigationLink(() => {
            baseEntity.closeForm();
            title.setTitle(MISAResource[resource.langCode]?.SideBar?.Inventory);
        });
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
        baseEntity.closeForm();
        title.setTitle(MISAResource[resource.langCode]?.SideBar?.Inventory);
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
        if (dialog.errorCode === Enum.ErorCode.DuplicateCode && modalForm.isShow === false) {
            dialog.setErrorCode(0);
            validateForm.value.SKUCode = MISAResource[resource.langCode].InventoryInvalidError.InventoryDuplicateCode;
            iSKUCode.value.autoFocus();
            return;
        }
        if (validateForm.value.inventoryName) {
            iInventoryName.value.autoFocus();
            return;
        }
        if (validateForm.value.SKUCode) {
            iSKUCode.value.autoFocus();
            return;
        }
        if (validateForm.value.costPrice) {
            iCostPrice.value.autoFocus();
            return;
        }
        if (validateForm.value.unitPrice) {
            iUnitPrice.value.autoFocus();
            return;
        }
    } catch (error) {
        console.log(error);
    }
};
/*
 **
 * Author: Tiến Trung (29/08/2023)
 * Description: lấy thông tin của detail
 */
const getDetail = async () => {
    if (formEditMode.value === Enum.EditMode.Update) {
        if (Enum.RegExp.Guid.test(inventoryId.value)) {
            dialog.setFunction(reloadPage);
            baseApi.path = Enum.Router.Inventory.Api;
            baseApi.method = Enum.ApiMethod.GET;
            const response = await baseApi.request(inventoryId.value);
            console.log(response);
            return response.data;
        } else {
            reloadPage();
        }
    } else {
        return {};
    }
};
/**
 * Author: Tiến Trung (29/08/2023)
 * Description: Hàm reload page khi có lỗi
 */
const reloadPage = () => {
    baseEntity.openForm(null, Enum.EditMode.Add, Enum.Router.Inventory.Name);
    setTimeout(() => {
        location.reload();
    }, 10);
};
/**
 * Author: Tiến Trung (05/07/2023)
 * Description: Hàm update form nếu form mới thì call api lấy newCode
 */
const updateForm = async () => {
    const data = await getDetail();
    let isActive = 1;
    if (data.IsActive === undefined) {
        isActive = 1;
    } else {
        isActive = data.IsActive ? 1 : 0;
    }
    formData.value = {
        inventoryName: data.InventoryName ? data.InventoryName : '',
        itemCategoryId: data.ItemCategoryId, //? data.ItemCategoryId : itemCategories.value[0].value,
        unitId: data.UnitId, //? data.UnitId : units.value[0].value,
        SKUCode: data.SKUCode ? data.SKUCode : '',
        // SKUCodeCustom: data.SKUCodeCustom?data.SKUCodeCustom:,
        costPrice: data.CostPrice ? convertCurrency(data.CostPrice) : '0',
        unitPrice: data.UnitPrice ? convertCurrency(data.UnitPrice) : '0',
        unitName: data.UnitName,
        description: data.Description,
        pictureId: data.PictureId,
        isActive: isActive,
        isShowMenu: data.IsShowMenu ? true : false,
        editMode: formEditMode.value,
        inventoryId: data.InventoryId,
        createdDate: data.CreatedDate,
    };
    imgUrl.value = data.PictureId
        ? import.meta.env.VITE_APP_BASEURL + Enum.Router.Picture.Api + '/' + formData.value.pictureId
        : '';
    dataTable.value = convertDataTable(data.Detail ? data.Detail : [], columnTable.value, resource.langCode) || [];
    updateProperties();
    iInventoryName.value.autoFocus();
    //Nếu là Copy thì gọi mã mới
    if (baseEntity.editMode === Enum.EditMode.Copy) {
        await getNewCode();
        formData.value.editMode = Enum.EditMode.Add;
        // setTimeout(() => {
        isEditForm.value = Enum.EditMode.None;
        formEditMode.value = Enum.EditMode.Copy;
        // }, 1000);
    }
    inventoryOldName.value = formData.value.inventoryName;
    SKUCodeOld.value = formData.value.SKUCode;
    couterChangeForm.value = 0;
};
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
 * Author: Tiến Trung (26/08/2023)
 * Description: Hàm save data gọi api
 */
const saveData = async (data) => {
    baseApi.path = Enum.Router.Inventory.Api;
    baseApi.data = data;
    baseApi.method = Enum.ApiMethod.POST;
    const res = await baseApi.request('SaveData');
    // const res = await employeeApi.create(data);
    console.log(res);
    toast.success(MISAResource[resource.langCode]?.Toast?.Success?.SaveSuccess);
};

/**
 * Author: Tiến Trung (26/08/2023)
 * Description: Hàm gửi form
 */
const submitForm = async (typeButtonSave) => {
    try {
        //vadidate nhập
        validateName(formData.value.inventoryName);
        validateCode(formData.value.SKUCode);
        validateCostPrice(formData.value.costPrice);
        validateUnitPrice(formData.value.unitPrice);
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
                icon: 'error-big',
                loading: false,
            });
        } else {
            //tạo một bản sao
            //validate list
            const copyObj = deepCopy(formData.value);
            //Set các dữ liệu cần thiết cho detail
            const dataCreateUpdate = dataTable.value.map((data) => {
                const costPrice = Number(data.CostPrice?.replace(/\./g, ''));
                const unitPrice = Number(data.UnitPrice?.replace(/\./g, ''));
                const isUpdateCode = data.newCode && data.newCode !== data.SKUCodeCustom;
                const isUpdateBarcode = data.newCode && data.newBarcode !== data.Barcode;
                let editMode = data.EditMode;
                //Nếu là copy hoặc add thì cho edit mode là thêm mới
                if (formEditMode.value === Enum.EditMode.Add || formEditMode.value === Enum.EditMode.Copy) {
                    editMode = Enum.EditMode.Add;
                }
                return {
                    ...data,
                    isUpdateCode,
                    isUpdateBarcode,
                    CostPrice: costPrice,
                    UnitPrice: unitPrice,
                    UnitId: formData.value.unitId ? formData.value.unitId : null,
                    ItemCategoryId: formData.value.itemCategoryId ? formData.value.itemCategoryId : null,
                    EditMode: editMode,
                };
            });
            //Cập nhật data phù hợp để gửi vào API
            // copyObj.detail = [...dataCreateUpdate, ...dataDelete.value];
            copyObj.SKUCodeCustom = copyObj.SKUCode;
            copyObj.isActive = copyObj.isActive ? true : false;
            copyObj.costPrice = Number(copyObj.costPrice?.replace(/\./g, ''));
            copyObj.unitPrice = Number(copyObj.unitPrice?.replace(/\./g, ''));
            copyObj.itemCategoryId = copyObj.itemCategoryId ? copyObj.itemCategoryId : null;
            copyObj.unitId = copyObj.unitId ? copyObj.unitId : null;

            //Ghép các data thành 1 mảng
            const listData = [copyObj, ...dataCreateUpdate, ...dataDelete.value];
            //Button loading
            setLoadingButton(typeButtonSave, true);
            await saveData(listData);
            setLoadingButton(typeButtonSave, false);
            //Bấm vào Button nào thì tương ứng chức năng đấy
            switch (typeButtonSave) {
                //Button lưu và thêm mới
                case buttonTypeSave.value.saveAdd:
                    formEditMode.value = Enum.EditMode.Add;
                    //trả về form chưa thay đổi để biết là form mới không hiện dialog khi đóng
                    isEditForm.value = Enum.EditMode.None;
                    couterChangeForm.value = 0;
                    formData.value.editMode = Enum.EditMode.Add;
                    baseEntity.openForm(null, Enum.EditMode.Add, Enum.Router.Inventory.Name);
                    updateForm();
                    break;
                //Button lưu và nhân bản
                case buttonTypeSave.value.saveCopy:
                    formEditMode.value = Enum.EditMode.Copy;
                    couterChangeForm.value = 0;
                    formData.value.editMode = Enum.EditMode.Copy;
                    await getNewCode();
                    //trả về form chưa thay đổi để biết là form mới không hiện dialog khi đóng
                    isEditForm.value = Enum.EditMode.None;
                    baseEntity.openForm(null, Enum.EditMode.Copy, Enum.Router.Inventory.Name);
                    //Load bảng mới
                    autoDataTable();
                    dataDelete.value = [];
                    break;
                default:
                    baseEntity.closeForm();
                    break;
            }
        }
    } catch (error) {
        setLoadingButton(typeButtonSave, false);
        console.log(error);
    }
};
/**
 * Author: Tiến Trung (29/08/2023)
 * Description: Hàm set loading cho button
 */
const setLoadingButton = (typeButtonSave, value) => {
    switch (typeButtonSave) {
        case buttonTypeSave.value.saveAdd:
            loadingButton.value.saveAdd = value;
            break;
        case buttonTypeSave.value.saveCopy:
            loadingButton.value.saveCopy = value;
            break;
        default:
            loadingButton.value.save = value;
            break;
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
///Validator-------------------------------------------------------
/**
 * Author: Tiến Trung (29/08/2023)
 * Description: Hàm validate form
 */
//Hàm validate mã
function validateCode(value) {
    try {
        const errorMessage = Validator(value, [
            Validator.isCode(MISAResource[resource.langCode]?.InventoryInvalidError?.InventoryCodeFormat),
            Validator.maxLength(20, MISAResource[resource.langCode]?.InventoryInvalidError?.InventoryCodeMaxLength),
        ]);
        validateForm.value.SKUCode = errorMessage;
        return errorMessage;
    } catch (error) {
        console.log(error);
    }
}
//Hàm validate name
function validateName(value) {
    try {
        const errorMessage = Validator(value, [
            Validator.isRequired(MISAResource[resource.langCode]?.InventoryInvalidError?.InventoryNameEmpty),
            Validator.maxLength(100, MISAResource[resource.langCode]?.InventoryInvalidError?.InventoryNameMaxLength),
        ]);
        validateForm.value.inventoryName = errorMessage;
        return errorMessage;
    } catch (error) {
        console.log(error);
    }
}
//Hàm validate giá
function validateCostPrice(value) {
    try {
        const errorMessage = Validator(value, [
            Validator.maxLength(
                18,
                MISAResource[resource.langCode]?.InventoryInvalidError?.InventoryCostPriceMaxLength,
            ),
        ]);
        validateForm.value.costPrice = errorMessage;
        return errorMessage;
    } catch (error) {
        console.log(error);
    }
}
//Hàm validate giá
function validateUnitPrice(value) {
    try {
        const errorMessage = Validator(value, [
            Validator.maxLength(
                18,
                MISAResource[resource.langCode]?.InventoryInvalidError?.InventoryUnitPriceMaxLength,
            ),
        ]);
        validateForm.value.unitPrice = errorMessage;
        return errorMessage;
    } catch (error) {
        console.log(error);
    }
}
//Hàm validate giá
function validateDescription(value) {
    try {
        const errorMessage = Validator(value, [
            Validator.maxLength(255, MISAResource[resource.langCode]?.InventoryInvalidError?.DescriptionMaxLength),
        ]);
        validateForm.value.description = errorMessage;
        return errorMessage;
    } catch (error) {
        console.log(error);
    }
}
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
 * Author: Tiến Trung (11/07/2023)
 * Description: hàm xử lý khi gõ phím gửi form
 */
const handleKeyDown = (e) => {
    if (e.ctrlKey && e.shiftKey && e.keyCode === Enum.Keyboard.S) {
        e.preventDefault();
        //Báo cho button biết có sự kiện click thì gửi form
        buttonSaveAdd.value.$emit('click');
        iInventoryName.value.autoFocus();
        return;
    }
    if (e.ctrlKey && e.shiftKey && e.keyCode === Enum.Keyboard.C) {
        e.preventDefault();
        //Báo cho button biết có sự kiện click thì gửi form
        buttonReplication.value.$emit('click');
        iInventoryName.value.autoFocus();
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
const modalForm = useModalForm();
const handleOpenForm = (type) => {
    openForm.value = type;
    modalForm.setAction(MISAResource[resource.langCode]?.FormTitle?.[type]?.Add);
    modalForm.setObjectForm({});
    dialog.setObjectData({});
    modalForm.setMethod(Enum.EditMode.Add);
    modalForm.open();

    if (openForm.value === Enum.Router.ItemCategory.Name) {
        modalForm.setAffterSuccess(getItemCategory);
    } else {
        modalForm.setAffterSuccess(getUnit);
    }
};
/**
 * Author: Tiến Trung (07/09/2023)
 * Description: Hàm update data nếu là code mới thì isupdatecode true
 */
const setDataEditMode = (value, oldValue) => {
    if (formData.value.EditMode !== Enum.EditMode.Add) {
        formData.value.IsUpdateCode = oldValue !== value;
    }
};
/**
 * Author: Tiến Trung (2/07/2023)
 * Description: khi component được tạo thì get data
 * mỗi lần cất và thêm thì lại tạo form mới
 */
onMounted(async () => {
    try {
        baseEntity.setEntity(Enum.Router.Inventory.Name);
        window.addEventListener('keydown', handleKeyDown);
        await getItemCategory();
        await getUnit();
        await updateForm();
        title.setTitle(
            MISAResource[resource.langCode]?.SideBar?.Inventory,
            formEditMode.value === Enum.EditMode.Add || formEditMode.value === Enum.EditMode.Copy
                ? MISAResource[resource.langCode]?.FormTitle?.Inventory?.Add
                : MISAResource[resource.langCode]?.FormTitle?.Inventory?.Update,
        );
    } catch (e) {
        console.log(e);
    }
});

/**
 * Author: Tiến Trung (26/08/2023)
 * Description: khi component hủy thì settitle cho header
 */
onUnmounted(() => {
    window.removeEventListener('keydown', handleKeyDown);
});
/**
 * Author: Tiến Trung (26/08/2023)
 * Description: khi component update thì settitle
 */
onUpdated(() => {
    title.setTitle(
        MISAResource[resource.langCode]?.SideBar?.Inventory,
        formEditMode.value === Enum.EditMode.Add || formEditMode.value === Enum.EditMode.Copy
            ? MISAResource[resource.langCode]?.FormTitle?.Inventory?.Add
            : MISAResource[resource.langCode]?.FormTitle?.Inventory?.Update,
    );
    if (modalForm.isShow) {
        window.removeEventListener('keydown', handleKeyDown);
    } else {
        window.addEventListener('keydown', handleKeyDown);
    }
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
                case Enum.EditMode.Copy:
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
</script>
<template>
    <div class="ntt-form">
        <div class="ntt-form__body">
            <div class="ntt-form__header">
                <h3 class="form__title">{{ MISAResource[resource.langCode]?.FormTitle?.BasicInfomation }}</h3>
            </div>
            <div class="wrapper-form">
                <MISARow justify="flex-start">
                    <MISACol display="flex" direction="column" rowGap="12">
                        <MISARow>
                            <MISARadioGroup :label="MISAResource[resource.langCode]?.Manage?.Inventory?.IsActive" row>
                                <MISARadio
                                    v-model:value="formData.isActive"
                                    :valueRadio="1"
                                    :optionName="MISAResource[resource.langCode]?.Manage?.Inventory?.Active"
                                    name="active"
                                >
                                </MISARadio>
                                <MISARadio
                                    v-model:value="formData.isActive"
                                    :valueRadio="0"
                                    :optionName="MISAResource[resource.langCode]?.Manage?.Inventory?.NoActive"
                                    name="active"
                                ></MISARadio>
                            </MISARadioGroup>
                        </MISARow>
                        <MISARow>
                            <MISAInput
                                v-model:value="formData.inventoryName"
                                ref="iInventoryName"
                                :label="MISAResource[resource.langCode]?.Manage?.Inventory?.InventoryName"
                                type="text"
                                validate="true"
                                row
                                require
                                @blur="(value) => onBlurInputFormUpdateData(value, inputType.Name)"
                                @input-validation="validateName"
                                :errorMessage="validateForm.inventoryName"
                            ></MISAInput>
                        </MISARow>
                        <MISARow>
                            <MISADropdown
                                @next-tab-enter="iSKUCode.autoFocus()"
                                combobox
                                :options="itemCategories"
                                v-model:value="formData.itemCategoryId"
                                :placeholder="MISAResource[resource.langCode]?.Manage?.Inventory?.SelectItem"
                                :label="MISAResource[resource.langCode]?.Manage?.Inventory?.ItemCategory"
                                row
                                buttonAdd
                                @blur="(value) => onBlurInputFormUpdateData(value, inputType.ItemCategory)"
                                :function="() => handleOpenForm(Enum.Router.ItemCategory.Name)"
                            ></MISADropdown>
                        </MISARow>
                        <MISARow>
                            <MISAInput
                                v-model:value="formData.SKUCode"
                                ref="iSKUCode"
                                :label="MISAResource[resource.langCode]?.Manage?.Inventory?.SKUCode"
                                type="text"
                                validate="true"
                                row
                                @blur="
                                    (value, oldValue) => {
                                        onBlurInputFormUpdateData(value, inputType.SKUCode);
                                        setDataEditMode(value, oldValue);
                                    }
                                "
                                @input-validation="
                                    (value, oldValue) => {
                                        validateCode(value);
                                        setDataEditMode(value, oldValue);
                                    }
                                "
                                :errorMessage="validateForm.SKUCode"
                            ></MISAInput>
                        </MISARow>
                        <MISARow>
                            <MISAInput
                                v-model:value="formData.costPrice"
                                ref="iCostPrice"
                                :label="MISAResource[resource.langCode]?.Manage?.Inventory?.CostPrice"
                                type="text"
                                :validate="dataTable.length === 0"
                                row
                                money
                                right
                                @input-validation="validateCostPrice"
                                :errorMessage="validateForm.costPrice"
                                :maxLength="18"
                                :readonly="dataTable.length > 0"
                            ></MISAInput>
                        </MISARow>
                        <MISARow>
                            <MISAInput
                                v-model:value="formData.unitPrice"
                                ref="iUnitPrice"
                                type="text"
                                :validate="dataTable.length === 0"
                                row
                                money
                                right
                                @input-validation="validateUnitPrice"
                                :errorMessage="validateForm.unitPrice"
                                :label="MISAResource[resource.langCode]?.Manage?.Inventory?.UnitPrice"
                                :maxLength="18"
                                :readonly="dataTable.length > 0"
                            >
                            </MISAInput>
                        </MISARow>
                        <MISARow>
                            <MISADropdown
                                @next-tab-enter="iIsShowMenu.autoFocus()"
                                @blur="(value) => onBlurInputFormUpdateData(value, inputType.Unit)"
                                combobox
                                :options="units"
                                v-model:value="formData.unitId"
                                :placeholder="MISAResource[resource.langCode]?.Manage?.Inventory?.SelectUnit"
                                :label="MISAResource[resource.langCode]?.Manage?.Inventory?.Unit"
                                row
                                buttonAdd
                                :function="() => handleOpenForm(Enum.Router.Unit.Name)"
                            ></MISADropdown>
                        </MISARow>
                        <MISARow></MISARow>
                        <MISARow>
                            <MISACheckbox
                                ref="iIsShowMenu"
                                custom
                                :valueCheckbox="formData.isShowMenu"
                                v-model:value="formData.isShowMenu"
                                :lable="MISAResource[resource.langCode]?.Manage?.Inventory?.IsShowMenu"
                            >
                            </MISACheckbox>
                        </MISARow>
                    </MISACol>
                </MISARow>
            </div>
            <div sty class="ntt-form__header" :style="{ marginTop: '30px' }">
                <h3 class="form__title">{{ MISAResource[resource.langCode]?.FormTitle?.AttributeInfomation }}</h3>
            </div>
            <div class="wrapper-form">
                <MISAInputManyTag
                    @update-tag="autoDataTable"
                    @remove-tag="handleRemoveTag"
                    :properties="properties.color"
                    :type="Enum.TypeProperties.Color"
                    :label="MISAResource[resource.langCode]?.Manage?.Inventory?.Color"
                >
                </MISAInputManyTag>
                <MISAInputManyTag
                    @update-tag="autoDataTable"
                    @remove-tag="handleRemoveTag"
                    :properties="properties.size"
                    :type="Enum.TypeProperties.Size"
                    :label="MISAResource[resource.langCode]?.Manage?.Inventory?.Size"
                >
                </MISAInputManyTag>
            </div>
            <div v-if="dataTable.length" class="form-table">
                <MISATableDetail @delete-detail="deleteDetail" :columns="columnTable" :dataTable="dataTable">
                </MISATableDetail>
            </div>
            <div sty class="ntt-form__header" :style="{ marginTop: '10px' }">
                <h3 class="form__title">{{ MISAResource[resource.langCode]?.FormTitle?.AdditionalInfomation }}</h3>
            </div>
            <div class="wrapper-form">
                <MISARow>
                    <MISACol display="flex" direction="column" rowGap="12">
                        <MISARow>
                            <MISATextArea
                                ref="iDescription"
                                isName
                                v-model:value="formData.description"
                                :label="MISAResource[resource.langCode]?.Manage?.Inventory?.Description"
                                validate="true"
                                row
                                class="custom-label"
                                :maxLength="255"
                                @input-validation="validateDescription"
                                :errorMessage="validateForm.description"
                            >
                            </MISATextArea>
                        </MISARow>
                        <MISARow>
                            <div class="form-image">
                                <div class="wrapper__label-image">
                                    <p class="label">{{ MISAResource[resource.langCode]?.Manage?.Inventory?.Image }}</p>
                                    <div class="wrapper-image">
                                        <input
                                            @change="handleChangeImg($event.target.files[0])"
                                            ref="file"
                                            type="file"
                                            name="inventory"
                                            id=""
                                        />
                                        <img class="image-file" v-if="imgUrl" :src="imgUrl" alt="" />

                                        <img
                                            v-else
                                            class="image-file"
                                            src="../../assets/img/img-inventory-default.jpg"
                                            alt=""
                                        />
                                        <div class="wrapper-button-file">
                                            <button @click="file.click()" class="btn-file">...</button>
                                            <button
                                                v-if="imgUrl"
                                                @click="
                                                    () => {
                                                        imgUrl = null;
                                                        formData.pictureId = null;
                                                    }
                                                "
                                                class="btn-file center btn-close"
                                            >
                                                <MISAIcon width="14" height="16" icon="close"></MISAIcon>
                                            </button>
                                        </div>
                                        <button class="btn-select__list-image">
                                            <p class="icon center">
                                                <MISAIcon width="22" height="12" icon="edit" />
                                            </p>
                                            <p>Biểu tượng</p>
                                        </button>
                                    </div>
                                </div>
                                <div class="wrapper__content">
                                    <p class="image__content">
                                        {{ MISAResource[resource.langCode]?.Manage?.Inventory?.ImageContent }}
                                    </p>
                                    <p class="image__content">
                                        {{ MISAResource[resource.langCode]?.Manage?.Inventory?.ImageValidate }}
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
                ref="buttonSave"
                :loading="loadingButton.save"
                @click="submitForm(buttonTypeSave.save)"
                :type="Enum.ButtonType.IconPri"
                :action="MISAResource[resource.langCode]?.Button?.Save"
                v-tooltip.absoluteTop="Enum.KeyboardShortcuts.CtrlS"
            >
                <template #icon>
                    <MISAIcon width="21" height="11" icon="save" />
                </template>
            </MISAButton>
            <MISAButton
                ref="buttonReplication"
                :loading="loadingButton.saveCopy"
                @click="submitForm(buttonTypeSave.saveCopy)"
                :type="Enum.ButtonType.IconPri"
                :action="MISAResource[resource.langCode]?.Button?.SaveAndReplication"
                v-tooltip.absoluteTop="Enum.KeyboardShortcuts.CtrlShiftC"
                sec
            >
                <template #icon>
                    <MISAIcon width="25" height="15" icon="replication" />
                </template>
            </MISAButton>
            <MISAButton
                ref="buttonSaveAdd"
                :loading="loadingButton.saveAdd"
                @click="submitForm(buttonTypeSave.saveAdd)"
                :type="Enum.ButtonType.IconPri"
                :action="MISAResource[resource.langCode]?.Button?.SaveAdd"
                sec
                v-tooltip.absoluteTop="Enum.KeyboardShortcuts.CtrlShiftS"
            >
                <template #icon>
                    <MISAIcon width="21" height="10" icon="plus" />
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
        </div>
        <UnitForm v-if="openForm === Enum.Router.Unit.Name && modalForm.isShow"></UnitForm>
        <ItemCategoryForm v-if="openForm === Enum.Router.ItemCategory.Name && modalForm.isShow"></ItemCategoryForm>
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
