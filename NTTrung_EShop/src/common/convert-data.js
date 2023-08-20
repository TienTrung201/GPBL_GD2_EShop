// import { useResource } from '../stores/resource';
import Enum from './enum';
import MISAResource from './resource';

/**
 * @param {string} str
 * Author: ntTrung (06/07/2023)
 * Description: Hàm chuyển chữ tiếng việt có dấu thành k dấu
 */
export function removeVietnameseTones(str) {
    str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, 'a');
    str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, 'e');
    str = str.replace(/ì|í|ị|ỉ|ĩ/g, 'i');
    str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, 'o');
    str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, 'u');
    str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, 'y');
    str = str.replace(/đ/g, 'd');
    str = str.replace(/À|Á|Ạ|Ả|Ã|Â|Ầ|Ấ|Ậ|Ẩ|Ẫ|Ă|Ằ|Ắ|Ặ|Ẳ|Ẵ/g, 'A');
    str = str.replace(/È|É|Ẹ|Ẻ|Ẽ|Ê|Ề|Ế|Ệ|Ể|Ễ/g, 'E');
    str = str.replace(/Ì|Í|Ị|Ỉ|Ĩ/g, 'I');
    str = str.replace(/Ò|Ó|Ọ|Ỏ|Õ|Ô|Ồ|Ố|Ộ|Ổ|Ỗ|Ơ|Ờ|Ớ|Ợ|Ở|Ỡ/g, 'O');
    str = str.replace(/Ù|Ú|Ụ|Ủ|Ũ|Ư|Ừ|Ứ|Ự|Ử|Ữ/g, 'U');
    str = str.replace(/Ỳ|Ý|Ỵ|Ỷ|Ỹ/g, 'Y');
    str = str.replace(/Đ/g, 'D');
    // Some system encode vietnamese combining accent as individual utf-8 characters
    // Một vài bộ encode coi các dấu mũ, dấu chữ như một kí tự riêng biệt nên thêm hai dòng này
    str = str.replace(/\u0300|\u0301|\u0303|\u0309|\u0323/g, ''); // ̀ ́ ̃ ̉ ̣  huyền, sắc, ngã, hỏi, nặng
    str = str.replace(/\u02C6|\u0306|\u031B/g, ''); // ˆ ̆ ̛  Â, Ê, Ă, Ơ, Ư
    // Remove extra spaces
    // Bỏ các khoảng trắng liền nhau
    str = str.replace(/ + /g, ' ');
    str = str.trim();
    // Remove punctuations
    // Bỏ dấu câu, kí tự đặc biệt
    // str = str.replace(
    //   /!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'|\"|\&|\#|\[|\]|~|\$|_|`|-|{|}|\||\\/g,
    //   " "
    // );
    return str;
}

// Author: ntTrung (20/06/2023)
/**
 * @param {string} date
 * Author: Tiến Trung (28/06/2023)
 * Description: Hàm chuyển ngày tháng cùng định dạng với input
 */
export const convertDateForBE = (date) => {
    try {
        if (date === null || date === undefined) {
            return null;
        }
        return convertDate(date).split('/').reverse().join('-');
    } catch (error) {
        console.log(error);
        return '';
    }
};
/**
 * @param {string} dateTime
 * Author: Tiến Trung (20/06/2023)
 * Description: Hàm chuyển dateTime sang ngày/tháng/năm
 */
export function convertDate(dateTime) {
    try {
        if (dateTime === null || dateTime === '') {
            return '';
        }
        const date = dateTime && new Date(dateTime);
        const newDate = date.getDate().toString().padStart(2, '0');
        const month = (date.getMonth() + 1).toString().padStart(2, '0');
        const year = date.getFullYear();
        return `${newDate}/${month}/${year}`;
    } catch (error) {
        console.log(error);
        return '';
    }
}
/**
 * @param {string} gender
 * Author: Tiến Trung (20/06/2023)
 * Description: Hàm chuyển dữ liệu giới tính Nam/Nữ
 */
