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
    getReportData: (state) => (reportID) => {
      return state.chartDatas.get(reportID);
    }
  },
  mutations: {
    setChartData(state, payload) {
      state.chartDatas.set(payload.reportID, payload.data);
    },
    setAllObservations(state, payload) {
      state.observations = payload.observations;
    }
  },
  actions: {
    // load data for a single chart from the server
    async loadReportData({ commit }, payload) {
      let response = await reportDataService.getReportData(payload.reportID);
      commit('setChartData', { reportID: payload.reportID, data: response });
    },
    // load data for all observations
    async loadAllObservations({ commit }) {
      let response = await ObservationsService.getObservations();
      commit('setAllObservations', { observations: response });
    }
  }
})

store.registerModule('auth', auth);

// define our own `useStore` composition function
export function useStore () {
  return baseUseStore(key);
}
