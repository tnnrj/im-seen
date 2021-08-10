import { InjectionKey } from 'vue';
import { createStore, Store, useStore as baseUseStore } from 'vuex';
import ChartDataService from '@/services/chart-data.service';'@/services/chart-data.service';

// define our type for the store state
export interface State {
  chartDatas: Map<string, any>;
}

// define injection key
export const key: InjectionKey<Store<State>> = Symbol()

// create store
export const store = createStore<State>({
  state: {
    chartDatas: new Map<string, any>()
  },
  getters: {
    getChartData: (state) => (queryId) => {
      return state.chartDatas.get(queryId);
    }
  },
  mutations: {
    setChartData(state, payload) {
      state.chartDatas.set(payload.queryId, payload.data);
    }
  },
  actions: {
    loadChartData({ commit }, payload) {
      ChartDataService.getChartData(payload.queryId).then(response => {
        commit('setChartData', { queryId: payload.queryId, data: response.data });
      });
    }
  }
})

// define our own `useStore` composition function
export function useStore () {
  return baseUseStore(key);
}
