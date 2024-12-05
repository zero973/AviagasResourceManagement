<template>
  <div>
    <p>Get-fetch</p>
    <button @click="getFetchData">Fetch Data</button>
    <p>Ответ сервера: {{ responseAnswerGet }}</p>
  </div>
  <br>
  <hr>
  <br>
  <div>
    <p>Post-fetch</p>
    <button @click="postFetchData">Fetch Data</button>
    <p>Ответ сервера: {{ responseAnswerPost }}</p>
  </div>
  <br>
  <hr>
  <br>
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
    <p>Privacy-fetch</p>
    <button @click="privacyFetch">Fetch Data</button>
    <p>Ответ сервера: {{ responseAnswerPrivacy }}</p>
  </div>
  <br>
  <hr>
  <br>
  <div>
    <p>Test id in array fetch</p>
    <button @click="arrayFetch">Fetch Data</button>
    <p>Ответ сервера: {{ responseAnswerPrivacy }}</p>
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

  const responseAnswerGet = ref('');
  const responseAnswerPost = ref('');
  const login = ref('');
  const password = ref('');
  const responseAnswerOnAuth = ref('');
  const responseAnswerPrivacy = ref('');
  const responseAnswerArray = ref('');
  const tokenPair = new TokensPair('', '');

  const getFetchData = () => {
    const headers = { "Content-Type": "application/json" };
    fetch("/api/Home/HelloWorldGet", { headers })
      .then(response => response.json())
      .then(data => (responseAnswerGet.value = data));
  }

  const postFetchData = () => {
    let user = {
      name: 'John',
      surname: 'Smith'
    };
    const headers = { "Content-Type": "application/json" };
    fetch("/api/Home/HelloWorldPost",
      {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(user)
      })
      .then(response => response.json())
      .then(data => (responseAnswerPost.value = data));
  }

  const loginFetch = () => {
    const credentials = new LoginModel(login.value, password.value);
    fetch("/api/Home/LogIn",
      {
        method: 'POST',
        body: JSON.stringify({ credentials })
      })
      .then(response => response.json())
      .then(data =>
      {
        responseAnswerOnAuth.value = data;
        tokenPair.AccessToken = data.AccessToken;
        tokenPair.RefreshToken = data.RefreshToken;
      });
  }

  const privacyFetch = () => {
    fetch("/api/Home/GetPrivacyData",
      {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          'Bearer': tokenPair.AccessToken
        }
      })
      .then(response => response.json())
      .then(data => (responseAnswerPrivacy.value = data));
  }

  const arrayFetch = async () => {
    try {
      const params = new BaseListParams(1, 100, {}, []);

      params.OrderBy['Name'] = true;

      const filter = new ComplexFilter('Name', ComplexFilterOperators.All, "qqa");
      params.Filters.push(filter);

      params.Filters = JSON.stringify(params.Filters);
      const {data} = await axios.get('/api/Cabinet/GetAll', {params});
      responseAnswerArray.value = data.data;
    } catch (err) {
      console.error(err);
    }
  }

</script>
