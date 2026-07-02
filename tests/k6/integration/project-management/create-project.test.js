import { check, group } from 'k6';
import { post } from '../../lib/client.js';

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
    brokerageName: 'Corretora Project Teste',
    cnpj: '12345678000195',
    email: 'contato@corretoraprojectteste.com.br',
    phone: '11999990000',
    ...overrides,
  });
}

export default function () {
  group('POST /api/v1/brokerages/{id}/projects — happy path', () => {
    const created = createBrokerage();
    const brokerageId = created.json('brokerageIdentifier');

    const res = post(`${BROKERAGES_ENDPOINT}/${brokerageId}/projects`, {
      projectName: 'Projeto Teste',
      description: 'Descrição do projeto de teste',
      starsCount: 0,
      forksCount: 0,
      contributorsCount: 1,
    });

    check(res, {
      'status is 200': (r) => r.status === 200,
    });
  });

  group('POST /api/v1/brokerages/{id}/projects — non-existent brokerage returns 404', () => {
    const res = post(`${BROKERAGES_ENDPOINT}/00000000-0000-0000-0000-000000000000/projects`, {
      projectName: 'Projeto Órfão',
      description: 'Não deveria ser criado',
      starsCount: 0,
      forksCount: 0,
      contributorsCount: 1,
    });

    check(res, {
      'status is 404': (r) => r.status === 404,
    });
  });
}
