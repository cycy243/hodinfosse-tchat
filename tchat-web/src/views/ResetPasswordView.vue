<template>
  <div>
    <h1>Reset your password</h1>
    <template v-if="$route.query.code === undefined">
      <p>To reset your password you'll need to enter your mail first</p>
      <div>
        <VForm :validation-schema="schema" @submit="onResetClicked">
          <div class="form_label">
            <div>
              <label :for="'email'">Email:</label>
              <VField type="text" :id="'email'" :name="'email'" v-model="email" />
            </div>
            <VErrorMessage :name="'email'" class="error_msg" />
          </div>
          <button type="submit">Reset</button>
        </VForm>
      </div>
    </template>
    <template v-else>
      <p>Change your password</p>
      <p v-if="errorMsg != undefined" class="error_msg">{{ errorMsg }}</p>
      <div>
        <VForm :validation-schema="schema" @submit="onConfirmtClicked">
          <div class="form_label">
            <div>
              <label :for="'password'">Password:</label>
              <VField type="password" :id="'password'" :name="'password'" v-model="newPassword" />
            </div>
            <VErrorMessage :name="'password'" class="error_msg" />
          </div>
          <div class="form_label">
            <div>
              <label :for="'confirm_password'">Confirm password:</label>
              <VField
                type="confirm_password"
                :id="'confirm_password'"
                :name="'confirm_password'"
                v-model="confirmNewPassword"
              />
            </div>
            <VErrorMessage :name="'confirm_password'" class="error_msg" /></div
        ></VForm>
        <button type="submit">Confirm</button>
      </div>
    </template>
  </div>
</template>
<script lang="ts">
import type UserRepository from '@/modules/repository/UserRepository'
import { ErrorMessage as VErrorMessage, Field as VField, Form as VForm } from 'vee-validate'
import { defineComponent } from 'vue'

export default defineComponent({
  name: 'ResetPasswordView',
  data() {
    return {
      email: this.$route.query.email !== undefined ? `${this.$route.query.email}` : '',
      newPassword: '',
      confirmNewPassword: '',
      errorMsg: '',
      schema: {
        email: 'required|min:3|max:150|email',
        password: 'required|min:9|max:100|excluded:password',
        confirm_password: 'required|passwords_mismatch:@password'
      }
    }
  },
  components: {
    VErrorMessage,
    VField,
    VForm
  },
  inject: ['UserRepository'],
  methods: {
    async onResetClicked() {
      const requestResult = await (this.UserRepository as UserRepository)?.requestResetPassword(
        this.email
      )
      if (requestResult) {
        this.errorMsg = ''
      }
    },
    async onConfirmtClicked() {
      if (this.newPassword === this.confirmNewPassword) {
        const requestResult = await (this.UserRepository as UserRepository)?.confirmResetPassword(
          this.email,
          `${this.$route.query.code}`,
          this.newPassword
        )
        if (requestResult) {
          this.errorMsg = ''
        }
      } else {
        this.errorMsg = 'The [password] and the [confirm password] field must have the same value'
      }
    }
  }
})
</script>
<style lang="css">
.form_label {
  display: flex;
  flex-direction: column;
}

.error_field {
  border: 1px solid red;
}

.error_msg {
  color: red;
  font-size: 10pt;
}
</style>
