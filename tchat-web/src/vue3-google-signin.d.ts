import { GAuth } from 'vue3-google-signin'

declare module '@vue/runtime-core' {
  interface ComponentCustomProperties {
    $gAuth: GAuth
  }
}
