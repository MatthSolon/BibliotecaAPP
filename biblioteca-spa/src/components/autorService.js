// src/services/autorService.js
import api from "./api";

export default {
  listar() {
    return api.get("/autor");
  },
  buscarPorId(id) {
    return api.get(`/autor/${id}`);
  },
  criar(data) {
    return api.post("/autor", data);
  },
  atualizar(id, data) {
    return api.put(`/autor/${id}`, data);
  },
  excluir(id) {
    return api.delete(`/autor/${id}`);
  }
};
