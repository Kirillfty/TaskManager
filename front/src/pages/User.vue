<template>
  <section class="profile-page">
    <div class="page-heading">
      <div>
        <p class="eyebrow">ACCOUNT SETTINGS</p>
        <h1>Мой профиль</h1>
        <p class="subtitle">Управляй личной информацией и настройками аккаунта.</p>
      </div>
      <div class="heading-decoration">
        <svg viewBox="0 0 64 64" aria-hidden="true"><path d="M32 5 38.5 24.5 58 31 38.5 37.5 32 57 25.5 37.5 6 31 25.5 24.5 32 5Z"/></svg>
      </div>
    </div>

    <div v-if="loading" class="state-card">
      <div class="loader"></div>
      <span>Загружаем профиль...</span>
    </div>
    <div v-else-if="errorMessage" class="state-card error-state">
      <div class="error-icon">!</div>
      <strong>Не удалось загрузить профиль</strong>
      <span>{{ errorMessage }}</span>
      <button class="retry-button" @click="loadProfile">Попробовать снова</button>
    </div>

    <template v-else-if="userData">
      <div class="profile-grid">
        <article class="profile-card">
          <div class="card-topline"><span>ПРОФИЛЬ</span><span class="verified-pill"><i></i> Аккаунт активен</span></div>
          <div class="identity">
            <div class="avatar">{{ initials }}</div>
            <div class="identity-copy">
              <h2>{{ userData.login || 'Пользователь' }}</h2>
              <p>Участник TaskFlow</p>
            </div>
          </div>
          <div class="profile-divider"></div>
          <div class="info-list">
            <div class="info-row">
              <div class="info-icon purple">
                <svg viewBox="0 0 24 24"><circle cx="12" cy="8" r="3.5"/><path d="M5 20c.5-3.5 3.2-5.5 7-5.5s6.5 2 7 5.5"/></svg>
              </div>
              <div><span>Имя пользователя</span><strong>{{ userData.login || 'Не указано' }}</strong></div>
            </div>
            <div class="info-row">
              <div class="info-icon blue">
                <svg viewBox="0 0 24 24"><rect x="4" y="5" width="16" height="14" rx="2"/><path d="M8 9h8M8 13h5"/></svg>
              </div>
              <div><span>ID пользователя</span><strong class="id-value">{{ userId || '—' }}</strong></div>
            </div>
            <div class="info-row">
              <div class="info-icon green">
                <svg viewBox="0 0 24 24"><path d="m5 12 4 4L19 6"/></svg>
              </div>
              <div><span>Статус аккаунта</span><strong>Активен</strong></div>
            </div>
          </div>
        </article>

        <div class="right-column">
          <article class="welcome-card">
            <div class="welcome-content">
              <span class="welcome-label">РАБОЧЕЕ ПРОСТРАНСТВО</span>
              <h2>Рады видеть тебя,<br>{{ userData.login || 'пользователь' }}!</h2>
              <p>Все твои задачи и проекты теперь под рукой. Хорошего дня и продуктивной работы.</p>
            </div>
            <div class="welcome-art">
              <div class="art-circle circle-one"></div><div class="art-circle circle-two"></div>
              <svg viewBox="0 0 120 120" aria-hidden="true"><rect x="27" y="20" width="67" height="82" rx="10" transform="rotate(7 27 20)"/><path d="m43 47 6 6 11-12M45 70l6 6 11-12M68 49h13M70 73h13"/></svg>
            </div>
          </article>
          <article class="security-card">
            <div class="security-icon">
              <svg viewBox="0 0 24 24"><path d="M12 3 20 6v5c0 5-3.4 8.5-8 10-4.6-1.5-8-5-8-10V6l8-3Z"/><path d="m9 12 2 2 4-4"/></svg>
            </div>
            <div class="security-copy">
              <strong>Твой аккаунт защищён</strong>
              <p>Авторизация выполняется с помощью защищённого токена доступа.</p>
            </div>
            <span class="security-check">✓</span>
          </article>
        </div>
      </div>
    </template>
  </section>
</template>

