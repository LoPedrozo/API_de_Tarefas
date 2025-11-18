📌 API de Gerenciamento de Tarefas – ASP.NET Core

Este projeto é uma API desenvolvida como parte da Prova A1 da disciplina de Programação Web / APIs.
A aplicação permite realizar o gerenciamento completo de tarefas utilizando rotas no padrão REST (GET, POST, PUT, DELETE), integradas a um banco de dados SQLite e organizadas em estrutura limpa utilizando o padrão Minimal API.

🚀 Tecnologias Utilizadas

.NET 8 / ASP.NET Core

C#

Entity Framework Core

SQLite

Minimal APIs

HTML/CSS/JS (página estática em wwwroot)

📂 Estrutura do Projeto
Prova-API-A1
│
├── Data/
│   └── TarefasDbContext.cs
│
├── Models/
│   └── Tarefas.cs
│
├── Routes/
│   ├── ROTA_GET.cs
│   ├── ROTA_POST.cs
│   ├── ROTA_PUT.cs
│   └── ROTA_DELETE.cs
│
├── wwwroot/
│   ├── index.html
│   ├── img/
│   └── js/
│       └── app.js
│
├── Program.cs
├── appsettings.json
├── tarefas.dev.db (SQLite)
└── TAREFASAPI.csproj

🧩 Funcionalidades

A API oferece:

✔ Listar todas as tarefas

GET /tarefas

✔ Buscar tarefa por ID

GET /tarefas/{id}

✔ Criar uma nova tarefa

POST /tarefas

✔ Atualizar uma tarefa existente

PUT /tarefas/{id}

✔ Excluir uma tarefa

DELETE /tarefas/{id}

Cada rota fica separada por responsabilidade dentro da pasta /Routes, deixando o código limpo e organizado.

🗄 Modelo da Tarefa

O modelo principal (Tarefas.cs) contém:

Id

Titulo

Descricao

Concluida (boolean)

DataCriacao

🔧 Banco de Dados

A API utiliza um banco SQLite, configurado em:

appsettings.json
tarefas.dev.db

▶ Como Executar o Projeto

Instale o SDK do .NET 8

Clone o repositório:

git clone https://github.com/SEU_USUARIO/Prova-API-A1.git


Acesse a pasta:

cd Prova-API-A1


Execute a aplicação:

dotnet run


A API será iniciada em:

https://localhost:7150
http://localhost:5150

🌐 Página Inicial

O projeto inclui uma página HTML simples dentro de wwwroot, contendo estrutura e visualização básica.

🙌 Agradecimentos

Agradeço ao professor pelo apoio, dedicação e pelo conteúdo transmitido durante o semestre.
Agradeço também à equipe que colaborou ativamente para o desenvolvimento deste projeto, mostrando comprometimento, organização e trabalho conjunto.

📎 Licença

Este projeto foi desenvolvido exclusivamente para fins acadêmicos.
