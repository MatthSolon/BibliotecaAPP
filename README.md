# BibliotecaAPI

Um aplicativo de biblioteca para gerenciar livros, autores e gêneros. Desenvolvido como uma API REST com ASP.NET Core e SQL Server, 
permitindo operações de CRUD e consulta de dados.

---

## 🔹 Funcionalidades

- Cadastro de livros, autores e gêneros.
- Consulta de livros por autor ou gênero.
- Atualização e exclusão de registros.
- Estrutura de API REST

---

## 🔹 Tecnologias Utilizadas

- **Backend:** ASP.NET Core 7
- **ORM:** Entity Framework Core
- **Banco de Dados:** SQL Server
- **Controle de Versão:** Git
- **Front-end :** Vue.js
---

## 🔹 Estrutura do Projeto

BibliotecaAPI/
│
├─ Controllers/ # Controladores da API
├─ Data/ # Configuração do DbContext
├─ Models/ # Models: Livro, Autor, Genero
├─ Migrations/ # Migrações do EF Core
├─ Program.cs # Configuração da aplicação
└─ appsettings.json # Configurações, incluindo string de conexão


biblioteca-frontend/
│
├─ public/                 
│
├─ src/
│   ├─ assets/             
│   │
│   ├─ components/          
│   │   ├─ LivroService.vue   
│   │   ├─ GeneroService.vue
│   │   ├─ AutorService.vue   
│   │   └─ Navbar.vue      
│   │
│   ├─ views/                       
│   │   ├─ Livros.vue 
│   │   ├─ Autores.vue        
│   │   └─ Generos.vue       
│   │
│   ├─ router/             
│   │   └─ index.js
│   │
│   ├─ services/          
│   │   └─ api.js           
│   │
│   ├─ App.vue              
│   └─ main.js             
│
├─ package.json             
├─ vite.config.js 
└─ README.md


---

## 🔹 Instalação

1. Clone o repositório:

```bash
git clone https://github.com/MatthSolon/BibliotecaAPI.git
cd BibliotecaAPI
```
---

2. Configure a string de conexão no appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=BibliotecaDB;Trusted_Connection=True;"
}
---

3. Restaure e rode as migrações

dotnet restore
dotnet ef database update
dotnet run
---

## 🔹 Frontend (Biblioteca Front)

### Instalação
```bash
cd biblioteca-frontend

# Instale as dependências
npm install
# Rodar em ambiente de desenvolvimento
npm run dev

# Gerar build de produção
npm run build

# Pré-visualizar build
npm run preview
```
---
A API estará disponível em: https://localhost:5001 (ou http://localhost:5000)

o front estara disponivel em: http://localhost:5173/
