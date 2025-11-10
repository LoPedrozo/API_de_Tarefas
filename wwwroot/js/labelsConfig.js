/**
 * Definição canônica de etiquetas do Kanban.
 */
export const LABELS = {
  verde: {
    id: 'verde',
    name: 'Verde',
    color: 'Verde',
    category: 'Vendas'
  },
  amarelo: {
    id: 'amarelo',
    name: 'Amarelo',
    color: 'Amarelo',
    category: 'Marketing'
  },
  cinza: {
    id: 'cinza',
    name: 'Cinza',
    color: 'Cinza',
    category: 'Operações'
  },
  financeiro: {
    id: 'financeiro',
    name: 'Financeiro',
    color: 'Azul',
    category: 'Financeiro'
  },
  marketing: {
    id: 'marketing',
    name: 'Marketing',
    color: 'Amarelo',
    category: 'Marketing',
    aliasFor: 'amarelo'
  },
  operacoes: {
    id: 'operacoes',
    name: 'Operações',
    color: 'Cinza',
    category: 'Operações',
    aliasFor: 'cinza'
  },
  produto: {
    id: 'produto',
    name: 'Produto',
    color: 'Roxo',
    category: 'Produto'
  },
  rh: {
    id: 'rh',
    name: 'RH',
    color: 'Magenta',
    category: 'Recursos Humanos'
  },
  suporte: {
    id: 'suporte',
    name: 'Suporte',
    color: 'Azul',
    category: 'Suporte'
  },
  tecnologia: {
    id: 'tecnologia',
    name: 'Tecnologia',
    color: 'Azul',
    category: 'Tecnologia'
  },
  vendas: {
    id: 'vendas',
    name: 'Vendas',
    color: 'Verde',
    category: 'Vendas',
    aliasFor: 'verde'
  }
};

const DISPLAY_LABEL_ORDER = [
  'verde',
  'amarelo',
  'cinza',
  'financeiro',
  'produto',
  'rh',
  'suporte',
  'tecnologia'
];

const LABEL_ALIAS_MAP = new Map();

const setAliases = (ids, target) => {
  ids.forEach(id => LABEL_ALIAS_MAP.set(id, target));
};

setAliases(['verde', 'verdes', 'vendas', 'vendass', 'tag1'], 'verde');
setAliases(['amarelo', 'amarelos', 'marketing', 'tag2'], 'amarelo');
setAliases(['cinza', 'cinzas', 'operacoes', 'operações', 'tag3'], 'cinza');
setAliases(['financeiro', 'financas', 'finanças'], 'financeiro');
setAliases(['produto', 'produtos'], 'produto');
setAliases(['rh', 'recursoshumanos', 'recursos'], 'rh');
setAliases(['suporte', 'support'], 'suporte');
setAliases(['tecnologia', 'tech', 'tecnologias'], 'tecnologia');
LABEL_ALIAS_MAP.set('string', null);
LABEL_ALIAS_MAP.set('inicial', null);

const strip = value =>
  value
    .normalize('NFD')
    .replace(/[\u0300-\u036f]/g, '')
    .replace(/[^a-z]/g, '')
    .toLowerCase();

export function isValidLabelId(id) {
  return Boolean(id && LABELS[id]);
}

export function getCanonicalLabelId(id) {
  if (!id) return null;
  const canonical = LABELS[id]?.aliasFor ?? id;
  return isValidLabelId(canonical) ? canonical : null;
}

export function normalizeLabel(input) {
  if (!input || typeof input !== 'string') return null;
  const sanitized = strip(input);
  if (!sanitized) return null;
  const mapped = LABEL_ALIAS_MAP.has(sanitized)
    ? LABEL_ALIAS_MAP.get(sanitized)
    : sanitized;

  if (!mapped) return null;
  const canonical = getCanonicalLabelId(mapped);
  return canonical ?? null;
}

export function getLabelOptions() {
  return DISPLAY_LABEL_ORDER.map(id => LABELS[id]).filter(Boolean);
}

export function getLabelDefinition(id) {
  const canonical = getCanonicalLabelId(id);
  return canonical ? LABELS[canonical] : null;
}

export function getDefaultLabelId() {
  return DISPLAY_LABEL_ORDER[0];
}
