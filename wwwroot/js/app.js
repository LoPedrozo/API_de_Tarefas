const LABEL_DEFINITIONS = {
  verde: {
    id: 'verde',
    name: 'Verde',
    category: 'Vendas',
    color: 'Verde',
    aliases: ['verde', 'vendas', 'tag1']
  },
  amarelo: {
    id: 'amarelo',
    name: 'Amarelo',
    category: 'Marketing',
    color: 'Amarelo',
    aliases: ['amarelo', 'marketing', 'tag2']
  },
  cinza: {
    id: 'cinza',
    name: 'Cinza',
    category: 'Operações',
    color: 'Cinza',
    aliases: ['cinza', 'operacoes', 'operações', 'tag3']
  }
};

const LABEL_DISPLAY_ORDER = ['verde', 'amarelo', 'cinza'];

const LABEL_ALIAS_LOOKUP = LABEL_DISPLAY_ORDER.reduce((acc, id) => {
  const def = LABEL_DEFINITIONS[id];
  if (!def) return acc;
  def.aliases?.forEach(alias => {
    acc[normalizeAlias(alias)] = id;
  });
  acc[normalizeAlias(id)] = id;
  return acc;
}, {});

function normalizeAlias(value) {
  return value
    .toString()
    .normalize('NFD')
    .replace(/[^a-z0-9]/gi, '')
    .toLowerCase();
}

function normalizeLabel(label) {
  if (!label) return null;
  const sanitized = normalizeAlias(label.trim());
  return LABEL_ALIAS_LOOKUP[sanitized] ?? null;
}

function getLabelDefinition(id) {
  return id ? LABEL_DEFINITIONS[id] ?? null : null;
}

function getLabelOptions() {
  return LABEL_DISPLAY_ORDER
    .map(id => LABEL_DEFINITIONS[id])
    .filter(Boolean);
}


const API_URL = typeof window !== 'undefined'
  ? `${window.location.origin}/api/tarefas`
  : '/api/tarefas';

const STATUS_TO_API = {
  'todo': 'Não concluída',
  'in-progress': 'Em progresso',
  'done': 'Concluída'
};

const PRIORITY_TO_API = {
  'baixa': 'Baixa',
  'medio': 'Medio',
  'alta': 'Alta',
  'critica': 'Critica'
};

const PRIORITY_LABEL = {
  'baixa': 'Baixa',
  'medio': 'Média',
  'alta': 'Alta',
  'critica': 'Crítica'
};

const COLOR_CLASS_MAP = {
  'Verde': 'verde',
  'Amarelo': 'amarelo',
  'Cinza': 'cinza'
};

const DEFAULT_LABEL_ID = LABEL_DISPLAY_ORDER[0];

const filterState = {
  searchTerm: '',
  labelId: '',
  priority: '',
  status: ''
};

let tasks = [];
let currentTaskId = null;

const elements = {
  modal: null,
  modalTitle: null,
  form: null,
  titleInput: null,
  descriptionInput: null,
  labelSelect: null,
  statusSelect: null,
  prioritySelect: null,
  titleError: null,
  toast: null,
  searchInput: null,
  labelFilter: null,
  priorityFilter: null,
  statusFilter: null,
  densityButton: null,
  resetButton: null
};

if (typeof document !== 'undefined') {
  document.addEventListener('DOMContentLoaded', () => {
    cacheDom();
    renderLabelSelectOptions(elements.labelFilter, { includeEmpty: true, emptyLabel: 'Todas' });
    renderLabelSelectOptions(elements.labelSelect, { includeEmpty: false });
    bindEvents();
    loadTasks();
  });
}

