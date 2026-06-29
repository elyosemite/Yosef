import { check, group } from 'k6';
import { post } from '../../lib/client.js';

export const options = {
  vus: 1,
  iterations: 1,
  thresholds: {
    checks: ['rate==1.0'],
  },
};

const ENDPOINT = '/api/project-management/api/v1/brokerages';

export default function () {
  group('POST /api/v1/brokerages — happy path', () => {
    const res = post(ENDPOINT, {
      brokerageName: 'Corretora Teste',
      cnpj: '12345678000195',
      email: 'contato@corretorateste.com.br',
      phone: '11999990000',
    });

    check(res, {
      'status is 201': (r) => r.status === 201,
      'body has brokerageIdentifier': (r) => r.json('brokerageIdentifier') !== null,
      'body has brokerageName': (r) => r.json('brokerageName') === 'Corretora Teste',
      'Location header is set': (r) => r.headers['Location'] !== undefined,
    });
  });

  group('POST /api/v1/brokerages — without optional phone', () => {
    const res = post(ENDPOINT, {
      brokerageName: 'Corretora Sem Telefone',
      cnpj: '98765432000111',
      email: 'contato@semtelefone.com.br',
    });

    check(res, {
      'status is 201': (r) => r.status === 201,
    });
  });

  group('POST /api/v1/brokerages — empty name returns 400', () => {
    const res = post(ENDPOINT, {
      brokerageName: '',
      cnpj: '12345678000195',
      email: 'contato@corretorateste.com.br',
    });

    check(res, {
      'status is 400': (r) => r.status === 400,
    });
  });

  group('POST /api/v1/brokerages — missing CNPJ returns 400', () => {
    const res = post(ENDPOINT, {
      brokerageName: 'Corretora Sem CNPJ',
      email: 'contato@corretorateste.com.br',
    });

    check(res, {
      'status is 400': (r) => r.status === 400,
    });
  });

  group('POST /api/v1/brokerages — invalid email returns 400', () => {
    const res = post(ENDPOINT, {
      brokerageName: 'Corretora Email Inválido',
      cnpj: '12345678000195',
      email: 'not-an-email',
    });

    check(res, {
      'status is 400': (r) => r.status === 400,
    });
  });

  group('POST /api/v1/brokerages — CNPJ with wrong length returns 400', () => {
    const res = post(ENDPOINT, {
      brokerageName: 'Corretora CNPJ Inválido',
      cnpj: '123',
      email: 'contato@corretorateste.com.br',
    });

    check(res, {
      'status is 400': (r) => r.status === 400,
    });
  });
}
