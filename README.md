
# 🚀 FIAP-Cloud-Games

Essa API está voltada para ser o backend de uma plataforma de venda de jogos digitais e de realizar uma gestão de servidores para partidas onlines. Para um desenvolvimento mais seguro, esse projeto foi dividio em quatro fases


---

## 📌 Primeira fase

Essa fase contempla a base do sistema, contendo as seguintes funcionalidades

* Cadastro de Usuário
* Deleção de Usuário
* Atualização de um usuário
* Autenticação
* Autorização
	* Usuário Padrão - Acesso padrão do sistema
	* Usuário Admin - Podendo deletar outros usuários, adicionar novos admins no sistema.
* Logs
* Tratamento de erros globalmente



---

## ⚙️ Tecnologias

* .NET 10
* ASP.NET Core Web API
* Entity Framework Core
* PostgreSQL

---

## 📦 Pré-requisitos

Antes de começar, você precisa ter instalado:

* .NET SDK 10
* Banco de dados (PostgreSQL)

---

## ▶️ Como rodar o projeto

### 1. Clonar o repositório

```bash
git clone https://github.com/seu-repo/sua-api.git
cd sua-api
```

### 2. Configurar variáveis de ambiente

Crie um arquivo `appsettings.Development.json` ou use variáveis:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=MinhaDb;Username=postgres;Password=123"
}
```

### 3. Rodar migrations

```bash
dotnet ef database update
```

### 4. Executar a API

```bash
dotnet run
```

A API estará disponível em:

```
https://localhost:5001
```

---

## 📚 Documentação da API

Se estiver usando Swagger:

```
https://localhost:5001/swagger
```

---

## 🔐 Autenticação (se houver)

Explique:

* Tipo: JWT, OAuth, etc.
* Como obter token

Exemplo:

```bash
POST /auth/login
```

Resposta:

```json
{
  "token": "jwt_token_aqui"
}
```

---

## 📌 Endpoints principais

### 👤 Usuários

* `GET /users`
* `POST /users`
* `PUT /users/{id}`
* `DELETE /users/{id}`

### 📊 Oportunidades

* `GET /opportunities`
* `POST /opportunities`

---

## 🗂️ Estrutura do projeto

```
/src
 ├── Api
 ├── Application
 ├── Domain
 ├── Infrastructure
```

---

## 🧪 Testes

```bash
dotnet test
```

---

## 🐳 Rodando com Docker (opcional)

```bash
docker-compose up -d
```

---

## 🚀 Deploy

Explique como publicar:

```bash
dotnet publish -c Release
```

Ou mencione:

* Azure
* AWS
* VPS

---

## 🤝 Contribuição

1. Crie uma branch:

```bash
git checkout -b feature/minha-feature
```

2. Commit:

```bash
git commit -m "feat: minha nova feature"
```

3. Push:

```bash
git push origin minha-feature
```

---

## 📄 Licença

Este projeto está sob a licença MIT.

---

## 📞 Contato

Seu nome ou time responsável
Email ou canal interno

