<template>
  <div v-if="loading">Loading...</div>
  <div v-else-if="data">
    <div>
      <div class="card-task" v-for="item in data" :key="item.id">
        <div>
          <h1>{{ item.title }}</h1>
          <p>{{ item.description }}</p>
          <p>Status: {{ item.status }}</p>
        </div>
        <div>
          <button class="delete-btn" @click="deleteTask(item.id)">Delete</button>
          <button class="edit-btn" @click="showModal = true">Edit</button>
          <button class="add-btn" @click="showModal2 = true">Add</button>
          <EditModal v-if="showModal" :show="showModal" :id="item.id"  @close="showModal = false" />
          <AddModal @close="showModal2 = false"  v-if="showModal2" :show="showModal2"/>
        </div>
      </div>
    </div>
  </div>
 
</template>
<script setup lang="ts">
import axios from 'axios'
import { ref , onMounted ,watch} from 'vue'
import EditModal from '../components/EditModal.vue'
import AddModal from '../components/AddModal.vue'
import { useRouter } from 'vue-router'
const router = useRouter();
const showModal = ref(false)
const showModal2 = ref(false);
const loading = ref(false)
const data = ref(null)
const status = ref('')
const userId = ref('');

async function Check() {
    const accessToken = localStorage.getItem('accessToken');
    console.log(accessToken);
    await axios.get('https://localhost:7172/api/Auth/check',{
        headers:{
            Authorization:`Bearer ${accessToken}`
        }
    }).then(function(res){
      userId.value = res.data; 
      console.log(userId);
    }).catch((error) => {
        console.error(error);
        router.push('/auth');
    });
    
}
async function deleteTask(id:number) {
    try {
      const accessToken = localStorage.getItem('accessToken');
      await axios.delete(`https://localhost:7172/api/Task/${id}`,{
        headers:{
            Authorization:`Bearer ${accessToken}`
    }})
      fetchData()
    } catch (error) {
      console.error(error)
    }
}
async function fetchData() {
    loading.value = true
    axios.get(`https://localhost:7172/by-user${userId.value}`).then(function (response) {
      data.value = response.data
      loading.value = false
    }).catch(function (error) {
      console.error(error)
      loading.value = false
    })
}

onMounted(async () => {
    await Check();
    await fetchData();
})
watch(status, () => {
  fetchData()
})
</script>