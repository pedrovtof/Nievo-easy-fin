# Changelog

All notable changes to this project will be documented in this file.

### Changelog

2026-11-11 - [CHG] - Split frontend API clients into VITE_BASE_AUTH_URL and VITE_BASE_CORE_URL

Detalhes:

- Configure separate Axios instances (authApi on port 5090 and coreApi on port 5203) in frontend/web/src/services/api.js and update .env-example and module API services - pedrovotf - 2026-11-11 - N/A - PENDING

2026-11-11 - [NEW] - Integrate Core API endpoints into web frontend and add Sidebar modules

Detalhes:

- Create Banks, Cards, and Categories pages, update Transactions and Budget pages to consume Core v1 REST endpoints, expand Sidebar navigation, and implement 100% in-memory Core API interceptors in mockData.js - pedrovotf - 2026-11-11 - N/A - 42d9786

2026-11-11 - [BUILD] - Add Nievo-AI submodule and configure agent skills

Detalhes:

- Integrate Nievo-Skills repository as a submodule under Nievo-AI, configure GEMINI.md directives, and link AI rules and agent skills - pedrovotf - 2026-11-11 - N/A - PENDING
