
<script setup lang="ts">
import { ref } from 'vue'
import axios from 'axios'
import {btn} from './functions.ts'
function AddTask(){
  const accessToken = localStorage.getItem('accessToken');
  axios.post('https://localhost:7172/api/Task',{
    title:taskData.value.title,
    description:taskData.value.description,
    status:taskData.value.status
  },{
    headers:{
      Authorization: `Bearer ${accessToken}`
    }
  })
}
const props = defineProps({
  show: Boolean,
  userId:Number
})

defineEmits(['close'])

const taskData = ref({
  title:'',
  description:'',
  status:"Done",
})
</script>

<template>
  <div v-if="show" class="modal">
    <div class="content">
      <input type="text" placeholder="Title" v-model="taskData.title" />
      <input type="text" placeholder="Description" v-model="taskData.description"/>
      <select v-model="taskData.status">
        <option value="Done">Done</option>
        <option value="Complete">Completed</option>
      </select>
      <button @click="AddTask(),btn()" class="add-btn">Добавить</button>
      <button @click="$emit('close'),btn()" class="delete-btn" >Закрыть</button>
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
