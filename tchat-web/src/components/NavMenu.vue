<script lang="ts">
import { defineComponent } from 'vue'
import { RouterLink } from 'vue-router'

export default defineComponent({
  name: 'NavMenu',
  components: { RouterLink },
  props: {
    userIsAuth: {
      required: true,
      type: Boolean
    },
    userIsAdmin: {
      required: true,
      type: Boolean
    },
    username: {
      required: true,
      type: String
    }
  }
})
</script>

<template>
  <div class="wrapper">
    <span v-if="userIsAuth">Hello {{ username }}</span>
    <nav>
      <RouterLink to="/">Home</RouterLink>
      <RouterLink :to="{ name: 'tchat' }" v-if="userIsAuth">Tchat</RouterLink>
      <RouterLink :to="{ name: 'login' }" v-if="!userIsAuth">Login</RouterLink>
      <RouterLink to="/about">About</RouterLink>
      <RouterLink :to="{ name: 'contact' }">Contact</RouterLink>
      <template v-if="userIsAdmin">
        <RouterLink :to="{ name: 'manage_users' }">Manage users</RouterLink>
        <RouterLink :to="{ name: 'manage_contact_message' }">Manage messages</RouterLink>
      </template>
    </nav>
  </div>
</template>
<style scoped>
nav {
  width: 100%;
  font-size: 12px;
  text-align: center;
  margin-top: 0;
}

nav a.router-link-exact-active {
  color: var(--color-text);
}

nav a.router-link-exact-active:hover {
  background-color: transparent;
}

nav a {
  display: inline-block;
  padding: 0 1rem;
  border-left: 1px solid var(--color-border);
}

nav a:first-of-type {
  border: 0;
}

@media (min-width: 1024px) {
  nav {
    text-align: left;
    margin-left: -1rem;
    font-size: 1rem;

    padding: 1rem 0;
    display: flex;
    flex-direction: row;
  }

  .wrapper {
    display: flex;
    flex-direction: row;
    place-items: flex-start;
    flex-wrap: wrap;
  }
}
</style>