<script setup lang="ts">
import axios from 'axios'
import { computed, onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'

interface UserProfile {
  login?: string
  [key: string]: unknown
}

const router = useRouter()
const userId = ref<string | number>('')
const userData = ref<UserProfile | null>(null)
const loading = ref(true)
const errorMessage = ref('')

const initials = computed(() => {
  const login = userData.value?.login?.trim() || 'U'
  return login.slice(0, 2).toUpperCase()
})

async function loadProfile() {
  loading.value = true
  errorMessage.value = ''
  const accessToken = localStorage.getItem('accessToken')

  if (!accessToken) {
    router.replace('/auth')
    return
  }

  try {
    const checkResponse = await axios.get('https://localhost:7172/api/Auth/check', {
      headers: { Authorization: `Bearer ${accessToken}` }
    })
    userId.value = checkResponse.data

    const userResponse = await axios.get(`https://localhost:7172/api/User/${userId.value}`, {
      headers: { Authorization: `Bearer ${accessToken}` }
    })
    userData.value = userResponse.data
  } catch (error) {
    console.error('Profile loading failed:', error)
    if (axios.isAxiosError(error) && (error.response?.status === 401 || error.response?.status === 403)) {
      localStorage.removeItem('accessToken')
      localStorage.removeItem('refreshToken')
      router.replace('/auth')
      return
    }
    errorMessage.value = 'Проверь подключение к серверу и попробуй ещё раз.'
  } finally {
    loading.value = false
  }
}

onMounted(loadProfile)
</script>

<style scoped>
.profile-page{font-family:Inter,-apple-system,BlinkMacSystemFont,"Segoe UI",sans-serif;color:#25283a}
.page-heading{display:flex;align-items:center;justify-content:space-between;margin-bottom:29px}
.eyebrow{margin:0 0 9px;color:#8175e9;font-size:10px;font-weight:800;letter-spacing:1.65px}
.page-heading h1{margin:0;color:#25283a;font-size:29px;line-height:1.2;letter-spacing:-1.1px;font-weight:750}
.subtitle{margin:9px 0 0;color:#9296a5;font-size:13px}
.heading-decoration{width:47px;height:47px;border-radius:15px;background:#eeecff;display:grid;place-items:center;color:#7669e8}
.heading-decoration svg{width:25px;height:25px;fill:none;stroke:currentColor;stroke-width:1.6;stroke-linejoin:round}
.profile-grid{display:grid;grid-template-columns:minmax(0,1.05fr) minmax(320px,.95fr);gap:20px;align-items:stretch}
.profile-card,.security-card{background:#fff;border:1px solid #eceef4;border-radius:16px;box-shadow:0 5px 24px #20233b06}
.profile-card{padding:25px 27px}
.card-topline{display:flex;justify-content:space-between;align-items:center;color:#a1a4b1;font-size:9px;letter-spacing:1.35px;font-weight:800}
.verified-pill{display:flex;align-items:center;gap:6px;padding:7px 10px;border-radius:20px;background:#eaf8f1;color:#35a779;font-size:10px;letter-spacing:0;font-weight:650}
.verified-pill i{width:6px;height:6px;border-radius:50%;background:#44bd8c}
.identity{display:flex;align-items:center;gap:17px;margin:27px 0 25px}
.avatar{width:67px;height:67px;border-radius:20px;background:linear-gradient(145deg,#9388ff,#6559d8);display:grid;place-items:center;color:#fff;font-size:23px;font-weight:750;letter-spacing:-1px;box-shadow:0 8px 20px #7669e83d}
.identity-copy h2{margin:0;color:#292c3e;font-size:20px;letter-spacing:-.5px}.identity-copy p{margin:6px 0 0;color:#a0a3b0;font-size:12px}
.profile-divider{height:1px;background:#eff0f5}
.info-list{display:grid;gap:0}
.info-row{display:flex;align-items:center;gap:13px;padding:17px 0;border-bottom:1px solid #f1f2f6}
.info-row:last-child{border-bottom:0;padding-bottom:2px}
.info-icon{width:37px;height:37px;border-radius:11px;display:grid;place-items:center}
.info-icon svg{width:18px;height:18px;fill:none;stroke:currentColor;stroke-width:1.7;stroke-linecap:round;stroke-linejoin:round}
.info-icon.purple{background:#f0eeff;color:#7c70e8}.info-icon.blue{background:#eaf4ff;color:#5795df}.info-icon.green{background:#eaf8f1;color:#42b887}
.info-row div:last-child{display:grid;gap:5px}.info-row span{font-size:10px;color:#a0a3b1}.info-row strong{font-size:12px;color:#3b3e50;font-weight:650}.info-row strong.id-value{font-family:ui-monospace,SFMono-Regular,Menlo,monospace;font-size:11px}
.right-column{display:flex;flex-direction:column;gap:17px}
.welcome-card{position:relative;overflow:hidden;min-height:258px;padding:25px 24px;border-radius:16px;background:linear-gradient(115deg,#28263f,#484276);color:#fff}
.welcome-content{position:relative;z-index:2;max-width:75%}
.welcome-label{color:#b8b2ed;font-size:9px;font-weight:800;letter-spacing:1.4px}
.welcome-card h2{margin:17px 0 10px;font-size:22px;line-height:1.28;letter-spacing:-.7px}
.welcome-card p{max-width:280px;margin:0;color:#c3c1d7;font-size:11px;line-height:1.7}
.welcome-art{position:absolute;right:-4px;bottom:-12px;width:150px;height:155px;display:grid;place-items:center}
.welcome-art svg{position:relative;z-index:1;width:102px;height:102px;fill:#aaa2ff22;stroke:#c7c2ff;stroke-width:2;stroke-linecap:round;stroke-linejoin:round;transform:rotate(-7deg)}
.art-circle{position:absolute;border-radius:50%;background:#a59cff1c}.circle-one{width:145px;height:145px;right:0;bottom:0}.circle-two{width:90px;height:90px;right:30px;bottom:28px;background:#a59cff16}
.security-card{display:flex;align-items:center;gap:13px;padding:19px 18px}
.security-icon{flex:0 0 39px;width:39px;height:39px;border-radius:12px;background:#eaf8f1;color:#43b98a;display:grid;place-items:center}
.security-icon svg{width:20px;height:20px;fill:none;stroke:currentColor;stroke-width:1.7;stroke-linecap:round;stroke-linejoin:round}
.security-copy{min-width:0}.security-copy strong{font-size:11px;color:#414456}.security-copy p{margin:5px 0 0;color:#9b9eab;font-size:10px;line-height:1.5}
.security-check{margin-left:auto;color:#43b98a;font-size:17px;font-weight:800}
.state-card{min-height:220px;padding:30px;background:#fff;border:1px solid #eceef4;border-radius:16px;display:flex;flex-direction:column;align-items:center;justify-content:center;gap:12px;color:#999dac;font-size:12px}
.loader{width:27px;height:27px;border:3px solid #eeecff;border-top-color:#7669e8;border-radius:50%;animation:spin .8s linear infinite}
@keyframes spin{to{transform:rotate(360deg)}}
.error-state strong{color:#373a4d;font-size:14px}.error-icon{width:32px;height:32px;border-radius:50%;background:#fff0f0;color:#e46f78;display:grid;place-items:center;font-weight:800}
.retry-button{margin-top:5px;padding:10px 15px;border:0;border-radius:9px;background:#7165df;color:white;font-size:11px;font-weight:650;cursor:pointer}
@media(max-width:1000px){.profile-grid{grid-template-columns:1fr}.right-column{display:grid;grid-template-columns:1fr 1fr}.welcome-card{min-height:235px}.security-card{align-self:stretch}}
@media(max-width:620px){.page-heading h1{font-size:25px}.subtitle{max-width:270px;line-height:1.5}.heading-decoration{width:39px;height:39px}.profile-card{padding:20px 18px}.right-column{display:flex}.welcome-card{min-height:230px}.welcome-content{max-width:82%}.welcome-art{opacity:.75;right:-35px}.identity{margin:23px 0}.verified-pill{font-size:9px;padding:6px 8px}}
</style>
