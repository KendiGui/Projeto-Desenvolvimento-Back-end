# Projeto Desenvolvimento Back-end

API desenvolvida em ASP.NET Core com autenticação JWT, Swagger e banco de dados SQL Server.

---

## Requisitos

Antes de rodar o projeto, tenha instalado:

* .NET 10.0
* Docker
* SQL Server Management Studio ou Azure Data Studio
* Entity Framework Core Tools

Para instalar o Entity Framework Core Tools globalmente:

```bash
dotnet tool install --global dotnet-ef
```

Caso já esteja instalado, atualize com:

```bash
dotnet tool update --global dotnet-ef
```

---

## Como rodar o SQL Server local com Docker

Execute o comando abaixo para baixar e subir uma imagem do SQL Server localmente:

```bash
docker run -e "ACCEPT_EULA=Y" ^
-e "SA_PASSWORD=SqlServer@123" ^
-p 1433:1433 ^
--name sqlserver-local ^
-d mcr.microsoft.com/mssql/server:2022-latest
```

No Linux/macOS ou Git Bash:

```bash
docker run -e "ACCEPT_EULA=Y" \
-e "SA_PASSWORD=SqlServer@123" \
-p 1433:1433 \
--name sqlserver-local \
-d mcr.microsoft.com/mssql/server:2022-latest
```

Verifique se o container está rodando:

```bash
docker ps
```

Caso precise iniciar novamente depois:

```bash
docker start sqlserver-local
```

Caso precise parar:

```bash
docker stop sqlserver-local
```

---

## Connection string local

No arquivo `appsettings.json`, certifique-se que a connection string esteja apontando para o banco local:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=ProjetoBackendDb;User Id=sa;Password=SqlServer@123;TrustServerCertificate=True;"
  }
}
```

O parâmetro `TrustServerCertificate=True` evita erro de certificado SSL em ambiente local.

---

## Criando o banco de dados com Entity Framework

Com o SQL Server rodando no Docker, execute o comando abaixo na raiz do projeto:

```bash
cd .\Infrastructure\
dotnet ef database update -c DatabaseContext
```

Caso esteja usando o Package Manager Console do Visual Studio:

```powershell
Update-Database
```

Se o comando `Update-Database` não for reconhecido, instale o pacote no projeto:

```bash
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

---

## Rodando a API

Na raiz do projeto da API, execute:

```bash
dotnet run
```

A aplicação será iniciada em uma URL parecida com:

```text
https://localhost:7000
http://localhost:5000
```

A porta pode variar dependendo da configuração do projeto.

---

## Acessando o Swagger

Com a API rodando, acesse no navegador:

```text
https://localhost:7000/swagger
```

No Swagger é possível visualizar e testar os endpoints disponíveis da API.

---

## Endpoints de autenticação

A API possui os seguintes endpoints de autenticação:

```text
POST /api/auth/register
POST /api/auth/login
GET  /api/auth/me
```

---

## Registrar usuário

Endpoint usado para criar um novo usuário no sistema.

```http
POST /api/auth/register
```

Exemplo de request:

```json
{
  "nome": "João Silva",
  "email": "joao@email.com",
  "senha": "123456"
}
```

Exemplo de resposta com sucesso:

```json
{
  "token": "jwt_token_aqui",
  "usuario": {
    "id": 1,
    "nome": "João Silva",
    "email": "joao@email.com"
  }
}
```

Possíveis status:

| Status | Descrição                      |
| ------ | ------------------------------ |
| 201    | Usuário registrado com sucesso |
| 400    | Dados inválidos                |
| 409    | E-mail já registrado           |
| 500    | Erro interno                   |

---

## Login

Endpoint usado para autenticar um usuário já cadastrado.

```http
POST /api/auth/login
```

Exemplo de request:

```json
{
  "email": "joao@email.com",
  "senha": "123456"
}
```

Exemplo de resposta com sucesso:

```json
{
  "token": "jwt_token_aqui",
  "usuario": {
    "id": 1,
    "nome": "João Silva",
    "email": "joao@email.com"
  }
}
```

Possíveis status:

| Status | Descrição                   |
| ------ | --------------------------- |
| 200    | Login realizado com sucesso |
| 400    | Dados inválidos             |
| 401    | Credenciais inválidas       |
| 500    | Erro interno                |

---

## Usando o token JWT

Após fazer login ou registrar um usuário, copie o token JWT retornado pela API.

No Swagger:

1. Clique no botão **Authorize**
2. Informe o token no formato:

```text
Bearer seu_token_aqui
```

Exemplo:

```text
Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

3. Clique em **Authorize**
4. Agora os endpoints protegidos poderão ser acessados

---

## Buscar dados do usuário autenticado

Endpoint protegido que retorna os dados do usuário logado.

```http
GET /api/auth/me
```

Esse endpoint exige autenticação via JWT.

Header necessário:

```http
Authorization: Bearer seu_token_aqui
```

Exemplo usando `curl`:

```bash
curl -X GET "https://localhost:7000/api/auth/me" ^
-H "Authorization: Bearer seu_token_aqui"
```

No Linux/macOS ou Git Bash:

```bash
curl -X GET "https://localhost:7000/api/auth/me" \
-H "Authorization: Bearer seu_token_aqui"
```

Possíveis status:

| Status | Descrição                                 |
| ------ | ----------------------------------------- |
| 200    | Dados do usuário retornados com sucesso   |
| 401    | Usuário não autenticado ou token inválido |
| 404    | Usuário não encontrado                    |
| 500    | Erro interno                              |

---

## Fluxo recomendado para testar

1. Subir o SQL Server com Docker
2. Configurar a connection string no `appsettings.json`
3. Rodar o comando:

```bash
dotnet ef database update
```

4. Rodar a API:

```bash
dotnet run
```

5. Acessar o Swagger:

```text
https://localhost:7000/swagger
```

6. Criar um usuário em:

```text
POST /api/auth/register
```

7. Fazer login em:

```text
POST /api/auth/login
```

8. Copiar o token JWT retornado
9. Autorizar no Swagger usando:

```text
Bearer seu_token_aqui
```

10. Testar o endpoint protegido:

```text
GET /api/auth/me
```

---