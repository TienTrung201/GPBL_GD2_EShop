<template lang="">
    <header class="header">
        <div class="header__left">
            <!-- <div class="header__toggle">
                <MISAIcon width="16" height="16" icon="toggle"></MISAIcon>
            </div> -->
            <div class="header__logo"></div>
            <!-- <h1 class="header__title">
                {{ MISAResource[resource.langCode]?.ProductName }}
            </h1> -->
        </div>
        <div class="header__right">
            <div class="header__menu">
                <!-- <p class="center">
                    <MISAIcon icon="menu"></MISAIcon>
                </p> -->
                <h1 class="header__right-title" :class="{ active: title.titleSec }">
                    {{ title.title }}<span v-if="title.titleSec">{{ ' / ' + title.titleSec }}</span>
                </h1>
            </div>
            <div class="header__controll center">
                <!-- <div class="header__controll__language"> -->
                <p
                    @click="handleChangeLangCode(Enum.Language.VN)"
                    :class="{ active: resource.langCode === Enum.Language.VN }"
                    class="center"
                >
                    <MISAIcon width="24" height="18" icon="flag-vn"></MISAIcon>
                </p>
                <p
                    @click="handleChangeLangCode(Enum.Language.EN)"
                    :class="{ active: resource.langCode === Enum.Language.EN }"
                    class="center"
                >
                    <MISAIcon width="24" height="18" icon="flag-en"></MISAIcon>
                </p>
                <MISADropdown v-model:value="account" :options="accountOption" selectEmpty></MISADropdown>
                <div class="wrapper-avatar">
                    <div class="avatar">
                        <img src="../../assets/icons/avatar-default.png" alt="" />
                    </div>
                    <p class="name-user">User</p>
                </div>
                <div class="controll__menu">
                    <ul class="controll__List">
                        <li class="controll__Item center">
                            <MISAIcon width="18" height="18" icon="message"></MISAIcon>
                        </li>
                        <li class="controll__Item center">
                            <MISAIcon width="16" height="18" icon="bell"></MISAIcon>
                        </li>
                        <li class="controll__Item center">
                            <MISAIcon width="18" height="18" icon="question"></MISAIcon>
                        </li>
                    </ul>
                </div>
                <!-- </div> -->
                <!-- <img class="header__controll-icon" src="../../assets/icons/cloud.svg" alt="" /> -->
                <!-- <img class="header__controll-icon" src="../../assets/icons/bell.png" alt="" /> -->
                <!-- <img class="header__controll-icon" src="../../assets/icons/question-header.png" alt="" /> -->
            </div>
        </div>
    </header>
</template>
<script setup>
import { useResource } from '../../stores/resource.js';
import { useTitleHeader } from '../../stores/title-header';
import MISAResource from '../../common/resource.js';
import Enum from '../../common/enum';
import { onMounted, ref } from 'vue';

const resource = useResource();
const title = useTitleHeader();
const accountOption = ref([
    {
        option: 'User',
        value: 'User',
    },
]);
const account = ref('User');
/**
 * Author: Tiến Trung (11/07/2023)
 * Description: Hàm thay đổi ngôn ngữ
 */

const handleChangeLangCode = (langCode) => {
    if (langCode === resource.langCode) {
        return;
    }
    location.reload();
    resource.setLangCode(langCode);
};
/**
 * Author: Tiến Trung (11/07/2023)
 * Description: khi component được tạo cài đặt title trang web cho web
 */
onMounted(() => {
    document.title = MISAResource[resource.langCode]?.Title;
    const path = window.location.pathname.split('/')[1];
    title.setTitle(MISAResource[resource.langCode].SideBar[path ? path : 'Home']);
});
</script>
<style lang="scss">
@import './theheader.scss';
</style>