function cacheDom() {
  elements.modal = document.getElementById('task-modal');
  elements.modalTitle = document.getElementById('modal-title');
  elements.form = document.getElementById('task-form');
  elements.titleInput = document.getElementById('task-title');
  elements.descriptionInput = document.getElementById('task-description');
  elements.labelSelect = document.getElementById('task-label');
  elements.statusSelect = document.getElementById('task-status');
  elements.prioritySelect = document.getElementById('task-priority');
  elements.titleError = document.getElementById('title-error');
  elements.toast = document.getElementById('toast');
  elements.searchInput = document.querySelector('[data-testid="search"]');
  elements.labelFilter = document.querySelector('[data-testid="tag-filter"]');
  elements.priorityFilter = document.querySelector('[data-testid="priority-filter"]');
  elements.statusFilter = document.querySelector('[data-testid="status-filter"]');
  elements.densityButton = document.querySelector('[data-testid="density-toggle"]');
  elements.resetButton = document.querySelector('[data-testid="reset-demo"]');
}

function bindEvents() {
  document.querySelectorAll('.add-task').forEach(button => {
    button.addEventListener('click', () => {
      const columnStatus = button.closest('.column')?.dataset.status ?? 'todo';
      openModal(null, columnStatus);
    });
  });

  elements.form.addEventListener('submit', handleSubmit);
  document.querySelector('[data-testid="modal-cancel"]').addEventListener('click', closeModal);
  elements.modal.addEventListener('click', event => {
    if (event.target === elements.modal) closeModal();
  });

  elements.searchInput.addEventListener('input', event => {
    filterState.searchTerm = event.target.value.toLowerCase();
    renderColumns();
  });

  elements.labelFilter.addEventListener('change', event => {
    const selected = event.target.value;
    const isValid = Boolean(getLabelDefinition(selected));
    filterState.labelId = isValid ? selected : '';
    event.target.value = filterState.labelId;
    renderColumns();
  });

  elements.priorityFilter.addEventListener('change', event => {
    filterState.priority = event.target.value;
    renderColumns();
  });

  elements.statusFilter.addEventListener('change', event => {
    filterState.status = event.target.value;
    renderColumns();
  });

  elements.densityButton.addEventListener('click', () => {
    document.body.classList.toggle('compact');
    elements.densityButton.textContent = document.body.classList.contains('compact')
      ? 'Densidade: Compacta'
      : 'Densidade: Padrão';
  });

  elements.resetButton.addEventListener('click', () => loadTasks(true));

  document.addEventListener('keydown', event => {
    if (event.key === 'Escape') {
      closeModal();
      return;
    }

    if (event.key.toLowerCase() === 'n' && !elements.modal.classList.contains('show')) {
      openModal();
    }
  });
}

async function loadTasks(showFeedback = false) {
  try {
    const response = await fetch(API_URL);
    if (!response.ok) throw new Error('Erro ao consultar a API.');
    const payload = await response.json();
    tasks = payload.map(mapApiTaskToUi);
    renderColumns();

    if (showFeedback) {
      showToast('Tarefas sincronizadas com a API.');
    }
  } catch (error) {
    console.error(error);
    showToast('Não foi possível carregar as tarefas.', true);
  }
}

function renderLabelSelectOptions(selectEl, { includeEmpty, emptyLabel = '' }) {
  if (!selectEl) return;
  const currentValue = selectEl.value;
  selectEl.innerHTML = '';

  if (includeEmpty) {
    const option = document.createElement('option');
    option.value = '';
    option.textContent = emptyLabel;
    selectEl.appendChild(option);
  }

  getLabelOptions().forEach(def => {
    const option = document.createElement('option');
    option.value = def.id;
    option.textContent = def.name;
    selectEl.appendChild(option);
  });

  if (!includeEmpty) {
    const validValue = getLabelDefinition(currentValue) ? currentValue : DEFAULT_LABEL_ID;
    selectEl.value = validValue;
  } else if (currentValue && getLabelDefinition(currentValue)) {
    selectEl.value = currentValue;
  } else {
    selectEl.value = '';
  }
}

function getCanonicalTaskLabelId(rawTags) {
  if (!Array.isArray(rawTags)) return null;
  for (const raw of rawTags) {
    const normalized = normalizeLabel(raw);
    if (normalized) {
      return normalized;
    }
  }
  return null;
}

