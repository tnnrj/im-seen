<template>
  <div class="observations-content">
    <div class="observation-table-filters">
      <div class="p-mr-2">
        <Button class="p-button-rounded p-button-text" :class="[ hasFilter() ? 'p-button-danger' : 'p-button-plain' ]" 
          :icon="hasFilter() ? 'pi pi-filter-slash' : 'pi pi-filter'" :disabled="!hasFilter()" @click="resetFilters()"/>
      </div>
      <div class="p-mr-2 filter-field">
        <Dropdown placeholder="Status" v-model="statusFilter" :options="statusFilterOptions" />
      </div>
      <div class="p-mr-2 filter-field">
        <Dropdown placeholder="Action" v-model="actionFilter" :options="actionFilterOptions" />
      </div>
      <div v-if="userRole != UserRole.SupportingActor" class="p-mr-2 filter-field">
        <Dropdown placeholder="Students" v-model="studentsFilter" :options="studentsFilterOptions" />
      </div>
    </div>
    <div class="observation-table-wrapper">
      <ObservationTable :records="observations" />
    </div>
  </div>
</template>

<script lang="ts">
import { computed, defineComponent, ref } from "vue";
import { useStore } from "@/store";
import ObservationTable from '@/components/ObservationTable.vue';
import * as _ from 'lodash';
import { ObservationStatus, UserRole } from "@/model/enums.model";

export default defineComponent({
  name: 'Observations',
  components: { ObservationTable },
  setup() {
    const store = useStore();
    if (!store.state.observations) store.dispatch('loadAllObservations');
    if (!store.state.user) store.dispatch('getUser');
    if (!store.state.myStudents) store.dispatch('getMyStudents');

    const observations = computed(() => {
      if (!store.state.observations) return null;

      let result = store.state.observations;
      if (statusFilter.value) {
        result = _.filter(result, o => o.status == statusFilter.value);
      }
      if (studentsFilter.value === 'My Students') {
        result = _.filter(result, o => _.includes(myStudents.value, o.studentID));
      }
      if (studentsFilter.value === 'Unidentified') {
        result = _.filter(result, o => !o.studentID);
      }
      if (actionFilter.value === 'Action Taken') {
        result = _.filter(result, o => o.action);
      }
      if (actionFilter.value === 'No Action') {
        result = _.filter(result, o => !o.action);
      }

      return result;
    });

    const userRole = computed(() => store.state.user?.role);
    const myStudents = computed(() => store.state.myStudents);

    const statusFilter = ref<ObservationStatus>();
    const statusFilterOptions = ref<ObservationStatus[]>(Object.keys(ObservationStatus) as ObservationStatus[]);
    const studentsFilter = ref<string>();
    const studentsFilterOptions = ref<string[]>(['My Students', 'Unidentified']);
    const actionFilter = ref<string>();
    const actionFilterOptions = ref<string[]>(['Action Taken', 'No Action']);

    const hasFilter = () => statusFilter.value || studentsFilter.value || actionFilter.value;
    const resetFilters = () => {
      statusFilter.value = undefined;
      studentsFilter.value = undefined;
      actionFilter.value = undefined;
    }

    return { observations, userRole, UserRole,
      statusFilter, statusFilterOptions, studentsFilter, studentsFilterOptions, actionFilter, actionFilterOptions, hasFilter, resetFilters
    };
  }
})
</script>

<style lang="scss">
.observations-content {
  height: 100%;
  padding: 1em;
  display: flex;
  flex-direction: column;
}
.observation-table-filters {
  background: #f8f9fa;
  color: #495057;
  border: 1px solid #e9ecef;
  border-width: 1px 0 1px 0;
  padding: 1em;
  display: flex;
}
.filter-field {
  text-align: left;
  width: 10em;
  .p-inputwrapper {
    width: 100%;
  }
}
.observation-table-wrapper {
  flex: 1;
  min-height: 0;
}
.clear-button {
  height: 3em;
  width: 3em;
  color: #6c757d;
  border: 0 none;
  border-radius: 50%;
  background: transparent;
  transition: background-color 0.2s, color 0.2s, box-shadow 0.2s;

  &:hover {
    color: #495057;
    background: #e9ecef;
  }
}
</style>