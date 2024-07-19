<template>
  <div>
    <h1>{{ $route.meta.viewTitleKey }}</h1>
    <div class="wrapper">
      <RouterView />
      <span>Or</span>
      <GoogleSignInButton @success="handleLoginSuccess" @error="handleLoginError" />
    </div>
    <div v-if="$route.name === 'login'">
      You don't have an account yet? Then
      <RouterLink :to="{ name: 'register' }">register</RouterLink>
    </div>
    <div v-else>
      You already have an account? Then <RouterLink :to="{ name: 'login' }">login</RouterLink>
    </div>
  </div>
</template>

<script lang="ts">
import type UserRepository from '@/modules/repository/UserRepository'
import { defineComponent, inject } from 'vue'

import * as keys from '@/utils/injectionKeys'

import { GoogleSignInButton, type CredentialResponse } from 'vue3-google-signin'

import { mapActions } from 'pinia'
import useAuthStore from '../stores/auth'
import { RouterView, RouterLink } from 'vue-router'

export default defineComponent({
  name: 'AuthView',
  components: { GoogleSignInButton, RouterView, RouterLink },
  data() {
    return {
      userRepo: inject(keys.userRepositoryKey) as UserRepository
    }
  },
  methods: {
    ...mapActions(useAuthStore, ['onLogin']),
    async handleLoginSuccess(response: CredentialResponse) {
      const result = await this.userRepo.authUserWithGoogle(response.credential ?? '')
      if (result.isSuccess) {
        this.userRepo.token = result.data?.token
        this.onLogin(result.data!)
        await this.$router.push({ name: 'tchat' })
      }
    },
    handleLoginError() {
      console.error('Login failed')
    }
  }
})
</script>
<style scoped lang="css">
h1 {
  text-align: center;
  margin-bottom: 3.5rem;
}

div:has(h1) {
  display: flex;
  flex-direction: column;
  justify-content: center;
}

.wrapper {
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.wrapper span {
  padding: 2rem;
}

.auth_menu ul {
  list-style-type: none;
  display: flex;
}

.wrapper + div {
  width: fit-content;
  margin-inline: auto;
}
</style>
