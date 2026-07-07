# 🚀 FIAP-Cloud-Games Users API

Essa API foi construída para compor o projeto do FIAP-Cloud-Games, com o intuito de realizar a gestão de usuários, 
autenticação e autorização na plataforma de jogos.

---

## 📌 Segunda fase

Essa fase consiste em refatorar o monolitico feito na primeira fase em uma arquitetura de microsserviços, contendo os 
seguintes microsserviços

* Microsserviço de Usuários (UsersAPI)
* Microsserviço de Catálogo (CatalogAPI e CatalogWorker)
* Microsserviço de Pagamentos (PaymentsWorker)
* Microsserviço de Notificações (NotificationsWorker)

## Imagem no docker
A imagem desse projeto está disponível no Docker Hub como `igoranthony12/users-api`. As variaveis de ambientes são 
mostradas na seção de `Como rodar o projeto via docker compose` e `Como rodar o projeto via kubernetes`.


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

## ▶️ Como rodar o projeto localmente

### 1. Clonar o repositório

```bash
git clone --recurse-submodules https://github.com/IgorAnthonyy/FIAP-Cloud-Games.git
cd FIAP-Cloud-Games
```

### 2. Configurar variáveis de ambiente

Crie e modifique o arquivo `src/FCG.Api/appsettings.Development.json` para o template abaixo:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=users;Username=fcg_user;Password=fcg_password"
  },
  "RabbitMQ": {
    "Host": "localhost",
    "VirtualHost": "/",
    "Username": "fcg_user",
    "Password": "fcg_password"
  },
  "Jwt": {
    "Key": "ChaveSuperSecretaECompridaDeExemplo123!",
    "Issuer": "FCGames"
  }
}
```

### 3. Rodar migrations

```bash
cd src/FCG.Infrastructure
dotnet ef database update --startup-project ../FCG.Api
```

### 4. Executar a API

```bash
cd src/FCG.Api
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
## ▶️ Como rodar o projeto via docker compose

### 1. Clonar o repositório

```bash
git clone --recurse-submodules https://github.com/IgorAnthonyy/FIAP-Cloud-Games.git
cd FIAP-Cloud-Games
```

### 2. Configurar variáveis de ambiente

Acesse o arquivo `docker-compose.yml` e modifique as seguintes variáveis de ambiente abaixo:
```
Chave da imagem Rabbit:
RABBITMQ_DEFAULT_USER: fcg_user
RABBITMQ_DEFAULT_PASS: fcg_password
RABBITMQ_DEFAULT_VHOST: /

Chave do banco:
ConnectionStrings__DefaultConnection: "Host=postgres;Database=users;Username=fcg_user;Password=fcg_password"

Chaves do Rabbit no projeto:
RabbitMQ__VirtualHost: /
RabbitMQ__Username: fcg_user
RabbitMQ__Password: fcg_password

Chaves do JWT no projeto:
Jwt__Key: "ChaveSuperSecretaECompridaDeExemplo123!"
Jwt__Issuer: "FCGames"
```

### 3. Executar o compose
#### 3.1 Subir os containers
```bash
cd FIAP-Cloud-Games
docker compose up -d
```

#### 3.2 Descer os containers
```bash
docker compose down
```
---
## ▶️ Como rodar o projeto via kubernetes

### 1. Clonar o repositório

```bash
git clone --recurse-submodules https://github.com/IgorAnthonyy/FIAP-Cloud-Games.git
cd FIAP-Cloud-Games
```

### 2. Configurar variáveis de ambiente

Acesse o arquivo `k8s/configmap.yaml` e modifique as variáveis de ambiente abaixo:
```yaml
Jwt__Issuer: "FCGames"
RabbitMQ__Host: "rabbitmq"
RabbitMQ__VirtualHost: "/"
RabbitMQ__Username: "fcg_user"
```

Acesse o arquivo `k8s/secrets.yaml` e modifique as variáveis de ambiente abaixo:
```yaml
ConnectionStrings__DefaultConnection: "Host=postgres;Database=users;Username=fcg_user;Password=fcg_password"
Jwt__Key: "ChaveSuperSecretaECompridaDeExemplo123!"
RabbitMQ__Password: "fcg_password"
```

> É necessário subir um serviço de RabbitMQ e do PostgreSQL para poder rodar o projeto no Kubernetes.

### 3. Executar o projeto
#### 3.1 Aplicar o kubernetes
```bash
cd FIAP-Cloud-Games
kubectl apply -f k8s/configmap.yaml
kubectl apply -f k8s/secrets.yaml
kubectl apply -f k8s/deployment.yaml
kubectl apply -f k8s/service.yaml
```

#### 3.2 Deletar o Configmap, Secret, Deployment e Service
```bash
kubectl delete configmap users-api-config
kubectl delete secret users-api-secrets
kubectl delete deployment users-api
kubectl delete service users-api-service
```

---

## 🗂️ Estrutura do projeto

```
/src
 ├── FCG.Api
 ├── FCG.Application
 ├── FCG.Domain
 ├── FCG.Infrastructure
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
