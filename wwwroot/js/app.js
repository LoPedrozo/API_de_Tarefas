const LABEL_MAP = {
  verde: {
    id: 'verde',
    name: 'Verde',
    categoria: 'Vendas',
    color: '#28a745',
    textColor: '#ffffff',
    token: 'Verde',
    aliases: ['verde', 'verdes', 'vendas', 'vendass', 'tag1']
  },
  amarelo: {
    id: 'amarelo',
    name: 'Amarelo',
    categoria: 'Marketing',
    color: '#ffc107',
    textColor: '#2b2104',
    token: 'Amarelo',
    aliases: ['amarelo', 'amarelos', 'marketing', 'tag2']
  },
  cinza: {
    id: 'cinza',
    name: 'Cinza',
    categoria: 'Operações',
    color: '#6c757d',
    textColor: '#ffffff',
    token: 'Cinza',
    aliases: ['cinza', 'cinzas', 'operacoes', 'operações', 'operacao', 'tag3']
  }
};

const LABEL_ORDER = Object.keys(LABEL_MAP);
const DEFAULT_LABEL_ID = LABEL_ORDER[0];
const BLOCKED_TAGS = new Set(['string', 'inicial']);
const LABEL_ALIAS_LOOKUP = buildAliasLookup();

const STATUS_OPTIONS = {
  'todo': { label: 'A Fazer', api: 'Não concluída', pillClass: 'pill-status-todo' },
  'in-progress': { label: 'Em Progresso', api: 'Em progresso', pillClass: 'pill-status-in-progress' },
  'done': { label: 'Concluída', api: 'Concluída', pillClass: 'pill-status-done' }
};

const PRIORITY_OPTIONS = {
  baixa: { label: 'Baixa', api: 'Baixa', pillClass: 'pill-priority-baixa' },
  medio: { label: 'Média', api: 'Medio', pillClass: 'pill-priority-medio' },
  alta: { label: 'Alta', api: 'Alta', pillClass: 'pill-priority-alta' },
  critica: { label: 'Crítica', api: 'Critica', pillClass: 'pill-priority-critica' }
};

const DATE_SENTINELS = {
  vencimento: 'Sem data de vencimento',
  conclusao: 'Sem data de conclusão'
};

const API_URL = `${window.location.origin}/api/tarefas`;

const filterState = {
  searchTerm: '',
  labelId: '',
  priority: '',
  status: ''
};

const modalState = {
  extraTags: []
};

const elements = {};
let tasks = [];
let currentTaskId = null;
const confirmState = {
  resolver: null
};

document.addEventListener('DOMContentLoaded', () => {
  cacheDom();
  setupLabelSelectors();
  bindEvents();
  // Começa sempre mostrando todas as colunas
  switchTab('all');
  loadTasks();
});

function cacheDom() {
  elements.modal = document.getElementById('task-modal');
  elements.modalTitle = document.getElementById('modal-title');
  elements.form = document.getElementById('task-form');
  elements.titleInput = document.getElementById('task-title');
  elements.descriptionInput = document.getElementById('task-description');
  elements.responsavelInput = document.getElementById('task-responsavel');
  elements.labelSelect = document.getElementById('task-label');
  elements.statusSelect = document.getElementById('task-status');
  elements.prioritySelect = document.getElementById('task-priority');
  elements.creationDateInput = document.getElementById('task-data-criacao');
  elements.dueDateInput = document.getElementById('task-data-vencimento');
  elements.conclusionDateInput = document.getElementById('task-data-conclusao');
  elements.estimativaInput = document.getElementById('task-estimativa');
  elements.arquivadaCheckbox = document.getElementById('task-arquivada');
  elements.extraTagInput = document.getElementById('extra-tag-input');
  elements.extraTagAddButton = document.getElementById('extra-tag-add');
  elements.extraTagsList = document.getElementById('extra-tags-list');
  elements.titleError = document.getElementById('title-error');
  elements.toast = document.getElementById('toast');
  elements.searchInput = document.querySelector('[data-testid="search"]');
  elements.labelFilter = document.querySelector('[data-testid="tag-filter"]');
  elements.priorityFilter = document.querySelector('[data-testid="priority-filter"]');
  elements.statusFilter = document.querySelector('[data-testid="status-filter"]');
  elements.densityButton = document.querySelector('[data-testid="density-toggle"]');
  elements.resetButton = document.querySelector('[data-testid="reset-demo"]');
  elements.modalCancelButton = document.querySelector('[data-testid="modal-cancel"]');
  elements.tabs = document.querySelectorAll('.tab');
  elements.confirmOverlay = document.getElementById('confirm-overlay');
  elements.confirmMessage = document.getElementById('confirm-message');
  elements.confirmConfirmButton = document.getElementById('confirm-confirm');
  elements.confirmCancelButton = document.getElementById('confirm-cancel');
}

