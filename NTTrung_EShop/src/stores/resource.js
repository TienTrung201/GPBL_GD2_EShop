import { defineStore } from 'pinia';
import Enum from '../common/enum';

export const useResource = defineStore('resource', {
    state: () => ({
        langCode: localStorage.getItem('lang') || Enum.Language.VN,
    }),
    /**
     * Author: Tiến Trung 24/06/2023)
     * Description: cài dặt ngôn ngữ
     */
    actions: {
        setLangCode(langCode) {
            this.langCode = langCode;
            localStorage.setItem('lang', langCode);
        },
    },
});
