<template>
  <div class="p-d-flex p-ai-center p-jc-center login-background">
    <div class="login-box p-d-flex p-flex-column p-ai-center">
      <div class="logo p-d-flex p-jc-center">
        <img src="../assets/school.png" />
      </div>
      <template v-if="submitted">
        <Loader class="p-mb-6" />
      </template>
      <template v-else>
        <form @submit.prevent="submit">
          <span class="input p-float-label">
            <InputText id="username" type="text" v-model="form.username" />
            <label for="username">Username</label>
          </span>
          <span class="input p-float-label">
            <Password id="password" v-model="form.password" :feedback="false" />
            <label for="password">Password</label>
          </span>
          <Button type="submit" label="Sign In" icon="pi pi-sign-in" />
        </form>
        <p v-if="showError" id="error">Username or Password is incorrect</p>
      </template>
    </div>
  </div>
</template>

<script lang='ts'>
import { defineComponent, ref } from "vue";
import Loader from "@/components/Loader.vue";
import { useStore } from "@/store/index";
import router from "@/router";

export default defineComponent({
  name: 'Login',
  components: { Loader },
  setup() {
    const store = useStore();
    const form = ref({ username: "", password: "" });
    const submitted = ref(false);
    const showError = ref(false);

    const submit = async function () {
      if (!form.value || !form.value.username) {
        showError.value = true;
        return;
      }

      showError.value = false;
      submitted.value = true;
      try {
        setTimeout(() => {
          store.dispatch('logIn', form.value);
          router.push('/');
        }, 1000);
      } catch (error) {
        submitted.value = false;
        showError.value = true;
      }
    }
    
    return { form, submitted, showError, submit };
  }
})
</script>

<style scoped lang='scss'>
.login-background {
  height: 100%;
  width: 100%;
  background: rgb(34,193,195);
  background: linear-gradient(168deg, rgba(34,193,195,1) 0%, rgba(253,187,45,1) 100%);
}

.login-box {
  height: 30em;
  width: 20em;
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