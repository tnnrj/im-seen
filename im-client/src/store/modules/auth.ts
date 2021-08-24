import axios from 'axios';

// template from https://www.smashingmagazine.com/2020/10/authentication-in-vue-js/
const state = {
  user: null
};
const getters = {
  isAuthenticated: state => !!state.user
};
const actions = {
  async logIn({commit}, form) {
    // await axios.post('login', form);
    commit('setUser', form.username);
  },
  async logOut({commit}){
    commit('setUser', null)
  }
};
const mutations = {
  setUser(state, username){
    state.user = username
  }
};

export default {
  state,
  getters,
  actions,
  mutations
};