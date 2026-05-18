# Project Memory - Nievo Easy Fin (Auth)

## Maintenance Log
- **2026-05-17:**
    - Fixed `env.py` to correctly use `PGSQL_DATABASE_AUTH_MIGRATION_CONNECTION_STRING`.
    - Corrected prefix to `sqlalchemy.` in `env.py`.
    - Installed `psycopg2-binary` (missing dependency).
    - Updated `requirements.txt`.
