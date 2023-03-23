import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import router from './router'

import './assets/main.css'
import axios from 'axios'

import { useAuthStore } from '@/stores/authStore'

const pinia = createPinia()
const app = createApp(App)

app.use(pinia)
app.use(router)

const authStore = useAuthStore()

axios.defaults.baseURL = 'https://localhost:7053'
axios.interceptors.request.use(config => {
    const token = authStore.accessToken
    if (token) {
        if (config.headers) {
            config.headers.Authorization = `Bearer ${token}`
        } else {
            config.headers = {}
        }
    }
    return config
})

function loadUserFromlocalStorage() {
    const user = localStorage.getItem('user')
    if (user) {
        authStore.$patch(JSON.parse(user))
    }
}

loadUserFromlocalStorage()

app.mount('#app')
