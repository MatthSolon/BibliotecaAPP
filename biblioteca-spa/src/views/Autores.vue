<template>
  <div class="page-container">
    <div class="header-section">
      <h1 class="page-title">
        <span class="title-icon">👥</span>
        Gerenciar Autores
      </h1>
      <p class="page-subtitle">Cadastre e organize os autores da sua biblioteca</p>
    </div>
    <div class="form-card">
      <div class="form-group">
        <div class="input-container">
          <input 
            v-model="novoAutor" 
            placeholder="Digite o nome do novo autor"
            class="modern-input"
            @keyup.enter="criarAutor"
          />
          <div class="input-underline"></div>
        </div>
        <button @click="criarAutor" class="primary-btn" :disabled="!novoAutor.trim()">
          <span class="btn-icon">➕</span>
          Adicionar Autor
        </button>
      </div>
    </div>
    <div v-if="loading" class="loading-container">
      <div class="spinner"></div>
      <p>Carregando autores...</p>
    </div>
    <div v-else class="cards-grid">
      <div 
        v-for="autor in autores" 
        :key="autor.autorID"
        class="author-card"
      >
        <div class="card-header">
          <div class="author-avatar">
            <span class="avatar-text">{{ autor.nome.charAt(0).toUpperCase() }}</span>
          </div>
          <div class="author-info">
            <h3 class="card-title">{{ autor.nome }}</h3>
            <span class="author-id">ID: {{ autor.autorID }}</span>
          </div>
          <div class="card-actions">
            <button @click="editarAutor(autor)" class="action-btn edit-btn">
              <span class="btn-icon">✏️</span>
            </button>
            <button @click="deletarAutor(autor.autorID)" class="action-btn delete-btn">
              <span class="btn-icon">🗑️</span>
            </button>
          </div>
        </div>
      </div>
    </div>
    <div v-if="!loading && autores.length === 0" class="empty-state">
      <div class="empty-icon">👨‍💼</div>
      <h3>Nenhum autor encontrado</h3>
      <p>Comece adicionando seu primeiro autor!</p>
    </div>
  </div>
</template>

<script>
import api from '../services/api'

export default {
  data() {
    return {
      autores: [],
      novoAutor: '',
      loading: true
    }
  },
  async mounted() {
    await this.listarAutores()
  },
  methods: {
    async listarAutores() {
      try {
        this.loading = true
        const res = await api.get('/Autores')
        this.autores = res.data.data || []
      } catch (error) {
        console.error('Erro ao carregar autores:', error)
      } finally {
        this.loading = false
      }
    },
    async criarAutor() {
      if (!this.novoAutor.trim()) return
      
      try {
        await api.post('/Autores', { nome: this.novoAutor.trim() })
        this.novoAutor = ''
        await this.listarAutores()
      } catch (error) {
        console.error('Erro ao criar autor:', error)
      }
    },
    async editarAutor(autor) {
      const novoNome = prompt("Novo nome do autor:", autor.nome)
      if (novoNome && novoNome.trim()) {
        try {
          await api.put(`/Autores/${autor.autorID}`, { nome: novoNome.trim() })
          await this.listarAutores()
        } catch (error) {
          console.error('Erro ao editar autor:', error)
        }
      }
    },
    async deletarAutor(id) {
      if (confirm('Tem certeza que deseja excluir este autor?')) {
        try {
          await api.delete(`/Autores/${id}`)
          await this.listarAutores()
        } catch (error) {
          console.error('Erro ao deletar autor:', error)
        }
      }
    }
  }
}
</script>

<style scoped>
  
.page-container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 2rem;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  min-height: 100vh;
}

.header-section {
  text-align: center;
  margin-bottom: 3rem;
}

.page-title {
  font-size: 2.5rem;
  color: white;
  margin-bottom: 0.5rem;
  text-shadow: 0 2px 4px rgba(0,0,0,0.3);
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 1rem;
}

.title-icon {
  font-size: 2rem;
}

.page-subtitle {
  color: rgba(255, 255, 255, 0.8);
  font-size: 1.1rem;
}


.form-card {
  background: rgba(255, 255, 255, 0.1);
  backdrop-filter: blur(20px);
  border-radius: 20px;
  padding: 2rem;
  margin-bottom: 3rem;
  border: 1px solid rgba(255, 255, 255, 0.2);
  box-shadow: 0 8px 32px rgba(0,0,0,0.1);
}

