
<script setup lang="ts">
import { ref } from 'vue'
import axios from 'axios'
const props = defineProps({
  show: Boolean,
  id: Number,
  userId:Number
})

const updateTask = ref({
  title: '',
  description: '',
  status: '',
})
async function Edit() {
    const accessToken = localStorage.getItem('accessToken');
  await axios.put('https://localhost:7172/api/Task/',{
    id: props.id,
    title: updateTask.value.title,
    description: updateTask.value.description,
    status: updateTask.value.status
  },{
    headers:{
        Authorization:`Bearer ${accessToken}`
    }
  })
}
defineEmits(['close'])
</script>

<template>
  <div v-if="show" class="modal">
    <div class="content">
      <input type="text" placeholder="Title" v-model="updateTask.title" />
      <input type="text" placeholder="Description" v-model="updateTask.description" />
      <select v-model="updateTask.status">
        <option value="Done">Done</option>
        <option value="Complete">Completed</option>
      </select>
      <button @click="$emit('close')" class="delete-btn">Закрыть</button>
      <button @click="Edit()" class="edit-btn">Изменить</button>
    </div>
  </div>
</template>

<style scoped>
.modal {
  position: fixed;
  inset: 0;
  background: #0008;
  display: grid;
  place-items: center;
}

.content {
  background: white;
  color: black;
  padding: 20px;
  border-radius: 10px;
}
input {
  display: block;
  margin-bottom: 10px;
  width: 100%;
  padding: 5px;
}
</style>
