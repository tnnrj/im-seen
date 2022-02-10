<template>
  <!-- <form class="form-upload" enctype="multipart/form-data" @submit.prevent="sendFile">
        <Toast />
        <h4>Student Preloading</h4>
        <h5>Please upload a csv file of student information.</h5>
        <input type="file" ref="file" @change="selectFile"/>
        
        <Button label="Upload" class="p-button-primary" @click="sendFile()"/>
    </form> -->
  <div>
    <Toast />
    <h4>Student Preloading</h4>
    <h5>Please upload a csv file of student information.</h5>
    <FileUpload
      name="file"
      :customUpload="true"
      @uploader="sendFile"
      :fileLimit="1"
      accept=".csv"
      :maxFileSize="10000000"
    >
      <template #empty>
        <p>Drag and drop files to here to upload.</p>
      </template>
    </FileUpload>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { useToast } from "primevue/usetoast";
import FileUpload from "primevue/fileupload";
import StudentsService from '@/services/students.service';

export default defineComponent({
  name: "FileUploadComponent",
  components: { FileUpload },
  data() {
    return {
      file: "",
    };
  },
  setup() {
    const toast = useToast();
    const sendFile = (event) => {
      const formData = new FormData();
      formData.append("file", event.files[0]);
 
      StudentsService.sendStudentsCSV(formData).then((respondData) => {
        toast.add({severity:'success', summary:'Success', detail:`File uploaded ${respondData.name} and ${respondData.size}`, life:3000});
      }).catch((err) => {
        toast.add({severity:'error', summary:'Error', detail:`${err.message}`, life:3000});
      });
    }

    return { sendFile };
  }
});

</script>

<style lang="scss">
.p-fileupload {
  width: 50%;
  margin: auto;
}
</style>