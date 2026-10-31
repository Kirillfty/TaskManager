import './assets/main.css'

import { createApp } from 'vue'
import App from './App.vue'
import { createRouter, createWebHistory } from 'vue-router'
import Home from './pages/Home.vue'
import Auth from './pages/Auth.vue'
import User from './pages/User.vue'
import MenuLayout from './layout/MenuLayout.vue'

const routes = [
  {
    path: '/',
    component: MenuLayout,
    children: [
      {
        path: '',
        name: 'home',
        component: Home
      },
      {
        path: 'user',
        name: 'user',
        component: User
      }
    ]
  },
  {
    path: '/auth',
    name: 'auth',
    component: Auth
  }
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes
})

createApp(App).use(router).mount('#app')
