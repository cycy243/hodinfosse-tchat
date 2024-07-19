import { createRouter, createWebHistory } from 'vue-router'

import useAuthStore from '../stores/auth'

import TchatView from '@/views/TchatView.vue'
import HomeView from '../views/HomeView.vue'
import UserProfileView from '@/views/UserProfileVue.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
      path: '/tchat',
      name: 'tchat',
      component: TchatView,
      meta: { requiresAuth: true }
    },
    {
      path: '/manage_contact_message',
      name: 'manage_contact_message',
      component: () => import('../views/admin/ContactMessageManagementView.vue'),
      meta: { requiresAdminRole: true }
    },
    {
      path: '/manage_users',
      name: 'manage_users',
      component: () => import('../views/admin/UserManagementView.vue'),
      meta: { requiresAdminRole: true }
    },
    {
      path: '/profile',
      name: 'user_profile',
      component: UserProfileView
    },
    {
      path: '/contact',
      name: 'contact',
      component: () => import('@/views/ContactView.vue')
    },
    {
      path: '/about',
      name: 'about',
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import('../views/AboutView.vue')
    },
    {
      path: '/reset-password',
      name: 'reset-password',
      component: () => import('@/views/ResetPasswordView.vue')
    },
    {
      path: '/reset-password?code=:code&email=:email',
      name: 'reset-password-with-value',
      component: () => import('@/views/ResetPasswordView.vue')
    },
    {
      path: '/',
      name: 'auth',
      component: () => import('@/views/AuthView.vue'),
      children: [
        {
          path: '/login',
          name: 'login',
          component: () => import('@/views/auth/LoginView.vue'),
          meta: {
            viewTitleKey: 'Login'
          }
        },
        {
          path: '/register',
          name: 'register',
          component: () => import('@/views/auth/RegisterView.vue'),
          meta: {
            viewTitleKey: 'Register'
          }
        }
      ]
    }
  ]
})

router.beforeEach((to, from, next) => {
  const store = useAuthStore()

  if (to.name === 'login' && store.isAuth) {
    next({ name: 'home' })
    return
  }

  if (!to.meta.requiresAuth && !to.meta.requiresAdminRole) {
    next()
    return
  }

  if (to.meta.requiresAdminRole && !store.user.isAdmin) {
    next({ name: 'home' })
    return
  }

  if (store.isAuth) {
    next()
  } else {
    next({ name: 'home' })
  }
})

export default router
