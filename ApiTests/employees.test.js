const axios = require('axios');

const BASE_URL = process.env.API_URL || 'https://localhost:60752';

describe('Employees API', () => {
  test('GET /api/v2/employees returns list', async () => {
    const res = await axios.get(`${BASE_URL}/api/v2/EmployeesApi`, {
      // якщо самопідписаний сертифікат:
      httpsAgent: new (require('https').Agent)({ rejectUnauthorized: false })
    });

    expect(res.status).toBe(200);
    expect(Array.isArray(res.data)).toBe(true);
    // очікуємо, що сидовані дані є
    expect(res.data.length).toBeGreaterThan(0);
  });
});