import { check, group } from 'k6';
import { patch, post } from '../../lib/client.js';

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
    brokerageName: 'Corretora Rename Teste',
    cnpj: '12345678000195',
    email: 'contato@corretorarenameteste.com.br',
    phone: '11999990000',
    ...overrides,
  });
}

export default function () {
  group('PATCH /api/v1/brokerages/{id}/name — happy path', () => {
    const created = createBrokerage();
    const id = created.json('brokerageIdentifier');

    const res = patch(`${BROKERAGES_ENDPOINT}/${id}/name`, {
      brokerageName: 'Corretora Renomeada',
    });

    check(res, {
      'status is 200': (r) => r.status === 200,
      'body has renamed brokerageName': (r) => r.json('brokerageName') === 'Corretora Renomeada',
    });
  });

  group('PATCH /api/v1/brokerages/{id}/name — empty name returns 400', () => {
    const created = createBrokerage();
    const id = created.json('brokerageIdentifier');

    const res = patch(`${BROKERAGES_ENDPOINT}/${id}/name`, {
      brokerageName: '',
    });

    check(res, {
      'status is 400': (r) => r.status === 400,
    });
  });

  group('PATCH /api/v1/brokerages/{id}/name — non-existent id returns 404', () => {
    const res = patch(`${BROKERAGES_ENDPOINT}/00000000-0000-0000-0000-000000000000/name`, {
      brokerageName: 'Não Importa',
    });

    check(res, {
      'status is 404': (r) => r.status === 404,
    });
  });
}
