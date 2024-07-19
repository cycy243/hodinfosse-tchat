import { describe, it, expect, beforeAll, beforeEach } from 'vitest'

import { VueWrapper, mount } from '@vue/test-utils'
import {
  ErrorMessage as VErrorMessage,
  Field as VField,
  Form as VForm,
  configure,
  defineRule,
  useField,
  useForm
} from 'vee-validate'
import FormField from '@/components/form/FormField.vue'

import { nextTick } from 'vue'
import { required } from '@vee-validate/rules'
import { GenericForm } from '@/components/form/GenericForm.vue'

import flushPromises from 'flush-promises'
import waitForExpect from 'wait-for-expect'

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

describe('GenericFormTests', async () => {
  let global: any
  let wrapper: VueWrapper
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
    await nextTick()
    wrapper = mount(GenericForm<{ id: string; name: string }>(), {
      props: {
        item: itemsOne[0],
        fieldProperties: [
          {
            inputLabel: 'Name',
            inputType: 'text',
            inputValue: 'jhklh',
            inputName: 'name',
            rules: 'required'
          }
        ]
      },
      global
    })
    await nextTick()
  })

  describe('props settings', () => {
    it('when props set for one field then form only has one field', async () => {
      expect(wrapper.findAll('input')).toHaveLength(1)

      const { validate } = useForm<{ name: string }>({
        validationSchema: {
          name: 'required'
        }
      })

      wrapper.find('input').setValue('')

      await validate()

      await nextTick()

      await flushPromises()

      expect(wrapper.find('.error_message').exists()).toBeTruthy()
    })
  })
})
