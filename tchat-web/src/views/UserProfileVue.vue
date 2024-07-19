<template>
  <div>
    <div class="wrapper">
      <img />
      <div>
        <h1>{{ user.userName }}</h1>
        <div class="user_global_info">
          <span>
            <span class="user_info_title">Name:</span
            ><span>{{ user.lastName ?? "User's name" }}</span>
          </span>
          <span>
            <span class="user_info_title">Firstname:</span>
            <span>{{ user.firstName ?? "User's firstname" }}</span>
          </span>
        </div>
      </div>
    </div>
    <div>
      <span class="user_info_title">Bio</span>
      <p>A bio</p>
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent, inject } from 'vue'

import { mapState } from 'pinia'
import useAuthStore from '@/stores/auth'

import * as keys from '@/utils/injectionKeys'
import type UserRepository from '@/modules/repository/UserRepository'

export default defineComponent({
  name: 'UserProfileVue',
  data() {
    return {
      userRepo: inject(keys.userRepositoryKey) as UserRepository
    }
  },
  computed: {
    ...mapState(useAuthStore, ['token', 'user'])
  }
})
</script>
<style>
.wrapper {
  display: flex;
  flex-direction: row;
}

.wrapper img {
  width: 10rem;
  height: 10rem;
  margin-inline-end: 1.5rem;
}

.user_global_info {
  display: flex;
  flex-direction: column;
  width: 100%;
}

.user_global_info span {
  display: flex;
  flex-direction: row;
}

.user_info_title {
  width: 7em;
}
</style>
