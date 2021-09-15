import AuthenticationService from "@/services/authentication.service";

// template from https://www.smashingmagazine.com/2020/10/authentication-in-vue-js/
const state = {
  user: null,
  isAuthenticated: false
};
const getters = {
  isAuthenticated: state => !!state.isAuthenticated,
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
    commit('setUser', null);
  }
};
const mutations = {
  setUser(state, username) {
    state.user = username;
    state.isAuthenticated = !!username;
  }
};

// check for existing auth
let token = localStorage.getItem('token');
if (token) {
  state.isAuthenticated = true;
}

export default {
  state,
  getters,
  actions,
  mutations
};