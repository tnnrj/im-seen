<template>
  <div>
    <h4>Student Preloading</h4>
    <h5>Please upload a csv file (&#60; 50MB) of student information.</h5>
    <FileUpload
      name="file"
      :customUpload="true"
      @uploader="sendFile"
      :fileLimit="1"
      accept=".csv"
      :maxFileSize="50000000"
    >
      <template #empty>
        <p>Drag and drop files to here to upload.</p>
      </template>
    </FileUpload>
  </div>

  <Dialog
    class="uploading-dialog"
    header="Uploading"
    v-model:visible="showUploadDialog"
    :modal="true"
    :contentStyle="{ height: '40em', width: '40em' }"
  >   
    <template v-if="isUploading">
      <Loader />
      <p>Uploading...</p>
    </template>
    <template v-else>
      {{ message }}
    </template>
  </Dialog>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import FileUpload from "primevue/fileupload";
import StudentsService from "@/services/students.service";
import Loader from "@/components/Loader.vue";

export default defineComponent({
  name: "FileUploadComponent",
  components: { FileUpload, Loader },
  setup() {
    const showUploadDialog = ref();
    const isUploading = ref(true);
    let message = ref<string>();

    const sendFile = async (event) => {
      showUploadDialog.value = true;
      const formData = new FormData();
      formData.append("file", event.files[0]);

      StudentsService.sendStudentsCSV(formData)
        .then((respondData) => {         
          message.value = "Uploaded successfully!";
        })
        .catch((err) => {
          message.value = "Error occured. Failed!";
        });
      isUploading.value = false;
    };

    return { sendFile, showUploadDialog, isUploading, message };
  },
});
</script>

<style lang="scss">
.p-fileupload {
  width: 50%;
  margin: auto;
}
</style>