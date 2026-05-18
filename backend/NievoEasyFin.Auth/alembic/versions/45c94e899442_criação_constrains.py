"""criação constrains

Revision ID: 45c94e899442
Revises: 93d5d8c8c3ac
Create Date: 2026-05-17 14:36:55.688483

"""
from typing import Sequence, Union

from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision: str = '45c94e899442'
down_revision: Union[str, Sequence[str], None] = '93d5d8c8c3ac'
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None


def upgrade() -> None:
    """Upgrade schema."""
    op.execute("""
        DO $$
            BEGIN
            IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_user_provider_sso_provider_id') THEN
                ALTER TABLE journey.user_provider_sso
                ADD CONSTRAINT fk_user_provider_sso_provider_id FOREIGN KEY (sso_provider_id) REFERENCES journey.sso_provider(id);
            END IF;


            IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_user_provider_sso_user_id') THEN
                ALTER TABLE journey.user_provider_sso
                ADD CONSTRAINT fk_user_provider_sso_user_id FOREIGN KEY (user_id) REFERENCES user_details."user"(id);
            END IF;


            IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_user_status_id') THEN
                ALTER TABLE user_details.user 
                ADD CONSTRAINT fk_user_status_id FOREIGN KEY (status_id) REFERENCES user_details.user_status (id);
            END IF;
        END $$;
    """)
    pass


def downgrade() -> None:
    """Downgrade schema."""
    op.execute("""
        DO $$
            BEGIN
            IF EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_user_provider_sso_provider_id') THEN
                ALTER TABLE journey.user_provider_sso
                DROP CONSTRAINT fk_user_provider_sso_provider_id;
            END IF;

            IF EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_user_provider_sso_user_id') THEN
                ALTER TABLE journey.user_provider_sso
                DROP CONSTRAINT fk_user_provider_sso_user_id;
            END IF;

            IF EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_user_status_id') THEN
                ALTER TABLE user_details.user 
                DROP CONSTRAINT fk_user_status_id;
            END IF;
        END $$;
    """)
    pass
