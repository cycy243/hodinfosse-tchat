<template>
  <div class="tchat_wrapper">
    <div class="msg_wrapper" ref="msgsDiv">
      <MessageItem
        v-for="msg in messages"
        :key="msg.id"
        :msg-content="msg.content"
        :msg-date-time-send="msg.sendDateTime.toString()"
        :msg-id="msg.id"
        :user-id="msg.userId"
        :username="msg.username"
        :is-from-connected-user="msg.userId === user.id"
        :class="{ user_msg: msg.userId === user.id }"
      />
    </div>
    <div class="msg_form">
      <textarea
        id="msg_input"
        name="msg_input"
        placeholder="Your text"
        cols="55"
        rows="5"
        v-model="msgContent"
      ></textarea>
      <button @click="async () => await sendMessage()">Send</button>
    </div>
  </div>
</template>

<script lang="ts">
import Message from '@/modules/model/Message'
import type MessageRepository from '@/modules/repository/MessageRepository'

import MessageItem from '@/components/tchat/MessageItem.vue'

import { defineComponent, inject, nextTick } from 'vue'

import { mapState } from 'pinia'
import useAuthStore from '../stores/auth'

import * as keys from '@/utils/injectionKeys'

import Pusher from 'pusher-js'

export default defineComponent({
  name: 'TchatView',
  components: {
    MessageItem
  },
  data() {
    return {
      messages: new Array<Message>(),
      msgContent: '',
      pusher: new Pusher(import.meta.env.VITE_PUSHER_CLIENT_ID, {
        cluster: import.meta.env.VITE_PUSHER_CLUSTER
      }),
      MessageRepo: inject(keys.messageRepositoryKey) as MessageRepository
    }
  },
  computed: {
    ...mapState(useAuthStore, ['user']),
    msgsContainer() {
      // Get the div that hold all the messages
      return this.$refs.msgsDiv as HTMLElement
    }
  },
  methods: {
    async sendMessage() {
      await this.MessageRepo.sendMessage(this.msgContent, this.user.id, this.user.token)

      await this.scrollToBottomMsgContainer()
    },
    async scrollToBottomMsgContainer() {
      // Wait for the render of the view, so if a message has been send, then it is first render
      await nextTick()

      // get the last child element
      const lastChildElement = this.msgsContainer?.lastElementChild
      // scroll to the bottom of the div that hold the messages
      this.msgsContainer?.scroll({
        behavior: 'smooth',
        top: this.msgsContainer?.scrollHeight + (lastChildElement?.scrollHeight || 0)
      })
    }
  },
  async mounted() {
    this.messages = (await this.MessageRepo.getMessages(this.user.token)) ?? new Array<Message>()

    var channel = this.pusher.subscribe(`global-tchat-${location.hostname}`)
    channel.bind('send-message', (data: any) => {
      this.messages.push(
        new Message(data.id, data.message, new Date(data.sendDateTime), data.uid, data.username)
      )
    })

    await this.scrollToBottomMsgContainer()
  },
  unmounted() {
    this.pusher.unsubscribe('send-message')
  }
})
</script>

<style scoped>
.msg_wrapper {
  display: flex;
  flex-direction: column;
  height: 350px;
  overflow-y: scroll;
}

.msg_form {
  display: flex;
  flex-direction: row;
}

.msg_form input {
  width: 100%;
}

div.mine {
  align-self: flex-end;
}

.msg {
  padding-bottom: 0.5em;
}

.user_msg {
  align-self: flex-end;
}

.tchat_wrapper {
  width: 50%;
}
</style>
