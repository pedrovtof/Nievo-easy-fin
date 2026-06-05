"""Create accept_terms table

Revision ID: e988be4b9630
Revises: 6edc3ae76319
Create Date: 2026-06-04 13:29:21.494327

"""
from typing import Sequence, Union

from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision: str = 'e988be4b9630'
down_revision: Union[str, Sequence[str], None] = '6edc3ae76319'
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None


def upgrade() -> None:
    """Upgrade schema."""
    op.execute("""
        CREATE TABLE IF NOT EXISTS journey.accept_terms(
            id SERIAL PRIMARY KEY,
            code  VARCHAR(50) NOT NULL,
            name VARCHAR(150),
            description VARCHAR(250),
            version INT,
            content TEXT,
            created_at timestamp without time zone DEFAULT now() NOT NULL,
            updated_at timestamp without time zone,
            active boolean DEFAULT true NOT NULL
        );

        GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE journey.accept_terms TO app_signup_service_efn;

        GRANT SELECT,USAGE ON SEQUENCE journey.accept_terms_id_seq TO app_signup_service_efn;

        CREATE TABLE IF NOT EXISTS journey.users_accepted_terms(
            id serial primary key,
            user_id int,
            accept_id int,
            accepted boolean,
            request_details json,
            created_at timestamp without time zone DEFAULT now() NOT NULL,
            updated_at timestamp without time zone
        );

        GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE journey.users_accepted_terms TO app_signup_service_efn;

        GRANT SELECT,USAGE ON SEQUENCE journey.users_accepted_terms_id_seq TO app_signup_service_efn;

        CREATE UNIQUE INDEX IF NOT EXISTS idx_accept_terms_code_version ON journey.accept_terms (code, version);

        CREATE INDEX IF NOT EXISTS idx_accept_terms_code ON journey.accept_terms (code);

        CREATE INDEX IF NOT EXISTS idx_accept_terms_active ON journey.accept_terms (active);
    """)
    pass


def downgrade() -> None:
    """Downgrade schema."""

    op.execute("""
        DROP TABLE IF EXISTS journey.users_accepted_terms;

        DROP TABLE IF EXISTS journey.accept_terms;
    """)
    pass
