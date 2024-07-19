<template>
  <div class="form_wrapper">
    <VForm @submit="handleSubmit" :validation-schema="schema" class="grid-form">
      <FormField
        v-for="prop in fieldProperties"
        :key="prop.inputName + '-' + getValue(itemLocal, prop.inputName)"
        :input-name="prop.inputName"
        :input-type="'text'"
        :label-name="prop.inputLabel"
        :input-value="getValue(itemLocal, prop.inputName)"
        @inputBlur="onInputBlur"
      />
      <button type="submit" class="grid-form-confirm-btn">Confirm</button>
    </VForm>
  </div>
</template>
<script lang="ts">
import { defineComponent } from 'vue'
import FormField from '../form/FormField.vue'

import { Form as VForm } from 'vee-validate'

export type FieldProperty = {
  inputName?: string
  inputType?: string
  inputLabel?: string
  inputValue?: string
  inputOptions?: Array<string>
  rules?: string
}

class FormFactory<T = unknown> {
  define() {
    return defineComponent({
      components: { FormField, VForm },
      props: {
        item: {
          type: Object as T,
          required: false,
          default: undefined
        },
        fieldProperties: {
          type: Array<FieldProperty>,
          required: true
        }
      },
      computed: {
        schema() {
          let result = {}

          this.fieldProperties.forEach((prop) => {
            if (prop.rules) {
              result = {
                ...result,
                [prop.inputName]: prop.rules
              }
            }
          })
          return result
        }
      },
      data() {
        return {
          itemLocal: Object as T
        }
      },
      watch: {
        item(newItem: Array<T>, oldItem: Array<T>) {
          this.itemLocal = newItem
        }
      },
      methods: {
        onInputBlur(event: { value: string; inputName: string }) {
          this.setValue(event.inputName, event.value)
        },
        getValue<T, K extends keyof T>(data: T, key: K) {
          return data === undefined ? undefined : data[key]
        },
        setValue<T>(item: T, key: string, value: unknown) {
          item = {
            ...item,
            [key]: value
          }
        },
        handleSubmit(item: T) {
          this.$emit('itemSaved', item)
        }
      },
      emits: {
        // eslint-disable-next-line @typescript-eslint/no-unused-vars
        itemSaved: (item: T) => true
      },
      mounted() {
        this.itemLocal = this.item
      }
    })
  }
}

const main = new FormFactory().define()

export function GenericForm<T>() {
  return main as ReturnType<FormFactory<T>['define']>
}

export default main
</script>
<style lang="css"></style>
