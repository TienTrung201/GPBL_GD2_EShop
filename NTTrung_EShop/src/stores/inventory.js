import { defineStore } from 'pinia';
import Enum from '../common/enum';
import router from '../router';
export const useInventory = defineStore('inventory', {
    state: () => ({
        dataSelect: {},
        uid: '',
        editMode: Enum.EditMode.None,
        submitForm: () => {}, //đây là hàm được gán vào dialog
    }),

    actions: {
        /**
         * Author: Tiến Trung 18/08/2023)
         * Description: chuyển sang form Inventory
         */
        openForm(uid, editMode) {
            this.editMode = editMode;
            this.uid = uid;
            if (uid) {
                router.push({ name: Enum.Router.InventoryForm.NameDetail, params: { id: this.uid } });
            } else {
                router.push({ name: Enum.Router.InventoryForm.Name });
            }
        },
        /**
         * Author: Tiến Trung 18/08/2023)
         * Description: đóng form trở về danh sách
         */
        closeForm() {
            this.uid = '';
            this.editMode = Enum.EditMode.None;
            router.push({ name: Enum.Router.Inventory.Name });
        },
        /**
         * Author: Tiến Trung 27/08/2023)
         * Description: đóng form trở về danh sách
         */
        setDataSelect(data) {
            this.dataSelect = data;
        },
    },
});
