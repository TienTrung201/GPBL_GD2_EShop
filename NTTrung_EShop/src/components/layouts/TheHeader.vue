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
                <ul class="menuBar">
                <li
                    @click="handleNavigateToPage(Enum.Router.Inventory.Name)"
                    :class="{
                        active: pathName === Enum.Router.Inventory.Name || pathName === Enum.Router.InventoryForm.Name,
                    }"
                    class="menuBar__item"
                >
                    <a class="menuBar__item-link" :to="Enum.Router.Inventory.Path">
                        <!-- <p class="menuBar__icon center">
                            <MISAIcon icon="product"></MISAIcon>
                        </p> -->
                        <p class="menuBar__content">{{ MISAResource[resource.langCode]?.SideBar?.Inventory }}</p>
                    </a>

                  
                </li>
                <li
                    @click="handleNavigateToPage(Enum.Router.ItemCategory.Name)"
                    :class="{
                        active: pathName === Enum.Router.ItemCategory.Name,
                    }"
                    class="menuBar__item"
                >
                    <a class="menuBar__item-link" :to="Enum.Router.ItemCategory.Path">
                        <!-- <p class="menuBar__icon center">
                            <MISAIcon icon="process"></MISAIcon>
                        </p> -->
                        <p class="menuBar__content">{{ MISAResource[resource.langCode]?.SideBar?.ItemCategory }}</p>
                    </a>

                  
                </li>
                <li
                    @click="handleNavigateToPage(Enum.Router.Unit.Name)"
                    :class="{
                        active: pathName === Enum.Router.Unit.Name,
                    }"
                    class="menuBar__item"
                >
                    <a class="menuBar__item-link" :to="Enum.Router.Unit.Path">
                        <!-- <p class="menuBar__icon center">
                            <MISAIcon icon="calc"></MISAIcon>
                        </p> -->
                        <p class="menuBar__content">{{ MISAResource[resource.langCode]?.SideBar?.Unit }}</p>
                    </a>

                  
                </li>
            </ul>
            </div>
            <div class="header__controll center">
               
                <div class="wrapper-avatar">
                    <div class="avatar">
                        <img src="../../assets/icons/minhson.jpg" alt="" />
                    </div>
                    <p class="name-user">Kiều Nguyễn Minh Sơn</p>
                </div>
                <div class="controll__menu">
                    <!-- <ul class="controll__List">
                        <li class="controll__Item center">
                            <MISAIcon width="18" height="18" icon="message"></MISAIcon>
                        </li>
                        <li class="controll__Item center">
                            <MISAIcon width="16" height="18" icon="bell"></MISAIcon>
                        </li>
                        <li class="controll__Item center">
                            <MISAIcon width="18" height="18" icon="question"></MISAIcon>
                        </li>
                    </ul> -->
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
import router from '../../router';
const resource = useResource();
const title = useTitleHeader();
const accountOption = ref([
    {
        option: 'Kiều Nguyễn Minh Sơn',
        value: 'Kiều Nguyễn Minh Sơn',
    },
]);
const account = ref('Kiều Nguyễn Minh Sơn');
const pathName = ref(window.location.pathname.split('/')[1]);
const sideBarElement = ref(null);
const pageNameOld = ref(Enum.Router.Home.Name);
const handleChangeTab = (tab) => {
    pathName.value = tab;
};
const handleNavigateToPage = (pageName) => {
    if (pageNameOld.value !== pageName) {
        pageNameOld.value = pageName;
        router.push({ name: pageName });
        handleChangeTab(pageName);
        title.setTitle(MISAResource[resource.langCode]?.SideBar[pageName]);
        return;
    }
};
/**
 * Author: Tiến Trung (11/07/2023)
 * Description: Hàm thay đổi ngôn ngữ
 */

// const handleChangeLangCode = (langCode) => {
//     if (langCode === resource.langCode) {
//         return;
//     }
//     location.reload();
//     resource.setLangCode(langCode);
// };
/**
 * Author: Tiến Trung (11/07/2023)
 * Description: khi component được tạo cài đặt title trang web cho web
 */
onMounted(() => {
    document.title = MISAResource[resource.langCode]?.Title;
    const path = window.location.pathname.split('/')[1];
    title.setTitle(MISAResource[resource.langCode].SideBar[path ? path : 'Home']);
    pageNameOld.value = pathName.value.replace('/', '');

});
</script>
<style lang="scss">
@import './theheader.scss';
@import './thesidebar.scss';
</style>
