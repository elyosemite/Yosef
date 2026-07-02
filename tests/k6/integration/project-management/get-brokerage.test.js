import { check, group } from 'k6';
import { get, post } from '../../lib/client.js';

export const options = {
  vus: 1,
  iterations: 1,
  thresholds: {
    checks: ['rate==1.0'],
  },
};

const BROKERAGES_ENDPOINT = '/api/project-management/api/v1/brokerages';

function createBrokerage(overrides = {}) {
  return post(BROKERAGES_ENDPOINT, {
    brokerageName: 'Corretora Get Teste',
    cnpj: '12345678000195',
    email: 'contato@corretoragetteste.com.br',
    phone: '11999990000',
    ...overrides,
  });
}

export default function () {
  group('GET /api/v1/brokerages/{id} — happy path', () => {
    const created = createBrokerage();
    const id = created.json('brokerageIdentifier');

    const res = get(`${BROKERAGES_ENDPOINT}/${id}`);

    check(res, {
      'status is 200': (r) => r.status === 200,
      'body has matching identifier': (r) => r.json('identifier') === id,
      'body has brokerageName': (r) => r.json('brokerageName') === 'Corretora Get Teste',
      'body has cnpj': (r) => r.json('cnpj') === '12345678000195',
      'body has email': (r) => r.json('email') === 'contato@corretoragetteste.com.br',
    });
  });

  group('GET /api/v1/brokerages/{id} — non-existent id returns 404', () => {
    const res = get(`${BROKERAGES_ENDPOINT}/00000000-0000-0000-0000-000000000000`);

    check(res, {
      'status is 404': (r) => r.status === 404,
    });
  });
}
