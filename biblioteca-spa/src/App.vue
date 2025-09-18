<template>
  <div>
    <nav>
      <router-link to="/generos">🎭 Gêneros</router-link> |
      <router-link to="/autores">✍️ Autores</router-link> |
      <router-link to="/livros">📚 Livros</router-link>
    </nav>
    <hr />
    <router-view />
  </div>
</template>

<script>
export default {
  name: "App",
};
</script>

<style>
nav {
  padding: 10px;
  background: #f4f4f4;
}
nav a {
  margin: 0 10px;
  text-decoration: none;
  color: #333;
}
nav a.router-link-exact-active {
  font-weight: bold;
  color: #42b983;
}
</style>
