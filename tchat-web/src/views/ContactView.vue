<template>
  <div>
    <h1>Contact</h1>
    <VForm :validation-schema="schema" @submit="onSubmit">
      <span v-if="errorMsg.length > 0">{{ errorMsg }}</span>
      <div>
        <FormField
          input-name="firstname"
          input-type="firstname"
          label-name="Firstname:"
          placeholder="Firstname"
        />
        <FormField
          input-name="lastname"
          input-type="lastname"
          label-name="Lastname:"
          placeholder="Lastname"
        />
        <FormField input-name="email" input-type="email" label-name="Email:" placeholder="Email" />
        <FormField
          input-name="subject"
          input-type="subject"
          label-name="Subject:"
          placeholder="Subject"
        />
        <FormField
          input-name="message"
          input-type="textarea"
          label-name="Your message:"
          placeholder="Message"
        />
      </div>
      <button type="submit">Send</button>
    </VForm>
  </div>
</template>
<script lang="ts">
import { defineComponent } from 'vue'

import { Form as VForm } from 'vee-validate'

import FormField from '@/components/form/FormField.vue'
import type ContactRepository from '@/modules/repository/ContactRepository'

import * as keys from '@/utils/injectionKeys'

export default defineComponent({
  name: 'ContactView',
  components: { VForm, FormField },
  inject: { ContactRepository: keys.contactRepositoryKey },
  data() {
    return {
      schema: {
        email: 'required|email',
        message: 'required|min:25|max:255',
        firstname: 'required|min:4',
        lastname: 'required|min:4',
        subject: 'required|min:4'
      },
      errorMsg: ''
    }
  },
  methods: {
    async onSubmit(values: any) {
      this.errorMsg = ''
      const result = await (this.ContactRepository as ContactRepository).sendMessage(
        values.email,
        values.firstname,
        values.lastname,
        values.message,
        values.subject
      )
      if (!result.isSuccess) {
        this.errorMsg = result.message
      }
    }
  }
})
</script>
<style lang=""></style>
