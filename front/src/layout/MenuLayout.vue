<template>
  <div class="app-shell">
    <div v-if="mobileOpen" class="sidebar-backdrop" @click="mobileOpen = false"></div>

    <aside class="sidebar" :class="{ 'sidebar-open': mobileOpen }">
      <div class="brand">
        <div class="brand-mark"><span></span><span></span><span></span></div>
        <span class="brand-name">Task<span>Flow</span></span>
      </div>

      <div class="workspace-label">WORKSPACE</div>
      <nav class="nav-list">
        <RouterLink to="/" class="nav-link" exact-active-class="active" @click="mobileOpen = false">
          <svg viewBox="0 0 24 24" aria-hidden="true"><rect x="3.5" y="3.5" width="7" height="7" rx="1.5"/><rect x="13.5" y="3.5" width="7" height="7" rx="1.5"/><rect x="3.5" y="13.5" width="7" height="7" rx="1.5"/><rect x="13.5" y="13.5" width="7" height="7" rx="1.5"/></svg>
          <span>Мои задачи</span>
        </RouterLink>
        <RouterLink to="/user" class="nav-link" active-class="active" @click="mobileOpen = false">
          <svg viewBox="0 0 24 24" aria-hidden="true"><circle cx="12" cy="8" r="3.5"/><path d="M5 20c.5-3.5 3.2-5.5 7-5.5s6.5 2 7 5.5"/></svg>
          <span>Мой профиль</span>
        </RouterLink>
      </nav>

      <div class="sidebar-bottom">
        <div class="upgrade-card">
          <div class="upgrade-icon">
            <svg viewBox="0 0 24 24" aria-hidden="true"><path d="m12 3 2.2 5.8L20 11l-5.8 2.2L12 19l-2.2-5.8L4 11l5.8-2.2L12 3Z"/></svg>
          </div>
          <strong>Работай продуктивнее</strong>
          <p>Держи все задачи под контролем в одном месте.</p>
        </div>
        <button class="logout-button" @click="logout">
          <svg viewBox="0 0 24 24" aria-hidden="true"><path d="M10 17l5-5-5-5M15 12H3"/><path d="M12 3h6a2 2 0 0 1 2 2v14a2 2 0 0 1-2 2h-6"/></svg>
          <span>Выйти из аккаунта</span>
        </button>
      </div>
    </aside>

    <main class="main-area">
      <header class="topbar">
        <button class="mobile-menu" aria-label="Открыть меню" @click="mobileOpen = !mobileOpen">
          <svg viewBox="0 0 24 24" aria-hidden="true"><path d="M4 6h16M4 12h16M4 18h16"/></svg>
        </button>
        <div class="breadcrumb">
          <span>Рабочее пространство</span>
          <svg viewBox="0 0 24 24" aria-hidden="true"><path d="m9 18 6-6-6-6"/></svg>
          <strong>{{ pageTitle }}</strong>
        </div>
        <div class="topbar-right">
          <div class="status-dot"></div>
          <span>Личный кабинет</span>
          <div class="top-avatar">U</div>
        </div>
      </header>
      <div class="page-content">
        <RouterView />
      </div>
    </main>
  </div>
</template>

<script setup lang="ts">
import { computed, ref } from 'vue'
import { RouterLink, RouterView, useRoute, useRouter } from 'vue-router'

const route = useRoute()
const router = useRouter()
const mobileOpen = ref(false)

const pageTitle = computed(() => route.path === '/user' ? 'Мой профиль' : 'Мои задачи')

function logout() {
  localStorage.removeItem('accessToken')
  localStorage.removeItem('refreshToken')
  router.push('/auth')
}
</script>

