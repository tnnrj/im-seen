import { InjectionKey } from 'vue';
import { createStore, Store, useStore as baseUseStore } from 'vuex';
import reportDataService from '@/services/report-data.service';
import ObservationsService from '@/services/observations.service';
import { Observation } from '@/model/observations.model';
import auth from './modules/auth';

// define our type for the store state
export interface State {
  chartDatas: Map<string, any>;
  observations: Observation[] | undefined;
}

// define injection key
export const key: InjectionKey<Store<State>> = Symbol()

// create store
export const store = createStore<State>({
  state: {
    chartDatas: new Map<string, any>(),
    observations: undefined
  },
  getters: {
    getReportData: (state) => (reportId) => {
      return state.chartDatas.get(reportId);
    }
  },
  mutations: {
    setChartData(state, payload) {
      state.chartDatas.set(payload.reportId, payload.data);
    },
    setAllObservations(state, payload) {
      state.observations = payload.observations;
    }
  },
  actions: {
    // load data for a single chart from the server
    async loadReportData({ commit }, payload) {
      let response = await reportDataService.getReportData(payload.reportId);
      commit('setChartData', { reportId: payload.reportId, data: response.data });
    },
    // load data for all observations
    async loadAllObservations({ commit }) {
      let response = await ObservationsService.getObservations();
      commit('setAllObservations', { observations: response.data });
    }
  }
})

store.registerModule('auth', auth);

// define our own `useStore` composition function
export function useStore () {
  return baseUseStore(key);
}
