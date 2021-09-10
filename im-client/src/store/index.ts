import { InjectionKey } from 'vue';
import { createStore, Store, useStore as baseUseStore } from 'vuex';
import ChartDataService from '@/services/chart-data.service';
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
    getChartData: (state) => (queryId) => {
      return state.chartDatas.get(queryId);
    }
  },
  mutations: {
    setChartData(state, payload) {
      state.chartDatas.set(payload.queryId, payload.data);
    },
    setAllObservations(state, payload) {
      state.observations = payload.observations;
    }
  },
  actions: {
    // load data for a single chart from the server
    async loadChartData({ commit }, payload) {
      let response = await ChartDataService.getChartData(payload.queryId);
      commit('setChartData', { queryId: payload.queryId, data: response.data });
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
