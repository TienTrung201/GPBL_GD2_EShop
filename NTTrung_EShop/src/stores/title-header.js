import { defineStore } from 'pinia';

export const useTitleHeader = defineStore('title', {
    state: () => ({
        title: '',
        titleSec: '',
    }),
    /**
     * Author: Tiến Trung 24/06/2023)
     * Description: cài dặt ngôn ngữ
     */
    actions: {
        setTitle(title, titleSec) {
            this.title = title;
            this.titleSec = titleSec;
        },
    },
});
