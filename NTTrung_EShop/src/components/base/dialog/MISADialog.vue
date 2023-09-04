<script setup>
import { storeToRefs } from 'pinia';
import { useDialog } from '../../../stores/dialog';
import Enum from '../../../common/enum';
import { onMounted, ref, onUpdated, onUnmounted } from 'vue';

const dialog = useDialog();
const iPriBtn = ref(null);
const iSecBtn = ref(null);
const iclose = ref(null);
const { actionDialog, buttonThird, content, type, buttonSec, icon, method, errorMessage, loadingBtn } =
    storeToRefs(dialog);
/**
 * Author: Tiến Trung (30/06/2023)
 * Description: Hàm click đóng dialog và modal
 */
const handleClose = () => {
    // dialog.setMethod(modal.method);
    dialog.close();
    dialog.closeNavigationLink();
    dialog.setCloseNavigationLink(() => {});
};

/**
 * Author: Tiến Trung (30/06/2023)
 * Description: Hàm click submit dialog
 */
const submitDialog = () => {
    try {
        switch (method.value) {
            case Enum.EditMode.None:
                dialog.close();
                break;
            default:
                dialog.setLoading(true);
                dialog.saveData(false);
                break;
        }
    } catch (error) {
        console.log(error);
    }
};
/**
 * Author: Tiến Trung (30/06/2023)
 * Description: Khi dialog được update trả lại trạng thái loading và fucus lại button
 */
onUpdated(() => {
    // isLoadingButton.value = false;
    if (!loadingBtn.value) {
        autoFocusButton();
    }
});
/**
 * Author: Tiến Trung (30/06/2023)
 * Description: hàm tự động focus vào button
 */
const autoFocusButton = () => {
    if (iPriBtn.value) {
        iPriBtn.value.autoFocus();
    } else {
        iSecBtn.value.autoFocus();
    }
};
/**
 * Author: Tiến Trung (30/06/2023)
 * Description: hàm close dialog bằng phím
 */
const onCloseDialog = (e) => {
    if (e.keyCode === Enum.Keyboard.ESC) {
        e.preventDefault();
        dialog.close();
    }
};
/**
 * Author: Tiến Trung (30/06/2023)
 * Description: Khi dialog được hiện tự động focus vào buton, thêm sự kiện phím cho dialog
 */
onMounted(() => {
    autoFocusButton();
    window.addEventListener('keydown', onCloseDialog);
});
/**
 * Author: Tiến Trung (30/06/2023)
 * Description: Khi dialog Hủy remove event
 */
onUnmounted(() => {
    window.removeEventListener('keydown', onCloseDialog);
});
</script>
<template lang="">
    <div class="overley-dialog">
        <div @keydown.esc="dialog.close()" @click.stop="" class="modal-dialog savett">
            <div class="modal-content">
                <div class="modal-header">
                    <h3 ntt class="modal-title">{{ 'MISA eShop' }}</h3>
                    <button
                        v-tooltip.absoluteTop="Enum.KeyboardShortcuts.Esc"
                        ref="iclose"
                        @click="dialog.close()"
                        class="modal-close center"
                    >
                        <MISAIcon width="14" height="16" icon="close"></MISAIcon>
                    </button>
                </div>
                <div class="modal-body">
                    <p v-if="icon" class="modal-icon center">
                        <MISAIcon width="38" height="38" :icon="icon"></MISAIcon>
                    </p>
                    <template v-if="errorMessage.length > 0">
                        <MISACol
                            ><p>{{ errorMessage }}</p>
                        </MISACol>
                    </template>
                    <p class="dialog-content" v-html="content"></p>
                </div>
                <div class="modal-footer">
                    <!-- <button @click="handleClose" v-if="buttonSec.length > 0" class="btn-sec btn-base btn-close-dialog">
                        {{ buttonSec }}
                    </button> -->
                    <div>
                        <MISAButton v-if="buttonThird" @click="dialog.close()" type="sec" :action="buttonThird">
                        </MISAButton>
                    </div>
                    <div>
                        <MISAButton
                            ref="iPriBtn"
                            :loading="loadingBtn"
                            @click="submitDialog"
                            :type="type"
                            :action="actionDialog"
                        ></MISAButton>
                        <MISAButton
                            @keydown.tab="iclose.focus()"
                            ref="iSecBtn"
                            :type="Enum.ButtonType.Sec"
                            @click="handleClose"
                            v-if="buttonSec !== undefined"
                            :action="buttonSec"
                        ></MISAButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<style lang="scss">
@import './dialog.scss';
</style>
