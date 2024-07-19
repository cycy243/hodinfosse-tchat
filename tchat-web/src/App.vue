<script lang="ts">
import { defineComponent } from 'vue'
import { RouterView } from 'vue-router'

import NavMenu from '@/components/NavMenu.vue'
import UserMenu from '@/components/menus/UserMenu.vue'

import { mapActions, mapState } from 'pinia'
import useAuthStore from '@/stores/auth'

export default defineComponent({
  name: 'App',
  components: { RouterView, NavMenu, UserMenu },
  computed: {
    ...mapState(useAuthStore, ['isAuth', 'user'])
  },
  methods: {
    ...mapActions(useAuthStore, ['onLogout']),
    onLogoutclicked() {
      this.onLogout()
      this.$router.push({ name: 'home' })
    }
  }
})
</script>

<template>
  <header>
    <div>
      <img alt="Vue logo" class="logo" src="@/assets/logo.svg" width="125" height="125" />

      <NavMenu :user-is-auth="isAuth" :username="user.userName" :user-is-admin="user.isAdmin" />
    </div>
    <UserMenu
      class="userMenu"
      v-if="isAuth"
      :username="user.userName"
      :user-profile-route-name="'user_profile'"
      @logout-clicked="onLogoutclicked()"
    />
  </header>

  <main class="page_content">
    <RouterView />
  </main>
</template>

<style scoped>
header {
  line-height: 1.5;
  max-height: 100vh;
  width: 100%;
  margin-bottom: 1rem;
  display: flex;
  justify-content: space-between;
}

header > div {
  display: flex;
  place-items: center;
  padding-right: calc(var(--section-gap) / 2);
}

.userMenu {
  position: fixed;
  top: 35px;
  right: 0;
}

.logo {
  display: block;
  margin: 0;
}

.page_content {
  width: 80%;
  display: flex;
  flex-direction: column;
  align-items: center;
}
</style>
