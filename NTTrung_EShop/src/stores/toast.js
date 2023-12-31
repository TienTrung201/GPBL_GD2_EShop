import { defineStore } from 'pinia';
import Enum from '../common/enum';
import MISAResource from '../common/resource';
import { v4 as uuidv4 } from 'uuid';
const langCode = localStorage.getItem(Enum.LocalStorage.LangCode) || Enum.Language.VN;
export const useToast = defineStore('toast', {
    state: () => ({
        isShow: false,
        title: '', //tiêu đề
        content: '', //nội dung
        icon: '', //loại icon
        type: '',
        method: '',
        listToast: [],
    }),

    actions: {
        /**
         * Author: Tiến Trung 24/06/2023)
         * Description: thêm toast message
         */
        push(toast) {
            this.listToast.push(toast);
            const duration = 5000;
            //time out close toast
            toast.idTimeOut = setTimeout(() => {
                this.remove(toast);
            }, duration + 500);
        },
        /**
         * Author: Tiến Trung 24/06/2023)
         * Description: toast success
         */
        success(message) {
            const toast = {
                title: MISAResource[langCode]?.Toast?.Title?.Success,
                content: message,
                type: Enum.Icon.Success,
                icon: Enum.Icon.Success,
                toastId: uuidv4(),
            };
            this.listToast.push(toast);
            const duration = 5000;
            //time out close toast
            toast.idTimeOut = setTimeout(() => {
                this.remove(toast);
            }, duration + 500);
        },
        /**
         * Author: Tiến Trung 24/06/2023)
         * Description: toast success
         */
        error(message) {
            const toast = {
                title: MISAResource[langCode]?.Toast?.Title?.Error,
                content: message,
                type: Enum.Icon.Error,
                icon: Enum.Icon.Error,
                toastId: uuidv4(),
            };
            this.listToast.push(toast);
            const duration = 5000;
            //time out close toast
            toast.idTimeOut = setTimeout(() => {
                this.remove(toast);
            }, duration + 500);
        },
        /**
         * Author: Tiến Trung 24/06/2023)
         * Description: toast info
         */
        info(message) {
            const toast = {
                title: MISAResource[langCode]?.Toast?.Title?.Info,
                content: message,
                type: Enum.Icon.Info,
                icon: 'info-icon',
                toastId: uuidv4(),
            };
            this.listToast.push(toast);
            const duration = 5000;
            //time out close toast
            toast.idTimeOut = setTimeout(() => {
                this.remove(toast);
            }, duration + 500);
        },
        /**
         * Author: Tiến Trung 24/06/2023)
         * Description: Xóa toast
         */
        remove(toastRemove) {
            clearTimeout(toastRemove.idTimeOut);

            this.listToast = this.listToast.filter((toast) => toast.toastId !== toastRemove.toastId);
        },
    },
});
