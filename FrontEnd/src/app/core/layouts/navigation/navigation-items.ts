import { NavigationNode } from '../types/navigation-node.type';

export const NAVIGATION_ROUTES = {
  dashboard: 'dashboard',
  lancamentos: 'lancamentos',
  contasBancarias: 'contas-bancarias',
  transferencias: 'transferencias',
  contasFixas: 'contas-fixas',
  metas: 'metas',
  saudeFinanceira: 'saude-financeira',
  categorias: 'categorias',
  meuPerfil: 'meu-perfil',
  configuracoes: 'configuracoes',
} as const;

export const NAVIGATION_ITEMS: NavigationNode[] = [
  {
    label: 'Dashboard',
    path: NAVIGATION_ROUTES.dashboard,
    icon: 'heroSquares2x2',
  },
  {
    label: 'Financeiro',
    icon: 'heroBanknotes',
    items: [
      {
        label: 'Lançamentos',
        path: NAVIGATION_ROUTES.lancamentos,
        icon: 'heroDocumentText',
      },
      {
        label: 'Contas Bancárias',
        path: NAVIGATION_ROUTES.contasBancarias,
        icon: 'heroBuildingLibrary',
      },
      {
        label: 'Transferências',
        path: NAVIGATION_ROUTES.transferencias,
        icon: 'heroArrowsRightLeft',
      },
      {
        label: 'Contas Fixas',
        path: NAVIGATION_ROUTES.contasFixas,
        icon: 'heroCalendarDays',
      },
    ],
  },
  {
    label: 'Planejamento',
    icon: 'heroChartBar',
    items: [
      {
        label: 'Metas',
        path: NAVIGATION_ROUTES.metas,
        icon: 'heroFlag',
      },
      {
        label: 'Saúde Financeira',
        path: NAVIGATION_ROUTES.saudeFinanceira,
        icon: 'heroHeart',
      },
      {
        label: 'Categorias',
        path: NAVIGATION_ROUTES.categorias,
        icon: 'heroTag',
      },
    ],
  },
];

const flatNavigationItems = NAVIGATION_ITEMS.flatMap(item =>
  'items' in item ? item.items : [item],
);
const pinnedBottomNavigationPaths = new Set<string>([
  NAVIGATION_ROUTES.dashboard,
  NAVIGATION_ROUTES.lancamentos,
  NAVIGATION_ROUTES.contasBancarias,
]);

export const BOTTOM_NAVIGATION_ITEMS = flatNavigationItems.filter(item =>
  pinnedBottomNavigationPaths.has(item.path),
);
export const MORE_NAVIGATION_ITEMS = flatNavigationItems.filter(
  item => !pinnedBottomNavigationPaths.has(item.path),
);