export function convertGender(gender, langCode) {
    try {
        if (gender === Enum.Gender.Female) {
            return MISAResource[langCode]?.Gender?.Female;
        }
        if (gender === Enum.Gender.Male) {
            return MISAResource[langCode]?.Gender?.Male;
        }
        return MISAResource[langCode]?.Gender?.Other;
    } catch (error) {
        console.log(error);
        return '';
    }
}

/**
 * @param {string} currency
 * Author: Tiến Trung (20/06/2023)
 * Description: Hàm chuyển định dạng tiền sang kiểu VN
 */
export const convertCurrency = (currency) => {
    try {
        let currencyString = currency.toLocaleString('vi-VN');

        return currencyString;
    } catch (error) {
        // console.log(error);
        return '';
    }
};
/**
 * @param {string} arr
 * Author: ntTrung (09/07/2023)
 * Description: Hàm chuyển chữ tiếng việt có dấu thành k dấu
 */
export const convertDataTable = (arr, columnsTable, langCode) => {
    try {
        const dataConvert = arr.map((data) => {
            const convertData = { ...data };
            columnsTable.forEach((column) => {
                const value = convertData[column.key];
                if (column.type === Enum.TypeDataTable.Gender) {
                    convertData[column.key] = convertGender(value, langCode);
                }
                if (column.type === Enum.TypeDataTable.Date) {
                    convertData[column.key] = convertDate(value);
                }
                if (column.type === Enum.TypeDataTable.Money) {
                    convertData[column.key] = convertCurrency(value);
                }
            });
            return convertData;
        });
        return dataConvert;
    } catch (error) {
        console.log(error);
    }
};
/**
 *  * @param {object} obj
 * Author: ntTrung (09/07/2023)
 * Description: sao chép đối tượng
 */
export function deepCopy(obj) {
    // copy the object by the constructor
    const copy = new obj.constructor();
    const keys = Object.keys(obj);
    keys.forEach((key) => {
        copy[key] = obj[key];
    });
    return copy;
}
/**
 * @param {number} number
 * Author: ntTrung (09/07/2023)
 * Description: định dạng số
 */
export function formatNumber(number) {
    const formatter = new Intl.NumberFormat('en-US');
    const formattedNumber = formatter.format(number);
    return formattedNumber;
}
/**
 * @param {string} align
 * Author: ntTrung (06/08/2023)
 * Description: chuyển kiểu align
 */
export function formatAlign(align) {
    let alignConvert = '';
    switch (align) {
        case Enum.AlignColumn.Center:
            alignConvert = Enum.AlignColumnFe.Center;
            break;
        case Enum.AlignColumn.Right:
            alignConvert = Enum.AlignColumnFe.Right;
            break;
        default:
            alignConvert = Enum.AlignColumnFe.Left;
            break;
    }
    return alignConvert;
}
/**
 * @param {string} input
 * Author: Tiến Trung (13/07/2023)
 * Description: Hàm convert chữ thường thành hoa
 */
export function convertToTitleCase(input) {
    if (!input) return '';

    // Tách các từ trong chuỗi bằng khoảng trắng hoặc dấu cách
    const words = input.toLowerCase().split(/[\s]+/);
    // Chuyển chữ cái đầu của từ thành chữ in hoa
    const capitalizedWords = words.map((word) => {
        return word.charAt(0).toUpperCase() + word.slice(1);
    });
    // Ghép các từ đã chuyển thành chuỗi mới
    return capitalizedWords.join(' ');
}
/**
 * @param {string} input
 * Author: Tiến Trung (13/07/2023)
 * Description: Hàm convert Enum filter sang tên filter
 */
export function convertToFilterIcon(typeFilter) {
    let filterName = '';
    switch (typeFilter) {
        case Enum.FilterBy.Equal:
            filterName = '=';
            break;
        case Enum.FilterBy.NotEqual:
            filterName = '≠';
            break;
        case Enum.FilterBy.Contain:
            filterName = '*';
            break;
        case Enum.FilterBy.NotContain:
            filterName = '!';
            break;
        case Enum.FilterBy.Greater:
            filterName = '>';
            break;
        case Enum.FilterBy.Smaller:
            filterName = '<';
            break;
        default:
            break;
    }

    return filterName;
}
