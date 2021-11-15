<template>
  <div class="user-content">
    <template v-if="usr && !saving">
      <div class="p-d-flex user-panel">       
        <span class="input p-float-label">
          <InputText id="firstname" v-model="usr.firstName" required />
          <label for="firstname">First Name</label>
        </span>
        <span class="input p-float-label">
          <InputText id="lastname" v-model="usr.lastName" required />
          <label for="lastname">Last Name</label>
        </span>
        <span class="input p-float-label">
          <InputText id="job" v-model="usr.jobTitle" />
          <label for="job">Job Title</label>
        </span> 
        <span class="input p-float-label">
          <Dropdown id="role" :options="userRoleOptions" v-model="usr.role" required />
          <label for="role">User Role</label>
        </span>
        <div width="100%">
          <Button class="p-button-primary" label="Save" @click="save" />
        </div>
      </div>
    </template>
    <template v-else>
      <Loader />
    </template>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, watchEffect } from "vue";
import * as _ from "lodash";
import { User } from "@/model/user.model";
import usersService from "@/services/users.service";
import { UserRole } from "@/model/enums.model";

export default defineComponent({
  name: "User",
  props: {
    user: Object
  },
  setup(props) {
    const usr = ref<User>();
    usr.value = props.user as User;
    watchEffect(() => {
      usr.value = props.user as User;
    });

    const saving = ref<boolean>(false);
    const save = function () {
      if (usr.value) {
        saving.value = true;
        usersService.updateUser(usr.value).then(() => saving.value = false);
      }
    };

    const userRoleOptions = ref<UserRole[]>(Object.keys(UserRole) as UserRole[])

    return { usr, save, saving, userRoleOptions };
  }
});
</script>

<style lang="scss">
.user-content {
  height: 100%;
}
.user-panel {  
  width: 100%;
  height: 100%;
  flex-flow: row wrap;
}
.input {
  width: 50%;
  margin: 1.25em 0;

  input {
    width: 90%;
  }
}
</style>