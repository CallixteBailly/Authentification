import AboutViewVue from '@/views/AboutView.vue'
import LoginViewVue from '@/views/Login.vue'
import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
      path: '/about',
      name: 'about',
      // component: () => import('../views/AboutView.vue')
      component: AboutViewVue
    },
    {
      path: '/login',
      name: 'login',
      component: LoginViewVue
    }
  ]
})

export default router
