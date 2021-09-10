<template>
  <div class="users-content">
    <DataTable :value="users" dataKey="username" :scrollable="true" scrollHeight="flex">
      <Column field="fullName" header="Name" :sortable="true" style="flex: 1 1 34%"></Column>
      <Column field="username" header="Username" :sortable="true" style="flex: 1 1 33%"></Column>
      <Column field="userRole" header="Role" :sortable="true" style="flex: 1 1 33%"></Column>
      <template #empty>
        <div style="height:100%; width:100%">
          <template v-if="users">
            No records to display
          </template>
          <template v-else>
            <Loader />
          </template>
        </div>
      </template>
    </DataTable>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import Loader from "@/components/Loader.vue";
import UsersService from '@/services/users.service';

export default defineComponent({
  name: 'Users',
  components: { Loader },
  setup() {
    const users = ref();
    UsersService.getAllUsers().then(response => {
      users.value = response.data;
    });

    return { users };
  }
})
</script>

<style lang="scss">
.users-content {
  height: 100%;
  padding: 1em;
}

</style>