<template>
  <div class="page-container">
    <div class="header-section">
      <h1 class="page-title">
        <span class="title-icon">📖</span>
        Gerenciar Livros
      </h1>
      <p class="page-subtitle">Cadastre e organize todos os livros da sua biblioteca</p>
    </div>

    <div class="form-card">
      <h3 class="form-title">Adicionar Novo Livro</h3>
      <div class="form-grid">
        <div class="input-container">
          <input 
            v-model="novoLivro.titulo" 
            placeholder="Título do livro"
            class="modern-input"
          />
          <div class="input-underline"></div>
        </div>

        <div class="input-container">
          <input 
            v-model="novoLivro.anoPublicacao" 
            type="number" 
            placeholder="Ano de Publicação"
            class="modern-input"
            min="1000"
            max="2030"
          />
          <div class="input-underline"></div>
        </div>

        <div class="select-container">
          <select v-model="novoLivro.autorID" class="modern-select">
            <option disabled value="">Selecione um Autor</option>
            <option v-for="a in autores" :key="a.autorID" :value="a.autorID">
              {{ a.nome }}
            </option>
          </select>
        </div>

        <div class="select-container">
          <select v-model="novoLivro.generoID" class="modern-select">
            <option disabled value="">Selecione um Gênero</option>
            <option v-for="g in generos" :key="g.generoID" :value="g.generoID">
              {{ g.nome }}
            </option>
          </select>
        </div>
      </div>

      <button 
        @click="criarLivro" 
        class="primary-btn full-width"
        :disabled="!isFormValid"
      >
        <span class="btn-icon">📚</span>
        Adicionar Livro
      </button>
    </div>

    <div v-if="loading" class="loading-container">
      <div class="spinner"></div>
      <p>Carregando livros...</p>
    </div>

    <div v-else class="books-grid">
      <div 
        v-for="livro in livros" 
        :key="livro.livroID"
        class="book-card"
      >
        <div class="book-cover">
          <div class="book-spine">
            <span class="book-title-spine">{{ livro.titulo }}</span>
          </div>
        </div>
        
        <div class="book-details">
          <h3 class="book-title">{{ livro.titulo }}</h3>
          
          <div class="book-info">
            <div class="info-item">
              <span class="info-label">📅 Ano:</span>
              <span class="info-value">{{ livro.anoPublicacao }}</span>
            </div>
            
            <div class="info-item">
              <span class="info-label">👤 Autor:</span>
              <span class="info-value">{{ livro.autor?.nome || 'N/A' }}</span>
            </div>
            
            <div class="info-item">
              <span class="info-label">🏷️ Gênero:</span>
              <span class="info-value">{{ livro.genero?.nome || 'N/A' }}</span>
            </div>
          </div>

          <div class="book-actions">
            <button @click="editarLivro(livro)" class="action-btn edit-btn">
              <span class="btn-icon">✏️</span>
              Editar
            </button>
            <button @click="deletarLivro(livro.livroID)" class="action-btn delete-btn">
              <span class="btn-icon">🗑️</span>
              Excluir
            </button>
          </div>
        </div>
      </div>
    </div>

    <div v-if="!loading && livros.length === 0" class="empty-state">
      <div class="empty-icon">📚</div>
      <h3>Nenhum livro encontrado</h3>
      <p>Comece adicionando seu primeiro livro à biblioteca!</p>
    </div>
  </div>
</template>

<script>
import api from '../services/api'

