"""criação tabela journeys

Revision ID: 93d5d8c8c3ac
Revises: 8ce926c3658f
Create Date: 2026-05-17 14:33:37.690801

"""
from typing import Sequence, Union

from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision: str = '93d5d8c8c3ac'
down_revision: Union[str, Sequence[str], None] = '8ce926c3658f'
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None


def upgrade() -> None:
    """Upgrade schema."""

    op.execute("""
        CREATE TABLE IF NOT EXISTS journey.sso_provider (
            id SERIAL PRIMARY KEY,
            name character varying(100),
            description character varying(250),
            created_at timestamp without time zone DEFAULT now() NOT NULL,
            updated_at timestamp without time zone DEFAULT now(),
            active boolean DEFAULT true NOT NULL
        );

        CREATE TABLE IF NOT EXISTS journey.user_provider_sso (
            id SERIAL PRIMARY KEY,
            sso_provider_id integer,
            user_id integer,
            created_at timestamp without time zone DEFAULT now() NOT NULL,
            updated_at timestamp without time zone DEFAULT now(),
            sub character varying NOT NULL
        );

        GRANT USAGE ON SCHEMA journey TO CROSS_DATABASE_USER;

        GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE journey.sso_provider TO app_signup_service_efn;

        GRANT SELECT,USAGE ON SEQUENCE journey.sso_provider_id_seq TO app_signup_service_efn;

        GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE journey.user_provider_sso TO app_signup_service_efn;

        GRANT SELECT,USAGE ON SEQUENCE journey.user_provider_sso_id_seq TO app_signup_service_efn;

        GRANT USAGE ON SCHEMA journey TO cross_database_user;
               
        GRANT USAGE ON SCHEMA journey TO app_signup_service_efn;
    """)
    pass

def downgrade() -> None:
    """Downgrade schema."""
    op.execute("""
        DROP TABLE IF EXISTS journey."sso_provider" CASCADE;

        DROP TABLE IF EXISTS journey."user_provider_sso" CASCADE;
    """)
    pass
