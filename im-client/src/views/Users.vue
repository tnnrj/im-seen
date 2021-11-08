<template>
  <div class="users-content p-d-flex">
    <Toast />
    <DataTable class="users-table" :value="users" dataKey="email" :scrollable="true" scrollHeight="flex">
      <Column field="lastName" header="Last Name" :sortable="true" style="flex: 1 1 20%"></Column>
      <Column field="firstName" header="First Name" :sortable="true" style="flex: 1 1 20%"></Column>
      <Column field="email" header="Email" :sortable="true" style="flex: 1 1 25%"></Column>      
      <Column field="jobTitle" header="Job Title" :sortable="true" style="flex: 1 1 20%"></Column>
      <Column field="userRole" header="Role" :sortable="true" style="flex: 1 1 15%"></Column>
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
    <div class="sidebar p-d-flex p-flex-column p-ai-end">
      <i class="pi pi-plus-circle" v-tooltip="'Add User'" @click="showAddUserDialog = true" />
    </div>
  </div>
  <Dialog class="add-user-dialog" header="Add New User" v-model:visible="showAddUserDialog" :modal="true" :contentStyle="{'height':'10em', 'width':'30em'}">
    <p>Add a new dashboard user by email. They will receive a link to activate their account within a few minutes.</p>
    <span class="input p-float-label">
      <InputText id="email" type="text" v-model="email" />
      <label for="email">Email Address</label>
    </span>
    <template #footer>
      <Button label="Cancel" icon="pi pi-times" @click="showAddUserDialog = false" class="p-button-text"/>
      <Button label="Add" icon="pi pi-check" :disabled="!emailValid" @click="addUser" />
    </template>
  </Dialog>
</template>

<script lang="ts">
import { computed, defineComponent, ref } from "vue";
import Loader from "@/components/Loader.vue";
import UsersService from '@/services/users.service';
import { User } from "@/model/user.model";
import authenticationService from "@/services/authentication.service";
import validatorService from "@/services/validator.service";
import { useToast } from "primevue/usetoast";

export default defineComponent({
  name: 'Users',
  components: { Loader },
  setup() {
    const toast = useToast();

    const users = ref<User[]>();
    const loadUsers = async () => {
      users.value = await UsersService.getAllUsers();
    }
    loadUsers();

    const showReportingDialog = ref();

    const email = ref<string>();
    const emailValid = computed(() => !!email.value && validatorService.isValidEmail(email.value));
    const showAddUserDialog = ref();
    const addUser = () => {
      if (!emailValid.value) {
        return;
      }
      authenticationService.addUser({ email: email.value as string }).then(() => {
        toast.add({severity:'success', summary:'Success', detail:'User invitation sent', life:3000});
        loadUsers();
      }).catch(() => {
        toast.add({severity:'error', summary:'Error', detail:'Failed to add user. Try again later', life:3000});
      });
      showAddUserDialog.value = false;
      email.value = '';
    }

    return { users, showReportingDialog, showAddUserDialog, email, emailValid, addUser };
  }
})
</script>

<style lang="scss">
.users-content {
  height: 100%;
  padding: 1em;
}

.users-table {
  flex: 1;
  margin-right: .5em;
}

.sidebar {
  width: 1.5em;

  i {
    cursor: pointer;
    padding: .25em;
    color: var(--surface-600);
    transition: 0.3s;

    &:hover {
      color: var(--surface-900);
    }
  }
}

.add-user-dialog {
  .input {
    margin-top: 2em;
    min-width: 15em;
    width: 70%;

    input {
      width: 100%;
    }
  }
}

</style>