function setupLabelSelectors() {
  renderLabelSelectOptions(elements.labelFilter, { includeEmpty: true, emptyLabel: 'Todas' });
  renderLabelSelectOptions(elements.labelSelect, { includeEmpty: false });
}

function bindEvents() {
  document.querySelectorAll('.add-task').forEach(button => {
    button.addEventListener('click', () => {
      const status = button.closest('.column')?.dataset.status ?? 'todo';
      openModal(null, status);
    });
  });

  elements.tabs.forEach(tab => {
    tab.addEventListener('click', () => {
      const columnName = tab.dataset.column;
      switchTab(columnName);
    });
  });

  elements.form.addEventListener('submit', handleSubmit);
  elements.modalCancelButton.addEventListener('click', closeModal);
  elements.modal.addEventListener('click', event => {
    if (event.target === elements.modal) closeModal();
  });

  elements.searchInput.addEventListener('input', event => {
    filterState.searchTerm = event.target.value.toLowerCase();
    renderColumns();
  });

  elements.labelFilter.addEventListener('change', event => {
    const selected = event.target.value;
    filterState.labelId = getLabelDefinition(selected) ? selected : '';
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

  elements.extraTagAddButton.addEventListener('click', () => addExtraTag(elements.extraTagInput.value));
  elements.extraTagInput.addEventListener('keydown', event => {
    if (event.key === 'Enter') {
      event.preventDefault();
      addExtraTag(elements.extraTagInput.value);
    }
  });

  if (elements.confirmCancelButton) {
    elements.confirmCancelButton.addEventListener('click', () => resolveConfirmDialog(false));
  }
  if (elements.confirmConfirmButton) {
    elements.confirmConfirmButton.addEventListener('click', () => resolveConfirmDialog(true));
  }
  if (elements.confirmOverlay) {
    elements.confirmOverlay.addEventListener('click', event => {
      if (event.target === elements.confirmOverlay) {
        resolveConfirmDialog(false);
      }
    });
  }
  document.addEventListener('keydown', event => {
    if (event.key === 'Escape' && confirmState.resolver) {
      resolveConfirmDialog(false);
    }
  });
}

function switchTab(columnName) {
  // Remove active de todas as abas
  elements.tabs.forEach(tab => tab.classList.remove('active'));

  // Adiciona active na aba selecionada
  const activeTab = Array.from(elements.tabs).find(tab => tab.dataset.column === columnName);
  if (activeTab) {
    activeTab.classList.add('active');
  }

  const columns = document.querySelectorAll('.column');

  // Se for "Todos", mostra todas as colunas
  if (columnName === 'all') {
    columns.forEach(column => column.classList.remove('hidden'));
    return;
  }

  // Senão, mostra só a coluna do status selecionado
  columns.forEach(column => {
    const status = column.dataset.status;
    if (status === columnName) {
      column.classList.remove('hidden');
    } else {
      column.classList.add('hidden');
    }
  });
}

async function loadTasks(showFeedback = false) {
  try {
    const response = await fetch(API_URL);
    if (!response.ok) throw new Error('Erro ao consultar a API');
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

function renderColumns() {
  document.querySelectorAll('.column').forEach(column => {
    const status = column.dataset.status;
    const wrapper = column.querySelector('.cards');
    wrapper.innerHTML = '';

    const filtered = tasks
      .filter(task => task.status === status)
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
    categoryChip.className = `chip chip-category`;
    categoryChip.textContent = task.label.categoria;
    categoryChip.style.backgroundColor = task.label.color;
    categoryChip.style.color = task.label.textColor;
    tagsContainer.appendChild(categoryChip);
    card.appendChild(tagsContainer);
  }

  const meta = document.createElement('div');
  meta.className = 'card-meta';
  meta.innerHTML = `
    <span><strong>Responsável:</strong> ${task.responsavel || '—'}</span>
    <span><strong>Criada:</strong> ${formatDateForDisplay(task.dataCriacao)}</span>
    <span><strong>Vencimento:</strong> ${task.dataVencimento ? formatDateForDisplay(task.dataVencimento) : 'Sem data'}</span>
    <span><strong>Conclusão:</strong> ${task.dataConclusao ? formatDateForDisplay(task.dataConclusao) : 'Sem data'}</span>
    <span><strong>Estimativa:</strong> ${formatEstimateForDisplay(task.estimativaHoras)}</span>
    <span><strong>Arquivada:</strong> ${task.arquivada ? 'Sim' : 'Não'}</span>
  `;
  card.appendChild(meta);

  if (task.extraTags.length) {
    const extraWrapper = document.createElement('div');
    extraWrapper.className = 'card-extra-tags';
    task.extraTags.forEach(tag => {
      const chip = document.createElement('span');
      chip.className = 'tag-small';
      chip.textContent = tag;
      extraWrapper.appendChild(chip);
    });
    card.appendChild(extraWrapper);
  }

  const footer = document.createElement('div');
  footer.className = 'card-footer';
  const statusPill = document.createElement('span');
  statusPill.className = `pill ${STATUS_OPTIONS[task.status]?.pillClass ?? ''}`;
  statusPill.textContent = STATUS_OPTIONS[task.status]?.label ?? 'A Fazer';
  footer.appendChild(statusPill);

  const priorityPill = document.createElement('span');
  priorityPill.className = `pill ${PRIORITY_OPTIONS[task.priority]?.pillClass ?? ''}`;
  priorityPill.textContent = PRIORITY_OPTIONS[task.priority]?.label ?? 'Média';
  footer.appendChild(priorityPill);
  card.appendChild(footer);

  const menu = document.createElement('div');
  menu.className = 'card-menu';
  menu.innerHTML = '&vellip;';
  const dropdown = document.createElement('div');
  dropdown.className = 'card-menu-dropdown';
  const editButton = document.createElement('button');
  editButton.textContent = 'Editar';
  editButton.addEventListener('click', event => {
    event.stopPropagation();
    openModal(task, task.status);
  });
  const deleteButton = document.createElement('button');
  deleteButton.textContent = 'Excluir';
  deleteButton.addEventListener('click', async event => {
    event.stopPropagation();
    await deleteTask(task.id, task.title);
  });
  dropdown.appendChild(editButton);
  dropdown.appendChild(deleteButton);
  menu.appendChild(dropdown);
  card.appendChild(menu);

  card.addEventListener('click', () => openModal(task, task.status));

  return card;
}

function shouldIncludeTask(task, filters) {
  const search = filters.searchTerm;
  const matchesSearch = !search
    || task.title.toLowerCase().includes(search)
    || task.description.toLowerCase().includes(search)
    || (task.responsavel && task.responsavel.toLowerCase().includes(search));

  const matchesLabel = !filters.labelId || task.labelId === filters.labelId;
  const matchesPriority = !filters.priority || task.priority === filters.priority;
  const matchesStatus = !filters.status || task.status === filters.status;

  return matchesSearch && matchesLabel && matchesPriority && matchesStatus;
}

function openModal(task = null, statusHint = 'todo') {
  currentTaskId = task?.id ?? null;
  elements.modalTitle.textContent = currentTaskId ? 'Editar Tarefa' : 'Nova Tarefa';
  elements.titleInput.value = task?.title ?? '';
  elements.descriptionInput.value = task?.description ?? '';
  elements.responsavelInput.value = task?.responsavel ?? '';
  elements.labelSelect.value = getLabelDefinition(task?.labelId) ? task.labelId : DEFAULT_LABEL_ID;
  elements.statusSelect.value = task?.status ?? statusHint ?? 'todo';
  elements.prioritySelect.value = task?.priority ?? 'medio';
  elements.creationDateInput.value = task?.dataCriacao ?? getTodayDateString();
  elements.dueDateInput.value = task?.dataVencimento ?? '';
  elements.conclusionDateInput.value = task?.dataConclusao ?? '';
  elements.estimativaInput.value = typeof task?.estimativaHoras === 'number' ? task.estimativaHoras : '';
  elements.arquivadaCheckbox.checked = task?.arquivada ?? false;
  modalState.extraTags = [...(task?.extraTags ?? [])];
  renderExtraTags();
  elements.titleError.textContent = '';
  elements.modal.classList.add('show');
}

function closeModal() {
  elements.modal.classList.remove('show');
  currentTaskId = null;
  elements.form.reset();
  elements.labelSelect.value = DEFAULT_LABEL_ID;
  elements.statusSelect.value = 'todo';
  elements.prioritySelect.value = 'medio';
  elements.creationDateInput.value = getTodayDateString();
  elements.dueDateInput.value = '';
  elements.conclusionDateInput.value = '';
  elements.estimativaInput.value = '';
  elements.arquivadaCheckbox.checked = false;
  modalState.extraTags = [];
  renderExtraTags();
  elements.titleError.textContent = '';
}

async function handleSubmit(event) {
  event.preventDefault();
  const title = elements.titleInput.value.trim();
  if (!title) {
    elements.titleError.textContent = 'O título é obrigatório.';
    return;
  }

  const creationDate = elements.creationDateInput.value || getTodayDateString();
  const conclusionDate = elements.conclusionDateInput.value;
  if (creationDate && conclusionDate && creationDate > conclusionDate) {
    elements.titleError.textContent = 'Data de conclusão não pode ser anterior à criação.';
    return;
  }

  const colorId = getLabelDefinition(elements.labelSelect.value) ? elements.labelSelect.value : DEFAULT_LABEL_ID;
  const extraTags = [...modalState.extraTags];

  const taskPayload = {
    id: currentTaskId,
    title,
    description: elements.descriptionInput.value.trim(),
    responsavel: elements.responsavelInput.value.trim(),
    status: elements.statusSelect.value || 'todo',
    priority: elements.prioritySelect.value || 'medio',
    dataCriacao: creationDate,
    dataVencimento: elements.dueDateInput.value,
    dataConclusao: conclusionDate,
    colorId,
    extraTags,
    tags: buildTagsPayload(colorId, extraTags),
    estimativaHoras: elements.estimativaInput.value ? Number(elements.estimativaInput.value) : null,
    arquivada: elements.arquivadaCheckbox.checked
  };

  try {
    await persistTask(taskPayload);
    closeModal();
    await loadTasks();
    showToast(currentTaskId ? 'Tarefa atualizada.' : 'Tarefa criada.');
  } catch (error) {
    console.error(error);
    showToast('Não foi possível salvar a tarefa.', true);
  }
}

async function persistTask(task) {
  const method = task.id ? 'PUT' : 'POST';
  const url = task.id ? `${API_URL}/${task.id}` : API_URL;
  const payload = mapUiTaskToApi(task);
  const response = await fetch(url, {
    method,
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload)
  });
  if (!response.ok) {
    throw new Error('Erro ao persistir a tarefa');
  }
}

async function deleteTask(id, title) {
  const confirmed = await openConfirmDialog(`Remover a tarefa "${title}"?`);
  if (!confirmed) return;
  try {
    const response = await fetch(`${API_URL}/${id}`, { method: 'DELETE' });
    if (!response.ok) throw new Error('Erro ao excluir');
    await loadTasks();
    showToast('Tarefa excluída.');
  } catch (error) {
    console.error(error);
    showToast('Não foi possível excluir.', true);
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
  LABEL_ORDER.forEach(id => {
    const def = LABEL_MAP[id];
    const option = document.createElement('option');
    option.value = def.id;
    option.textContent = def.name;
    selectEl.appendChild(option);
  });
  if (!includeEmpty) {
    selectEl.value = getLabelDefinition(currentValue) ? currentValue : DEFAULT_LABEL_ID;
  } else if (currentValue && getLabelDefinition(currentValue)) {
    selectEl.value = currentValue;
  } else {
    selectEl.value = '';
  }
}

function addExtraTag(raw) {
  const value = raw.trim();
  if (!value) return;
  const normalized = normalizeAlias(value);
  if (BLOCKED_TAGS.has(normalized)) {
    elements.extraTagInput.value = '';
    return;
  }
  if (LABEL_ALIAS_LOOKUP[normalized]) {
    elements.extraTagInput.value = '';
    return;
  }
  const exists = modalState.extraTags.some(tag => tag.toLowerCase() === value.toLowerCase());
  if (exists) {
    elements.extraTagInput.value = '';
    return;
  }
  modalState.extraTags.push(value);
  elements.extraTagInput.value = '';
  renderExtraTags();
}

function removeExtraTag(tag) {
  modalState.extraTags = modalState.extraTags.filter(item => item !== tag);
  renderExtraTags();
}

function renderExtraTags() {
  elements.extraTagsList.innerHTML = '';
  if (!modalState.extraTags.length) return;
  modalState.extraTags.forEach(tag => {
    const chip = document.createElement('span');
    chip.className = 'chip';
    chip.textContent = tag;
    const removeBtn = document.createElement('button');
    removeBtn.type = 'button';
    removeBtn.setAttribute('aria-label', `Remover tag ${tag}`);
    removeBtn.textContent = '×';
    removeBtn.addEventListener('click', () => removeExtraTag(tag));
    chip.appendChild(removeBtn);
    elements.extraTagsList.appendChild(chip);
  });
}

function mapApiTaskToUi(dto) {
  const sanitizedTags = sanitizeTags(dto.tags);
  const colorId = pickColorTag(sanitizedTags);
  const label = colorId ? LABEL_MAP[colorId] : null;
  const extraTags = sanitizedTags.filter(tag => normalizeAlias(tag) !== normalizeAlias(label?.token ?? ''));

  return {
    id: dto.id,
    title: dto.titulo ?? 'Sem título',
    description: dto.descricao ?? '',
    status: normalizeStatusFromApi(dto.status),
    priority: normalizePriority(dto.prioridade),
    dataCriacao: normalizeDateValue(dto.dataCriacao) || getTodayDateString(),
    dataVencimento: normalizeOptionalDate(dto.dataVencimento),
    dataConclusao: normalizeOptionalDate(dto.dataConclusao),
    responsavel: dto.responsavel ?? '',
    tags: sanitizedTags,
    extraTags,
    labelId: colorId ?? null,
    label,
    estimativaHoras: parseEstimateValue(dto.estimativaHoras),
    arquivada: isArchived(dto.arquivada)
  };
}

function mapUiTaskToApi(task) {
  return {
    titulo: task.title,
    descricao: task.description,
    status: statusToApi(task.status),
    dataCriacao: task.dataCriacao || getTodayDateString(),
    dataVencimento: task.dataVencimento || DATE_SENTINELS.vencimento,
    dataConclusao: task.dataConclusao || DATE_SENTINELS.conclusao,
    prioridade: priorityToApi(task.priority),
    responsavel: task.responsavel || '',
    tags: task.tags,
    estimativaHoras: formatEstimateForApi(task.estimativaHoras),
    arquivada: Boolean(task.arquivada)
  };
}

function buildTagsPayload(colorId, extraTags) {
  const result = [];
  if (colorId && LABEL_MAP[colorId]) {
    result.push(LABEL_MAP[colorId].token);
  }
  extraTags.forEach(tag => {
    const trimmed = tag.trim();
    if (!trimmed) return;
    const normalized = trimmed.toLowerCase();
    if (BLOCKED_TAGS.has(normalized)) return;
    if (LABEL_ALIAS_LOOKUP[normalizeAlias(trimmed)]) return;
    if (result.some(existing => existing.toLowerCase() === normalized)) return;
    result.push(trimmed);
  });
  return result;
}

function pickColorTag(tags) {
  const normalizedSet = new Set(tags.map(tag => normalizeAlias(tag)));
  for (const id of LABEL_ORDER) {
    const def = LABEL_MAP[id];
    if (normalizedSet.has(normalizeAlias(def.token))) {
      return id;
    }
    if (def.aliases.some(alias => normalizedSet.has(normalizeAlias(alias)))) {
      return id;
    }
  }
  return null;
}

function sanitizeTags(tags) {
  if (!Array.isArray(tags)) return [];
  const seen = new Set();
  const cleaned = [];
  tags.forEach(tag => {
    if (typeof tag !== 'string') return;
    const trimmed = tag.trim();
    if (!trimmed) return;
    const normalized = trimmed.toLowerCase();
    if (BLOCKED_TAGS.has(normalized)) return;
    if (seen.has(normalized)) return;
    seen.add(normalized);
    cleaned.push(trimmed);
  });
  return cleaned;
}

function normalizeStatusFromApi(value) {
  if (!value) return 'todo';
  const normalized = value.normalize('NFD').replace(/[^a-z]/gi, '').toLowerCase();
  if (normalized.includes('naoconcl') || normalized.includes('afazer') || normalized.includes('todo')) {
    return 'todo';
  }
  if (normalized.includes('progres') || normalized.includes('andament')) {
    return 'in-progress';
  }
  if (normalized.includes('conclu')) {
    return 'done';
  }
  return 'todo';
}

function statusToApi(value) {
  return STATUS_OPTIONS[value]?.api ?? STATUS_OPTIONS['todo'].api;
}

function normalizePriority(value) {
  const normalized = (value ?? '').toString().trim().toLowerCase();
  if (normalized.startsWith('crit')) return 'critica';
  if (normalized.startsWith('alt')) return 'alta';
  if (normalized.startsWith('baix')) return 'baixa';
  return 'medio';
}

function priorityToApi(value) {
  return PRIORITY_OPTIONS[value]?.api ?? PRIORITY_OPTIONS.medio.api;
}

function parseEstimateValue(value) {
  if (!value) return null;
  const match = value.toString().match(/\d+(?:[\.,]\d+)?/);
  if (!match) return null;
  const parsed = parseFloat(match[0].replace(',', '.'));
  return Number.isNaN(parsed) ? null : parsed;
}

function formatEstimateForApi(value) {
  if (value === null || value === '' || Number.isNaN(value)) {
    return 'Sem estimativa';
  }
  return `${value} horas restantes`;
}

function formatEstimateForDisplay(value) {
  if (value === null || value === undefined || value === '') return 'Sem estimativa';
  return `${value}h restantes`;
}

function normalizeDateValue(value) {
  if (!value) return '';
  const text = value.toString();
  if (text.toLowerCase().includes('sem')) return '';
  return text.slice(0, 10);
}

function normalizeOptionalDate(value) {
  return normalizeDateValue(value);
}

function parseLocalDate(value) {
  if (!value) return null;
  const [datePart] = value.toString().split('T');
  const parts = datePart.split('-').map(Number);
  if (parts.length !== 3) return null;
  const [year, month, day] = parts;
  if ([year, month, day].some(num => Number.isNaN(num))) return null;
  const date = new Date(year, month - 1, day);
  return Number.isNaN(date.getTime()) ? null : date;
}

function formatDateForDisplay(value) {
  if (!value) return 'Sem data';
  const date = parseLocalDate(value);
  if (!date) return value;
  return date.toLocaleDateString('pt-BR');
}

function isArchived(value) {
  if (typeof value === 'boolean') return value;
  if (typeof value === 'number') return value !== 0;
  if (value === null || value === undefined) return false;
  const normalized = normalizeAlias(value);
  if (!normalized) return false;
  if (normalized === 'true' || normalized === '1') return true;
  if (normalized === 'false' || normalized === '0') return false;
  if (normalized.includes('nao')) return false;
  if (normalized.includes('sim')) return true;
  return normalized.includes('arquiv');
}

function getLabelDefinition(id) {
  return id ? LABEL_MAP[id] ?? null : null;
}

function getTodayDateString() {
  return new Date().toISOString().split('T')[0];
}

function showToast(message, isError = false) {
  if (!elements.toast) return;
  elements.toast.textContent = message;
  elements.toast.classList.toggle('error', isError);
  elements.toast.classList.add('show');
  setTimeout(() => elements.toast.classList.remove('show'), 3200);
}

function buildAliasLookup() {
  const lookup = {};
  LABEL_ORDER.forEach(id => {
    const def = LABEL_MAP[id];
    def.aliases.forEach(alias => {
      lookup[normalizeAlias(alias)] = id;
    });
    lookup[normalizeAlias(def.token)] = id;
    lookup[normalizeAlias(id)] = id;
  });
  return lookup;
}

function normalizeAlias(value) {
  return (value ?? '')
    .toString()
    .normalize('NFD')
    .replace(/[^a-z0-9]/gi, '')
    .toLowerCase();
}

function openConfirmDialog(message) {
  if (!elements.confirmOverlay) {
    return Promise.resolve(window.confirm(message));
  }
  elements.confirmMessage.textContent = message;
  elements.confirmOverlay.classList.add('show');
  return new Promise(resolve => {
    confirmState.resolver = resolve;
  });
}

function resolveConfirmDialog(result) {
  if (elements.confirmOverlay) {
    elements.confirmOverlay.classList.remove('show');
  }
  if (confirmState.resolver) {
    const resolver = confirmState.resolver;
    confirmState.resolver = null;
    resolver(result);
  }
}
