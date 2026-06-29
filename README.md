# Raízes do Nordeste — Back-end

API REST em **ASP.NET Core (.NET 10)** para uma rede de food service (cardápio regional do
Nordeste). Cobre autenticação JWT com perfis de acesso, catálogo de produtos, cardápio por
unidade, controle de estoque, pedidos com máquina de status e pagamento (gateway mockado),
programa de fidelidade (com consentimento LGPD) e trilha de auditoria.

A documentação interativa dos endpoints fica disponível via **Swagger** assim que a API sobe.

---

## Sumário

- [Arquitetura](#arquitetura)
- [Requisitos](#requisitos)
- [Setup do projeto](#setup-do-projeto)
  - [1. Subir o SQL Server com Docker](#1-subir-o-sql-server-com-docker)
  - [2. Configurar a connection string](#2-configurar-a-connection-string)
  - [3. Banco de dados e seed](#3-banco-de-dados-e-seed)
  - [4. Rodar a API](#4-rodar-a-api)
  - [5. Acessar o Swagger](#5-acessar-o-swagger)
- [Perfis de acesso e usuários de seed](#perfis-de-acesso-e-usuários-de-seed)
- [Autenticação JWT](#autenticação-jwt)
- [Formato de erro padrão](#formato-de-erro-padrão)
- [Fluxos da aplicação](#fluxos-da-aplicação)
  - [Autenticação](#fluxo-1--autenticação)
  - [Unidades](#fluxo-2--unidades)
  - [Produtos](#fluxo-3--produtos)
  - [Cardápio por unidade](#fluxo-4--cardápio-por-unidade)
  - [Estoque](#fluxo-5--estoque)
  - [Pedidos e pagamento](#fluxo-6--pedidos-e-pagamento)
  - [Fidelidade (LGPD)](#fluxo-7--fidelidade-lgpd)
  - [Auditoria](#fluxo-8--auditoria)
- [Fluxo recomendado para testar de ponta a ponta](#fluxo-recomendado-para-testar-de-ponta-a-ponta)

---

## Arquitetura

Solução em camadas (.NET):

| Projeto | Responsabilidade |
| ------- | ---------------- |
| `Projeto Desenvolvimento Back-end` | Camada de API: controllers, middlewares, configuração (DI, JWT, Swagger), seed |
| `Service` | Regras de negócio (services), gateways (ex.: pagamento mockado) |
| `Domain` | Entidades, contratos (requests/responses), enums, exceções, helpers |
| `Infrastructure` | Acesso a dados: `DbContext`, repositórios, migrations |
| `Core` | Abstrações de infraestrutura (Unit of Work) |

---

## Requisitos

Antes de rodar o projeto, tenha instalado:

* .NET 10.0 SDK
* Docker (para o SQL Server local)
* SQL Server Management Studio ou Azure Data Studio (opcional, para inspecionar o banco)
* Entity Framework Core Tools (opcional — só é necessário se você for criar/aplicar migrations manualmente)

Para instalar o Entity Framework Core Tools globalmente:

```bash
dotnet tool install --global dotnet-ef
```

Caso já esteja instalado, atualize com:

```bash
dotnet tool update --global dotnet-ef
```

---

## Setup do projeto

### 1. Subir o SQL Server com Docker

Execute o comando abaixo para baixar e subir uma imagem do SQL Server localmente. A senha
abaixo (`SuaSenhaForte123!`) é a mesma usada na connection string padrão em `appsettings.json` —
se você trocar uma, troque a outra.

No Windows (PowerShell / cmd):

```bash
docker run -e "ACCEPT_EULA=Y" ^
-e "SA_PASSWORD=SuaSenhaForte123!" ^
-p 1433:1433 ^
--name sqlserver-local ^
-d mcr.microsoft.com/mssql/server:2022-latest
```

No Linux/macOS ou Git Bash:

```bash
docker run -e "ACCEPT_EULA=Y" \
-e "SA_PASSWORD=SuaSenhaForte123!" \
-p 1433:1433 \
--name sqlserver-local \
-d mcr.microsoft.com/mssql/server:2022-latest
```

Verifique se o container está rodando:

```bash
docker ps
```

Comandos úteis depois:

```bash
docker start sqlserver-local   # iniciar novamente
docker stop sqlserver-local    # parar
```

### 2. Configurar a connection string

No arquivo `Projeto Desenvolvimento Back-end/appsettings.json`, a connection string padrão (chave
`Connection`) já aponta para o banco local:

```json
{
  "ConnectionStrings": {
    "Connection": "Server=localhost;Database=Projeto_Backend;User Id=sa;Password=SuaSenhaForte123!;TrustServerCertificate=True;"
  }
}
```

> `TrustServerCertificate=True` evita erro de certificado SSL em ambiente local. A chave da
> connection string é **`Connection`** (não `DefaultConnection`).

O mesmo arquivo também guarda as configurações do JWT (`JwtSettings`), usadas para assinar e
validar os tokens:

```json
{
  "JwtSettings": {
    "Secret": "...",
    "Issuer": "RaizesDoNordeste",
    "Audience": "RaizesDoNordesteAPI",
    "ExpirationMinutes": 60
  }
}
```

### 3. Banco de dados e seed

**Você não precisa rodar migrations manualmente.** Ao iniciar, a API:

1. aplica automaticamente todas as migrations pendentes (`Database.MigrateAsync`); e
2. popula o banco com dados iniciais (seed) caso ainda não exista nenhum usuário.

O seed cria:

* **5 usuários** (um por perfil) — veja [usuários de seed](#perfis-de-acesso-e-usuários-de-seed);
* **2 unidades** (Recife Centro e Salvador Shopping);
* **4 produtos** (tapioca, cuscuz, suco de cajá, bolo de macaxeira);
* **cardápio e estoque** para ambas as unidades. Em Recife, o "Bolo de macaxeira" começa com
  estoque baixo (1 unidade) de propósito, para facilitar o teste do erro de estoque insuficiente.

Basta ter o SQL Server rodando e a connection string correta. Na primeira execução o banco é
criado e populado sozinho.

**Aplicar migrations manualmente (opcional).** Se preferir criar o banco antes de subir a API:

```bash
cd ./Infrastructure
dotnet ef database update -c DatabaseContext
```

No Package Manager Console do Visual Studio (com `Infrastructure` como projeto padrão):

```powershell
Update-Database -Context DatabaseContext
```

### 4. Rodar a API

Na pasta do projeto da API:

```bash
cd "./Projeto Desenvolvimento Back-end"
dotnet run
```

A aplicação sobe em uma URL parecida com (a porta pode variar conforme a configuração):

```text
https://localhost:7000
http://localhost:5000
```

### 5. Acessar o Swagger

O Swagger UI é servido na **raiz** da aplicação:

```text
https://localhost:7000/index.html
```

No Swagger é possível visualizar e testar todos os endpoints, além de autenticar com o token JWT
(botão **Authorize**).

---

## Perfis de acesso e usuários de seed

A API trabalha com 5 perfis (claim de `Role` no token JWT):

| Perfil | Descrição |
| ------ | --------- |
| `ADMIN` | Acesso total |
| `GERENTE` | Gestão da operação (produtos, cardápio, estoque, pedidos, auditoria) |
| `ATENDENTE` | Atendimento: consulta estoque, avança/cancela pedidos |
| `COZINHA` | Produção: avança o status de preparo dos pedidos |
| `CLIENTE` | Faz pedidos e gerencia a própria fidelidade |

Usuários criados pelo seed (senha entre parênteses):

| Perfil | E-mail | Senha |
| ------ | ------ | ----- |
| ADMIN | `admin@raizes.com` | `Admin@123` |
| GERENTE | `gerente@raizes.com` | `Gerente@123` |
| ATENDENTE | `atendente@raizes.com` | `Atendente@123` |
| COZINHA | `cozinha@raizes.com` | `Cozinha@123` |
| CLIENTE | `cliente@raizes.com` | `Cliente@123` |

---

## Autenticação JWT

A maioria dos endpoints exige autenticação. O fluxo é:

1. Faça **login** (ou **registre** um usuário) e copie o `token` retornado.
2. No Swagger, clique em **Authorize** e informe:

```text
Bearer seu_token_aqui
```

3. Clique em **Authorize**. Os endpoints protegidos passam a aceitar suas requisições.

Em chamadas diretas (curl/Postman), envie o header:

```http
Authorization: Bearer seu_token_aqui
```

O token expira conforme `JwtSettings:ExpirationMinutes` (padrão: 60 minutos). Endpoints com
restrição de perfil retornam **403 Forbidden** quando o perfil do token não é suficiente.

---

## Formato de erro padrão

Erros tratados retornam um corpo padronizado (`ErroResponse`):

```json
{
  "erro": "ESTOQUE_INSUFICIENTE",
  "mensagem": "Não há quantidade suficiente para um ou mais itens.",
  "detalhes": [
    { "field": "itens[0].quantidade", "issue": "Disponível: 1" }
  ],
  "timestamp": "2026-06-28T12:00:00Z",
  "path": "/api/pedidos",
  "requestId": "..."
}
```

`detalhes` é opcional e detalha quais campos falharam na validação.

---

## Fluxos da aplicação
### Fluxo 1 — Autenticação

| Método | Rota | Descrição |
| ------ | ---- | --------- |
| POST | `/api/auth/register` | Cria um usuário |
| POST | `/api/auth/login` | Autentica e retorna token |
| GET | `/api/auth/me` | Dados do usuário autenticado |

**Registrar** — `POST /api/auth/register`

```json
{
  "nome": "João Silva",
  "email": "joao@email.com",
  "senha": "123456",
  "role": "CLIENTE"
}
```

`role` é opcional (padrão `CLIENTE`). Resposta de sucesso (**201**):

```json
{
  "id": 1,
  "nome": "João Silva",
  "email": "joao@email.com",
  "role": "CLIENTE",
  "token": "jwt_token_aqui",
  "expiresAt": "2026-06-28T13:00:00Z"
}
```

| Status | Descrição |
| ------ | --------- |
| 201 | Usuário registrado |
| 400 | Dados inválidos |
| 409 | E-mail já registrado |

**Login** — `POST /api/auth/login`

```json
{
  "email": "joao@email.com",
  "senha": "123456"
}
```

Resposta de sucesso (**200**) tem o mesmo formato do registro (com `token`).

| Status | Descrição |
| ------ | --------- |
| 200 | Login realizado |
| 400 | Dados inválidos |
| 401 | Credenciais inválidas |

**Usuário autenticado** — `GET /api/auth/me`

```json
{
  "id": 1,
  "nome": "João Silva",
  "email": "joao@email.com",
  "role": "CLIENTE",
  "ativo": true
}
```

| Status | Descrição |
| ------ | --------- |
| 200 | Dados retornados |
| 401 | Não autenticado / token inválido |
| 404 | Usuário não encontrado |

### Fluxo 2 — Unidades

Lojas/filiais da rede. Cada unidade tem seu próprio cardápio e estoque.

| Método | Rota | Descrição |
| ------ | ---- | --------- |
| GET | `/api/unidades?pagina=1&tamanhoPagina=10` | Lista paginada de unidades |
| GET | `/api/unidades/{unidadeId}` | Detalha uma unidade |
| POST | `/api/unidades` | Cadastra uma unidade |
| POST | `/api/unidades/{unidadeId}` | Atualiza uma unidade |

Exemplo de corpo (cadastro/atualização):

```json
{
  "nome": "Recife Centro",
  "cidade": "Recife",
  "estado": "PE",
  "endereco": "Rua da Aurora, 100",
  "ativa": true
}
```

### Fluxo 3 — Produtos

Catálogo global de produtos (independente de unidade).

| Método | Rota | Descrição |
| ------ | ---- | --------- |
| GET | `/api/produtos?pagina=1&tamanhoPagina=10` | Lista paginada de produtos |
| GET | `/api/produtos/{produtoId}` | Detalha um produto |
| POST | `/api/produtos` | Cadastra um produto |
| PUT | `/api/produtos/{produtoId}` | Atualiza um produto |
| DELETE | `/api/produtos/{produtoId}` | Desativa um produto |

Exemplo de corpo (cadastro/atualização):

```json
{
  "nome": "Tapioca de queijo coalho",
  "descricao": "Tapioca recheada com queijo coalho",
  "preco": 12.90,
  "categoria": "Salgados",
  "ativo": true,
  "sazonal": false
}
```

| Status | Descrição |
| ------ | --------- |
| 200 / 201 | Sucesso |
| 204 | Lista vazia (GET) / produto desativado (DELETE) |
| 404 | Produto não encontrado |
| 422 | Dados inválidos |

### Fluxo 4 — Cardápio por unidade

Define **quais produtos** cada unidade oferece, sua disponibilidade e um **preço customizado**
opcional (ex.: variação regional). Um produto só pode ser pedido em uma unidade se estiver no
cardápio dela e disponível.

| Método | Rota | Descrição |
| ------ | ---- | --------- |
| GET | `/api/unidades/{unidadeId}/cardapio` | Lista o cardápio da unidade |
| POST | `/api/unidades/{unidadeId}/produtos` | Adiciona um produto ao cardápio |
| PUT | `/api/unidades/{unidadeId}/produtos/{produtoId}` | Atualiza item do cardápio |

Exemplo de corpo (adicionar/atualizar):

```json
{
  "produtoId": 1,
  "disponivel": true,
  "precoCustomizado": 13.90
}
```

`precoCustomizado` é opcional; quando nulo, vale o preço do produto. O cardápio retornado já traz
o preço efetivo de cada item naquela unidade.

### Fluxo 5 — Estoque

Saldo de cada produto por unidade e o histórico de movimentações.

| Método | Rota | Descrição |
| ------ | ---- | --------- |
| GET | `/api/unidades/{unidadeId}/estoque` | Saldo de estoque da unidade |
| POST | `/api/estoque/movimentos` | Registra um movimento |
| GET | `/api/estoque/movimentos?unidadeId=&produtoId=` | Lista movimentos (com filtros) |

Exemplo de movimento — `POST /api/estoque/movimentos`:

```json
{
  "unidadeId": 1,
  "produtoId": 4,
  "tipoMovimento": "ENTRADA",
  "quantidade": 10,
  "motivo": "Reposição"
}
```

`tipoMovimento` aceita:

* `ENTRADA` — soma ao saldo;
* `SAIDA` — subtrai do saldo (falha com **409** se não houver saldo suficiente);
* `AJUSTE` — define o saldo exato para o valor informado.

A resposta do saldo (`GET .../estoque`) inclui o campo `estoqueBaixo`, que fica `true` quando a
quantidade atual está abaixo ou igual à mínima. Todo movimento é registrado na auditoria.

| Status | Descrição |
| ------ | --------- |
| 200 / 201 | Sucesso |
| 404 | Unidade ou produto não encontrado |
| 409 | Estoque insuficiente para a saída |
| 422 | Dados inválidos |

### Fluxo 6 — Pedidos e pagamento

Coração da aplicação. Ao criar um pedido, a API valida disponibilidade e estoque, processa o
**pagamento (gateway mockado)** e, se aprovado, dá baixa no estoque e credita pontos de
fidelidade — tudo em uma transação.

| Método | Rota | Descrição |
| ------ | ---- | --------- |
| POST | `/api/pedidos` | Cria pedido e processa o pagamento |
| GET | `/api/pedidos?canalPedido=&status=&pagina=1&limit=10` | Lista pedidos (cliente vê só os próprios) |
| GET | `/api/pedidos/{pedidoId}` | Detalha um pedido |
| PATCH | `/api/pedidos/{pedidoId}/status` | Avança o status |
| POST | `/api/pedidos/{pedidoId}/cancelar` | Cancela o pedido |

**Criar pedido** — `POST /api/pedidos`:

```json
{
  "unidadeId": 1,
  "canalPedido": "APP",
  "formaPagamento": "MOCK",
  "itens": [
    { "produtoId": 1, "quantidade": 2 },
    { "produtoId": 3, "quantidade": 1 }
  ]
}
```

* `canalPedido` aceita: `APP`, `TOTEM`, `BALCAO`, `PICKUP`, `WEB`.
* `formaPagamento` é opcional (padrão `MOCK`).

**Gateway de pagamento (mock).** O pagamento é aprovado quando `formaPagamento` = `MOCK` **e** o
total do pedido é maior que zero. Qualquer outra forma de pagamento é **recusada** (útil para
testar o caminho de recusa).

**Máquina de status.** O pedido nasce em `AGUARDANDO_PAGAMENTO`. O resultado do pagamento o leva
automaticamente a `PAGO` ou `PAGAMENTO_RECUSADO`. As transições válidas são:

```text
CRIADO              → AGUARDANDO_PAGAMENTO
AGUARDANDO_PAGAMENTO → PAGO | PAGAMENTO_RECUSADO | CANCELADO
PAGO                → EM_PREPARO | CANCELADO
EM_PREPARO          → PRONTO
PRONTO              → ENTREGUE
```

Quem pode avançar o status (`PATCH .../status`), além de ADMIN e GERENTE (que podem qualquer
transição válida):

* **COZINHA** → `EM_PREPARO`, `PRONTO` (e `ENTREGUE`);
* **ATENDENTE** → `ENTREGUE`.

Exemplo — `PATCH /api/pedidos/1/status`:

```json
{ "novoStatus": "EM_PREPARO" }
```

**Cancelamento** (`POST .../cancelar`) só é permitido a partir de `AGUARDANDO_PAGAMENTO` ou `PAGO`.
Se o pedido já estava `PAGO`, o estoque baixado é **estornado** automaticamente.

Resposta de um pedido (`PedidoResponse`):

```json
{
  "pedidoId": 1,
  "status": "PAGO",
  "canalPedido": "APP",
  "unidadeId": 1,
  "clienteId": 5,
  "total": 33.80,
  "createdAt": "2026-06-28T12:00:00Z",
  "pagamento": {
    "id": 1,
    "pedidoId": 1,
    "status": "APROVADO",
    "formaPagamento": "MOCK",
    "transactionId": "mock-abc123",
    "createdAt": "2026-06-28T12:00:00Z"
  },
  "itens": [
    { "produtoId": 1, "nome": "Tapioca de queijo coalho", "quantidade": 2, "precoUnitario": 12.90, "subtotal": 25.80 }
  ]
}
```

| Status | Descrição |
| ------ | --------- |
| 200 / 201 | Sucesso |
| 403 | Cliente tentando acessar pedido de outro / perfil sem permissão na transição |
| 404 | Pedido, unidade ou produto não encontrado |
| 409 | Transição de status inválida |
| 422 | Canal inválido, item inválido ou estoque insuficiente |

**Consultar o pagamento** de um pedido:

| Método | Rota | Descrição |
| ------ | ---- | --------- |
| GET | `/api/pedidos/{pedidoId}/pagamento` | Retorna o pagamento do pedido |

(Cliente só acessa o pagamento dos próprios pedidos — caso contrário **403**.)

### Fluxo 7 — Fidelidade (LGPD)

Programa de pontos do cliente autenticado. A cada pedido **pago**, o cliente acumula
automaticamente `floor(total)` pontos (1 ponto por unidade inteira do valor).

| Método | Rota | Descrição |
| ------ | ---- | --------- |
| GET | `/api/fidelidade/saldo` | Saldo de pontos do cliente |
| GET | `/api/fidelidade/historico` | Histórico de créditos/débitos |
| POST | `/api/fidelidade/resgatar` | Resgata (debita) pontos |
| PUT | `/api/fidelidade/consentimento` | Consentimento de marketing (LGPD) |

**Resgatar** — `POST /api/fidelidade/resgatar`:

```json
{ "pontos": 50, "descricao": "Resgate de brinde" }
```

Falha com **422** se `pontos` for menor ou igual a zero ou maior que o saldo disponível.

**Consentimento** — `PUT /api/fidelidade/consentimento`:

```json
{ "consentimentoMarketing": true }
```

Registra a opção do cliente e a data do consentimento (atende à LGPD). O saldo retornado inclui
`consentimentoMarketing` e `consentimentoData`.

### Fluxo 8 — Auditoria

Trilha de auditoria das ações sensíveis (criação/alteração/cancelamento de pedido, ajuste de
estoque etc.), gravada automaticamente pelos serviços. Apenas para gestão.

| Método | Rota | Acesso | Descrição |
| ------ | ---- | ------ | --------- |
| GET | `/api/auditorias?entidade=&entidadeId=&pagina=1&tamanhoPagina=10` | 🔒 (ADMIN, GERENTE) | Lista paginada de registros |

Cada registro traz a ação, a entidade afetada, o estado anterior/posterior (em JSON), o usuário e
o IP de origem:

```json
{
  "id": 10,
  "usuarioId": 5,
  "acao": "CRIAR_PEDIDO",
  "entidade": "Pedido",
  "entidadeId": 1,
  "dadosAntes": null,
  "dadosDepois": "{ \"id\": 1, \"status\": \"PAGO\", \"total\": 33.80, \"canal\": \"APP\" }",
  "ip": "127.0.0.1",
  "createdAt": "2026-06-28T12:00:00Z"
}
```

---

## Fluxo recomendado para testar de ponta a ponta

1. Suba o SQL Server com Docker e confira a connection string.
2. Rode a API (`dotnet run`) — migrations e seed são aplicados automaticamente.
3. Abra o Swagger na raiz (`https://localhost:7000/`).
4. Faça **login** com um usuário do seed (ex.: `cliente@raizes.com` / `Cliente@123`) em
   `POST /api/auth/login` e copie o `token`.
5. Clique em **Authorize** e informe `Bearer seu_token_aqui`.
6. Liste as unidades (`GET /api/unidades`) e o cardápio de uma delas
   (`GET /api/unidades/1/cardapio`) para escolher produtos disponíveis.
7. Crie um pedido em `POST /api/pedidos` com `formaPagamento: "MOCK"` — ele deve ser **aprovado**,
   dar baixa no estoque e creditar pontos. Tente também `formaPagamento` diferente de `MOCK` para
   ver a **recusa**.
8. Consulte o pedido (`GET /api/pedidos/{id}`) e o pagamento (`GET /api/pedidos/{id}/pagamento`).
9. Veja seus pontos em `GET /api/fidelidade/saldo` e o `GET /api/fidelidade/historico`.
10. Autentique-se como **COZINHA** e avance o status do pedido
    (`PATCH /api/pedidos/{id}/status` → `EM_PREPARO`, depois `PRONTO`).
11. Autentique-se como **ADMIN** ou **GERENTE** e confira a trilha em `GET /api/auditorias`.