function mapApiTaskToUi(dto) {
  const labelId = getCanonicalTaskLabelId(dto.tags);
  const label = getLabelDefinition(labelId);
  const priority = normalizePriority(dto.prioridade);

  return {
    id: dto.id,
    title: dto.titulo ?? 'Sem título',
    description: dto.descricao ?? '',
    status: normalizeStatus(dto.status),
    priority,
    labelId: label?.id ?? null,
    label: label ?? null,
    priorityLabel: PRIORITY_LABEL[priority]
  };
}

function normalizeStatus(value) {
  if (!value) return 'todo';
  const normalized = value
    .normalize('NFD')
    .replace(/[\u0300-\u036f]/g, '')
    .toLowerCase();

  if (normalized.includes('progres') || normalized.includes('andament')) {
    return 'in-progress';
  }

  if (normalized.includes('concluida') && !normalized.includes('nao')) {
    return 'done';
  }

  return 'todo';
}

function normalizePriority(value) {
  const normalized = (value ?? '').toString().trim().toLowerCase();
  if (normalized.startsWith('crit')) return 'critica';
  if (normalized.startsWith('alt')) return 'alta';
  if (normalized.startsWith('baix')) return 'baixa';
  if (normalized.startsWith('med')) return 'medio';
  return 'medio';
}

function shouldIncludeTask(task, filters) {
  const search = filters.searchTerm?.toLowerCase() ?? '';
  const matchesSearch = !search
    || task.title.toLowerCase().includes(search)
    || task.description.toLowerCase().includes(search);

  const matchesLabel = !filters.labelId || task.labelId === filters.labelId;
  const matchesPriority = !filters.priority || task.priority === filters.priority;
  const matchesStatus = !filters.status || task.status === filters.status;

  return matchesSearch && matchesLabel && matchesPriority && matchesStatus;
}

function renderColumns() {
  document.querySelectorAll('.column').forEach(column => {
    const columnStatus = column.dataset.status;
    const wrapper = column.querySelector('.cards');
    wrapper.innerHTML = '';

    const filtered = tasks
      .filter(task => task.status === columnStatus)
      .filter(task => shouldIncludeTask(task, filterState));

    if (!filtered.length) {
      const empty = document.createElement('p');
      empty.className = 'card-description';
      empty.textContent = 'Sem tarefas aqui.';
      wrapper.appendChild(empty);
      return;
    }

    filtered.forEach(task => wrapper.appendChild(createCard(task)));
  });
}

function createCard(task) {
  const card = document.createElement('div');
  card.className = 'card';
  card.dataset.id = task.id;
  card.tabIndex = 0;

  const title = document.createElement('div');
  title.className = 'card-title';
  title.textContent = task.title;
  card.appendChild(title);

  if (task.description) {
    const description = document.createElement('p');
    description.className = 'card-description';
    description.textContent = task.description;
    card.appendChild(description);
  }

  if (task.label) {
    const tagsContainer = document.createElement('div');
    tagsContainer.className = 'card-tags';

    const categoryChip = document.createElement('span');
    const colorClass = COLOR_CLASS_MAP[task.label.color] ?? 'cinza';
    categoryChip.className = `chip chip-category chip-color-${colorClass}`;
    categoryChip.textContent = task.label.category;
    tagsContainer.appendChild(categoryChip);

    card.appendChild(tagsContainer);
  }

  const priority = document.createElement('span');
  priority.className = `card-priority priority-${task.priority}`;
  const priorityLabel = task.priorityLabel ?? PRIORITY_LABEL[task.priority] ?? '';
  priority.textContent = priorityLabel;
  priority.setAttribute('aria-label', `Prioridade ${priorityLabel}`);
  card.appendChild(priority);

  const menu = document.createElement('div');
  menu.className = 'card-menu';
  menu.innerHTML = '&vellip;';
  const dropdown = document.createElement('div');
  dropdown.className = 'card-menu-dropdown';
  const editButton = document.createElement('button');
  editButton.textContent = 'Editar';
  editButton.addEventListener('click', event => {
    event.stopPropagation();
    openModal(task);
  });
  const deleteButton = document.createElement('button');
  deleteButton.textContent = 'Excluir';
  deleteButton.addEventListener('click', async event => {
    event.stopPropagation();
    await deleteTask(task.id);
  });
  dropdown.appendChild(editButton);
  dropdown.appendChild(deleteButton);
  menu.appendChild(dropdown);
  card.appendChild(menu);

  card.addEventListener('click', () => openModal(task));

  return card;
}

