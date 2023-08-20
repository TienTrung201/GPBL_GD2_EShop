import Enum from '../common/enum';
import Axios from './axios.ext';
import baseAxios from './base-axios';
// import baseAxios from './baseaxios';
/**
 * Author: Tiến Trung (06/07/2023)
 * Description: base Api cho Employee
 */
const employeeApi = {
    method: Enum.ApiMethod.GET,
    param: {},
    path: '/Employees',
    data: {},
    request(subPath) {
        let urlPath = this.path;
        if (subPath && subPath.length > 0) {
            urlPath = `${this.path}/${subPath}`;
        }
        return Axios.request(urlPath, this.param, this.method, this.data);
    },

    // getAll(limit) {
    //     const url = '/Employees';
    //     return baseAxios.get(url, {
    //         params: {
    //             limit,
    //         },
    //     });
    // },
    /**
     * @param {Array} data //Mảng cột được truyền vào
     * Author: Tiến Trung (04/08/2023)
     * Description: Hàm gọi Api xuất excel
     */
    downloadExcel(data) {
        const url = `/Employees/Excel`;
        const parsedData = JSON.parse(JSON.stringify(data));
        return baseAxios.post(url, parsedData, {
            responseType: 'arraybuffer',
        });
    },

    // create(data) {
    //     const url = '/Employees';
    //     return baseAxios.post(url, data);
    // },

    // update(id, data) {
    //     const url = `/Employees/${id}`;
    //     return baseAxios.put(url, data);
    // },

    // delete(id) {
    //     const url = `/Employees/${id}`;
    //     return baseAxios.delete(url);
    // },
};

export default employeeApi;
