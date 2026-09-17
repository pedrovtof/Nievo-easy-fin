"""create goals tables

Revision ID: 2aaf6a5ab21b
Revises: ec155a6d740b
Create Date: 2026-09-17 07:39:40.244723

"""
from typing import Sequence, Union

from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision: str = '2aaf6a5ab21b'
down_revision: Union[str, Sequence[str], None] = 'ec155a6d740b'
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None


def upgrade() -> None:
    """Upgrade schema."""
    op.execute("""
        CREATE TABLE IF NOT EXISTS goals.goals (
            id SERIAL  PRIMARY KEY,
            name VARCHAR(150),
            description VARCHAR(255),
            active bool DEFAULT true NOT NULL,
            user_id INT,
            amount INT,
            is_percent INT,
            expire_at DATE,
            created_at TIMESTAMP without time zone DEFAULT now() NOT NULL,
            updated_at TIMESTAMP without time zone
        );

        CREATE TABLE IF NOT EXISTS goals.category (
            id SERIAL  PRIMARY KEY,
            name VARCHAR(150),
            description VARCHAR(255),
            active bool DEFAULT true NOT NULL,
            user_id INT,
            goals_id INT,
            parrent_category INT,
            created_at TIMESTAMP without time zone DEFAULT now() NOT NULL,
            updated_at TIMESTAMP without time zone
        );

        GRANT USAGE ON SCHEMA goals TO cross_database_user;

        GRANT USAGE ON SCHEMA goals TO app_core_service_efn;

        GRANT SELECT,INSERT,UPDATE ON TABLE goals."category" TO app_core_service_efn;

        GRANT SELECT,INSERT,UPDATE ON TABLE goals."goals" TO app_core_service_efn;
    """)
    pass


def downgrade() -> None:
    """Downgrade schema."""
    op.execute("""
        DROP TABLE IF EXISTS "goals"."category" CASCADE;

        DROP TABLE IF EXISTS "goals"."goals" CASCADE;
    """)
    pass
