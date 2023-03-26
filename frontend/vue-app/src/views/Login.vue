<template>
  <div class="login">
    <Form @submit="handleLogin" :validation-schema="schema">
      <h1>Se connecter</h1>
        <TextInput v-model="email" name="email" type="text" label="E-mail" placeholder="Your email address"
          success-message="Got it, we won't spam you!" />
        <TextInput v-model="password" name="password" type="password" label="Password" placeholder="Your password"
          success-message="Nice and secure!" />
      <div className="forget">
        <label><input type="checkbox" v-model="rememberMe" />Se souvenir de moi <a href="#">Mot de passe
            oublié</a></label>
      </div>
      <button class="submit-btn" type="submit" @click="handleLogin">Se connecter</button>
    </Form>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useAuthStore } from '@/stores/authStore'
import router from '@/router'
import * as Yup from 'yup'
import { Form } from 'vee-validate';
import TextInput from '../components/TextInput.vue';

const schema = Yup.object().shape({
  email: Yup.string().email().required(),
  password: Yup.string().min(6).required(),
});

const email = ref('')
const password = ref('')
const rememberMe = ref(false)
const errors = ref({ email: '', password: '' })

const authStore = useAuthStore()

const handleLogin = async () => {
  try {
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



<style >
:root {
  --primary-color: #0071fe;
  --error-color: #f23648;
  --error-bg-color: #fddfe2;
  --success-color: #21a67a;
  --success-bg-color: #e0eee4;
}

.login {
  display: grid;
  align-content: center;
}
.forget {
  margin: 30px 0 15px;
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
.submit-btn:hover {
  transform: scale(1.1);
}
</style>
