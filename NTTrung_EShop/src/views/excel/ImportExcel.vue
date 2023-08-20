<template>
    <div class="container">
        <div class="choseFile">
            <MISAButton @click="handleFileInput" type="pri" action="Choose Excel File"></MISAButton>
            <input ref="input" @change="handleFileChange" type="file" id="file" class="file-input" />
            <div class="file-name" id="file-name">{{ fileName ? fileName : 'No file selected' }}</div>
        </div>
        <div class="saveImport">
            <MISAButton :type="Enum.ButtonType.Pri" action="Nhập file"></MISAButton>
        </div>
    </div>
</template>
<script setup>
import { ref } from 'vue';
import Enum from '../../common/enum';

const fileName = ref(null);
const input = ref(null);
/**
 * Author: Tiến Trung (24/07/2023)
 * Description: click vào input
 */
function handleFileInput() {
    input.value.click();
}
/**
 * Author: Tiến Trung (24/07/2023)
 * Description: Chọn file
 */
function handleFileChange(event) {
    const file = event.target.files[0];
    if (file) {
        fileName.value = file.name;
    } else {
        fileName.value = null;
    }
}
</script>
<style lang="scss">
.choseFile {
    display: flex;
    column-gap: 12px;
    align-items: center;
}
.saveImport {
    padding-top: 50px;
    display: flex;
    justify-content: flex-end;
}
.container {
    background-color: #fff;
    border-radius: 8px;
    padding: 20px;
    box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
    width: 100%;
    text-align: center;
}

.file-input {
    display: none;
}

.file-label {
    background-color: #007bff;
    color: #fff;
    padding: 10px 20px;
    border-radius: 4px;
    cursor: pointer;
    transition: background-color 0.2s;
}

.file-label:hover {
    background-color: #0056b3;
}

.file-name {
    font-size: 13px;
}
</style>
