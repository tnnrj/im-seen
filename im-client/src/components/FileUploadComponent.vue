<template>
  <div>
    <Toast />
    <h4>Student Preloading</h4>
    <h5>Please use <a href="" @click.prevent="downloadCSVFile()">this template</a> for uploading. File size &#60; 50MB.</h5>
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
import { useToast } from "primevue/usetoast";
import StudentsService from "@/services/students.service";
import Loader from "@/components/Loader.vue";

export default defineComponent({
  name: "FileUploadComponent",
  components: { FileUpload, Loader },
  setup() {
    const toast = useToast();

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
          // TODO: cannot get error message from backend
          message.value = `Error occured. Failed! ${err.message}`;
        });
      isUploading.value = false;
    };

    const downloadCSVFile = async () => {
      StudentsService.getFileCSV()
        .then((respondData) => {         
          const blob = new Blob([respondData], { type: 'text/csv' });
          const link = document.createElement('a');
          link.href = URL.createObjectURL(blob);
          link.download = "template";
          link.click();
          URL.revokeObjectURL(link.href);
        })
        .catch((err) => {
          toast.add({severity:'error', summary:'Error', detail:'Unable to get file.', life:3000});         
        });
    }

    return { sendFile, downloadCSVFile, showUploadDialog, isUploading, message };
  },
});
</script>

<style lang="scss">
.p-fileupload {
  width: 50%;
  margin: auto;
}
</style>