<template>
  <div class="message_item" :class="{ user_message_item: isFromConnectedUser }">
    <span>{{ isFromConnectedUser ? 'Me' : username }}</span>
    <p>{{ msgContent }}</p>
    <span>{{ msgDateTimeSend }}</span>
  </div>
</template>
<script lang="ts">
import { defineComponent } from 'vue'

export default defineComponent({
  name: 'MessageItem',
  props: {
    username: {
      type: String,
      required: true
    },
    userId: {
      type: String,
      required: true
    },
    msgContent: {
      type: String,
      required: true
    },
    msgId: {
      type: String,
      required: true
    },
    msgDateTimeSend: {
      type: String,
      required: true
    },
    isFromConnectedUser: {
      type: Boolean,
      required: false,
      default: false
    }
  },
  emits: ['onDelete', 'onUpdate', 'onReact']
})
</script>
<style>
.message_item {
  background-color: rgba(48, 128, 128, 0.61);
  width: 50%;
  max-width: 450px;
}

.user_message_item {
  background-color: rgba(106, 255, 255, 0.555);
}
</style>
