<template>
  <div class="p-d-flex p-ai-center p-jc-center login-background">
    <div class="register-box p-d-flex p-flex-column p-ai-center">
      <div class="logo p-d-flex p-jc-center">
        <img src="../assets/school.png" />
      </div>
      <template v-if="submitted">
        <Loader class="p-mb-6" />
      </template>
      <template v-else>
        <form @submit.prevent="submit">
          <span class="input p-float-label">
            <InputText id="username" type="text" v-model="form.username" disabled/>
            <label for="username">Username</label>
          </span>
          <span class="input p-float-label">
            <InputText id="firstname" v-model="form.firstname" required/>
            <label for="firstname">First Name</label>
          </span>
          <span class="input p-float-label">
            <InputText id="lastname" v-model="form.lastname" required/>
            <label for="lastname">Last Name</label>
          </span>
          <span class="input p-float-label">
            <InputText id="job" v-model="form.job" />
            <label for="job">Job Title</label>
          </span>
          <span class="input p-float-label">
            <Password id="password" v-model="form.password" required/>
            <label for="password">New Password</label>
          </span>
          <span class="input p-float-label">
            <Password id="passwordConfirm" v-model="form.passwordConfirm" :feedback="false" required/>
            <label for="password">Confirm Password</label>
          </span>
          <Button type="submit" label="Register" icon="pi pi-sign-in" />
        </form>
        <p v-if="errorText" id="error">{{errorText}}</p>
        <p v-if="form.password && form.passwordConfirm && form.password != form.passwordConfirm">Passwords do not match</p>
      </template>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import Loader from "@/components/Loader.vue";
import { useStore } from "@/store/index";
import router from "@/router";
import authenticationService from "@/services/authentication.service";

export default defineComponent({
  name: 'Login',
  components: { Loader },
  props: {
    username: {
      type: String,
      required: true
    },
    token: {
      type: String,
      required: true
    }
  },
  setup(props) {
    const store = useStore();
    const form = ref({ firstname: "", lastname: "", job: "", password: "", passwordConfirm: "", currentPassword: props.token, username: props.username });
    const submitted = ref(false);
    const errorText = ref();

    const submit = async function () {
      if (!form.value || !form.value.firstname || !form.value.lastname || !form.value.password || !form.value.passwordConfirm) {
        errorText.value = "Please fill in all required fields";
        return;
      }

      errorText.value = "";
      submitted.value = true;
      try {
        await authenticationService.changePassword(form.value);
        await store.dispatch('logIn', form.value);
        router.push('/');
      } catch (error) {
        submitted.value = false;
        errorText.value = "An error has occurred. Please try again later.";
      }
    }
    
    return { form, submitted, errorText, submit };
  }
})
</script>

<style lang="scss">
.register-box {
  height: 43em;
  width: 23em;
  border-radius: 10px;
  background-color: white;
}

.logo {
  margin-top: 2em;
  margin-bottom: 1.5em;

  img {
    border-radius: 10px;
    height: 10em;
    width: 10em;
  }
}

.input {
  width: 80%;
  margin-bottom: 1.5em;
}

#error {
  font-size: .8em;
  color: red;
}
</style>