import { describe, it, expect, beforeAll, beforeEach } from 'vitest'

import { mount } from '@vue/test-utils'
import { GenericGridComponent } from '@/components/grid/GridComponent.vue'
import {
  ErrorMessage as VErrorMessage,
  Field as VField,
  Form as VForm,
  configure,
  defineRule
} from 'vee-validate'
import FormField from '@/components/form/FormField.vue'

import { nextTick } from 'vue'
import { required } from '@vee-validate/rules'

// Define validation rules
defineRule('required', required)
configure({
  generateMessage: (ctx: { rule: { name: string }; field: string }) => {
    console.log(ctx)

    const messages = {
      required: `${ctx.field} is required`
    }

    return messages[ctx.rule.name] ? messages[ctx.rule.name] : 'This field is invalid'
  }
})

describe('GridComponentTest', async () => {
  let global: any
  let wrapper: any
  let itemsOne: Array<{ id: string; name: string }>

  beforeEach(async () => {
    global = {
      components: {
        VForm,
        VField,
        VErrorMessage,
        FormField
      }
    }
    itemsOne = [{ id: '123', name: 'Test' }]
    wrapper = mount(GenericGridComponent<{ id: string; name: string }>(), {
      props: {
        items: itemsOne,
        properties: [
          { name: 'id', colName: 'id', toShow: true, isUnique: true },
          { name: 'name', colName: 'Content', toShow: true, inForm: true }
        ]
      },
      global
    })
    await nextTick()
  })

  describe('grid items', () => {
    it('when one item and only 2 cols to render then only one rows in the grid and three cols are rendered(because of the command column)', async () => {
      const trs = wrapper.findAll('tbody tr')
      expect(trs.length).toBe(1)

      const wrapper2 = mount(GenericGridComponent<{ id: string; name: string }>(), {
        props: {
          items: [
            { id: '123', name: 'Test' },
            { id: '456', name: 'Test4' }
          ],
          properties: [
            { name: 'id', colName: 'id', toShow: true, isUnique: true },
            { name: 'name', colName: 'Content', toShow: true, inForm: true }
          ]
        },
        global
      })

      await nextTick()

      const trs2 = wrapper2.findAll('tbody tr')
      expect(trs2.length).toBe(2)
    })

    it('when one item and only 1 col to render then only one row in the grid and two cols are rendered(because of the command column)', async () => {
      const wrapper = mount(GenericGridComponent<{ id: string; name: string }>(), {
        props: {
          items: [{ id: '123', name: 'Test' }],
          properties: [
            { name: 'id', colName: 'id', toShow: false, isUnique: true },
            { name: 'name', colName: 'Content', toShow: true, inForm: true }
          ]
        },
        global
      })

      await nextTick()

      const trs = wrapper.findAll('tbody tr')
      expect(trs.length).toBe(1)
    })

    it("when the grid as isEditable to false then the button with 'edit' is not visible", async () => {
      const wrapper = mount(GenericGridComponent<{ id: string; name: string }>(), {
        props: {
          items: [{ id: '123', name: 'Test' }],
          properties: [
            { name: 'id', colName: 'id', toShow: false, isUnique: true },
            { name: 'name', colName: 'Content', toShow: true, inForm: true }
          ],
          gridOptions: {
            isEditable: false
          }
        },
        global
      })

      await nextTick()

      const editBtns = wrapper.findAll('tbody .grid_action_btn')
      expect(editBtns.length).toBe(0)
    })
  })

  describe('set actions', async () => {
    it('when no option specified then grid can create, delete, edit and refresh', async () => {
      expect(wrapper.findAll('.grid_action_btn').length).toBe(4)
    })

    it('when no option specified then grid can create is not allowed then no btn create is shown', async () => {
      const wrapper = mount(GenericGridComponent<{ id: string; name: string }>(), {
        props: {
          items: [{ id: '123', name: 'Test' }],
          properties: [
            { name: 'id', colName: 'id', toShow: true, isUnique: true },
            { name: 'name', colName: 'Content', toShow: true, inForm: true }
          ],
          gridOptions: {
            isAddable: false
          }
        },
        global
      })

      await nextTick()

      expect(wrapper.findAll('.create_btn').length).toBe(0)
    })

    it('when all grid option for action button is false then no action button are present in the grid', async () => {
      const wrapper = mount(GenericGridComponent<{ id: string; name: string }>(), {
        props: {
          items: [{ id: '123', name: 'Test' }],
          properties: [
            { name: 'id', colName: 'id', toShow: true, isUnique: true },
            { name: 'name', colName: 'Content', toShow: true, inForm: true }
          ],
          gridOptions: {
            isAddable: false,
            isDeletable: false,
            isEditable: false,
            isRefreshable: false
          }
        },
        global
      })

      await nextTick()

      expect(wrapper.findAll('.grid_action_btn').length).toBe(0)
    })
  })

  describe('press action button', () => {
    it('when user click on create, then a form is shown', async () => {
      const wrapper = mount(GenericGridComponent<{ id: string; name: string }>(), {
        props: {
          items: [{ id: '123', name: 'Test' }],
          properties: [
            { name: 'id', colName: 'id', toShow: false, isUnique: true },
            { name: 'name', colName: 'Content', toShow: true, inForm: true }
          ]
        },
        global
      })

      await nextTick()

      expect(wrapper.find('.form_wrapper').exists()).toBeFalsy()

      await wrapper.find('.grid_action_btn.create_btn').trigger('click')

      expect(wrapper.find('.form_wrapper').isVisible()).toBeTruthy()
    })

    it("when refresh clicked then emit 'refresh'", async () => {
      await wrapper.find('.grid_action_btn.refresh_btn').trigger('click')

      expect(wrapper.emitted()).toHaveProperty('refresh')
    })

    it("when delete of one row is clicked then 'delete' is emited'", async () => {
      await wrapper.find('tbody tr:first-child .delete-btn').trigger('click')

      expect(wrapper.emitted()).toHaveProperty('delete')

      await nextTick()

      wrapper.emitted('delete')[0][1]()

      await nextTick()

      const trs = wrapper.findAll('tbody tr')
      expect(trs).toHaveLength(itemsOne.length - 1)
    })

    it("when one row is clicked then 'select' is emited'", async () => {
      await wrapper.find('tbody tr:first-child td:first-child').trigger('click')

      expect(wrapper.emitted()).toHaveProperty('select')
    })

    it("when edit of one row is clicked then 'edit' is emited'", async () => {
      await wrapper.find('tbody tr:first-child .edit-btn').trigger('click')

      expect(wrapper.emitted()).toHaveProperty('edit')
    })
  })

  describe('grid column', () => {
    it('when only one property is shown in the column then there is only one column in the grid (excepted the command one)', async () => {
      const wrapper = mount(GenericGridComponent<{ id: string; name: string }>(), {
        props: {
          items: [{ id: '123', name: 'Test' }],
          properties: [
            { name: 'id', colName: 'id', toShow: false, isUnique: true },
            { name: 'name', colName: 'Content', toShow: true, inForm: true }
          ]
        },
        global
      })

      await nextTick()

      expect(wrapper.findAll('thead td')).toHaveLength(2)
      expect(wrapper.findAll('thead td')).not.contains('id')
      expect(wrapper.findAll('tbody td')).toHaveLength(2)
    })
  })
})
