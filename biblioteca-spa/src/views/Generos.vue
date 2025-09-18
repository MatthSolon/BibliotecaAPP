<template>
  <div class="page-container">
    <div class="header-section">
      <h1 class="page-title">
        <span class="title-icon">🏷️</span>
        Gerenciar Gêneros
      </h1>
      <p class="page-subtitle">Organize e categorize sua biblioteca por gêneros literários</p>
    </div>

    <div class="form-card">
      <div class="form-group">
        <div class="input-container">
          <input 
            v-model="novoGenero" 
            placeholder="Digite o nome do novo gênero"
            class="modern-input"
            @keyup.enter="criarGenero"
          />
          <div class="input-underline"></div>
        </div>
        <button @click="criarGenero" class="primary-btn" :disabled="!novoGenero.trim()">
          <span class="btn-icon">➕</span>
          Adicionar Gênero
        </button>
      </div>
    </div>

    <div v-if="loading" class="loading-container">
      <div class="spinner"></div>
      <p>Carregando gêneros...</p>
    </div>

    <div v-else class="cards-grid">
      <div 
        v-for="genero in generos" 
        :key="genero.generoID"
        class="genre-card"
      >
        <div class="card-header">
          <h3 class="card-title">{{ genero.nome }}</h3>
          <div class="card-actions">
            <button @click="editarGenero(genero)" class="action-btn edit-btn">
              <span class="btn-icon">✏️</span>
            </button>
            <button @click="deletarGenero(genero.generoID)" class="action-btn delete-btn">
              <span class="btn-icon">🗑️</span>
            </button>
          </div>
        </div>
        <div class="card-footer">
          <span class="genre-id">ID: {{ genero.generoID }}</span>
        </div>
      </div>
    </div>
    
    <div v-if="!loading && generos.length === 0" class="empty-state">
      <div class="empty-icon">📚</div>
      <h3>Nenhum gênero encontrado</h3>
      <p>Comece adicionando seu primeiro gênero literário!</p>
    </div>
  </div>
</template>

<script>
import api from '../services/api'

export default {
  data() {
    return {
      generos: [],
      novoGenero: '',
      loading: true
    }
  },
  async mounted() {
    await this.listarGeneros()
  },
  methods: {
    async listarGeneros() {
      try {
        this.loading = true
        const res = await api.get('/Generos')
        this.generos = res.data.data || []
      } catch (error) {
        console.error('Erro ao carregar gêneros:', error)
      } finally {
        this.loading = false
      }
    },
    async criarGenero() {
      if (!this.novoGenero.trim()) return
      
      try {
        await api.post('/Generos', { nome: this.novoGenero.trim() })
        this.novoGenero = ''
        await this.listarGeneros()
      } catch (error) {
        console.error('Erro ao criar gênero:', error)
      }
    },
    async editarGenero(genero) {
      const novoNome = prompt("Novo nome do gênero:", genero.nome)
      if (novoNome && novoNome.trim()) {
        try {
          await api.put(`/Generos/${genero.generoID}`, { nome: novoNome.trim() })
          await this.listarGeneros()
        } catch (error) {
          console.error('Erro ao editar gênero:', error)
        }
      }
    },
    async deletarGenero(id) {
      if (confirm('Tem certeza que deseja excluir este gênero?')) {
        try {
          await api.delete(`/Generos/${id}`)
          await this.listarGeneros()
        } catch (error) {
          console.error('Erro ao deletar gênero:', error)
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
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  gap: 1.5rem;
}

.genre-card {
  background: rgba(255, 255, 255, 0.1);
  backdrop-filter: blur(10px);
  border-radius: 15px;
  padding: 1.5rem;
  border: 1px solid rgba(255, 255, 255, 0.2);
  transition: all 0.3s ease;
  animation: fadeInUp 0.5s ease-out;
}

.genre-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 10px 25px rgba(0,0,0,0.2);
  background: rgba(255, 255, 255, 0.15);
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1rem;
}

.card-title {
  color: white;
  font-size: 1.2rem;
  margin: 0;
}

.card-actions {
  display: flex;
  gap: 0.5rem;
}

.card-footer {
  border-top: 1px solid rgba(255, 255, 255, 0.1);
  padding-top: 1rem;
}

.genre-id {
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
}
</style>
