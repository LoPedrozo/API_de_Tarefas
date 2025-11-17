Aslam Rekik e Ângelo da Mata Panzera

# API de Tarefas (TAREFASAPI)

Uma API RESTful simples para gerenciamento de tarefas, desenvolvida em ASP.NET Core com C#. Inclui um frontend básico em HTML/CSS/JavaScript para interação via interface web.

## Funcionalidades

- **CRUD de Tarefas**: Criar, ler, atualizar e excluir tarefas.
- **Campos da Tarefa**:
  - Título (obrigatório)
  - Descrição
  - Status (A Fazer, Em Progresso, Concluído)
  - Prioridade (Baixa, Média, Alta, Crítica)
  - Responsável
  - Datas (Criação, Vencimento, Conclusão)
  - Estimativa de horas
  - Tags (Setor e tags adicionais)
  - Arquivada (sim/não)
- **Interface Web**: Kanban simples com colunas para status, filtros por setor/prioridade/busca.
- **Banco de Dados**: SQLite integrado via Entity Framework Core.
- **Documentação**: Swagger UI disponível em desenvolvimento.

## Tecnologias Utilizadas

- **Backend**: ASP.NET Core 7.0
- **Banco de Dados**: SQLite
- **ORM**: Entity Framework Core
- **Frontend**: HTML5, CSS3, JavaScript (ES6+)
- **Documentação**: Swashbuckle (Swagger)

## Pré-requisitos

- .NET 7.0 SDK instalado
- Navegador web moderno

## Como Executar

1. **Clone ou baixe o projeto**:
   ```
   git clone <url-do-repositorio>
   cd API_de_Tarefas
   ```

2. **Restaure as dependências**:
   ```
   dotnet restore
   ```

3. **Execute a aplicação**:
   ```
   dotnet run
   ```

4. **Acesse a aplicação**:
   - API: `https://localhost:5001` ou `http://localhost:5000`
   - Interface Web: `https://localhost:5001/index.html` ou `http://localhost:5000/index.html`
   - Swagger: `https://localhost:5001/swagger` (em modo desenvolvimento)

## Endpoints da API

### GET /api/tarefas
Retorna todas as tarefas.

### GET /api/tarefas/{id}
Retorna uma tarefa específica pelo ID.

### POST /api/tarefas
Cria uma nova tarefa. Corpo da requisição deve conter os dados da tarefa em JSON.

### PUT /api/tarefas/{id}
Atualiza uma tarefa existente pelo ID. Corpo da requisição deve conter os dados atualizados em JSON.

### DELETE /api/tarefas/{id}
Exclui uma tarefa pelo ID.

## Estrutura do Projeto

- `Program.cs`: Ponto de entrada da aplicação, configuração de serviços e middleware.
- `Models/Tarefas.cs`: Modelo de dados da tarefa, incluindo enums e conversores JSON.
- `Data/TarefasDbContext.cs`: Contexto do Entity Framework para acesso ao banco.
- `Routes/`: Classes estáticas para definição dos endpoints (GET, POST, PUT, DELETE).
- `wwwroot/`: Arquivos estáticos do frontend (HTML, CSS, JS, imagens).
- `appsettings.json`: Configurações da aplicação (conexão do banco, etc.).

## Banco de Dados

O banco SQLite é criado automaticamente na primeira execução em `tarefas.db` (ou conforme configurado em `appsettings.json`).

Para aplicar migrações manuais (se necessário):
```
dotnet ef database update
```

## Desenvolvimento

- Modo desenvolvimento: Define `ASPNETCORE_ENVIRONMENT=Development` para habilitar Swagger e outras ferramentas.
- O banco é recriado automaticamente se não existir.
- CORS habilitado para permitir requisições do frontend.

## Contribuição

1. Faça um fork do projeto.
2. Crie uma branch para sua feature (`git checkout -b feature/nova-feature`).
3. Commit suas mudanças (`git commit -am 'Adiciona nova feature'`).
4. Push para a branch (`git push origin feature/nova-feature`).
5. Abra um Pull Request.

## Licença

Este projeto é de código aberto. Consulte o arquivo LICENSE para mais detalhes.
