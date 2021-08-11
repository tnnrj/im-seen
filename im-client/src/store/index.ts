import { InjectionKey } from 'vue';
import { createStore, Store, useStore as baseUseStore } from 'vuex';
import ChartDataService from '@/services/chart-data.service';
import ReportsService from '@/services/reports.service';
import { Report, UiReport } from '@/model/reports.model';

// define our type for the store state
export interface State {
  chartDatas: Map<string, any>;
  reports: Report[] | undefined;
}

// define injection key
export const key: InjectionKey<Store<State>> = Symbol()

// create store
export const store = createStore<State>({
  state: {
    chartDatas: new Map<string, any>(),
    reports: undefined
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
    setAllReports(state, payload) {
      state.reports = payload.reports;
    }
  },
  actions: {
    // load data for a single chart from the server
    loadChartData({ commit }, payload) {
      ChartDataService.getChartData(payload.queryId).then(response => {
        commit('setChartData', { queryId: payload.queryId, data: response.data });
      });
    },
    // load data for all reports
    loadAllReports({ commit }) {
      ReportsService.getReports().then(response => {
        commit('setAllReports', { reports: response.data });
      });
    }
  }
})

// define our own `useStore` composition function
export function useStore () {
  return baseUseStore(key);
}
