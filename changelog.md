# Changelog

All notable changes to this project will be documented in this file.

### Changelog

2026-11-12 - [REFACTOR] - Remove obsolete TransactionIngress property from PostPaymentTransactionRequest (#65)

Detalhes:

- Remove unused TransactionIngress (income) boolean property from PostPaymentTransactionRequest in favor of TransactionSituation - pedrovotf - 2026-11-12 - #65 - PENDING

2026-11-12 - [TEST] - Add outflow amount negation rule, update payment validator thresholds, tests, and docs (#65)

Detalhes:

- Add EnumTransactionSituation and negate transaction amount on Saida in PaymentService.PostPaymentTransaction, adjust PostPaymentTransactionValidatorAsync minimum bounds, update and add unit/integration tests in PostPaymentTransactionAsyncTest, and update MkDocs documentation and Mermaid diagrams - pedrovotf - 2026-11-12 - #65 - 1da3629

2026-11-12 - [NEW] - Add transaction situation (Entrada/Saida) to Payment API, database, and frontend Transactions page (#65)

Detalhes:

- Add payment.transaction_situation table and fk_payment_transaction_situation migration, implement GET /api/public/v1/Payment/situation endpoint and integrate transaction_situation into POST/GET /api/public/v1/Payment, update frontend Transactions page and mockData under The Silent Navigator design system, add backend and frontend unit/integration tests, and update MkDocs documentation and diagrams - pedrovotf - 2026-11-12 - #65 - 642c103

2026-11-12 - [FIX] - Validate duplicate user bank card (card_id + user_id) in PostUserCard (#55)

Detalhes:

- Add pre-creation uniqueness check via GetUserBankCardByCardIdAndUserId in CoreUserService.PostUserCard to prevent idx_unic_user_bank_card constraint exception, add POSTUSERCARDASYNC_CORESERVICE_400_USER_CARD_ALREADY_EXISTS in EnumErrosApi, expand unit and integration tests in PostUserCardAsyncTest and UserServiceDirectTest, and update MkDocs documentation and diagrams - pedrovotf - 2026-11-12 - #55 - PENDING

2026-11-11 - [CHG] - Split frontend API clients into VITE_BASE_AUTH_URL and VITE_BASE_CORE_URL

Detalhes:

- Configure separate Axios instances (authApi on port 5090 and coreApi on port 5203) in frontend/web/src/services/api.js and update .env-example and module API services - pedrovotf - 2026-11-11 - N/A - PENDING

2026-11-11 - [NEW] - Integrate Core API endpoints into web frontend and add Sidebar modules

Detalhes:

- Create Banks, Cards, and Categories pages, update Transactions and Budget pages to consume Core v1 REST endpoints, expand Sidebar navigation, and implement 100% in-memory Core API interceptors in mockData.js - pedrovotf - 2026-11-11 - N/A - 42d9786

2026-11-11 - [BUILD] - Add Nievo-AI submodule and configure agent skills

Detalhes:

- Integrate Nievo-Skills repository as a submodule under Nievo-AI, configure GEMINI.md directives, and link AI rules and agent skills - pedrovotf - 2026-11-11 - N/A - PENDING
