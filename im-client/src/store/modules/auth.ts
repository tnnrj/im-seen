import AuthenticationService from "@/services/authentication.service";

// template from https://www.smashingmagazine.com/2020/10/authentication-in-vue-js/
const state = {
  user: null
};
const getters = {
  isAuthenticated: state => !!state.user,
  userRole: state => {
    return { username: state.user };
  }
};
const actions = {
  async logIn({commit}, form) {
    await AuthenticationService.login(form);    
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