export default {
  data() {
    return {
      livros: [],
      autores: [],
      generos: [],
      loading: true,
      novoLivro: {
        titulo: '',
        anoPublicacao: '',
        autorID: '',
        generoID: ''
      }
    }
  },
  computed: {
    isFormValid() {
      return this.novoLivro.titulo.trim() && 
             this.novoLivro.autorID && 
             this.novoLivro.generoID
    }
  },
  async mounted() {
    await Promise.all([
      this.listarLivros(),
      this.listarAutores(),
      this.listarGeneros()
    ])
  },
  methods: {
    async listarLivros() {
      try {
        this.loading = true
        const res = await api.get('/Livros')
        this.livros = res.data.data || []
      } catch (error) {
        console.error('Erro ao carregar livros:', error)
      } finally {
        this.loading = false
      }
    },
    async listarAutores() {
      try {
        const res = await api.get('/Autores')
        this.autores = res.data.data || []
      } catch (error) {
        console.error('Erro ao carregar autores:', error)
      }
    },
    async listarGeneros() {
      try {
        const res = await api.get('/Generos')
        this.generos = res.data.data || []
      } catch (error) {
        console.error('Erro ao carregar gêneros:', error)
      }
    },
    async criarLivro() {
      if (!this.isFormValid) return
      
      try {
        await api.post('/Livros', {
          ...this.novoLivro,
          titulo: this.novoLivro.titulo.trim()
        })
        this.novoLivro = { titulo: '', anoPublicacao: '', autorID: '', generoID: '' }
        await this.listarLivros()
      } catch (error) {
        console.error('Erro ao criar livro:', error)
      }
    },
    async editarLivro(livro) {
      const novoTitulo = prompt("Novo título:", livro.titulo)
      if (novoTitulo && novoTitulo.trim()) {
        try {
          await api.put(`/Livros/${livro.livroID}`, {
            titulo: novoTitulo.trim(),
            anoPublicacao: livro.anoPublicacao,
            autorID: livro.autor?.autorID,
            generoID: livro.genero?.generoID
          })
          await this.listarLivros()
        } catch (error) {
          console.error('Erro ao editar livro:', error)
        }
      }
    },
    async deletarLivro(id) {
      if (confirm('Tem certeza que deseja excluir este livro?')) {
        try {
          await api.delete(`/Livros/${id}`)
          await this.listarLivros()
        } catch (error) {
          console.error('Erro ao deletar livro:', error)
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

.form-title {
  color: white;
  margin-bottom: 1.5rem;
  font-size: 1.3rem;
}

.form-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 1.5rem;
  margin-bottom: 2rem;
}

.input-container {
  position: relative;
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

.select-container {
  position: relative;
}

.modern-select {
  width: 100%;
  padding: 1rem;
  background: rgba(255, 255, 255, 0.1);
  border: 1px solid rgba(255, 255, 255, 0.3);
  border-radius: 10px;
  color: white;
  font-size: 1rem;
  cursor: pointer;
  transition: all 0.3s ease;
}

.modern-select:focus {
  outline: none;
  border-color: #feca57;
  box-shadow: 0 0 10px rgba(254, 202, 87, 0.3);
}

.modern-select option {
  background: #333;
  color: white;
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

.full-width {
  width: 100%;
  justify-content: center;
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

.books-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(350px, 1fr));
  gap: 2rem;
}

.book-card {
  background: rgba(255, 255, 255, 0.1);
  backdrop-filter: blur(10px);
  border-radius: 15px;
  overflow: hidden;
  border: 1px solid rgba(255, 255, 255, 0.2);
  transition: all 0.3s ease;
  animation: fadeInUp 0.5s ease-out;
}

.book-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 15px 30px rgba(0,0,0,0.2);
}

.book-cover {
  height: 120px;
  background: linear-gradient(135deg, #667eea, #764ba2);
  position: relative;
  display: flex;
  align-items: center;
  justify-content: center;
}

.book-spine {
  width: 80%;
  height: 80%;
  background: linear-gradient(135deg, #2c3e50, #34495e);
  border-radius: 5px;
  display: flex;
  align-items: center;
  justify-content: center;
  box-shadow: inset 0 2px 10px rgba(0,0,0,0.3);
}

.book-title-spine {
  color: white;
  font-size: 0.8rem;
  text-align: center;
  font-weight: bold;
  transform: rotate(-90deg);
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  max-width: 100px;
}

.book-details {
  padding: 1.5rem;
}

.book-title {
  color: white;
  font-size: 1.3rem;
  margin: 0 0 1rem 0;
}

.book-info {
  margin-bottom: 1.5rem;
}

.info-item {
  display: flex;
  justify-content: space-between;
  margin-bottom: 0.5rem;
}

.info-label {
  color: rgba(255, 255, 255, 0.7);
  font-size: 0.9rem;
}

.info-value {
  color: white;
  font-weight: 500;
}

.book-actions {
  display: flex;
  gap: 1rem;
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
  
  .form-grid {
    grid-template-columns: 1fr;
  }
  
  .books-grid {
    grid-template-columns: 1fr;
  }
}
</style>
