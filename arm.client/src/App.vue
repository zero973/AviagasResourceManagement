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
    <p>Получить шкафы</p>
    <button @click="getCabinets">Fetch Data</button>
    <p>Ответ сервера: {{ responseAnswerCabinets }}</p>
  </div>
  <br>
  <hr>
  <br>
  <div>
    <p>Изменение данных о шкафах</p>
    <p>Id:</p>
    <input v-model="cabinetId" type="text" placeholder="Id" />
    <p>Название:</p>
    <input v-model="cabinetName" type="text" placeholder="Название" />
    <p>Полное название:</p>
    <input v-model="cabinetFullname" type="text" placeholder="Полное название" />
    <button @click="addData">Добавить</button>
    <button @click="editData">Изменить</button>
    <button @click="deleteData">Удалить</button>
    <p>Ответ сервера: {{ responseEditData }}</p>
  </div>
</template>

<script setup>
  import { ref } from 'vue'
  import axios from 'axios'
  import LoginModel from "@/LoginModel.js";
  import {ComplexFilterOperators} from "@/ComplexFilterOperators.js";
  import ComplexFilter from "@/ComplexFilter.js";
  import BaseListParams from "@/BaseListParams.js";
  import Cabinet from "@/Cabinet.js";

  const login = ref('');
  const password = ref('');

  const cabinetId = ref('');
  const cabinetName = ref('');
  const cabinetFullname = ref('');

  const responseAnswerOnAuth = ref('');
  const responseGetCurrentUserId = ref('');
  const responseAnswerCabinets = ref('');
  const responseEditData = ref('');

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

  const getCabinets = async () => {
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
          responseAnswerCabinets.value = response.data;
        })
        .catch(error => {
          console.error(error);
        });
    } catch (err) {
      console.error(err);
    }
  }

  const addData = async () => {

    const newCabinet = new Cabinet(cabinetId.value, cabinetName.value, cabinetFullname.value);

    const headers = {
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + sessionStorage.getItem('AccessToken')
    };

    await axios.post('/api/Cabinet/Add', newCabinet,
      {
        headers: headers
      })
      .then(x => {
        responseEditData.value = x.data;
      })
      .catch(x => console.error(x));

  }

  const editData = async () => {

    const cabinet = new Cabinet(cabinetId.value, cabinetName.value, cabinetFullname.value);

    const headers = {
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + sessionStorage.getItem('AccessToken')
    };

    await axios.post('/api/Cabinet/Update', cabinet,
      {
        headers: headers
      })
      .then(x => {
        responseEditData.value = x.data;
      })
      .catch(x => console.error(x));

  }

  const deleteData = async () => {

    const cabinet = new Cabinet(cabinetId.value, cabinetName.value, cabinetFullname.value);

    const headers = {
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + sessionStorage.getItem('AccessToken')
    };

    await axios.post('/api/Cabinet/Delete', cabinet,
      {
        headers: headers
      })
      .then(x => {
        responseEditData.value = x.data;
      })
      .catch(x => console.error(x));

  }

</script>
