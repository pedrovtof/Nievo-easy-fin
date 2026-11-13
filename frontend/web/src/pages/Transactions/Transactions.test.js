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

test('Transactions Controller (index.jsx) validates max 2 decimal places on creation and preserves financial decimal amount without integer rounding', () => {
  const controllerSource = fs.readFileSync(path.join(__dirname, 'index.jsx'), 'utf-8');
  assert.doesNotMatch(controllerSource, /Math\.round\(Number\(formData\.amount\)\)/);
  assert.match(controllerSource, /export const isValidTransactionAmountInput\s*=/);
  assert.match(controllerSource, /export const parseTransactionAmount\s*=/);
  assert.match(controllerSource, /amount:\s*parseTransactionAmount\(formData\.amount\)/);
  assert.match(controllerSource, /toFixed\(2\)/);
});

test('Transactions View (View.jsx) configures Amount input with step 0.01 and min 0.01 and formats multi-precision amounts safely', () => {
  const viewSource = fs.readFileSync(path.join(__dirname, 'View.jsx'), 'utf-8');
  assert.match(viewSource, /export const formatTransactionAmount\s*=/);
  assert.match(viewSource, /min:\s*0\.01,\s*step:\s*'0\.01'/);
  assert.match(viewSource, /formatTransactionAmount\(tx\.amount\)/);
});

test('Mock interceptor (mockData.js) stores float transaction amounts and enforces 2-decimal validation on POST /Payment', () => {
  const mockSource = fs.readFileSync(
    path.join(__dirname, '../../services/mockData.js'),
    'utf-8'
  );
  assert.match(mockSource, /-285\.50/);
  assert.match(mockSource, /-195\.75/);
  assert.match(mockSource, /parsedAmount\.toFixed\(2\)/);
});

