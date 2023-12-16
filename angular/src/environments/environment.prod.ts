import { Environment } from '@abp/ng.core';

const baseUrl = 'https://localhost:5000';

export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'BishalAgroSeed',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:5000/api/',
    redirectUri: baseUrl,
    clientId: 'BishalAgroSeed_App',
    responseType: 'code',
    scope: 'offline_access BishalAgroSeed',
    requireHttps: true
  },
  apis: {
    default: {
      url: 'https://localhost:5000/api',
      rootNamespace: 'BishalAgroSeed',
    },
  },
} as Environment;
