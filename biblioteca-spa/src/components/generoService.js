// src/services/generoService.js
import api from "./api";

export default {
  listar() {
    return api.get("/genero");
  },
  buscarPorId(id) {
    return api.get(`/genero/${id}`);
  },
  criar(data) {
    return api.post("/genero", data);
  },
  atualizar(id, data) {
    return api.put(`/genero/${id}`, data);
  },
  excluir(id) {
    return api.delete(`/genero/${id}`);
  }
};
