import { describe, it, expect, beforeAll, beforeEach } from 'vitest'

import { VueWrapper, mount, shallowMount, createLocalVue } from '@vue/test-utils'
import {
  ErrorMessage as VErrorMessage,
  Field as VField,
  Form as VForm,
  configure,
  defineRule,
  useField,
  useForm
} from 'vee-validate'

import VeeValidate from 'vee-validate'

import { required } from '@vee-validate/rules'

import FormField from '@/components/form/FormField.vue'

import { nextTick } from 'vue'
import flushPromises from 'flush-promises'

defineRule('required', required)

configure({
  generateMessage: (ctx: { rule: { name: string }; field: string }) => {
    const messages = {
      required: `${ctx.field} is required`
    }

    return messages[ctx.rule.name] || 'This field is invalid'
  }
})

describe('FormFieldTests', async () => {
  let global: any
  let wrapper: VueWrapper

  beforeEach(async () => {
    global = {
      components: {
        VField,
        VErrorMessage
      }
    }
    wrapper = mount(FormField, {
      props: {
        inputName: 'test-input',
        inputType: 'text',
        labelName: 'Test label',
        placeholder: 'Your input'
      },
      global
    })
    await nextTick()
  })

  describe('properties attribution', () => {
    it('when all data are given then the input is setted up with them', () => {
      const input = wrapper.find('input')
      expect(input.exists()).toBeTruthy()
      const attributes = input.attributes()
      expect(attributes.id).toBe('test-input')
      expect(attributes.type).toBe('text')
      expect(attributes.value).toBeUndefined()
      expect(attributes.name).toBe('test-input')
      expect(attributes.placeholder).toBe('Your input')
    })

    it('when input value is setted then the input has it as a value', async () => {
      wrapper = mount(FormField, {
        props: {
          inputName: 'test-input',
          inputType: 'text',
          inputValue: 'Start value',
          labelName: 'Test label',
          placeholder: 'Your input'
        },
        global
      })
      await nextTick()
      const input = wrapper.find('input')

      expect(input.element.value).toBe('Start value')
    })

    it('when input value prop is re-set then the input has it as a value', async () => {
      wrapper = mount(FormField, {
        props: {
          inputName: 'test-input',
          inputType: 'text',
          inputValue: 'Start value',
          labelName: 'Test label',
          placeholder: 'Your input'
        },
        global
      })
      await nextTick()
      await wrapper.setProps({
        inputValue: 'Second value'
      })
      await nextTick()
      const input = wrapper.find('input')
      expect(input.element.value).toBe('Second value')
    })

    it('when type is "textarea" then no input displayed but only a textarea', async () => {
      wrapper = mount(FormField, {
        props: {
          inputName: 'test-input',
          inputType: 'textarea',
          labelName: 'Test label',
          placeholder: 'Your input'
        },
        global
      })
      await nextTick()

      expect(wrapper.find('input').exists()).toBeFalsy()
      const textarea = wrapper.find('textarea')
      expect(textarea.exists()).toBeTruthy()
      const attributes = textarea.attributes()
      expect(attributes.cols).toBe('50')
      expect(attributes.rows).toBe('5')
      expect(attributes.id).toBe('test-input')
      expect(attributes.value).toBeUndefined()
      expect(attributes.name).toBe('test-input')
      expect(attributes.placeholder).toBe('Your input')
    })

    it('when type is "textarea" and a start value is given then a textarea is displayed with the given value', async () => {
      wrapper = mount(FormField, {
        props: {
          inputName: 'test-input',
          inputType: 'textarea',
          labelName: 'Test label',
          placeholder: 'Your input',
          inputValue: 'Start value'
        },
        global
      })
      await nextTick()

      expect(wrapper.find('input').exists()).toBeFalsy()
      const textarea = wrapper.find('textarea')

      expect(textarea.exists()).toBeTruthy()
      const attributes = textarea.attributes()
      expect(attributes.cols).toBe('50')
      expect(attributes.rows).toBe('5')
      expect(attributes.id).toBe('test-input')
      expect(textarea.element.value).toBe('Start value')
      expect(attributes.name).toBe('test-input')
      expect(attributes.placeholder).toBe('Your input')
    })
  })

  describe('event emit', () => {
    it("when input lose focus then emit 'inputBlur' event", async () => {
      const input = wrapper.find('input')
      input.trigger('blur')
      expect(wrapper.emitted()).toHaveProperty('inputBlur')

      const wrapperTextarea = mount(FormField, {
        props: {
          inputName: 'test-input',
          inputType: 'textarea',
          labelName: 'Test label',
          placeholder: 'Your input',
          inputValue: 'Start value'
        },
        global
      })
      await nextTick()

      const textarea = wrapperTextarea.find('textarea')
      textarea.trigger('blur')
      expect(wrapper.emitted()).toHaveProperty('inputBlur')
    })
  })
})
