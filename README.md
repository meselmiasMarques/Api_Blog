📘 Blog API – ASP.NET Core 9

  API REST desenvolvida em ASP.NET Core 9, com autenticação JWT, controle de usuários, perfis (roles), 
  posts, categorias, tags e upload de imagens — ideal para estudos ou como base para um blog completo.

🚀 Tecnologias Utilizadas

ASP.NET Core 9

Entity Framework Core

JWT Authentication

Swagger / OpenAPI

SQL Server

Clean Controllers + ViewModels

Identity simplificado (Users / Roles)

📂 Estrutura Principal

  A API possui módulos completos para:
  
  Autenticação
  Usuários
  Perfis (Roles)
  Posts
  Categorias
  Tags
  Upload de imagem Base64

🔐 Autenticação

A API utiliza JWT.

Endpoints

Método	Rota	       Descrição
POST	/v1/accounts	Registro de usuário
POST	/v1/accounts/login	Login e obtenção do token
POST	/v1/accounts/upload-image	Upload de imagem em Base64

🧑‍💼 Users
Método	Rota
GET	/v1/users
GET	/v1/users/{id}
PUT	/v1/users/{id}
DELETE	/v1/users/{id}
🎭 Roles

Gerenciamento de perfis de acesso:

Método	Rota
GET	/v1/roles
POST	/v1/roles
GET	/v1/roles/{id}
PUT	/v1/roles/{id}
DELETE	/v1/roles/{id}
POST	/v1/roles/user — atribuir role ao usuário
DELETE	/v1/roles/user/{userid}/{roleid} — remover role

🏷️ Tags
Método	Rota
GET	/v1/tags
POST	/v1/tags
GET	/v1/tags/{id}
PUT	/v1/tags/{id}
DELETE	/v1/tags/{id}

🗂️ Categorias
Método	Rota
GET	/v1/categories
POST	/v1/categories
GET	/v1/categories/{id}
PUT	/v1/categories/{id}
DELETE	/v1/categories/{id}

✍️ Posts
Método	Rota
GET	/v1/posts
POST	/v1/posts
GET	/v1/posts/{id}
PUT	/v1/posts/{id}
DELETE	/v1/posts/{id}
GET	/v1/posts/category/{category}

Parâmetros de paginação disponíveis:

page

pageSize

🧭 Executando o Projeto
1. Restaurar pacotes
dotnet restore

2. Rodar migrações (se houver)
dotnet ef database update

3. Executar aplicação
dotnet run

4. Abrir Swagger

Acesse:

https://localhost:5001/swagger

🔒 Aviso Importante

Este projeto não deve conter chaves secretas, como:

SendGrid API Key

Tokens JWT

Connection Strings sensíveis

Use o arquivo:

appsettings.Development.json


e mantenha fora do GitHub.

📜 Licença

Este projeto está disponível sob a licença MIT.
Sinta-se à vontade para usar, estudar e modificar.
