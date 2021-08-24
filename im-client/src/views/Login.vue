<template>
  <div>
    <template v-if="submitted">    
      <Loader />
    </template>
    <template v-else>
        <form @submit.prevent="submit">
          <div>
            <label for="username">Username:</label>
            <input type="text" name="username" v-model="form.username" />
          </div>
          <div>
            <label for="password">Password:</label>
            <input type="password" name="password" v-model="form.password" />
          </div>
          <button type="submit">Login</button>
        </form>
        <p v-if="showError" id="error">Username or Password is incorrect</p>
    </template>
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
      submitted.value = true;
      try {
        await store.dispatch('logIn', form.value);
        router.push('/');
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
* {
  box-sizing: border-box;
}
label {
  padding: 12px 12px 12px 0;
  display: inline-block;
}
button[type=submit] {
  background-color: #4CAF50;
  color: white;
  padding: 12px 20px;
  cursor: pointer;
  border-radius:30px;
}
button[type=submit]:hover {
  background-color: #45a049;
}
input {
  margin: 5px;
  box-shadow:0 0 15px 4px rgba(0,0,0,0.06);
  padding:10px;
  border-radius:30px;
}
#error {
  color: red;
}
</style>