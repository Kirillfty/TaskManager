<template>
  <section class="tasks-page">
    <header class="tasks-heading">
      <div>
        <span class="eyebrow">РАБОЧЕЕ ПРОСТРАНСТВО</span>
        <h1>Мои задачи<span class="title-count" v-if="!loading">{{ data.length }}</span></h1>
        <p class="subtitle">Планируй работу и следи за своим прогрессом.</p>
      </div>
      <button class="create-button" type="button" @click="showModal2 = true">
        <svg viewBox="0 0 24 24" aria-hidden="true"><path d="M12 5v14M5 12h14"/></svg>
        <span>Новая задача</span>
      </button>
    </header>

    <div class="summary-row" v-if="!loading && data.length">
      <div class="summary-item">
        <span class="summary-icon all-icon">
          <svg viewBox="0 0 24 24" aria-hidden="true"><path d="M8 6h12M8 12h12M8 18h12M4 6h.01M4 12h.01M4 18h.01"/></svg>
        </span>
        <span><strong>{{ data.length }}</strong><small>Всего задач</small></span>
      </div>
      <div class="summary-item">
        <span class="summary-icon done-icon">
          <svg viewBox="0 0 24 24" aria-hidden="true"><path d="m5 12 4 4L19 6"/></svg>
        </span>
        <span><strong>{{ completedCount }}</strong><small>Завершено</small></span>
      </div>
      <div class="summary-item">
        <span class="summary-icon progress-icon">
          <svg viewBox="0 0 24 24" aria-hidden="true"><circle cx="12" cy="12" r="8"/><path d="M12 8v4l3 2"/></svg>
        </span>
        <span><strong>{{ data.length - completedCount }}</strong><small>В процессе</small></span>
      </div>
    </div>

    <div v-if="loading" class="state-card">
      <span class="loader"></span>
      <strong>Загружаем задачи</strong>
      <p>Секунду, подготавливаем твоё рабочее пространство.</p>
    </div>

    <div v-else-if="data.length" class="tasks-grid">
      <article v-for="(item, index) in data" :key="item.id" class="task-card">
        <div class="card-topline">
          <span class="task-number">ЗАДАЧА {{ String(index + 1).padStart(2, '0') }}</span>
          <span class="status-pill" :class="statusClass(item.status)">
            <span class="status-dot"></span>{{ statusLabel(item.status) }}
          </span>
        </div>

        <div class="task-content">
          <h2>{{ item.title || 'Без названия' }}</h2>
          <p>{{ item.description || 'Для этой задачи пока нет описания.' }}</p>
        </div>

        <div class="card-footer">
          <span class="task-id">#{{ item.id }}</span>
          <div class="card-actions">
            <button class="icon-button edit-action" type="button" aria-label="Редактировать задачу" title="Редактировать" @click="openEdit(item)">
              <svg viewBox="0 0 24 24" aria-hidden="true"><path d="m14 5 5 5M4 20l4.2-.8L19 8.4a2.1 2.1 0 0 0-3-3L5.2 16.2 4 20Z"/></svg>
            </button>
            <button class="icon-button delete-action" type="button" aria-label="Удалить задачу" title="Удалить" @click="deleteTask(item.id)">
              <svg viewBox="0 0 24 24" aria-hidden="true"><path d="M4 7h16M10 11v6M14 11v6M6 7l1 13h10l1-13M9 7V4h6v3"/></svg>
            </button>
          </div>
        </div>
      </article>
    </div>

    <div v-else class="empty-state">
      <div class="empty-illustration">
        <svg viewBox="0 0 64 64" aria-hidden="true"><rect x="13" y="10" width="38" height="45" rx="7"/><path d="M23 23h18M23 32h18M23 41h10"/><path d="m39 44 4 4 8-9"/></svg>
      </div>
      <h2>Пока нет задач</h2>
      <p>Создай первую задачу — и пусть большие планы превращаются в маленькие победы.</p>
      <button class="create-button" type="button" @click="showModal2 = true">
        <svg viewBox="0 0 24 24" aria-hidden="true"><path d="M12 5v14M5 12h14"/></svg>
        Создать первую задачу
      </button>
    </div>

    <EditModal
      v-if="selectedTask"
      :show="true"
      :id="selectedTask.id"
      @close="selectedTask = null"
    />
    <AddModal
      v-if="showModal2"
      :show="showModal2"
      @close="showModal2 = false"
    />
  </section>
</template>

