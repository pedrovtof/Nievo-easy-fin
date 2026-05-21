"""criação tabela user_details

Revision ID: 8ce926c3658f
Revises: 69f42ef7955a
Create Date: 2026-05-17 14:26:02.546208

"""
from typing import Sequence, Union

from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision: str = '8ce926c3658f'
down_revision: Union[str, Sequence[str], None] = '69f42ef7955a'
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None


def upgrade() -> None:
    """Upgrade schema."""
    op.execute("""
        CREATE TABLE IF NOT EXISTS user_details.user (
            id SERIAL PRIMARY KEY,
            name character varying(150),
            email character varying(100),
            phone bigint,
            status_id integer DEFAULT 1 NOT NULL,
            created_at timestamp without time zone DEFAULT now() NOT NULL,
            updated_at timestamp without time zone,
            password text
        );

        CREATE TABLE IF NOT EXISTS user_details.user_status (
            id SERIAL PRIMARY KEY,
            name character varying(150) NOT NULL,
            description character varying(255) NOT NULL,
            created_at timestamp without time zone DEFAULT now() NOT NULL,
            updated_at timestamp without time zone,
            active integer DEFAULT 0 NOT NULL
        );

        CREATE UNIQUE INDEX IF NOT EXISTS idx_unic_user_email ON user_details.user (email);

        CREATE UNIQUE INDEX IF NOT EXISTS idx_unic_user_phone ON user_details.user (phone);

        CREATE UNIQUE INDEX IF NOT EXISTS idx_unic_user_details_name ON user_details.user_status (name);

        CREATE INDEX IF NOT EXISTS idx_user_status ON user_details.user (status_id);

        GRANT SELECT ON user_details.user TO cross_database_user;

        GRANT USAGE ON SCHEMA user_details TO cross_database_user;

        GRANT USAGE ON SCHEMA user_details TO app_signup_service_efn;

        GRANT SELECT ON TABLE user_details."user" TO cross_database_user;

        GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE user_details."user" TO app_signup_service_efn;

        GRANT SELECT ON TABLE user_details.user_status TO app_signup_service_efn;

        GRANT USAGE ON SCHEMA user_details TO cross_database_user;
    """)
    pass


def downgrade() -> None:
    """Downgrade schema."""
    op.execute("""
        DROP TABLE IF EXISTS user_details."user" CASCADE;

        DROP TABLE IF EXISTS user_details."user_status" CASCADE;  

        DROP SCHEMA IF EXISTS user_details;
    """)
    pass
