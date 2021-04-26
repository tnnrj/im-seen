<template>
  <div class="loader-content p-d-flex p-ai-center p-jc-center">
    <img src="@/assets/wedges.gif" />
  </div>
</template>

<script>
export default {
  name: "Loader"
}
</script>

<style>
.loader-content {
  height: 100%;
  width: 100%;
}

img {
  width: 20%;
  max-width: 200px;
}
</style>