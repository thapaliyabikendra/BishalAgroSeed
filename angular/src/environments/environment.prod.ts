import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:5000';

export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'BishalAgroSeed',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'http://localhost:5001',
    redirectUri: baseUrl,
    clientId: 'BishalAgroSeed_App',
    responseType: 'code',
    scope: 'offline_access BishalAgroSeed',
    requireHttps: false
  },
  apis: {
    default: {
      url: 'http://localhost:5001',
      rootNamespace: 'BishalAgroSeed',
    },
  },
} as Environment;
