<template>
  <div>
    <span @click="() => (displayMenu = !displayMenu)">{{ username?.substring(0, 1) }}</span>
    <ul v-if="displayMenu">
      <li><RouterLink :to="{ name: userProfileRouteName }">Profile</RouterLink></li>
      <li><a @click.prevent="() => onLogout()">Logout</a></li>
    </ul>
  </div>
</template>
<script lang="ts">
import { defineComponent } from 'vue'

import { RouterLink } from 'vue-router'

export default defineComponent({
  name: 'UserMenu',
  components: { RouterLink },
  emits: ['logoutClicked'],
  data() {
    return {
      displayMenu: false
    }
  },
  props: {
    username: {
      required: true,
      type: String
    },
    userProfileRouteName: {
      required: true,
      type: String
    }
  },
  methods: {
    onLogout() {
      console.log('Usermenu: logout clicked')

      this.$emit('logoutClicked')
    }
  }
})
</script>
<style scoped lang="css">
div {
  padding: 0;
  display: flex;
  flex-direction: column;
}

div > span + ul {
  position: absolute;
  top: 3rem;
  list-style-type: none;
  margin: 0;
  padding: 0;
  padding-inline-start: 1%;
}

div > span {
  width: 3em;
  height: 3em;
  border-radius: 50px;
  border: 0.14em solid hsla(160, 100%, 37%, 1);
  display: flex;
  align-items: center;
  justify-content: center;
}
</style>
