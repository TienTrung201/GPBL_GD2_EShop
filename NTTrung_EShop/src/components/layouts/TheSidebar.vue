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
                    :class="{
                        active: pathName === Enum.Router.Inventory.Name || pathName === Enum.Router.InventoryForm.Name,
                    }"
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
                    @click="handleNavigateToPage(Enum.Router.ItemCategory.Name)"
                    :class="{
                        active: pathName === Enum.Router.ItemCategory.Name,
                    }"
                    class="menuBar__item"
                >
                    <a class="menuBar__item-link" :to="Enum.Router.ItemCategory.Path">
                        <p class="menuBar__icon center">
                            <MISAIcon icon="product"></MISAIcon>
                        </p>
                        <p class="menuBar__content">{{ MISAResource[resource.langCode]?.SideBar?.ItemCategory }}</p>
                    </a>

                    <p class="titleContent center">{{ MISAResource[resource.langCode]?.SideBar?.ItemCategory }}</p>
                </li>
                <li
                    @click="handleNavigateToPage(Enum.Router.Unit.Name)"
                    :class="{
                        active: pathName === Enum.Router.Unit.Name,
                    }"
                    class="menuBar__item"
                >
                    <a class="menuBar__item-link" :to="Enum.Router.Unit.Path">
                        <p class="menuBar__icon center">
                            <MISAIcon icon="product"></MISAIcon>
                        </p>
                        <p class="menuBar__content">{{ MISAResource[resource.langCode]?.SideBar?.Unit }}</p>
                    </a>

                    <p class="titleContent center">{{ MISAResource[resource.langCode]?.SideBar?.Unit }}</p>
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
import { ref, onMounted } from 'vue';
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
const pageNameOld = ref(Enum.Router.Home.Name);
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
    if (pageNameOld.value !== pageName) {
        pageNameOld.value = pageName;
        router.push({ name: pageName });
        handleChangeTab(pageName);
        title.setTitle(MISAResource[resource.langCode]?.SideBar[pageName]);
        return;
    }
};
onMounted(() => {
    pageNameOld.value = pathName.value.replace('/', '');
});
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
