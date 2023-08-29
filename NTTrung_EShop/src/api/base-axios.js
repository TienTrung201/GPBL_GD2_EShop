import axios from 'axios';
import Enum from '../common/enum';
import { useDialog } from '../stores/dialog';
const langCode = localStorage.getItem(Enum.LocalStorage.LangCode) || Enum.Language.VN;
import MISAResource from '../common/resource';

/**
 * Author: Tiến Trung (06/07/2023)
 * Description: Lấy mã ngôn ngữ
 */
const getLanguage = () => {
    const language = localStorage.getItem(Enum.LocalStorage.LangCode) || Enum.Language.VN;
    if (language === Enum.Language.VN) {
        return 'vi-VN';
    } else {
        return 'en-US';
    }
};
/**
 * Author: Tiến Trung (06/07/2023)
 * Description: Cấu hình axios
 */
// Create new instance
const baseAxios = axios.create({
    baseURL: import.meta.env.VITE_APP_BASEURL,

    headers: {
        'Content-Type': 'application/json',
        ContentLanguage: getLanguage(),
    },
});

// Add a request interceptor
baseAxios.interceptors.request.use(
    function (config) {
        // Do something before request is sent
        return config;
    },
    function (error) {
        // Do something with request error
        return Promise.reject(error);
    },
);

// Add a response interceptor
baseAxios.interceptors.response.use(
    function (response) {
        const dialog = useDialog();
        // Any status code that lie within the range of 2xx cause this function to trigger
        // Do something with response data
        //Nếu có ErrorCode thì bật dialog thông báo lỗi
        //cho trường hợp xóa thành công nhưng có bản ghi không
        //hợp lệ
        switch (response.data.ErrorCode) {
            case Enum.ErorCode.ExistedConstrain:
                dialog.setMethod(Enum.EditMode.None);
                dialog.open({
                    title: MISAResource[langCode]?.Dialog?.Warning?.Title,
                    content: response.data.UserMessage,
                    action: MISAResource[langCode]?.Button?.Close,
                    type: Enum.ButtonType.Pri,
                    icon: 'warning',
                    loading: false,
                    errorResponse: true,
                });
                break;
            default:
                if (dialog.errorResponse === false) {
                    dialog.close();
                }
                break;
        }
        return response;
    },
    function (error) {
        const dialog = useDialog();
        //Nếu có bất kì lỗi nào từ BE trả về thì nhảy hết vào đây
        dialog.setMethod(Enum.EditMode.None);
        dialog.open({
            title: MISAResource[langCode]?.Dialog?.Warning?.Title,
            content: error.response.data.UserMessage || MISAResource[langCode].ErrorMisaAlert,
            action: MISAResource[langCode]?.Button?.Close,
            type: Enum.ButtonType.Pri,
            icon: 'warning',
            loading: false,
            errorResponse: true,
            errorCode: error.response.data.ErrorCode,
        });
        // Any status codes that falls outside the range of 2xx cause this function to trigger
        // Do something with response error
        return Promise.reject(error);
    },
);

export default baseAxios;
