<script setup lang="ts">
import { useTestStore } from '@/stores'

const store = useTestStore()

function sendEvent() {
  store.sendTestMessage()
}

</script>

<template>
  <main>
    <h1>Send message test</h1>    
    <br/>
    <button @click="sendEvent">Send Message</button>
    <br/>

    <h1 v-if="store.message">{{ store.message }}</h1>
    
  </main>
</template>
