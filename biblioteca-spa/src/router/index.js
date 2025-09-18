import { createRouter, createWebHistory } from "vue-router";
import Generos from "../views/Generos.vue";
import Autores from "../views/Autores.vue";
import Livros from "../views/Livros.vue";

const routes = [
  { path: "/", redirect: "/generos" },
  { path: "/generos", component: Generos },
  { path: "/autores", component: Autores },
  { path: "/livros", component: Livros }
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

export default router;
