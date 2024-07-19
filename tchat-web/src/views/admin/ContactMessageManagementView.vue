<template>
  <div>
    <ContactMessagesGrid
      :items="messages"
      :properties="cols"
      @refresh="handleRefresh"
      @select="itemSelected"
      @delete="handleDelete"
      @item-saved="handleEdit"
      :delete-btn-txt="'delete'"
      :grid-options="gridOptions"
      :grid-text-options="gridTextOptions"
    />
  </div>
</template>
<script lang="ts">
import { defineComponent, inject } from 'vue'

import { GenericGridComponent } from '../../components/grid/GridComponent.vue'

import useAuthStore from '@/stores/auth'
import { mapState } from 'pinia'
import ContactMessage from '@/modules/model/ContactMessage'

import type ContactRepository from '@/modules/repository/ContactRepository'
import ApiResponse from '@/modules/repository/ApiResponse'
import { ContactMessageVM } from '@/modules/viewModels/contactMessageVM'

import * as keys from '@/utils/injectionKeys'
import type User from '@/modules/model/User'

export default defineComponent({
  name: 'MessageManagementView',
  components: {
    ContactMessagesGrid: GenericGridComponent<ContactMessage>()
  },
  data() {
    return new ContactMessageVM(
      inject<ContactRepository>(keys.contactRepositoryKey)!,
      useAuthStore().user as User
    )
  },
  computed: {
    ...mapState(useAuthStore, ['user'])
  },
  methods: {
    itemSelected(item: ContactMessage) {
      this.selectedMessage = item
    },
    async handleDelete(item: ContactMessage, done: () => void) {
      if (item !== undefined) {
        var deleteResult: ApiResponse<boolean> = await this.contactRepo.deleteMessage(
          item,
          this.user.token
        )
        if (deleteResult.isSuccess) {
          done()
        }
      }
    },
    async handleEdit(item: ContactMessage) {
      if (item !== undefined) {
        var editResult: ApiResponse<ContactMessage | undefined> =
          await this.contactRepo.addResponse(item, item.response, this.user.token)
      }
    },
    async handleRefresh() {
      const getMessagesResult: ApiResponse<ContactMessage[]> = await this.contactRepo.getMessages(
        undefined,
        this.user.token
      )
      if (getMessagesResult.isSuccess) {
        this.messages = getMessagesResult.data!
      }
    }
  },
  async mounted() {
    await this.handleRefresh()
  }
})
</script>
<style lang="css"></style>
