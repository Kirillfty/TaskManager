
<script setup lang="ts">
import { ref } from 'vue'
import axios from 'axios'
import { useRouter } from 'vue-router'

const router = useRouter()

const isLogin = ref(true)
const login = ref('')
const password = ref('')
const confirmPassword = ref('')
const error = ref('')
const loading = ref(false)

async function Register() {
  error.value = ''

  if (password.value !== confirmPassword.value) {
    error.value = 'Пароли не совпадают'
    return
  }

  loading.value = true

  try {
    const response = await axios.post(
      'https://localhost:7172/api/Auth/register',
      {
        login: login.value,
        password: password.value
      }
    )

    localStorage.setItem(
      'token',
      JSON.stringify({
        accessToken: response.data.accessToken,
        refreshToken: response.data.refreshToken
      })
    )

    await router.push('/')
  } catch (err:any) {
    error.value =
      err.response?.data?.message || 'Ошибка регистрации'
  } finally {
    loading.value = false
  }
}

async function Login() {
  error.value = ''
  loading.value = true

  try {
    const response = await axios.post(
      'https://localhost:7172/api/Auth/login',
      {
        login: login.value,
        password: password.value
      }
    )

    localStorage.setItem('accessToken',response.data.accessToken)
    localStorage.setItem('refreshToken',response.data.refreshToken)


    await router.push('/')
  } catch (err:any) {
    error.value =
      err.response?.data?.message || 'Ошибка входа'
  } finally {
    loading.value = false
  }
}

async function submitForm() {
  if (isLogin.value) {
    await Login()
  } else {
    await Register()
  }
}
</script>

<template>
  <div class="auth">
    <h2>{{ isLogin ? 'Вход' : 'Регистрация' }}</h2>

    <form @submit.prevent="submitForm">
      <label>Логин</label>
      <input
        v-model="login"
        type="text"
        placeholder="Введите логин"
        required
      />

      <label>Пароль</label>
      <input
        v-model="password"
        type="password"
        placeholder="Введите пароль"
        required
      />

      <template v-if="!isLogin">
        <label>Повторите пароль</label>
        <input
          v-model="confirmPassword"
          type="password"
          placeholder="Повторите пароль"
          required
        />
      </template>

      <p v-if="error" class="error">{{ error }}</p>

      <button type="submit" :disabled="loading">
        {{
          loading
            ? 'Подождите...'
            : isLogin
              ? 'Войти'
              : 'Зарегистрироваться'
        }}
      </button>
    </form>

    <p>
      {{ isLogin ? 'Нет аккаунта?' : 'Уже есть аккаунт?' }}

      <a href="#" @click.prevent="isLogin = !isLogin; error = ''">
        {{ isLogin ? 'Регистрация' : 'Войти' }}
      </a>
    </p>
  </div>
</template>

<style scoped>
.auth {
  width: 320px;
  max-width: calc(100% - 40px);
  margin: 80px auto;
  padding: 24px;
  border-radius: 12px;
  background: #fff;
  color: #222;
  box-shadow: 0 4px 20px #0002;
  font-family: Arial, sans-serif;
}

h2 {
  text-align: center;
}

label {
  display: block;
  margin: 14px 0 6px;
}

input {
  box-sizing: border-box;
  width: 100%;
  padding: 10px;
  border: 1px solid #ccc;
  border-radius: 6px;
}

button {
  width: 100%;
  margin-top: 20px;
  padding: 11px;
  border: none;
  border-radius: 6px;
  background: #333;
  color: white;
  cursor: pointer;
}

button:disabled {
  opacity: 0.6;
  cursor: wait;
}

.error {
  color: #dc2626;
  font-size: 14px;
}

p {
  text-align: center;
  font-size: 14px;
}

a {
  color: #2563eb;
  cursor: pointer;
}
</style>
