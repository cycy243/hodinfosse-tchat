<template>
  <div>
    <span v-if="errorMsg.length > 0">{{ errorMsg }}</span>
    <VForm :validation-schema="schema" @submit="onSubmit">
      <div class="form_fieldset">
        <FormField input-type="email" input-name="email" label-name="Email" />
        <FormField input-name="username" label-name="Username" />
      </div>
      <div class="form_fieldset">
        <FormField input-name="lastname" label-name="Last name" />
        <FormField input-name="firstname" label-name="First name" />
        <FormField input-type="date" input-name="birthdate" label-name="Birthdate" />
      </div>
      <div class="form_fieldset">
        <FormField input-type="password" input-name="password" label-name="Password" />
        <FormField
          input-type="password"
          input-name="confirm_password"
          label-name="Confirm password"
        />
      </div>
      <button type="submit">Register</button>
    </VForm>
  </div>
</template>
<script lang="ts">
import { defineComponent } from 'vue'

import { mapActions } from 'pinia'
import useAuthStore from '@/stores/auth'

import FormField from '@/components/form/FormField.vue'
import type RegisterDto from '@/modules/dto/RegisterDto'
import type UserRepository from '@/modules/repository/UserRepository'

import * as keys from '@/utils/injectionKeys'

export default defineComponent({
  name: 'RegisterView',
  components: { FormField },
  inject: {
    UserRepository: {
      from: keys.userRepositoryKey
    }
  },
  data() {
    return {
      schema: {
        username: 'required|min:3|max:100',
        firstname: 'required|min:3|max:100',
        lastname: 'required|min:3|max:100',
        email: 'required|min:3|max:100|email',
        password: 'required|min:9|max:100|excluded:password',
        confirm_password: 'required|passwords_mismatch:@password'
      },
      errorMsg: ''
    }
  },
  methods: {
    ...mapActions(useAuthStore, ['onLogin']),
    async onSubmit(values: RegisterDto) {
      this.errorMsg = ''
      const result = await (this.UserRepository as UserRepository).register(values)
      if (result.isSuccess) {
        this.onLogin(result.data!)
        this.$router.push({ name: 'tchat' })
      } else {
        this.errorMsg = result.message
      }
    }
  }
})
</script>
<style lang="css">
.form_fieldset {
  margin-block: 1rem;
}
</style>
