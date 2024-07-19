<template>
  <div class="form_field">
    <label :for="inputName">{{ labelName }}</label>
    <div>
      <VField
        v-if="inputType !== 'textarea'"
        :id="inputName"
        :type="inputType"
        :name="inputName"
        :placeholder="placeholder"
        class="form_field_input"
        :class="{ has_error: errorMsg.length > 0 }"
        v-model="localValue"
        @blur="(value: any) => emitInputChange(value.srcElement.value, inputName)"
      />
      <VField
        v-else
        :name="inputName"
        class="form_field_input"
        :class="{ has_error: errorMsg.length > 0 }"
        @blur="(value: any) => emitInputChange(value.srcElement.value, inputName)"
        v-slot="{ field }"
        v-model="localValue"
      >
        <textarea
          v-bind="field"
          :id="inputName"
          :name="inputName"
          :placeholder="placeholder"
          :cols="col"
          :rows="row"
        ></textarea>
      </VField>
      <span v-if="displayError" class="error_msg_content">{{ errorMsg }}</span>
    </div>
    <VErrorMessage as="div" :name="inputName" class="error_message" v-slot="{ message }">
      <span
        :onmouseover="() => hoverTheAttention(message ?? '')"
        :onmouseleave="() => hoverTheAttention(message ?? '')"
        >!</span
      >
    </VErrorMessage>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue'

import { ErrorMessage as VErrorMessage, Field as VField } from 'vee-validate'

export default defineComponent({
  name: 'FormField',
  emits: ['inputBlur'],
  data() {
    return {
      errorMsg: '',
      displayError: false,
      localValue: ''
    }
  },
  components: {
    VErrorMessage,
    VField
  },
  props: {
    inputName: {
      type: String,
      required: true
    },
    inputValue: {
      type: String,
      default: '',
      required: false
    },
    inputType: {
      type: String,
      required: false,
      default: 'text'
    },
    labelName: String,
    placeholder: String,
    col: {
      type: Number,
      required: false,
      default: 50
    },
    row: {
      type: Number,
      required: false,
      default: 5
    }
  },
  watch: {
    inputValue(newVal: string, oldVal: string) {
      this.localValue = newVal
    }
  },
  methods: {
    emitInputChange(value: string, inputName: string) {
      this.$emit('inputBlur', { value, inputName })
    },
    hoverTheAttention(message: string) {
      this.errorMsg = message
      this.displayError = !this.displayError
    }
  },
  mounted() {
    this.localValue = this.inputValue
  }
})
</script>

<style lang="css" scoped>
.form_field,
.error_message {
  display: flex;
  flex-direction: row;
}
.error_message div {
  display: none;
}

.form_field .form_field_input {
  width: 12rem;
}

.form_field label {
  width: 9rem;
}

.has_error {
  border-color: red;
}

.error_msg_content {
  position: absolute;
  color: red;
  margin-top: 1.4rem;
  background-color: brown;
  margin-left: -12rem;
}

.error_message span {
  margin-left: 0.5rem;
  width: 2rem;
  width: 2rem;
}

button {
  margin: auto;
}
</style>
