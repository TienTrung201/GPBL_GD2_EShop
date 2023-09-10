import Enum from '../common/enum';
import Axios from './axios.ext';
import baseAxios from './base-axios';

/**
 * Author: Tiến Trung (06/07/2023)
 * Description: base Api
 */
const baseApi = {
    method: Enum.ApiMethod.GET,
    param: {},
    path: '',
    data: {},
    request(subPath) {
        let urlPath = this.path;
        if (subPath && subPath.length > 0) {
            urlPath = `${this.path}/${subPath}`;
        }
        return Axios.request(urlPath, this.param, this.method, this.data);
    },
    /**
     * @param {Array} data //Mảng cột được truyền vào
     * Author: Tiến Trung (04/08/2023)
     * Description: Hàm gọi Api xuất excel
     */
    downloadExcel(data) {
        const url = `${this.path}/Excel`;
        const parsedData = JSON.parse(JSON.stringify(data));
        return baseAxios.post(url, parsedData, {
            responseType: 'arraybuffer',
        });
    },
    /**
     * @param {Array} data
     * Author: Tiến Trung (26/08/2023)
     * Description: Hàm gọi Api gửi ảnh
     */
    uploadPirture(data) {
        const url = `${this.path}`;
        return baseAxios.post(url, data, {
            headers: {
                'content-type': 'multipart/form-data',
            },
        });
    },
};
export default baseApi;
