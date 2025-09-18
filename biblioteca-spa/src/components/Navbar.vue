<template>
  <nav class="navbar">
    <h1>📚 Biblioteca</h1>
    <ul>
      <li><router-link to="/generos">Gêneros</router-link></li>
      <li><router-link to="/autores">Autores</router-link></li>
      <li><router-link to="/livros">Livros</router-link></li>
    </ul>
  </nav>
</template>

<script>
export default {};
</script>

<style>
.navbar {
  display: flex;
  align-items: center;
  justify-content: space-between;
  background-color: #2c3e50;
  padding: 10px 20px;
  color: white;
}

.navbar h1 {
  margin: 0;
}

.navbar ul {
  list-style: none;
  display: flex;
  gap: 15px;
  margin: 0;
  padding: 0;
}

.navbar a {
  color: white;
  text-decoration: none;
}

.navbar a:hover {
  text-decoration: underline;
}
</style>
