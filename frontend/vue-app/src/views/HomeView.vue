<script lang="ts">
import TheWelcome from '../components/TheWelcome.vue'
import { computed } from 'vue'
import { useAuthStore } from '@/stores/authStore';

export default {
  setup() {
    const authStore = useAuthStore()

    const firstName = computed(() => authStore.firstName)
    const lastName = computed(() => authStore.lastName)
    const isAuth = computed(() => authStore.isAuthenticated)

    return {
      firstName,
      lastName,
      isAuth
    }
  }
}
</script>

<template>
  <main>
    <div v-if="isAuth">
      <h1>Bienvenue, {{ firstName }} {{ lastName }}</h1>
      <p>Vous êtes connecté.</p>
    </div>
    <div v-else>
      <p>Connecter vous ici.</p>
      <router-link to="/login">Connexion</router-link>
    </div>
    <TheWelcome />
  </main>
</template>
