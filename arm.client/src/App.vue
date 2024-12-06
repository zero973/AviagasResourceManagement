<template>
  <div>
    <p>Auth-fetch</p>
    <p>Логин:</p>
    <input v-model="login" type="text" placeholder="Логин" />
    <p>Пароль:</p>
    <input v-model="password" type="password" placeholder="Пароль" />
    <button @click="loginFetch">Войти</button>
    <p>Ответ сервера: {{ responseAnswerOnAuth }}</p>
  </div>
  <br>
  <hr>
  <br>
  <div>
    <p>Get current user Id</p>
    <button @click="getCurrentUserId">Get Id</button>
    <p>Ответ сервера: {{ responseGetCurrentUserId }}</p>
  </div>
  <br>
  <hr>
  <br>
  <div>
    <p>Test id in array fetch</p>
    <button @click="arrayFetch">Fetch Data</button>
    <p>Ответ сервера: {{ responseAnswerArray }}</p>
  </div>
</template>

<script setup>
  import { ref } from 'vue'
  import axios from 'axios'
  import LoginModel from "@/LoginModel.js";
  import TokensPair from "@/TokensPair.js";
  import {ComplexFilterOperators} from "@/ComplexFilterOperators.js";
  import ComplexFilter from "@/ComplexFilter.js";
  import BaseListParams from "@/BaseListParams.js";

  const login = ref('');
  const password = ref('');
  const responseAnswerOnAuth = ref('');
  const responseGetCurrentUserId = ref('');
  const responseAnswerArray = ref('');

  const loginFetch = async () => {

    const credentials = new LoginModel(login.value, password.value);

    await axios.post('/api/Authorization/LogIn', credentials)
      .then(x => {
        responseAnswerOnAuth.value = x.data.data;
        sessionStorage.setItem('AccessToken', x.data.data.accessToken);
        sessionStorage.setItem('RefreshToken', x.data.data.refreshToken);
      })
      .catch(x => console.error(x));

  }

  const getCurrentUserId = async () => {

    const headers = {
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + sessionStorage.getItem('AccessToken')
    };

    await axios.post('/api/Authorization/GetCurrentUserId', {},
    {
      headers: headers
    })
      .then(x => {
        responseGetCurrentUserId.value = x.data;
      })
      .catch(x => console.error(x));

  }

  const arrayFetch = async () => {
    try {
      const params = new BaseListParams(1, 100, {}, []);

      params.OrderBy['Name'] = true;

      const filter = new ComplexFilter('Name', ComplexFilterOperators.All, "qqa");
      params.Filters.push(filter);

      params.Filters = JSON.stringify(params.Filters);

      axios({
        method: 'get',
        url: '/api/Cabinet/GetAll',
        params: params,
        headers: {
          'Content-Type': 'application/json',
          'Authorization': 'Bearer ' + sessionStorage.getItem('AccessToken')
        }
      })
        .then(response => {
          responseAnswerArray.value = response.data;
        })
        .catch(error => {
          console.error(error);
        });

      responseAnswerArray.value = data;
    } catch (err) {
      console.error(err);
    }
  }

</script>
