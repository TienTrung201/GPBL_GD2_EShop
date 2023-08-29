import './css/main.css';
import 'tippy.js/dist/tippy.css';
import 'tippy.js/dist/svg-arrow.css';
import { createApp } from 'vue';
import { createPinia } from 'pinia';
import VueTippy from 'vue-tippy';
import App from './App.vue';
import router from './router';
import { tooltipDerective } from './custom-derective';
//Component
import MISAButton from './components/base/button/MISAButton.vue';
import MISADatePicker from './components/base/datepicker/MISADatePicker.vue';
import MISAGroupButton from './components/base/button/MISAGroupButton.vue';
import MISAIcon from './components/base/icon/MISAIcon.vue';
import MISAInput from './components/base/input/MISAInput.vue';
import MISATextArea from './components/base/input/MISATextArea.vue';
import MISARadioGroup from './components/base/input/MISARadioGroup.vue';
import MISARadio from './components/base/input/MISARadio.vue';
import MISACheckbox from './components/base/input/MISACheckbox.vue';
import MISAGroupInput from './components/base/input/MISAGroupInput.vue';
import MISAModalForm from './components/base/modalform/MISAModalForm.vue';
import MISADialog from './components/base/dialog/MISADialog.vue';
import MISAToast from './components/base/toastmessage/MISAToast.vue';
import MISADropdown from './components/base/dropdown/MISADropdown.vue';
import MISASelectFilter from './components/base/dropdown/MISASelectFilter.vue';
import MISARow from './components/base/grid/MISARow.vue';
import MISACol from './components/base/grid/MISACol.vue';
import MISAInputManyTag from './components/base/input-many-tag/MISAInputManyTag.vue';
import MISALoading from './components/base/loading/MISALoading.vue';

const app = createApp(App);

app.directive('tooltip', tooltipDerective);

app.use(createPinia());
app.component('MISAModalForm', MISAModalForm);
app.component('MISAToast', MISAToast);
app.component('MISADialog', MISADialog);
app.component('MISAButton', MISAButton);
app.component('MISAGroupButton', MISAGroupButton);
app.component('MISAIcon', MISAIcon);
app.component('MISARow', MISARow);
app.component('MISACol', MISACol);
app.component('MISAInput', MISAInput);
app.component('MISATextArea', MISATextArea);
app.component('MISAGroupInput', MISAGroupInput);
app.component('MISARadioGroup', MISARadioGroup);
app.component('MISARadio', MISARadio);
app.component('MISACheckbox', MISACheckbox);
app.component('MISADropdown', MISADropdown);
app.component('MISASelectFilter', MISASelectFilter);
app.component('MISADatePicker', MISADatePicker);
app.component('MISALoading', MISALoading);
app.component('MISAInputManyTag', MISAInputManyTag);

app.use(VueTippy, {
    directive: 'tooltip-tippy',
    defaultProps: {
        delay: [500, 0],
    },
});
app.use(router);

app.mount('#app');
