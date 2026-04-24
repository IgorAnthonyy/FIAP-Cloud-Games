
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
git clone https://github.com/IgorAnthonyy/FIAP-Cloud-Games.git
cd sua-api
```

### 2. Configurar variáveis de ambiente

Modifique o seu arquivo `appsettings.Development.json` para o template abaixo:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=seubanco;Username=seuusuario;Password=suasenhga"
  },
  "EmailSettings": {
    "Host": "Host do email utilizado,
    "Port": 587 // exemplo de porta,
    "User": "Email do sistema",
    "Password": "Senha do email do sistema"
  },
  "Jwt": {
    "Key": "Chave para JWT",
    "Issuer": "FCGames"
  }
}
```

### 3. Rodar migrations

```bash
dotnet ef database update
```

### 4. Executar a API

```bash
dotnet run --launch-profile http
```

A API estará disponível em:

```
https://localhost:7210
```

---

## 📚 Documentação da API

Existe o Swagger UI para você conseguir visualizar melhor os endpoints já feitos e pode ser visto na seguinte url:

```
https://localhost:7210/swagger
```

---

## 🗂️ Estrutura do projeto

```
/src
 ├── FCG.Api
 ├── FCG.Application
 ├── FCG.Domain
 ├── FCG.Infrastructure
 |── FCG.Tests
```

---

## 🧪 Testes

```bash
dotnet test
```

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


## 📞 Contato

O time responsável desse sistema:
- Igor Anthony - igor.anthony.iop@gmail.com
- Nathalia Greice - nponce410@gmail.com
- Otávio de Andrade - otavio_andrade@live.com
- Pedro Henrique Barros - pedrobarros0101@outlook.com
- Sérgio Henrique - ssergioh3@gmail.com

