/**
 * @param {string} value chuỗi kiểm tra
 * @param {Array} rules hàm cần kiểm tra
 * Author: Tiến Trung (29/06/2023)
 * Description: Hàm validate
 */
export const Validator = (value, rules = []) => {
    let errorMessage = '';
    if (rules && rules.length > 0) {
        for (let i = 0; i < rules.length; i++) {
            errorMessage = rules[i].test(value);
            if (errorMessage) {
                return errorMessage;
            }
        }
    }
    return '';
};
/**
 * @param {String} message
 * Author: Tiến Trung (29/06/2023)
 * Description: validate bắt buộc phải nhập
 */
Validator.isRequired = (message) => {
    return {
        test: (value) => {
            if (Array.isArray(value)) {
                return value.length ? '' : message || 'Vui lòng nhập trường này';
            } else {
                return value ? '' : message || 'Vui lòng nhập trường này';
            }
        },
    };
};
/**
 * @param {String} message
 * Author: Tiến Trung (29/06/2023)
 * Description: validate định dạng email
 */
Validator.isEmail = (message) => {
    return {
        test: (value) => {
            if (value === '' || value === null || value === undefined) return '';
            var regex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
            return regex.test(value) ? undefined : message || 'Vui lòng nhập email';
        },
    };
};
/**
 * @param {String} message
 * Author: Tiến Trung (29/06/2023)
 * Description: validate định dạng mã
 */
Validator.isCode = (message) => {
    return {
        test: (value) => {
            if (value === '' || value === null || value === undefined) return '';
            var regex = /^[A-Za-z]+[0-9]*$/;
            return regex.test(value) ? undefined : message || 'Mã không hợp lệ';
        },
    };
};
/**
 * @param {String} message
 * Author: Tiến Trung (29/06/2023)
 * Description: validate độ dài nhỏ nhất
 */
Validator.minLength = (min, message) => {
    return {
        test: (value) => {
            if (value === '' || value === null || value === undefined) return '';
            return value.length >= min ? undefined : message || `Vui lòng nhập tối thiểu ${min} kí tự`;
        },
    };
};
/**
 * @param {Number} max
 * @param {String} message
 * Author: Tiến Trung (29/06/2023)
 * Description: validate độ dài lớn nhất
 */
Validator.maxLength = (max, message) => {
    return {
        test: (value) => {
            if (value === '' || value === null || value === undefined) return '';
            return value.length <= max
                ? undefined
                : message.replace('@number', max) || `Vui lòng nhập tối đa ${max} kí tự`;
        },
    };
};
/**
 * @param {Number} max
 * @param {String} message
 * Author: Tiến Trung (29/06/2023)
 * Description: validate độ dài nhỏ nhát
 */
Validator.minLength = (max, message) => {
    return {
        test: (value) => {
            if (value === '' || value === null || value === undefined) return '';
            return value.length >= max
                ? undefined
                : message.replace('@number', max) || `Vui lòng nhập tối thiểu ${max} kí tự`;
        },
    };
};
