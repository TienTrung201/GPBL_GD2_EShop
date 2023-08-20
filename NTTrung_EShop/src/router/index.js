import { createRouter, createWebHistory } from 'vue-router';
import EmployeeList from '../Views/employee/EmployeeList.vue';
import ProductList from '../Views/product/ProductList.vue';
import ImportExcel from '../views/excel/ImportExcel.vue';
import DepartmentList from '../views/department/DepartmentList.vue';
import PositionList from '../views/position/PositionList.vue';
import HomePage from '../views/home/HomePage.vue';
import Inventory from '../views/inventory/InventoryList.vue';
import InventoryForm from '../views/inventory/InventoryForm.vue';
import Enum from '../common/enum';
const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        {
            path: Enum.Router.Home.path,
            name: Enum.Router.Home.name,
            component: HomePage,
        },
        {
            path: Enum.Router.Employee.path,
            name: Enum.Router.Employee.name,
            component: EmployeeList,
        },
        {
            path: Enum.Router.Department.path,
            name: Enum.Router.Department.name,
            component: DepartmentList,
        },
        {
            path: '/product',
            name: 'product',
            component: ProductList,
        },
        {
            path: Enum.Router.Position.path,
            name: Enum.Router.Position.name,
            component: PositionList,
        },
        {
            path: '/excel',
            name: 'excel',
            component: ImportExcel,
        },
        {
            path: Enum.Router.Inventory.path,
            name: Enum.Router.Inventory.name,
            component: Inventory,
        },
        {
            path: Enum.Router.InventoryForm.path,
            name: Enum.Router.InventoryForm.name,
            component: InventoryForm,
        },
    ],
});

export default router;
