import { defineStore } from 'pinia';
import Enum from '../common/enum';

export const useDialog = defineStore('dialog', {
    state: () => ({
        isShow: false,
        actionDialog: '', //tên hành động
        title: '', //tiêu đề
        content: '', //nội dung
        type: '', //loại button
        buttonSec: undefined, //nội dung nút phụ
        objectData: {}, // đối tượng nhận được
        icon: '', //loại icon
        method: Enum.EditMode.None,
        errorMessage: '',
        buttonThird: '', //button thứ 3 ở bên trái dialog
        loadingBtn: false,
        errorResponse: false, //Nếu có error từ response thì không đóng dialog
        errorCode: 0,
        saveData: () => {}, //đây là hàm được gán vào dialog
        closeNavigationLink: () => {},
    }),

    actions: {
        /**
         * Author: Tiến Trung 24/06/2023)
         * Description: thay đổi trạng thái loading
         */
        setLoading(isloading) {
            this.loadingBtn = isloading;
        },
        /**
         * Author: Tiến Trung 24/06/2023)
         * Description: gán hàm vào dialog
         */
        setFunction(fn) {
            this.saveData = fn;
        },
        /**
         * Author: Tiến Trung 24/06/2023)
         * Description: gán hàm chuyển hướng vào dialog
         */
        setCloseNavigationLink(fn) {
            this.closeNavigationLink = fn;
        },
        /**
         * Author: Tiến Trung 24/06/2023)
         * Description: thay đổi trạng thái dialog
         */
        toggleDialog() {
            this.isShow = !this.isShow;
        },
        /**
         * Author: Tiến Trung 24/06/2023)
         * Description: mở dialog hiển thị thông tin
         */
        open(dialog) {
            this.isShow = true;
            this.actionDialog = dialog.action;
            this.content = dialog.content;
            this.title = dialog.title;
            this.type = dialog.type;
            this.buttonSec = dialog.buttonSec;
            this.name = dialog.name;
            this.icon = dialog.icon;
            this.buttonThird = dialog.buttonThird;
            this.loadingBtn = dialog.loading;
            this.errorResponse = dialog.errorResponse ? dialog.errorResponse : false;
            this.errorContentResponse = dialog.errorContentResponse;
            this.errorCode = dialog.errorCode ? dialog.errorCode : 0;
        },
        /**
         * Author: Tiến Trung 24/06/2023)
         * Description: Đóng dialog reset thông tin
         */
        close() {
            this.isShow = false;
            this.objectData = {};
            this.actionDialog = '';
            this.content = '';
            this.title = '';
            this.type = '';
            this.buttonSec = undefined;
            this.name = '';
            this.icon = '';
            this.errorMessage = '';
            this.buttonThird = '';
            this.loadingBtn = false;
            this.errorResponse = false;
            this.method = this.method = Enum.EditMode.None;
        },
        /**
         * Author: Tiến Trung 24/06/2023)
         * Description: set mã lỗi
         */
        setErrorCode(errorCode) {
            this.errorCode = errorCode;
        },
        /**
         * Author: Tiến Trung 24/06/2023)
         * Description: gán đối tượng vào dialog
         */
        setObjectData(data) {
            this.objectData = data;
        },
        /**
         * Author: Tiến Trung 24/06/2023)
         * Description: set method cho dialog
         */
        setMethod(method) {
            this.method = method;
        },
        /**
         * Author: Tiến Trung 24/06/2023)
         * Description: set lỗi thông báo cho dialog
         */
        setErrorMessage(error) {
            this.errorMessage = error;
        },
    },
});
