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
            path: Enum.Router.Home.Path,
            name: Enum.Router.Home.Name,
            component: HomePage,
        },
        {
            path: Enum.Router.Employee.Path,
            name: Enum.Router.Employee.Name,
            component: EmployeeList,
        },
        {
            path: Enum.Router.Department.Path,
            name: Enum.Router.Department.Name,
            component: DepartmentList,
        },
        {
            path: '/product',
            name: 'product',
            component: ProductList,
        },
        {
            path: Enum.Router.Position.Path,
            name: Enum.Router.Position.Name,
            component: PositionList,
        },
        {
            path: '/excel',
            name: 'excel',
            component: ImportExcel,
        },
        {
            path: Enum.Router.Inventory.Path,
            name: Enum.Router.Inventory.Name,
            component: Inventory,
        },
        {
            path: Enum.Router.InventoryForm.Path,
            name: Enum.Router.InventoryForm.Name,
            component: InventoryForm,
        },
    ],
});

export default router;
