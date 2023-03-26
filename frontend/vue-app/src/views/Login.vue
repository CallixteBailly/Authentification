<template>
  <div class="login">
    <h1>Se connecter</h1>
    <div class="inputbox">
      <input type="email" name="email" v-model="email" required />
      <label>Email</label>
    </div>
    <span v-if="errors.email">{{ errors.email }}</span>
    <div class="inputbox">
      <input type="password" name="password" v-model="password" required />
      <label>Mot de passe</label>
    </div>
    <span v-if="errors.password">{{ errors.password }}</span>
    <div className="forget">
      <label><input type="checkbox" v-model="rememberMe" />Se souvenir de moi <a href="#">Mot de passe oublié</a></label>
    </div>
    <button type="submit" @click="handleLogin">Se connecter</button>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useAuthStore } from '@/stores/authStore'
import router from '@/router'
import * as Yup from 'yup'

const schema = Yup.object().shape({
  email: Yup.string().email('Invalid email address').required('Email is required'),
  password: Yup.string().min(8, 'Password must be at least 8 characters').required('Password is required')
})

const email = ref('')
const password = ref('')
const rememberMe = ref(false)
const errors = ref({ email: '', password: '' })

const authStore = useAuthStore()

const handleLogin = async () => {
  try {
    await schema.validate({
      email: email.value,
      password: password.value
    }, { abortEarly: false })

    await authStore.login({
      email: email.value,
      password: password.value
    })

    // Si la connexion réussit, rediriger vers la page d'accueil
    // Ici, vous pouvez utiliser une librairie de routage comme Vue Router
    router.push('/')
  } catch (error) {
    if (error instanceof Yup.ValidationError) {
      errors.value = { email: '', password: '' }
      error.inner.forEach((e) => {
        if (e.path === 'email') {
          errors.value.email = e.message
        } else if (e.path === 'password') {
          errors.value.password = e.message
        }
      })
    }
    console.log(error)
  }
}
</script>



<style scoped>
.login {
  display: grid;
  align-content: center;
}

.inputbox {
  position: relative;
  margin: 20px 0 0;
  width: 310px;
  border-bottom: 2px solid #ffffff;
}

input:focus~label,
input:valid~label {
  top: -5px;
}

.inputbox input {
  width: 100%;
  height: 50px;
  background: transparent;
  border: none;
  outline: none;
  font-size: 1em;
  padding: 0 35px 0 5px;
  color: #fff;
}

.inputbox label {
  position: absolute;
  top: 55%;
  left: 10px;
  transform: translateY(-50%);
  color: #ffffff;
  font-size: 1em;
  pointer-events: none;
  transition: .5s;
}

.inputbox ion-icon {
  position: absolute;
  right: 8px;
  color: #fff;
  font-size: 1.2em;
  top: 20px;
}

.forget {
  margin: 10px 0 15px;
  font-size: .9em;
  color: #fff;
  display: flex;
  justify-content: space-between;
}

.forget label input {
  margin-right: 3px;
}

.forget label a {
  color: #fff;
  text-decoration: none;
}

.forget label a:hover {
  text-decoration: underline;
}

button {
  width: 310px;
  height: 40px;
  border-radius: 40px;
  background: #fff;
  border: none;
  outline: none;
  cursor: pointer;
  font-size: 1em;
  font-weight: 600;
}
</style>
