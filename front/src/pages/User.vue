<template>
    <h1>{{ userData.login }}</h1>
</template>
<script lang="ts" setup>
import axios from 'axios';
import { useRouter } from 'vue-router';
import {ref, onMounted} from 'vue'
const userId = ref();
const router  = useRouter();
const userData = ref('')

async function GetUserInfo(userId:any) {
    await axios.get('https://localhost:7172/api/User/'+userId.value)
    .then((res)=>{
        return userData.value = res.data;
    })
    
}

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

onMounted(async ()=>{
    await Check();
    await GetUserInfo(userId);
})
</script>