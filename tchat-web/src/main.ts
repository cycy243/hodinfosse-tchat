import './assets/main.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'

import { RouterLink } from 'vue-router'
import { Form as VForm, Field as VField, ErrorMessage as VErrorMessage } from 'vee-validate'

import GoogleSignInPlugin from 'vue3-google-signin'

import VeeValidatePlugin from '@/includes/veeValidateValidationPlusgin'

import * as keys from '@/utils/injectionKeys'

import App from './App.vue'
import router from './router'
import redaxios from 'redaxios'
import ContactRepository from './modules/repository/ContactRepository'
import MessageRepository from './modules/repository/MessageRepository'
import UserRepository from './modules/repository/UserRepository'

const app = createApp(App)

app.use(createPinia())
app.use(router)

// Enregistrement des plugins

app.use(VeeValidatePlugin)

app.use(GoogleSignInPlugin, {
  clientId: import.meta.env.VITE_GOOGLE_CLIENT_ID
})

// Enregistrement des composant dans l'application

app.component('RouterLink', RouterLink)
app.component('VForm', VForm)
app.component('VField', VField)
app.component('VErrorMessage', VErrorMessage)

// Configuration des éléments de l'application utilisé pour l'injection de dépendances

const redaxiosCli = redaxios.create({
  baseURL: import.meta.env.VITE_API_URL,
  fetch,
  responseType: 'json'
})

// Injection des dépendances

app.provide(keys.contactRepositoryKey, new ContactRepository(redaxiosCli))
app.provide(keys.messageRepositoryKey, new MessageRepository(redaxiosCli))
app.provide(keys.userRepositoryKey, new UserRepository(redaxiosCli))

app.mount('#app')