<script setup lang="ts">
import axios from 'axios'
import { computed, onMounted, ref } from 'vue'
import EditModal from '../components/EditModal.vue'
import AddModal from '../components/AddModal.vue'
import { useRouter } from 'vue-router'

interface Task {
  id: number
  title: string
  description: string
  status: string
}

const router = useRouter()
const showModal2 = ref(false)
const loading = ref(true)
const data = ref<Task[]>([])
const userId = ref('')
const selectedTask = ref<Task | null>(null)

const completedCount = computed(() =>
  data.value.filter(task => ['done', 'complete', 'completed'].includes((task.status || '').toLowerCase())).length
)

function statusLabel(status: string) {
  const value = (status || '').toLowerCase()
  if (['done', 'complete', 'completed'].includes(value)) return 'Завершена'
  if (['in progress', 'inprogress', 'progress', 'в процессе'].includes(value)) return 'В процессе'
  if (['todo', 'new', 'pending', 'ожидает'].includes(value)) return 'Ожидает'
  return status || 'Без статуса'
}

function statusClass(status: string) {
  const value = (status || '').toLowerCase()
  if (['done', 'complete', 'completed'].includes(value)) return 'is-done'
  if (['in progress', 'inprogress', 'progress', 'в процессе'].includes(value)) return 'is-progress'
  return 'is-pending'
}

function openEdit(item: Task) {
  selectedTask.value = item
}

async function Check() {
  const accessToken = localStorage.getItem('accessToken')
  if (!accessToken) {
    router.push('/auth')
    return
  }

  try {
    const res = await axios.get('https://localhost:7172/api/Auth/check', {
      headers: { Authorization: `Bearer ${accessToken}` }
    })
    userId.value = res.data
  } catch (error) {
    console.error(error)
    router.push('/auth')
  }
}

async function deleteTask(id: number) {
  try {
    const accessToken = localStorage.getItem('accessToken')
    await axios.delete(`https://localhost:7172/api/Task/${id}`, {
      headers: { Authorization: `Bearer ${accessToken}` }
    })
    await fetchData()
  } catch (error) {
    console.error(error)
  }
}

async function fetchData() {
  loading.value = true
  try {
    const response = await axios.get(`https://localhost:7172/by-user${userId.value}`)
    data.value = Array.isArray(response.data) ? response.data : []
  } catch (error) {
    console.error(error)
    data.value = []
  } finally {
    loading.value = false
  }
}

onMounted(async () => {
  await Check()
  if (userId.value) await fetchData()
  else loading.value = false
})
</script>

