// src/services/livroService.js
import api from "./api";

export default {
  listar() {
    return api.get("/livro");
  },
  buscarPorId(id) {
    return api.get(`/livro/${id}`);
  },
  criar(data) {
    return api.post("/livro", data);
  },
  atualizar(id, data) {
    return api.put(`/livro/${id}`, data);
  },
  excluir(id) {
    return api.delete(`/livro/${id}`);
  }
};
