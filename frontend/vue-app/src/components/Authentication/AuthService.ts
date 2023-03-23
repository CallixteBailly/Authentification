import type { ILoginRequest, IVerifyTokenRequest } from '@/stores/auth.type'
import { useAuthStore } from '@/stores/authStore'

export default {
  setup() {
    const authStore = useAuthStore()

    const handleLogin = (request: ILoginRequest) => {
      authStore.login(request)
    }

    const handleVerifyToken = (request: IVerifyTokenRequest) => {
      authStore.verifyToken(request)
    }

    return {
      handleLogin,
      handleVerifyToken
    }
  }
}
