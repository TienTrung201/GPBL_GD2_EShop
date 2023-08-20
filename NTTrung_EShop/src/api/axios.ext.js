import baseAxios from './base-axios';
import Enum from '../common/enum.js';
/**
 * Author: Tiến Trung (06/07/2023)
 * Description: Custom baseAxios để sử dụng
 */
const Axios = {
    request(path, param, method, data) {
        let request;
        switch (method) {
            case Enum.ApiMethod.GET:
                request = baseAxios.get(path, {
                    params: param,
                });
                break;
            case Enum.ApiMethod.POST:
                request = baseAxios.post(path, data);
                break;
            case Enum.ApiMethod.PUT:
                request = baseAxios.put(path, data);
                break;
            case Enum.ApiMethod.DELETE:
                request = baseAxios.delete(path, { data: data });
                break;
        }
        return request;
    },
};

export default Axios;
