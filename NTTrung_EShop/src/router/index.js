import { createRouter, createWebHistory } from 'vue-router';
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
            path: Enum.Router.Inventory.Path,
            name: Enum.Router.Inventory.Name,
            component: Inventory,
        },
        {
            path: Enum.Router.InventoryForm.Path,
            name: Enum.Router.InventoryForm.Name,
            component: InventoryForm,
            children: [
                {
                    path: Enum.Router.InventoryForm.PathDetail,
                    name: Enum.Router.InventoryForm.NameDetail,
                    component: InventoryForm,
                },
            ],
        },
        // {
        //     path: Enum.Router.InventoryForm.PathChildren,
        //     name: Enum.Router.InventoryForm.Name,
        //     component: InventoryForm,
        // },
    ],
});

export default router;
