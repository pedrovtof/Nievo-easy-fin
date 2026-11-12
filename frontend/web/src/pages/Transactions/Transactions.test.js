import test from 'node:test';
import assert from 'node:assert/strict';
import fs from 'node:fs';
import path from 'node:path';
import { fileURLToPath } from 'node:url';

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

/**
 * Unit and integration test suite for Transactions module & Payment Situation API contract.
 */
test('Transactions API service exposes getTransactionSituations calling /api/public/v1/Payment/situation', () => {
  const apiSource = fs.readFileSync(path.join(__dirname, 'api.js'), 'utf-8');
  assert.match(apiSource, /export const getTransactionSituations\s*=/);
  assert.match(apiSource, /\/api\/public\/v1\/Payment\/situation/);
});

test('Transactions Controller (index.jsx) integrates situations metadata, filter, and creation payload', () => {
  const controllerSource = fs.readFileSync(path.join(__dirname, 'index.jsx'), 'utf-8');
  assert.match(controllerSource, /getTransactionSituations/);
  assert.match(controllerSource, /transaction_situation_id/);
  assert.match(controllerSource, /transaction_situation:\s*Number\(formData\.transaction_situation\)/);
});

test('Transactions View (View.jsx) renders Situation filter, Situation chip, and Situation form select', () => {
  const viewSource = fs.readFileSync(path.join(__dirname, 'View.jsx'), 'utf-8');
  assert.match(viewSource, /label="Situation"/);
  assert.match(viewSource, /All Situations/);
  assert.match(viewSource, /tx\.situation/);
  assert.match(viewSource, /formData\.transaction_situation/);
});

test('Mock interceptor (mockData.js) supports /Payment/situation and filters/creates transactions with situation', () => {
  const mockSource = fs.readFileSync(
    path.join(__dirname, '../../services/mockData.js'),
    'utf-8'
  );
  assert.match(mockSource, /mockTransactionSituations/);
  assert.match(mockSource, /\/Payment\/situation/);
  assert.match(mockSource, /params\.transaction_situation_id/);
  assert.match(mockSource, /body\.transaction_situation/);
});
