import { check, group } from 'k6';
import { post } from '../../lib/client.js';

export const options = {
  vus: 1,
  iterations: 1,
  thresholds: {
    checks: ['rate==1.0'],
  },
};

const ENDPOINT = '/api/v1/brokerages';

export default function () {
  group('POST /api/v1/brokerages — happy path', () => {
    const res = post(ENDPOINT, {
      brokerageName: 'Corretora Teste',
      contributorsCount: 3,
      secret: 'super-secret',
    });

    check(res, {
      'status is 201': (r) => r.status === 201,
      'body has brokerageIdentifier': (r) => r.json('brokerageIdentifier') !== null,
      'body has brokerageName': (r) => r.json('brokerageName') === 'Corretora Teste',
      'Location header is set': (r) => r.headers['Location'] !== undefined,
    });
  });

  group('POST /api/v1/brokerages — empty name returns 400', () => {
    const res = post(ENDPOINT, {
      brokerageName: '',
      contributorsCount: 1,
      secret: 'super-secret',
    });

    check(res, {
      'status is 400': (r) => r.status === 400,
    });
  });

  group('POST /api/v1/brokerages — missing secret returns 400', () => {
    const res = post(ENDPOINT, {
      brokerageName: 'Corretora Sem Secret',
      contributorsCount: 1,
    });

    check(res, {
      'status is 400': (r) => r.status === 400,
    });
  });

  group('POST /api/v1/brokerages — negative contributorsCount returns 400', () => {
    const res = post(ENDPOINT, {
      brokerageName: 'Corretora Inválida',
      contributorsCount: -1,
      secret: 'super-secret',
    });

    check(res, {
      'status is 400': (r) => r.status === 400,
    });
  });
}
