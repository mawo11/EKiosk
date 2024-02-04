import { defineStore } from 'pinia'
import { messageClient } from '@/messages/messageClient'

type TestStore = {
  message: string | null,
}

export const useTestStore = defineStore('testStore', {
  state: (): TestStore => {  
    return {
      message: null,
    }
  },
  getters: {
  },
  actions: {
    sendTestMessage() {
      messageClient.once('test-message-response', (val: any) => {
        this.message = val
      })

      messageClient.send('test-message')
    },
  }
})

