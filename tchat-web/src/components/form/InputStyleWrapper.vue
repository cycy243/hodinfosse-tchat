<template>
  <div class="form_input">
    <slot></slot>
  </div>
</template>
<script lang="ts">
import { defineComponent } from 'vue'

export default defineComponent({
  name: 'InputStyleWrapper'
})
</script>
<style lang="css">
.form_input {
  margin: 0.2rem;
  display: flex;
}

.form_input label {
  width: 10rem;
}
</style>
