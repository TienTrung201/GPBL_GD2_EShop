<template lang="">
    <div class="overley-modal">
        <div class="ntt-modal-form">
            <div class="ntt-modal-form__header">
                <MISARow justify="space-between">
                    <div class="ntt-modal--right">
                        <h3 class="ntt-modal-form__title">{{ `${action} ${props.object}` }}</h3>
                        <MISARow colGap="24px"><slot name="checkboxs"></slot></MISARow>
                    </div>
                    <button
                        v-tooltip.absoluteTop="Enum.KeyboardShortcuts.Esc"
                        @click="closeModal"
                        class="modal-close center"
                    >
                        <MISAIcon width="14" height="16" icon="close"></MISAIcon>
                    </button>
                </MISARow>
            </div>
            <div class="ntt-modal-form__body">
                <slot> </slot>
            </div>
            <div class="ntt-modal-form__footer">
                <slot name="action"></slot>
            </div>
        </div>
    </div>
</template>
<script setup>
import { storeToRefs } from 'pinia';
import { useModalForm } from '../../../stores/modalform';
import Enum from '../../../common/enum';

const props = defineProps({
    //tên đối tượng của form
    object: {
        type: String,
        require: true,
        default: () => '',
    },
});
const emit = defineEmits(['close-modal']);
const modalForm = useModalForm();
const { action } = storeToRefs(modalForm);

/**
 * Author: Tiến Trung (27/06/2023)
 * Description: hàm emit báo cho cha đóng modal
 */
function closeModal() {
    emit('close-modal');
    // modalForm.close();
}
</script>
<style lang="scss" scoped>
@import './modal-form.scss';
</style>
