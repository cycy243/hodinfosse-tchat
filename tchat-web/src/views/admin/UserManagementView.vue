<template>
  <div>
    <UsersGrid
      :items="users"
      :properties="cols"
      :grid-options="gridOptions"
      @refresh="handleRefresh"
      @select="itemSelected"
      @delete="handleDelete"
      @item-saved="handleSave"
      :edit-btn-txt="'Edit'"
      :delete-btn-txt="'Delete'"
    />
  </div>
</template>
<script lang="ts">
import { defineComponent } from 'vue'

import { GenericGridComponent } from '../../components/grid/GridComponent.vue'
import User from '@/modules/model/User'
import UserRepository from '@/modules/repository/UserRepository'

import useAuthStore from '@/stores/auth'
import { mapState } from 'pinia'

import * as keys from '@/utils/injectionKeys'

export default defineComponent({
  name: 'UserManagementView',
  components: {
    UsersGrid: GenericGridComponent<User>()
  },
  inject: { UserRepository: keys.userRepositoryKey },
  data() {
    return {
      users: new Array<User>(),
      gridOptions: {
        isEditable: true,
        isAddable: true,
        isDeletable: true,
        isRefreshable: true
      },
      cols: [
        { name: 'id', colName: '', toShow: true, inForm: false },
        {
          name: 'userName',
          colName: 'Username',
          toShow: true,
          inForm: true,
          validation: 'required|min:3|max:150'
        },
        {
          name: 'lastName',
          colName: 'Lastname',
          toShow: true,
          inForm: true,
          validation: 'required|min:3|max:150'
        },
        {
          name: 'firstName',
          colName: 'Firstname',
          toShow: true,
          inForm: true,
          validation: 'required|min:3|max:150'
        },
        {
          name: 'email',
          colName: 'Email',
          toShow: true,
          inForm: true,
          validation: 'required|email'
        },
        { name: 'roles', colName: 'role', toShow: false }
      ]
    }
  },
  computed: {
    ...mapState(useAuthStore, ['isAuth', 'user'])
  },
  methods: {
    itemSelected(item: User) {
      console.log(item.email)
    },
    async handleDelete(item: User) {
      if (this.user.id == item.id) {
        return
      }
      const deleteUsersResult = await (this.UserRepository as UserRepository).delete(
        item.id,
        this.user.token
      )
      if (deleteUsersResult.isSuccess) {
        this.users = this.users.filter((u) => u.id !== item.id)
      }
    },
    async handleSave(item: User) {
      if (!item.id || item.id.length === 0) {
        const addUsersResult = await (this.UserRepository as UserRepository).addUser(
          item,
          this.user.token
        )
        if (addUsersResult.isSuccess) {
          this.users = [...this.users, addUsersResult.data!]
        }
      } else {
        const editUsersResult = await (this.UserRepository as UserRepository).editUser(
          item,
          this.user.token
        )
        if (editUsersResult.isSuccess) {
          this.users = [
            ...this.users.filter((u) => u.id !== editUsersResult.data!.id),
            editUsersResult.data!
          ]
        }
      }
    },
    async handleRefresh() {
      const getUsersResult = await (this.UserRepository as UserRepository).getAll(this.user.token)
      if (getUsersResult.isSuccess) {
        this.users = getUsersResult.data!
      }
    }
  },
  async mounted() {
    await this.handleRefresh()
  }
})
</script>
<style lang="css"></style>
