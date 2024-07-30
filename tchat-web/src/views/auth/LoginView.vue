<template>
  <div>
    <span v-if="errorMsg.length > 0">{{ errorMsg }}</span>
    <VForm :validation-schema="schema" @submit="onSubmitForm" class="form">
      <FormField
        input-name="login"
        label-name="Email or username:"
        placeholder="Your email or username"
      />
      <FormField
        input-name="password"
        input-type="password"
        label-name="Password:"
        placeholder="Your password"
      />
      <button type="submit">Log in</button>
    </VForm>
    <p>
      You forgot your password? Go here then
      <RouterLink :to="{ name: 'reset-password' }">password forget</RouterLink>
    </p>
  </div>
</template>
<script lang="ts">
import { defineComponent } from 'vue'

import { Form as VForm } from 'vee-validate'

import FormField from '@/components/form/FormField.vue'

import { mapActions } from 'pinia'
import useAuthStore from '@/stores/auth'
import UserRepository from '@/modules/repository/UserRepository'

import * as keys from '@/utils/injectionKeys'

export default defineComponent({
  name: 'LoginView',
  components: { VForm, FormField },
  inject: {
    UserRepository: {
      from: keys.userRepositoryKey
    }
  },
  data() {
    return {
      schema: {
        login: 'required|min:3|max:100',
        password: 'required|min:9|max:100|excluded:password'
      },
      errorMsg: ''
    }
  },
  methods: {
    ...mapActions(useAuthStore, ['onLogin']),
    async onSubmitForm(values: any) {
      this.errorMsg = ''
      const result = await (this.UserRepository as UserRepository)?.login(
        values.login,
        values.password
      )

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
.login_form {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.login_form > div {
  margin-bottom: 1.2rem;
}

.login_form button {
  width: 50%;
}

.login_form + p {
  font-size: 10pt;
}

.form {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.form button {
  margin-top: 1rem;
}
</style>