<style scoped>
.app-shell{min-height:100vh;background:#f5f6fa;color:#202334;font-family:Inter,-apple-system,BlinkMacSystemFont,"Segoe UI",sans-serif}
.sidebar{position:fixed;inset:0 auto 0 0;z-index:20;width:258px;padding:29px 18px 20px;background:#171923;color:#a4a7b7;display:flex;flex-direction:column}
.brand{display:flex;align-items:center;gap:12px;padding:0 12px 43px}
.brand-mark{width:31px;height:31px;border-radius:10px;background:linear-gradient(145deg,#8b7cff,#6254e8);display:flex;align-items:center;justify-content:center;gap:2px;box-shadow:0 5px 18px #7565ef55}
.brand-mark span{width:3px;border-radius:4px;background:#fff}
.brand-mark span:nth-child(1){height:9px}.brand-mark span:nth-child(2){height:15px}.brand-mark span:nth-child(3){height:11px}
.brand-name{font-size:20px;font-weight:750;letter-spacing:-.7px;color:#fff}.brand-name span{color:#9b91ff}
.workspace-label{padding:0 13px 12px;font-size:10px;letter-spacing:1.5px;font-weight:750;color:#656a7e}
.nav-list{display:grid;gap:7px}
.nav-link{height:47px;padding:0 13px;display:flex;align-items:center;gap:13px;border-radius:10px;color:#a4a7b7;text-decoration:none;font-size:13px;font-weight:550;transition:.2s}
.nav-link svg,.logout-button svg{width:19px;height:19px;fill:none;stroke:currentColor;stroke-width:1.7;stroke-linecap:round;stroke-linejoin:round}
.nav-link:hover{background:#ffffff0c;color:#fff}.nav-link.active{background:linear-gradient(100deg,#7164e9,#6559d8);color:#fff;box-shadow:0 7px 18px #6254e833}
.sidebar-bottom{margin-top:auto}
.upgrade-card{padding:16px 15px 17px;margin:0 2px 18px;border:1px solid #ffffff12;border-radius:13px;background:linear-gradient(145deg,#222437,#1d1f2d)}
.upgrade-icon{width:29px;height:29px;margin-bottom:12px;border-radius:9px;background:#7164e92b;display:grid;place-items:center;color:#a69cff}
.upgrade-icon svg{width:17px;height:17px;fill:none;stroke:currentColor;stroke-width:1.6;stroke-linejoin:round}
.upgrade-card strong{display:block;color:#f1f1f8;font-size:12px}.upgrade-card p{margin:6px 0 0;color:#85899d;font-size:11px;line-height:1.55}
.logout-button{width:100%;height:44px;display:flex;align-items:center;gap:12px;padding:0 13px;border:0;border-top:1px solid #ffffff12;background:transparent;color:#969aab;font:inherit;font-size:12px;text-align:left;cursor:pointer;transition:.2s}
.logout-button:hover{color:#ff929b}.main-area{min-height:100vh;margin-left:258px}
.topbar{height:76px;padding:0 42px;background:#fff;border-bottom:1px solid #eceef3;display:flex;align-items:center;justify-content:space-between}
.breadcrumb{display:flex;align-items:center;gap:10px;color:#a0a3b1;font-size:12px}.breadcrumb svg{width:14px;height:14px;fill:none;stroke:#b4b7c2;stroke-width:1.8}.breadcrumb strong{color:#34374a;font-weight:650}
.topbar-right{display:flex;align-items:center;gap:9px;color:#777b8d;font-size:11px}.status-dot{width:7px;height:7px;border-radius:50%;background:#51c69a;box-shadow:0 0 0 3px #51c69a20}.top-avatar{width:31px;height:31px;margin-left:9px;border-radius:10px;background:#eeeaff;color:#7568df;display:grid;place-items:center;font-size:12px;font-weight:750}
.page-content{max-width:1440px;margin:0 auto;padding:38px 42px 56px}
.mobile-menu{display:none;border:0;background:transparent;color:#44485a;padding:5px}.mobile-menu svg{width:22px;height:22px;fill:none;stroke:currentColor;stroke-width:1.8;stroke-linecap:round}
@media(max-width:800px){.sidebar{transform:translateX(-100%);transition:transform .25s ease}.sidebar.sidebar-open{transform:translateX(0)}.sidebar-backdrop{position:fixed;inset:0;z-index:19;background:#11152280}.main-area{margin-left:0}.topbar{height:66px;padding:0 20px;gap:12px}.mobile-menu{display:block}.breadcrumb{margin-right:auto}.topbar-right>span{display:none}.page-content{padding:26px 20px 40px}}
@media(max-width:480px){.topbar{padding:0 14px}.page-content{padding:22px 14px 32px}.breadcrumb{font-size:11px;gap:5px}}
</style>
