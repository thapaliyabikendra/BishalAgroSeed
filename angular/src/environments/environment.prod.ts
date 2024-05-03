import { Environment } from '@abp/ng.core';

const baseUrl = 'http://www.bishalagroseed.com';

export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'BishalAgroSeed',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'http://www.bishalagroseed.com/sso',
    redirectUri: baseUrl,
    clientId: 'BishalAgroSeed_App',
    responseType: 'code',
    scope: 'offline_access BishalAgroSeed',
    requireHttps: false
  },
  apis: {
    default: {
      url: 'http://www.bishalagroseed.com/api/api',
      rootNamespace: 'BishalAgroSeed',
    },
  },
} as Environment;
