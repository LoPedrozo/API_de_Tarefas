# Documentação do Frontend - API de Tarefas

Esta documentação descreve os componentes do frontend da aplicação TAREFASAPI, incluindo HTML, CSS e JavaScript. O frontend consiste em uma interface Kanban simples para gerenciamento de tarefas, localizada em `wwwroot/index.html` e `wwwroot/js/app.js`.

## 1. Estrutura HTML (`wwwroot/index.html`)

O arquivo `index.html` define a estrutura da página web, incluindo cabeçalho, colunas Kanban, modal de formulário e elementos de confirmação/toast.

### Elementos Principais:

- **`<head>`**: Contém metadados, links para fontes (Google Fonts), e estilos CSS embutidos.
- **`<header>`**: Cabeçalho fixo com logo, campo de busca, filtros (setor e prioridade) e botão de ajuda.
- **`<main>`**: Área principal com abas (tabs) e colunas Kanban (A Fazer, Em Progresso, Concluído).
- **Modal (`#task-modal`)**: Formulário para criar/editar tarefas, com seções para informações principais, datas e tags.
- **Overlay de Confirmação (`#confirm-overlay`)**: Diálogo para confirmar exclusão de tarefas.
- **Toast (`#toast`)**: Notificações temporárias para feedback ao usuário.

### Funcionalidades HTML:

- Responsivo com media queries para dispositivos móveis.
- Acessibilidade: Labels, ARIA attributes, e navegação por teclado.
- Estrutura semântica com elementos como `<header>`, `<main>`, `<section>`.

## 2. Estilos CSS (Embutidos em `index.html`)

Os estilos estão definidos no `<style>` dentro do `<head>`. Utilizam variáveis CSS para cores e temas consistentes.

### Variáveis Principais:

- `--green-700`, `--green-200`, `--green-primary`, etc.: Cores do tema verde.
- `--white`, `--gray-50`, `--gray-500`: Cores neutras.
- `--shadow`: Sombra para elementos elevados.
- `--input-border`: Cor de borda para inputs.

### Seções de Estilo:

- **Reset e Globais**: Remove margens/padding padrão, define fonte (Poppins), e estilos para botões, inputs, selects.
- **Header**: Posicionamento fixo, layout flexível, responsivo.
- **Main e Colunas**: Grid layout para colunas, responsivo (3 colunas desktop, 2 tablet, 1 mobile).
- **Cards**: Estilos para cartões de tarefa, incluindo hover, drag, e elementos internos (título, descrição, tags, meta, footer).
- **Modal**: Overlay, conteúdo, formulário, seções organizadas.
- **Confirmação e Toast**: Diálogos e notificações.
- **Animações**: Transições suaves (fadeIn, hover effects), keyframes para entrada de elementos.
- **Responsividade**: Media queries para ajustar layout em telas menores (1023px, 768px, 599px).

### Destaques:

- Tema verde consistente para botões e destaques.
- Hover e focus states para interatividade.
- Animações para melhorar UX (ex.: cards flutuando ao hover).
- Flexível para diferentes tamanhos de tela.

## 3. JavaScript (`wwwroot/js/app.js`)

O arquivo `app.js` gerencia a lógica do frontend, incluindo carregamento de tarefas, manipulação de DOM, interações e comunicação com a API.

### Constantes e Variáveis Globais:

- `LABEL_MAP`: Mapeamento de setores (verde: Vendas, amarelo: Marketing, cinza: Operações) com aliases e cores.
- `STATUS_OPTIONS`: Opções de status (todo, in-progress, done) com labels e classes CSS.
- `PRIORITY_OPTIONS`: Opções de prioridade com ícones SVG.
- `API_URL`: URL base da API (`/api/tarefas`).
- `filterState`: Estado dos filtros (busca, setor, prioridade).
- `modalState`: Estado do modal (tags extras).
- `tasks`: Array de tarefas carregadas.
- `currentTaskId`: ID da tarefa sendo editada.

### Funções Principais:

- **`cacheDom()`**: Armazena referências aos elementos DOM em `elements`.
- **`setupLabelSelectors()`**: Preenche selects de setor.
- **`bindEvents()`**: Vincula eventos (clicks, submits, keydowns).
- **`switchTab(columnName)`**: Alterna visibilidade de colunas baseado na aba selecionada.
- **`loadTasks(showFeedback)`**: Carrega tarefas da API e renderiza colunas.
- **`renderColumns()`**: Filtra e renderiza tarefas nas colunas.
- **`createCard(task)`**: Cria elemento DOM para um cartão de tarefa.
- **`shouldIncludeTask(task, filters)`**: Verifica se tarefa passa pelos filtros.
- **`openModal(task, statusHint)`**: Abre modal para criar/editar tarefa.
- **`closeModal()`**: Fecha modal e reseta estado.
- **`handleSubmit(event)`**: Processa submissão do formulário, persiste tarefa via API.
- **`persistTask(task)`**: Envia POST/PUT para API.
- **`deleteTask(id, title)`**: Confirma e deleta tarefa via DELETE.
- **`renderLabelSelectOptions(selectEl, options)`**: Preenche selects de setor.
- **`addExtraTag(raw)` / `removeExtraTag(tag)`**: Gerencia tags extras no modal.
- **`renderExtraTags()`**: Renderiza chips de tags extras.
- **`mapApiTaskToUi(dto)` / `mapUiTaskToApi(task)`**: Converte entre formatos API e UI.
- **`buildTagsPayload(colorId, extraTags)`**: Constrói array de tags para API.
- **`pickColorTag(tags)`**: Identifica setor baseado em tags.
- **`sanitizeTags(tags)`**: Limpa e normaliza tags.
- **`normalizeStatusFromApi(value)` / `statusToApi(value)`**: Converte status.
- **`normalizePriority(value)` / `priorityToApi(value)`**: Converte prioridade.
- **`parseEstimateValue(value)` / `formatEstimateForApi(value)`**: Trata estimativa de horas.
- **`formatDateForDisplay(value)`**: Formata datas para exibição.
- **`isArchived(value)`**: Verifica se tarefa está arquivada.
- **`showToast(message, type)`**: Exibe notificações.
- **`openConfirmDialog(message)`**: Abre diálogo de confirmação.
- **`resolveConfirmDialog(result)`**: Resolve promessa de confirmação.

### Funcionalidades JavaScript:

- **Carregamento Dinâmico**: Tarefas carregadas da API e renderizadas dinamicamente.
- **Interação**: Clicks em cards abrem modal, botões adicionam/excluem tarefas.
- **Filtros e Busca**: Filtra tarefas por texto, setor, prioridade.
- **Drag and Drop Implícito**: Cards têm classe 'dragging' para visual.
- **Validação**: Verifica título obrigatório, datas consistentes.
- **Feedback**: Toasts para sucesso/erro, confirmações para ações destrutivas.
- **Responsividade**: Ajusta layout baseado em abas em mobile.

### Dependências:

- Nenhum framework externo; puro JavaScript ES6+.
- Usa Fetch API para requisições HTTP.
- Manipulação direta de DOM.

Esta documentação pode ser copiada e colada diretamente em um documento Word para referência.
