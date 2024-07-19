import { describe, it, expect, beforeAll, beforeEach } from 'vitest'

import { RouterLinkStub, VueWrapper, mount } from '@vue/test-utils'
import NavMenu from '../NavMenu.vue'
import { RouterLink } from 'vue-router'
import { nextTick } from 'vue'

describe('NavMenuTests', () => {
  let global: any
  let wrapper: VueWrapper

  beforeEach(async () => {
    global = {
      components: {
        RouterLink: RouterLinkStub
      }
    }
    wrapper = mount(NavMenu, {
      props: {
        userIsAdmin: false,
        userIsAuth: false,
        username: ''
      },
      global
    })
    await nextTick()
  })

  describe('props setting tests', () => {
    it("when user isn't logged in then no login button is displayed", () => {
      expect(
        wrapper.findAll('RouterLink').filter((rl) => rl.text().includes('Login'))
      ).toHaveLength(0)
    })

    it("when user isn't logged as an admin in then no management button displayed", () => {
      expect(
        wrapper.findAll('RouterLink').filter((rl) => rl.text().includes('Manage'))
      ).toHaveLength(0)
    })
  })
})
