<template lang="">
  <div>
    <div v-if="formIsVisible && (gridOptions.isAddable || gridOptions.isEditable)" class="form_wrapper">
      <GridForm :fieldProperties="formProperties" :item="selectedItem" @item-saved="handleSubmit" />
    </div>
    <div>
      <span @click="onRefresh" class="grid_action_btn refresh_btn" v-if="gridOptions.isRefreshable">{{ gridTextOptions.refreshTxt || 'Refresh' }}</span>
      <span @click="onCreateClicked" class="grid_action_btn create_btn" v-if="gridOptions.isAddable">{{ gridTextOptions.addTxt || 'Create' }}</span>
    </div>
    <table>
      <thead>
        <template v-for="(value, key) in properties">
        <td v-if="value.toShow" :key="key">
          <th>{{ value.colName }}</th>
        </td>
        </template>
          <td v-if="showActionBtn">
          </td>
      </thead>
      <tbody>
        <tr v-for="(item, key) in itemsLocal" :key="key">
          <template v-for="(value, index) in properties" :key="index">
            <td @click="() => onSelect(item)" v-if="value.toShow">{{ getValue(item, value.name) }}</td>
          </template>
          <td v-if="showActionBtn">
            <span @click="() => onEdit(item)" class="grid_action_btn edit-btn" v-if="gridOptions.isEditable">{{ gridTextOptions.editTxt || 'Edit' }}</span>
            <span @click="() => onDelete(item)" class="grid_action_btn delete-btn" v-if="gridOptions.isDeletable">{{ gridTextOptions.deleteTxt || 'Delete' }}</span>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
<script lang="ts">
import { defineComponent, type PropType } from 'vue'
import { type FieldProperty, GenericForm } from '../../../src/components/form/GenericForm.vue'

export interface GridProperty {
  name: string
  colName?: string | undefined
  toShow?: boolean
  inForm?: boolean,
  isUnique?: boolean,
  validation?: string,
  defaultValue?: string,
  options?: Array<string>
}

export type GridOptions = {
  isEditable?: boolean,
  isAddable?: boolean,
  isDeletable?: boolean,
  isRefreshable?: boolean
}

export interface GridTextOptions {
  deleteTxt?: string,
  editTxt?: string,
  addTxt?: string,
  refreshTxt?: string
}

export interface ColCommandOption {
  name: string,
  id: string
}

class GridComponentFactory<T = unknown> {
  define() {
    return defineComponent({
      components: { GridForm: GenericForm<T>() },
      props: {
        items: {
          type: Array<T>,
          required: true
        },
        gridOptions: {
          type: Object as PropType<GridOptions>,
          default: () => ({
            isEditable: true,
            isAddable: true,
            isDeletable: true,
            isRefreshable: true
          })
        },
        gridTextOptions: {
          type: Object as PropType<GridTextOptions>,
          default: () => ({
          deleteTxt: 'Delete',
          editTxt: 'Edit',
          addTxt: 'Add',
          refreshTxt: 'Refresh'
          })
        },
        properties: {
          type: Array<GridProperty>,
          required: true
        },
        editBtnTxt: {
          type: String,
          default: ''
        },
        deleteBtnTxt: {
          type: String,
          default: ''
        }
      },
      computed: {
        uniqueCol() {
          return this.properties.find(p => p.isUnique)?.name || ''
        },
        showActionBtn() {
          return this.gridOptions.isEditable || this.gridOptions.isDeletable
        },
      },
      data() {
        return {
          selectedItem: {
            type: Object as T,
            required: true
          },
          itemsLocal: new Array<T>(),
          formIsVisible: false,
          formProperties: new Array<FieldProperty>()
        }
      },
      watch: {
        items(newItems: Array<T>, oldItems: Array<T>) {
          this.itemsLocal = [...newItems]
        }
      },
      methods: {
        onSelect(item: T) {
          this.selectedItem = item;
          this.$emit('select', item)
        },
        onInputBlur(event: {value: string, inputName: string}) {
          this.setValue(event.inputName, event.value)
        },
        onRefresh() {
          this.$emit('refresh')
        },
        onDelete(item: T) {
          this.$emit('delete', item, () => {
            this.itemsLocal = this.items.filter(i => this.getValue(i, this.uniqueCol) !== this.getValue(item, this.uniqueCol))
          })
        },
        onEdit(item: T) {
          this.selectedItem = item
          this.showForm()
          this.$emit('edit', item)
        },
        getValue<T, K extends keyof T>(data: T, key: K) {
          if(data === undefined) { return undefined }
          return data[key];
        },
        setValue(key: string, value: unknown) {
          this.selectedItem = {
            ...this.selectedItem,
            [key]: value
          };
        },
        handleSubmit(item: T) {
          this.itemsLocal = this.items.filter(i => this.getValue(i, this.uniqueCol) !== this.getValue(item, this.uniqueCol))
          this.itemsLocal.push(item)
          this.$emit('itemSaved', {
            ...this.selectedItem,
            ...item
          })
        },
        onCreateClicked() {
          this.selectedItem = undefined
          this.showForm()
        },
        showForm() {
          this.formIsVisible = true
        }
      },
      emits: {
        // eslint-disable-next-line @typescript-eslint/no-unused-vars
        select: (item: T) => true,
        refresh: () => true,
        // eslint-disable-next-line @typescript-eslint/no-unused-vars
        delete: (item: T, done: () => void) => true,
        // eslint-disable-next-line @typescript-eslint/no-unused-vars
        edit: (item: T) => true,
        // eslint-disable-next-line @typescript-eslint/no-unused-vars
        itemSaved: (item: T) => true,
      },
      mounted() {
        this.itemsLocal = [...this.items]
        this.formProperties = []
        this.properties.forEach(prop => {
          if(prop.inForm) {
            this.formProperties.push({
              inputName: prop.name,
              rules: prop.validation,
              inputValue: prop.defaultValue,
              inputLabel: prop.colName,
              inputType: 'text',
            })
          }
        });
      }
    })
  }
}

const main = new GridComponentFactory().define()

export function GenericGridComponent<T>() {
  return main as ReturnType<GridComponentFactory<T>['define']>
}

export default main
</script>
<style lang="css" scoped>
.no_display {
  display: none;
}

thead td {
  text-align: center;
}

td {
  padding: .2rem;
}

.grid_action_btn {
  cursor: pointer;
  padding-inline: .5rem;
  padding-block: .2rem;
  margin-inline: .5rem;
  border-radius: 25px;
  border: 1px solid grey;
}

.grid_action_btn:hover {
  border-color: transparent;
}
</style>
