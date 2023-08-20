<template lang="">
    <div class="ntt-group-toast-message">
        <div v-for="(toast, index) in listToast" :key="index" :class="`toast toast-${toast.type}`">
            <div class="left-toast">
                <p class="i-toast center">
                    <MISAIcon class="icon-toast" :icon="toast.type"></MISAIcon>
                </p>
                <p class="toast-title">{{ toast.title }}</p>
                <p class="toast-text">{{ toast.content }}</p>
            </div>
            <div class="right-toast">
                <!-- <p class="toast-action">{{ MISAResource[resource.langCode]?.Toast?.Undo }}</p> -->
                <button @click="removeToast(toast)" class="btn-close center close-dialog">
                    <img src="../../../assets/icons/close.svg" alt="" />
                </button>
            </div>
        </div>
        <!-- <div class="toast toast-error none">
            <div class="left-toast">
                <img class="i-toast" src="../../../assets/icons//exclamation-triangle.svg" alt="ntt" />
                <p class="toast-title">Thất bại</p>
                <p class="toast-text">Không thể cập nhật dữ liệu. Vui lòng kiểm tra lại kết nối internet</p>
            </div>
            <div class="right-toast">
                <p class="toast-action">Xem hướng dấn</p>
            </div>
        </div>
        <div class="toast toast-warning none">
            <div class="left-toast">
                <img class="i-toast" src="../../../assets/icons/wranning-icon.svg" alt="ntt" />
                <p class="toast-title">Cảnh báo!</p>
                <p class="toast-text">Dữ liệu của bạn đã bị thay đổi bởi người khác</p>
            </div>
            <div class="right-toast">
                <button class="btn-close center">
                    <img src="../../../assets/icons/close.svg" alt="" />
                </button>
            </div>
        </div>
        <div class="toast toast-info none">
            <div class="left-toast">
                <img class="i-toast" src="../../../assets/icons/info-icon.svg" alt="ntt" />
                <p class="toast-title">Thông tin!</p>
                <p class="toast-text">Bạn nên đọc kĩ nội dung trước khi gửi phê duyệt</p>
            </div>
            <div class="right-toast">
                <button class="btn-close center">
                    <img src="../../../assets/icons/close.svg" alt="" />
                </button>
            </div>
        </div> -->
    </div>
</template>
<script setup>
import { storeToRefs } from 'pinia';
import { useToast } from '../../../stores/toast';
// import { useResource } from '../../../stores/resource.js';
// import MISAResource from '../../../common/resource';
const toast = useToast();
const { listToast } = storeToRefs(toast);
// const resource = useResource();
/**
 * Author: Tiến Trung (2/07/2023)
 * Description: Hàm xóa toast message(Đóng)
 */
const removeToast = (toastId) => {
    toast.remove(toastId);
};
</script>
<style lang="scss">
@import './toast.scss';
</style>