<style scoped>
.tasks-page{width:100%;color:#242638}
.tasks-heading{display:flex;align-items:flex-end;justify-content:space-between;gap:24px;margin-bottom:28px}
.eyebrow{display:block;margin-bottom:10px;color:#9699aa;font-size:10px;font-weight:750;letter-spacing:1.55px}
.tasks-heading h1{display:flex;align-items:center;gap:12px;margin:0;color:#222437;font-size:30px;font-weight:760;letter-spacing:-1.15px}
.title-count{display:inline-grid;place-items:center;min-width:27px;height:25px;padding:0 7px;border-radius:8px;background:#eeecff;color:#7064df;font-size:12px;letter-spacing:0}
.subtitle{margin:8px 0 0;color:#9295a5;font-size:13px}
.create-button{height:43px;display:inline-flex;align-items:center;justify-content:center;gap:9px;padding:0 17px;border:0;border-radius:10px;background:#7064e8;color:#fff;font-size:12px;font-weight:650;box-shadow:0 7px 16px #7064e82b;cursor:pointer;transition:transform .2s,box-shadow .2s,background .2s}
.create-button:hover{transform:translateY(-2px);background:#6256d8;box-shadow:0 10px 22px #7064e83b}
.create-button svg{width:17px;height:17px;fill:none;stroke:currentColor;stroke-width:2;stroke-linecap:round}
.summary-row{display:grid;grid-template-columns:repeat(3,minmax(0,1fr));gap:14px;margin-bottom:24px}
.summary-item{min-height:76px;display:flex;align-items:center;gap:12px;padding:14px 17px;border:1px solid #ececf3;border-radius:13px;background:#fff;box-shadow:0 3px 12px #25294a05}
.summary-icon{width:38px;height:38px;display:grid;place-items:center;border-radius:11px}
.summary-icon svg{width:19px;height:19px;fill:none;stroke:currentColor;stroke-width:1.7;stroke-linecap:round;stroke-linejoin:round}
.all-icon{background:#f0efff;color:#7569e8}.done-icon{background:#e9f8f1;color:#35a77b}.progress-icon{background:#fff4e7;color:#e39b42}
.summary-item strong,.summary-item small{display:block}.summary-item strong{color:#292b3d;font-size:17px;font-weight:750;line-height:1.2}.summary-item small{margin-top:4px;color:#999bab;font-size:10px}
.tasks-grid{display:grid;grid-template-columns:repeat(2,minmax(0,1fr));gap:16px}
.task-card{position:relative;min-width:0;min-height:206px;display:flex;flex-direction:column;padding:19px 20px 15px;border:1px solid #e9eaf1;border-radius:15px;background:#fff;box-shadow:0 4px 16px #24274306;transition:transform .2s,border-color .2s,box-shadow .2s;overflow:hidden}
.task-card:before{position:absolute;top:0;left:0;width:3px;height:100%;background:#d9d5ff;content:"";transition:background .2s}
.task-card:hover{transform:translateY(-3px);border-color:#dcd9ff;box-shadow:0 12px 28px #29264d0d}
.task-card:hover:before{background:#7669e9}
.card-topline{display:flex;align-items:center;justify-content:space-between;gap:12px}
.task-number{color:#a0a2b1;font-size:9px;font-weight:750;letter-spacing:1.15px}
.status-pill{display:inline-flex;align-items:center;gap:6px;max-width:60%;padding:6px 9px;border-radius:20px;font-size:10px;font-weight:650;white-space:nowrap}
.status-dot{width:6px;height:6px;flex:0 0 6px;border-radius:50%;background:currentColor}
.is-done{background:#eaf8f1;color:#319b70}.is-progress{background:#fff4e6;color:#c88731}.is-pending{background:#f0efff;color:#7064d9}
.task-content{flex:1;padding:19px 0 17px}
.task-content h2{margin:0 0 8px;color:#292b3e;font-size:16px;font-weight:700;letter-spacing:-.35px;overflow-wrap:anywhere}
.task-content p{display:-webkit-box;margin:0;color:#85899a;font-size:12px;line-height:1.65;overflow:hidden;-webkit-line-clamp:2;-webkit-box-orient:vertical;overflow-wrap:anywhere}
.card-footer{display:flex;align-items:center;justify-content:space-between;padding-top:12px;border-top:1px solid #f0f0f5}
.task-id{color:#b0b2bf;font-size:10px;font-weight:600}
.card-actions{display:flex;gap:7px}
.icon-button{width:31px;height:31px;display:grid;place-items:center;border:1px solid #ededf3;border-radius:9px;background:#fff;color:#85899b;cursor:pointer;transition:background .18s,color .18s,border-color .18s}
.icon-button svg{width:15px;height:15px;fill:none;stroke:currentColor;stroke-width:1.7;stroke-linecap:round;stroke-linejoin:round}
.edit-action:hover{border-color:#e2dfff;background:#f2f0ff;color:#7064df}
.delete-action:hover{border-color:#ffdddd;background:#fff1f1;color:#df626b}
.state-card,.empty-state{min-height:260px;display:flex;flex-direction:column;align-items:center;justify-content:center;padding:36px 22px;border:1px dashed #dedfea;border-radius:16px;background:#fff;text-align:center}
.state-card strong,.empty-state h2{margin:14px 0 0;color:#303247;font-size:16px;font-weight:700}
.state-card p,.empty-state p{max-width:360px;margin:7px 0 0;color:#9699a8;font-size:12px;line-height:1.65}
.loader{width:27px;height:27px;border:3px solid #e9e7ff;border-top-color:#7569e8;border-radius:50%;animation:spin .8s linear infinite}
@keyframes spin{to{transform:rotate(360deg)}}
.empty-illustration{width:65px;height:65px;display:grid;place-items:center;border-radius:20px;background:#f0efff;color:#766ae6}
.empty-illustration svg{width:42px;height:42px;fill:none;stroke:currentColor;stroke-width:1.6;stroke-linecap:round;stroke-linejoin:round}
.empty-state h2{margin-top:17px;font-size:17px}
.empty-state .create-button{margin-top:19px}
@media(max-width:900px){.tasks-grid{grid-template-columns:1fr}.summary-row{gap:10px}}
@media(max-width:600px){.tasks-heading{align-items:flex-start;flex-direction:column;margin-bottom:22px}.tasks-heading h1{font-size:26px}.create-button{width:100%}.summary-row{grid-template-columns:1fr;gap:8px}.summary-item{min-height:64px}.task-card{padding:17px 17px 13px}}
</style>
