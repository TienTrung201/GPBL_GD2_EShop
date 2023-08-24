<template lang="">
    <!-- sidebar close-sidebar -->
    <div ref="sideBarElement" class="" :class="['sidebar', { 'close-sidebar': sideBar }]">
        <nav>
            <ul class="menuBar">
                <li
                    @click="handleNavigateToPage(Enum.Router.Home.Name)"
                    :class="{ active: pathName === '' || pathName === Enum.Router.Home.Name }"
                    class="menuBar__item"
                >
                    <a class="menuBar__item-link" to="/">
                        <p class="menuBar__icon center">
                            <MISAIcon icon="home"></MISAIcon>
                        </p>
                        <p class="menuBar__content">{{ MISAResource[resource.langCode]?.SideBar?.Home }}</p>
                    </a>

                    <p class="titleContent center">{{ MISAResource[resource.langCode]?.SideBar?.Home }}</p>
                </li>
                <li
                    @click="handleNavigateToPage(Enum.Router.Inventory.Name)"
                    :class="{ active: pathName === Enum.Router.Inventory.Name }"
                    class="menuBar__item"
                >
                    <a class="menuBar__item-link" :to="Enum.Router.Inventory.Path">
                        <p class="menuBar__icon center">
                            <MISAIcon icon="product"></MISAIcon>
                        </p>
                        <p class="menuBar__content">{{ MISAResource[resource.langCode]?.SideBar?.Inventory }}</p>
                    </a>

                    <p class="titleContent center">{{ MISAResource[resource.langCode]?.SideBar?.Inventory }}</p>
                </li>
                <li
                    @click="handleNavigateToPage(Enum.Router.Department.Name)"
                    :class="{ active: pathName === Enum.Router.Department.Name }"
                    class="menuBar__item"
                >
                    <a class="menuBar__item-link" :to="Enum.Router.Department.Path">
                        <p class="menuBar__icon center">
                            <MISAIcon icon="product"></MISAIcon>
                        </p>
                        <p class="menuBar__content">{{ MISAResource[resource.langCode]?.SideBar?.Department }}</p>
                    </a>

                    <p class="titleContent center">{{ MISAResource[resource.langCode]?.SideBar?.Department }}</p>
                </li>

                <li
                    @click="handleNavigateToPage(Enum.Router.Employee.Name)"
                    :class="{ active: pathName === Enum.Router.Employee.Name }"
                    class="menuBar__item"
                >
                    <a class="menuBar__item-link" :to="Enum.Router.Employee.Path">
                        <p class="menuBar__icon center">
                            <MISAIcon icon="profile"></MISAIcon>
                        </p>
                        <p class="menuBar__content">{{ MISAResource[resource.langCode]?.SideBar?.Employee }}</p>
                    </a>
                    <div class="titleItem center">
                        <p class="titleContent center">{{ MISAResource[resource.langCode]?.SideBar?.Employee }}</p>
                    </div>
                </li>

                <li
                    @click="handleNavigateToPage(Enum.Router.Position.Name)"
                    :class="{ active: pathName === Enum.Router.Position.Name }"
                    class="menuBar__item"
                >
                    <a class="menuBar__item-link" :to="Enum.Router.Position.Path">
                        <p class="menuBar__icon center">
                            <MISAIcon icon="position"></MISAIcon>
                        </p>
                        <p class="menuBar__content">{{ MISAResource[resource.langCode]?.SideBar?.Position }}</p>
                    </a>
                    <div class="titleItem center">
                        <p class="titleContent center">{{ MISAResource[resource.langCode]?.SideBar?.Position }}</p>
                    </div>
                </li>
            </ul>
        </nav>
        <!-- <div @click="handleClickZoomOutSidebar()" class="sideBar-close">
            <div class="sideBar-close__icon">
                <p class="menuBar__icon center">
                    <MISAIcon width="14" height="14" icon="left"></MISAIcon>
                </p>
            </div>
            <p class="sideBar-close__content">{{ MISAResource[resource.langCode]?.SideBar?.CloseSidebar }}</p>
        </div> -->
    </div>
    <!-- sidebar -->
</template>
<script setup>
import { ref } from 'vue';
import router from '../../router';
import { useResource } from '../../stores/resource.js';
import MISAResource from '../../common/resource.js';
import Enum from '../../common/enum';
import { useTitleHeader } from '../../stores/title-header';
import { convertToTitleCase } from '../../common/convert-data';
const title = useTitleHeader();
const resource = useResource();
const pathName = ref(window.location.pathname.split('/')[1]);
const sideBarElement = ref(null);

/**
 * @param {string} tab
 * Author: Tiến Trung (21/06/2023)
 * Description: Hàm mục đích khi điều hướng thay đổi thêm active vào điều hướng đó
 */
const handleChangeTab = (tab) => {
    pathName.value = tab;
};
/**
 * @param {string} pageName
 * Author: Tiến Trung (21/06/2023)
 * Description: Hàm chuyển trang khi click vào sidebar
 */
const handleNavigateToPage = (pageName) => {
    router.push({ name: pageName });
    handleChangeTab(pageName);
    title.setTitle(MISAResource[resource.langCode]?.SideBar[convertToTitleCase(pageName)]);
};
/**
 * Author: Tiến Trung (21/06/2023)
 * Description: Hàm đóng mở sideBar
 */
const sideBar = ref(false);
// const handleClickZoomOutSidebar = () => {
//     sideBar.value = !sideBar.value;
//     sideBarElement.value.style.zIndex = sideBar.value ? 2 : 1;
// };
</script>
<style lang="scss">
@import './thesidebar.scss';
</style>
