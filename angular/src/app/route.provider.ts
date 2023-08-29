import { RoutesService, eLayoutType } from '@abp/ng.core';
import { APP_INITIALIZER } from '@angular/core';

export const APP_ROUTE_PROVIDER = [
  { provide: APP_INITIALIZER, useFactory: configureRoutes, deps: [RoutesService], multi: true },
];

function configureRoutes(routesService: RoutesService) {
  return () => {
    routesService.add([
      {
        path: '/',
        name: '::Menu:Home',
        iconClass: 'fas fa-home',
        order: 1,
        layout: eLayoutType.application,
      },
      {
        path: '/configurations',
        name: '::Menu:Configurations',
        iconClass: 'fas fa-home',
        order: 2,
        layout: eLayoutType.application,
      },
      {
        path: '/customers',
        name: '::Menu:Customers',
        iconClass: 'fas fa-home',
        order: 3,
        layout: eLayoutType.application,
      },
      {
        path: '/categories',
        name: '::Menu:Categories',
        iconClass: 'fas fa-home',
        order: 4,
        layout: eLayoutType.application,
      },
      {
        path: '/brands',
        name: '::Menu:Brands',
        iconClass: 'fas fa-home',
        order: 5,
        layout: eLayoutType.application,
      },
      {
        path: '/company-infos',
        name: '::Menu:CompanyInfos',
        iconClass: 'fas fa-home',
        order: 6,
        layout: eLayoutType.application,
      },
      {
        path: '/products',
        name: '::Menu:Products',
        iconClass: 'fas fa-home',
        order: 7,
        layout: eLayoutType.application,
      },
      {
        path: '/cycle-counts',
        name: '::Menu:CycleCounts',
        iconClass: 'fas fa-home',
        order: 6,
        layout: eLayoutType.application,
      },
    ]);
  };
}
