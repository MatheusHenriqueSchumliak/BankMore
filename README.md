# BankMore - API Conta Corrente

API RESTful para gestão de contas correntes, autenticação, movimentações e consulta de saldo, seguindo padrões DDD, CQRS, autenticação JWT e integração com SQLite.

---

## ✅ Requisitos Atendidos

- Cadastro e autenticação de usuários (JWT)
- Movimentação (depósito, saque, transferência entre contas)
- Consulta de saldo
- DDD, CQRS, MediatR, Dapper, testes unitários e de integração
- Swagger para documentação automática
- Pronto para Docker e Docker Compose

---

## 🚀 Como Executar

1. Clone o repositório:
   ```bash
   git clone https://github.com/MatheusHenriqueSchumliak/BankMore.git
   ```
2. Na raiz do projeto, execute:
   ```bash
   docker-compose up --build
   ```
3. Acesse a API em [http://localhost:8080/swagger](http://localhost:8080/swagger)

O banco de dados SQLite será criado automaticamente no volume `contacorrente-data`.

---

## 🔗 Principais Endpoints

### POST `/api/ContaCorrente`
Cadastrar conta corrente

```json
{
  "cpf": "123.456.789-00",
  "senha": "minhaSenha",
  "nome": "Nome do Usuário"
}
```
**Retorno:** 200 com número da conta e token, ou 400 se CPF inválido.

---

### POST `/api/auth/login`
Efetuar login

```json
{
  "numeroConta": "1234567",
  "cpf": "123.456.789-00",
  "senha": "minhaSenha"
}
```
**Retorno:** 200 com token JWT, ou 401 se dados inválidos.

---

### PUT `/api/ContaCorrente/inativar`
Inativar conta corrente (requer JWT)

```json
{
  "senha": "minhaSenha"
}
```
**Retorno:** 204 em caso de sucesso, 400/401/403 em caso de erro.

---

### POST `/api/movimentacao/movimentar`
Movimentar conta corrente (depósito, saque, transferência – requer JWT)

```json
{
  "idRequisicao": "opcional-guid-para-idempotencia",
  "numeroConta": 1234567, // opcional, se omitido usa do token
  "valor": 100.00,
  "tipo": "C" // "C" para crédito, "D" para débito
}
```
**Retorno:** 204 em caso de sucesso, 400/403 em caso de erro.

---

### GET `/api/ContaCorrente/saldo`
Consultar saldo (requer JWT)

**Retorno:** 200 com saldo, nome, número da conta e data/hora.

---

## 🔒 Autenticação

- Todos os endpoints (exceto cadastro e login) exigem JWT no header  
  `Authorization: Bearer <token>`.
- O token é obtido no login e deve ser enviado em cada requisição autenticada.

---

## 🗃 Banco de Dados

- Utiliza SQLite, com as tabelas `contacorrente`, `movimento`.
- O arquivo do banco é persistido no volume Docker `contacorrente-data`.

---

## 🧪 Testes

- Testes unitários e de integração implementados com xUnit e Moq.
- Para rodar os testes:
  ```
  dotnet test
  ```

---

## 📃 Swagger

- Documentação interativa disponível em [http://localhost:8080/swagger](http://localhost:8080/swagger) após subir a aplicação.
- Inclui exemplos de requisição e resposta, além de suporte a autenticação JWT.

---

## 🐳 Execução com Docker Compose

```bash
docker-compose up --build
```

- O serviço `contacorrente-api` será exposto na porta 8080.
- O banco SQLite será persistido no volume `contacorrente-data`.

---

## ⚙️ Observações Técnicas

- Arquitetura DDD e CQRS
- Autenticação JWT obrigatória em todos os endpoints protegidos
- Persistência com Dapper e SQLite
- Testes automatizados e documentação Swagger
- Pronto para rodar em containers Docker

---

<hr>
Desenvolvido por Matheus Henrique Schumliak - [GitHub](https://github.com/MatheusHenriqueSchumliak/BankMore)
