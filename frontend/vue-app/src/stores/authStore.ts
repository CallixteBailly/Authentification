import { defineStore } from 'pinia'
import axios from 'axios'
import type { ILoginRequest, IVerifyTokenRequest, IUser } from './auth.type'

export const useAuthStore = defineStore({
    id: 'auth',
    state: (): IUser => ({
        userId: '',
        firstName: '',
        lastName: '',
        email: '',
        accessToken: '',
        refreshToken: '',
        tokenExpiration: '',
        isAuthenticated: false
    }),
    actions: {
        async login(request: ILoginRequest) {
            const response = await axios.post('/authentication/login', request)
            const user: IUser = {
                userId: response.data.userId,
                firstName: response.data.firstName,
                lastName: response.data.lastName,
                email: response.data.email,
                accessToken: response.data.token.accessToken,
                refreshToken: response.data.token.refreshToken,
                tokenExpiration: response.data.token.expiration,
                isAuthenticated: true
            }
            this.$patch(user)
            localStorage.setItem('accessToken', user.accessToken ?? '')
            localStorage.setItem('refreshToken', user.refreshToken ?? '')
            localStorage.setItem('tokenExpiration', user.tokenExpiration ?? '')
            localStorage.setItem('user', JSON.stringify(user))
        },
        async verifyToken(request: IVerifyTokenRequest) {
            const response = await axios.post('/authentication/verify-token', request)
            this.accessToken = response.data.accessToken
            this.tokenExpiration = response.data.expiration

            const user = localStorage.getItem('user')
            if (user) {
                this.$patch(JSON.parse(user))
            }
        },
        logout() {
            const user: IUser = {
                userId: '',
                firstName: '',
                lastName: '',
                email: '',
                accessToken: '',
                refreshToken: '',
                tokenExpiration: '',
                isAuthenticated: false
            }
            this.$patch(user)
            localStorage.removeItem('user')
        },
        // Load user data from localStorage on app load
        loadUserFromLocalStorage() {
            const user = JSON.parse(localStorage.getItem('user') || '{}')
            this.userId = user.userId
            this.firstName = user.firstName
            this.lastName = user.lastName
            this.email = user.email
            this.isAuthenticated = user.isAuthenticated
            this.accessToken = localStorage.getItem('accessToken') || ''
            this.refreshToken = localStorage.getItem('refreshToken') || ''
            this.tokenExpiration = localStorage.getItem('tokenExpiration') || ''
        }
    }
})
