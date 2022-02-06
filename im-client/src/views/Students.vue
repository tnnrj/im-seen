<template>
  <div class="content p-d-flex">
    <div class="file-upload-section">
      <form class="form-upload" enctype="multipart/form-data">
        <label for="file" class="label">Please upload a csv file.</label><br><br>
        <input type="file" />
      </form>
    </div>
  </div>
</template>

<script lang="ts">
import { computed, defineComponent, ref } from "vue";
import UsersService from "@/services/users.service";
import { User } from "@/model/user.model";
import authenticationService from "@/services/authentication.service";
import validatorService from "@/services/validator.service";
import { useToast } from "primevue/usetoast";
import UserComponent from "@/components/User.vue";
import * as _ from "lodash";

export default defineComponent({
  name: "Students",
  components: {},
  setup() {
    return {};
  },
});
</script>

<style lang="scss">
.content {
  height: 30%;
  padding: 1em;
}

.file-upload-section {
  background: white;
  flex: 1;
}

.form-upload {
  margin-top: 5em;
}
</style>