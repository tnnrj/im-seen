<template>
  <div class="file-upload-content">
    <Toast />
    <ConfirmPopup></ConfirmPopup>
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
    header="Please wait..."
    v-model:visible="showUploadDialog"
    :closeOnEscape="false"
    :modal="true"
    :closable="!isUploading"
    :contentStyle="{ height: '12em', width: '15em' }"
  >   
    <template v-if="isUploading">
      <ProgressSpinner />
      <p>Uploading...</p>
    </template>
    <template v-else>
      <p>{{ message }}</p>     
    </template>
  </Dialog>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import FileUpload from "primevue/fileupload";
import { useToast } from "primevue/usetoast";
import ProgressSpinner from 'primevue/progressspinner';
import StudentsService from "@/services/students.service";

export default defineComponent({
  name: "FileUploadComponent",
  components: { FileUpload, ProgressSpinner },
  setup() {
    const toast = useToast();

    const showUploadDialog = ref();
    const isUploading = ref(true);
    let message = ref<string>();

    const sendFile = async (event) => {
      isUploading.value = true;
      showUploadDialog.value = true;
      const formData = new FormData();
      formData.append("file", event.files[0]);

      setTimeout(() => {
        StudentsService.sendStudentsCSV(formData)
        .then((respondData) => {         
          message.value = "Uploaded successfully!";
          setTimeout(() => {
            message.value = "Refreshing...";
            setTimeout(() => {
              window.location.reload();
            }, 1000);
          }, 1000);
        })
        .catch((err) => {
          message.value = `${err.response.data}`;
        }).finally(() => {
          isUploading.value = false;
        });
      }, 2000);    
    };

    const downloadCSVFile = async () => {
      StudentsService.getFileCSV()
        .then((respondData) => {         
          const blob = new Blob([respondData], { type: 'text/csv' });
          const link = document.createElement('a');
          link.href = URL.createObjectURL(blob);
          link.download = "template.csv";
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
  width: 100%;
  margin: auto;
}
.uploading-dialog {
  margin: auto;
}
.file-upload-content {  
  text-align: center;
}
</style>