function openModal(task = null, status = 'todo') {
  currentTaskId = task?.id ?? null;
  elements.modalTitle.textContent = currentTaskId ? 'Editar Tarefa' : 'Nova Tarefa';
  elements.titleInput.value = task?.title ?? '';
  elements.descriptionInput.value = task?.description ?? '';
  const defaultStatus = task?.status ?? status ?? 'todo';
  const validLabelId = getLabelDefinition(task?.labelId) ? task.labelId : DEFAULT_LABEL_ID;
  elements.labelSelect.value = validLabelId;
  elements.statusSelect.value = defaultStatus;
  elements.prioritySelect.value = task?.priority ?? 'medio';
  elements.titleError.textContent = '';
  elements.modal.classList.add('show');
}

function closeModal() {
  elements.modal.classList.remove('show');
  currentTaskId = null;
  elements.form.reset();
  elements.statusSelect.value = 'todo';
  elements.prioritySelect.value = 'medio';
  elements.labelSelect.value = DEFAULT_LABEL_ID;
  elements.titleError.textContent = '';
}

async function handleSubmit(event) {
  event.preventDefault();
  elements.titleError.textContent = '';

  const title = elements.titleInput.value.trim();
  if (!title) {
    elements.titleError.textContent = 'O título é obrigatório.';
    return;
  }

  const taskPayload = {
    id: currentTaskId,
    title,
    description: elements.descriptionInput.value.trim(),
    status: elements.statusSelect.value,
    labelId: getLabelDefinition(elements.labelSelect.value)
      ? elements.labelSelect.value
      : DEFAULT_LABEL_ID,
    priority: elements.prioritySelect.value
  };

  const isEditing = Boolean(taskPayload.id);

  try {
    await persistTask(taskPayload);
    closeModal();
    await loadTasks();
    showToast(isEditing ? 'Tarefa atualizada.' : 'Tarefa criada.');
  } catch (error) {
    console.error(error);
    showToast('Não foi possível salvar a tarefa.', true);
  }
}

async function persistTask(task) {
  const method = task.id ? 'PUT' : 'POST';
  const url = task.id ? `${API_URL}/${task.id}` : API_URL;
  const response = await fetch(url, {
    method,
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(mapUiTaskToApi(task))
  });

  if (!response.ok) {
    throw new Error('Erro ao persistir a tarefa.');
  }
}

function mapUiTaskToApi(task) {
  const label = getLabelDefinition(task.labelId) ?? getLabelDefinition(DEFAULT_LABEL_ID);
  return {
    titulo: task.title,
    descricao: task.description,
    status: STATUS_TO_API[task.status] ?? STATUS_TO_API.todo,
    prioridade: PRIORITY_TO_API[task.priority] ?? PRIORITY_TO_API.medio,
    tags: label ? [label.name] : []
  };
}

async function deleteTask(id) {
  if (!confirm('Deseja realmente excluir esta tarefa?')) return;

  try {
    const response = await fetch(`${API_URL}/${id}`, { method: 'DELETE' });
    if (!response.ok) throw new Error('Erro ao excluir.');
    await loadTasks();
    showToast('Tarefa excluída.');
  } catch (error) {
    console.error(error);
    showToast('Não foi possível excluir.', true);
  }
}

function showToast(message, isError = false) {
  if (!elements.toast) return;
  elements.toast.textContent = message;
  elements.toast.classList.toggle('error', isError);
  elements.toast.classList.add('show');
  setTimeout(() => elements.toast.classList.remove('show'), 3200);
}