.form-group {
  display: flex;
  gap: 1rem;
  align-items: flex-end;
}

.input-container {
  position: relative;
  flex: 1;
}

.modern-input {
  width: 100%;
  padding: 1rem 0;
  background: transparent;
  border: none;
  border-bottom: 2px solid rgba(255, 255, 255, 0.3);
  color: white;
  font-size: 1rem;
  transition: all 0.3s ease;
}

.modern-input::placeholder {
  color: rgba(255, 255, 255, 0.6);
}

.modern-input:focus {
  outline: none;
  border-bottom-color: #feca57;
}

.input-underline {
  position: absolute;
  bottom: 0;
  left: 0;
  width: 0;
  height: 2px;
  background: linear-gradient(90deg, #feca57, #ff6b6b);
  transition: width 0.3s ease;
}

.modern-input:focus + .input-underline {
  width: 100%;
}

.primary-btn {
  background: linear-gradient(135deg, #feca57, #ff6b6b);
  border: none;
  color: white;
  padding: 1rem 2rem;
  border-radius: 50px;
  font-weight: bold;
  cursor: pointer;
  transition: all 0.3s ease;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  min-width: max-content;
}

.primary-btn:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 5px 20px rgba(0,0,0,0.3);
}

.primary-btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.action-btn {
  background: rgba(255, 255, 255, 0.1);
  border: 1px solid rgba(255, 255, 255, 0.2);
  color: white;
  padding: 0.5rem;
  border-radius: 8px;
  cursor: pointer;
  transition: all 0.3s ease;
  display: flex;
  align-items: center;
  gap: 0.3rem;
}

.action-btn:hover {
  background: rgba(255, 255, 255, 0.2);
  transform: translateY(-1px);
}

.edit-btn:hover {
  background: rgba(52, 152, 219, 0.3);
}

.delete-btn:hover {
  background: rgba(231, 76, 60, 0.3);
}

.btn-icon {
  font-size: 0.9rem;
}

.cards-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(350px, 1fr));
  gap: 1.5rem;
}

.author-card {
  background: rgba(255, 255, 255, 0.1);
  backdrop-filter: blur(10px);
  border-radius: 15px;
  padding: 1.5rem;
  border: 1px solid rgba(255, 255, 255, 0.2);
  transition: all 0.3s ease;
  animation: fadeInUp 0.5s ease-out;
}

.author-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 10px 25px rgba(0,0,0,0.2);
  background: rgba(255, 255, 255, 0.15);
}

.card-header {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.card-title {
  color: white;
  font-size: 1.2rem;
  margin: 0;
}

.card-actions {
  display: flex;
  gap: 0.5rem;
  margin-left: auto;
}

.author-info {
  flex: 1;
}

.author-avatar {
  width: 50px;
  height: 50px;
  background: linear-gradient(135deg, #feca57, #ff6b6b);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: bold;
  font-size: 1.2rem;
  color: white;
  flex-shrink: 0;
}

.author-id {
  color: rgba(255, 255, 255, 0.6);
  font-size: 0.9rem;
}

.loading-container {
  text-align: center;
  padding: 3rem;
  color: white;
}

.spinner {
  width: 40px;
  height: 40px;
  border: 4px solid rgba(255, 255, 255, 0.3);
  border-top: 4px solid white;
  border-radius: 50%;
  animation: spin 1s linear infinite;
  margin: 0 auto 1rem;
}

.empty-state {
  text-align: center;
  padding: 3rem;
  color: white;
}

.empty-icon {
  font-size: 4rem;
  margin-bottom: 1rem;
}

@keyframes fadeInUp {
  from {
    opacity: 0;
    transform: translateY(30px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}

@media (max-width: 768px) {
  .page-container {
    padding: 1rem;
  }
  
  .page-title {
    font-size: 2rem;
  }
  
  .form-group {
    flex-direction: column;
    align-items: stretch;
  }
  
  .cards-grid {
    grid-template-columns: 1fr;
  }
  
  .card-header {
    flex-direction: column;
    align-items: stretch;
    gap: 1rem;
  }
}
</style>
