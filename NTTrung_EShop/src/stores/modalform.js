import { defineStore } from 'pinia';
import Enum from '../common/enum.js';
export const useModalForm = defineStore('modalform', {
    state: () => ({
        isShow: false,
        action: '', //Hành động thêm hoặc sửa(title form)
        method: Enum.EditMode.None, //Method form
        object: {}, //Đối tượng truyền vào form
        affterSubmitSuccess: async () => {},
    }),
    getters: {
        /**
         * Author: Tiến Trung 24/06/2023)
         * Description: trạng thái modal
         */
        show() {
            return this.isShow;
        },
        /**
         * Author: Tiến Trung 24/06/2023)
         * Description: Hành động của modal
         */
        actionType() {
            return this.action;
        },
    },
    actions: {
        /**
         * Author: Tiến Trung 24/06/2023)
         * Description: thay đổi trạng thái modal
         */
        toggleModal() {
            this.isShow = !this.isShow;
        },
        /**
         * Author: Tiến Trung 24/06/2023)
         * Description: đóng modal
         */
        close() {
            this.isShow = false;
            this.object = {};
            this.method = Enum.EditMode.None;
            this.affterSubmitSuccess = async () => {};
        },
        /**
         * Author: Tiến Trung 24/06/2023)
         * Description: Mở modal
         */
        open() {
            this.isShow = true;
        },
        /**
         * Author: Tiến Trung 24/06/2023)
         * Description: set hành động cho modal thêm sửa hay xóa
         */
        setAction(type) {
            this.action = type;
        },
        /**
         * Author: Tiến Trung 24/06/2023)
         * Description: set đối tượng cho modal
         */
        setObjectForm(data) {
            this.object = data;
        },
        /**
         * Author: Tiến Trung 24/06/2023)
         * Description: resert modal khi thêm form mới
         */
        resetNewForm() {
            this.isShow = true;
            this.object = {};
        },
        /**
         * Author: Tiến Trung 24/06/2023)
         * Description: set method cho modal
         */
        setMethod(method) {
            this.method = method;
        },
        /**
         * Author: Tiến Trung 24/06/2023)
         * Description: hàm thực thi sau khi lưu thành công
         */
        setAffterSuccess(fn) {
            this.affterSubmitSuccess = fn;
        },
    },
});
