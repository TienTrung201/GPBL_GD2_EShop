<script setup>
import { storeToRefs } from 'pinia';
import { useDialog } from '../../../stores/dialog';
import Enum from '../../../common/enum';
import { onMounted, ref, onUpdated, onUnmounted } from 'vue';
import { useModalForm } from '../../../stores/modalform';

const emit = defineEmits(['delete-data', 'submit-form']);
const dialog = useDialog();
const modal = useModalForm();
const iPriBtn = ref(null);
const iSecBtn = ref(null);
const iclose = ref(null);
const { actionDialog, buttonThird, title, content, type, buttonSec, icon, method, errorMessage, loadingBtn } =
    storeToRefs(dialog);
/**
 * Author: Tiến Trung (30/06/2023)
 * Description: Hàm click đóng dialog và modal
 */
const handleClose = () => {
    // dialog.setMethod(modal.method);
    dialog.close();
    dialog.navigationLink();
};

/**
 * Author: Tiến Trung (30/06/2023)
 * Description: Hàm click submit dialog
 */
const submitDialog = () => {
    if (method.value === Enum.EditMode.None) {
        dialog.close();
    }
    if (
        method.value === Enum.EditMode.Add ||
        method.value === Enum.EditMode.Update ||
        method.value === Enum.EditMode.Copy
    ) {
        dialog.setLoading(true);
        dialog.saveData(false);
    }
    if (method.value === Enum.EditMode.Delete) {
        dialog.setLoading(true);
        dialog.saveData();
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
    if (iSecBtn.value) {
        iSecBtn.value.autoFocus();
    } else {
        iPriBtn.value.autoFocus();
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
                    <h3 ntt class="modal-title">{{ title }}</h3>
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
                        <MISAIcon width="24" height="24" :icon="icon"></MISAIcon>
                    </p>
                    <template v-if="errorMessage.length > 0">
                        <MISACol
                            ><p>{{ errorMessage }}</p>
                        </MISACol>
                    </template>
                    <p>{{ content }}</p>
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
                            ref="iSecBtn"
                            :type="Enum.ButtonType.Sec"
                            @click="handleClose"
                            v-if="buttonSec !== undefined"
                            :action="buttonSec"
                        ></MISAButton>

                        <MISAButton
                            @keydown.tab="iclose.focus()"
                            ref="iPriBtn"
                            :loading="loadingBtn"
                            @click="submitDialog"
                            :type="type"
                            :action="actionDialog"
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
