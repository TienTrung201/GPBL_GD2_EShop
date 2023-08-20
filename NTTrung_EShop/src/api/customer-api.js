import Enum from '../common/enum.js';
import Axios from './axios.ext.js';

/**
 * Author: Tiến Trung (06/07/2023)
 * Description: base Api cho Customer
 */
const customerApi = {
    method: Enum.ApiMethod.GET,
    param: {},
    path: '/Customers',
    request(subPath) {
        let urlPath = this.path;
        if (subPath && subPath.length > 0) {
            urlPath = `${this.path}/${subPath}`;
        }
        return Axios.request(urlPath, this.param, this.method);
    },
};

export default customerApi;
