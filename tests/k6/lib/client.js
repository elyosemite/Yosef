import http from 'k6/http';

export const BASE_URL = __ENV.BASE_URL || 'http://localhost';

export const JSON_HEADERS = {
  'Content-Type': 'application/json',
  'Accept': 'application/json',
};

export function post(path, body) {
  return http.post(`${BASE_URL}${path}`, JSON.stringify(body), { headers: JSON_HEADERS });
}

export function get(path) {
  return http.get(`${BASE_URL}${path}`, { headers: JSON_HEADERS });
}

export function patch(path, body) {
  return http.patch(`${BASE_URL}${path}`, JSON.stringify(body), { headers: JSON_HEADERS });
}
