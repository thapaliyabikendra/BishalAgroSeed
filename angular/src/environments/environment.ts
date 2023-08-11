import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: false,
  application: {
    baseUrl,
    name: 'BishalAgroSeed',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44358/',
    redirectUri: baseUrl,
    clientId: 'BishalAgroSeed_App',
    responseType: 'code',
    scope: 'offline_access BishalAgroSeed',
    requireHttps: true,
  },
  apis: {
    default: {
      url: 'https://localhost:44358',
      rootNamespace: 'BishalAgroSeed',
    },
  },
} as Environment;
