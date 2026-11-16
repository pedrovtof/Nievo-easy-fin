import test from 'node:test';
import assert from 'node:assert/strict';
import fs from 'node:fs';
import path from 'node:path';
import { fileURLToPath } from 'node:url';

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

/**
 * Unit and integration test suite for User Details endpoint (/api/public/v1/User/details)
 * across Settings page, Header navigation, and mockData interceptor.
 */
test('Settings API service exposes getUserDetails calling GET /api/public/v1/User/details via coreApi', () => {
  const apiSource = fs.readFileSync(path.join(__dirname, 'api.js'), 'utf-8');
  assert.match(apiSource, /import\s*\{[^}]*coreApi[^}]*\}\s*from\s*['"]\.\.\/\.\.\/services\/api['"]/);
  assert.match(apiSource, /export const getUserDetails\s*=/);
  assert.match(apiSource, /coreApi\.get\(['"]\/api\/public\/v1\/User\/details['"]\)/);
});

test('Settings Controller (index.jsx) fetches getUserDetails and normalizes name, email, phone, and status', () => {
  const controllerSource = fs.readFileSync(path.join(__dirname, 'index.jsx'), 'utf-8');
  assert.match(controllerSource, /getUserDetails/);
  assert.match(controllerSource, /export const normalizeUserDetailsProfile\s*=/);
  assert.match(controllerSource, /fullName:\s*raw\.name\s*\|\|\s*raw\.fullName/);
  assert.match(controllerSource, /phone:\s*raw\.phone/);
  assert.match(controllerSource, /status:\s*raw\.status\s*\|\|\s*['"]ACTIVE['"]/);
});

test('Settings View (View.jsx) displays user details (name, email, phone, status) and complies with The Silent Navigator', () => {
  const viewSource = fs.readFileSync(path.join(__dirname, 'View.jsx'), 'utf-8');
  assert.match(viewSource, /export const formatUserPhone\s*=/);
  assert.match(viewSource, /ProfileHeroCard/);
  assert.match(viewSource, /User Details/);
  assert.match(viewSource, /Account Status/);
  assert.match(viewSource, /Registered Phone/);
  assert.match(viewSource, /formatUserPhone\(profile\.phone\)/);
  assert.match(viewSource, /label="Phone Number"/);
  assert.match(viewSource, /label="Status"/);

  const stylesSource = fs.readFileSync(path.join(__dirname, 'styles.js'), 'utf-8');
  assert.doesNotMatch(stylesSource, /1px solid/);
  assert.match(stylesSource, /ProfileHeroCard/);
});

test('Header API and Controller integrate GET /api/public/v1/User/details to display authenticated user name and status', () => {
  const headerApiSource = fs.readFileSync(
    path.join(__dirname, '../../components/Header/api.js'),
    'utf-8'
  );
  assert.match(headerApiSource, /export const getUserDetails\s*=/);
  assert.match(headerApiSource, /\/api\/public\/v1\/User\/details/);

  const headerControllerSource = fs.readFileSync(
    path.join(__dirname, '../../components/Header/index.jsx'),
    'utf-8'
  );
  assert.match(headerControllerSource, /getUserDetails/);
  assert.match(headerControllerSource, /export const extractHeaderUserDetails\s*=/);
  assert.match(headerControllerSource, /userStatus/);

  const headerViewSource = fs.readFileSync(
    path.join(__dirname, '../../components/Header/View.jsx'),
    'utf-8'
  );
  assert.match(headerViewSource, /userStatus/);
});

test('Mock interceptor (mockData.js) supports GET /User/details returning email, name, phone, and status', () => {
  const mockSource = fs.readFileSync(
    path.join(__dirname, '../../services/mockData.js'),
    'utf-8'
  );
  assert.match(mockSource, /\/User\/details/);
  assert.match(mockSource, /email:\s*currentUser\.email/);
  assert.match(mockSource, /name:\s*currentUser\.name/);
  assert.match(mockSource, /phone:\s*currentUser\.phone\s*\?\?\s*null/);
  assert.match(mockSource, /status:\s*currentUser\.status\s*\|\|\s*['"]ACTIVE['"]/);
});
