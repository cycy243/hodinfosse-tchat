import User from '@/modules/model/User'
import { defineStore } from 'pinia'

export default defineStore('authStore', {
  state: () => ({
    isAuth: false,
    user: new User('', '', '', '', '', '')
  }),
  getters: {
    token: () => localStorage.getItem('token')
  },
  actions: {
    onLogin(user: User) {
      this.user = new User(
        user.lastName,
        user.id,
        user.firstName,
        user.userName,
        user.email,
        user.token,
        user.roles
      )
      this.isAuth = true
      localStorage.setItem('token', user.token)
      localStorage.setItem('user', JSON.stringify(user))
    },
    onLogout() {
      this.isAuth = false
      this.user = new User('', '', '', '', '', '')
      localStorage.removeItem('token')
      localStorage.removeItem('user')
    }
  }
})
