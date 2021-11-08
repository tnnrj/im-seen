import { InjectionKey } from 'vue';
import { createStore, Store, useStore as baseUseStore } from 'vuex';
import reportDataService from '@/services/report-data.service';
import ObservationsService from '@/services/observations.service';
import { Observation } from '@/model/observations.model';
import authenticationService from "@/services/authentication.service";
import studentsService from '@/services/students.service';
import { User } from '@/model/user.model';

// define our type for the store state
export interface State {
  chartDatas: Map<string, any>;
  observations: Observation[] | undefined;
  user: User | undefined;
  myStudentIds: string[] | undefined;
  isAuthenticated: boolean;
}

// define injection key
export const key: InjectionKey<Store<State>> = Symbol()

// create store
export const store = createStore<State>({
  state: {
    chartDatas: new Map<string, any>(),
    observations: undefined,
    user: undefined,
    myStudentIds: undefined,
    isAuthenticated: authenticationService.isLoggedIn()
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
    },
    setUser(state, user) {
      state.user = user;
      state.isAuthenticated = !!user;
    },
    setMyStudentIds(state, payload) {
      state.myStudentIds = payload;
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
    },
    // authentication below
    async logIn({commit}, form) {
      await authenticationService.login(form);
      let u = await authenticationService.getUser();
      commit('setUser', u);
    },
    async logOut({commit}) {
      authenticationService.logout();
      commit('setUser', null);
    },
    async getUser({commit}) {
      let u = await authenticationService.getUser();
      commit('setUser', u);
    },
    async getMyStudentIds({commit}) {
      let s = await studentsService.getMyStudentIds();
      commit('setMyStudentIds', s);
    }
  }
})

// define our own `useStore` composition function
export function useStore () {
  return baseUseStore(key);
}
