"""create goals constrains

Revision ID: 7159b0d736ad
Revises: 2aaf6a5ab21b
Create Date: 2026-09-17 08:51:47.874207

"""
from typing import Sequence, Union

from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision: str = '7159b0d736ad'
down_revision: Union[str, Sequence[str], None] = '2aaf6a5ab21b'
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None


def upgrade() -> None:
    """Upgrade schema."""
    op.execute("""
        DO $$
            BEGIN
            IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_goals_category') THEN
                ALTER TABLE goals.category
                ADD CONSTRAINT fk_goals_category FOREIGN KEY (goals_id) REFERENCES goals.goals(id);
            END IF;
        END $$;

        GRANT SELECT, USAGE ON ALL SEQUENCES IN SCHEMA goals TO app_core_service_efn;

        CREATE INDEX IF NOT EXISTS idx_goals_category_parent ON  goals.category (parrent_category);

        CREATE INDEX IF NOT EXISTS idx_goals_category_active ON  goals.category (active);

        CREATE INDEX IF NOT EXISTS idx_goals_category_user ON  goals.category (user_id);

        CREATE INDEX IF NOT EXISTS idx_goals_user ON  goals.goals (user_id);

        CREATE INDEX IF NOT EXISTS idx_goals_active ON  goals.goals (active);

        CREATE INDEX IF NOT EXISTS idx_goals_expire_at ON  goals.goals (expire_at);
    """)
    pass


def downgrade() -> None:
    """Downgrade schema."""
    op.execute("""
        DO $$
            BEGIN
            IF EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_goals_category') THEN
                ALTER TABLE goals.category
                DROP CONSTRAINT fk_goals_category;
            END IF;
        END $$;

        REVOKE SELECT, USAGE ON ALL SEQUENCES IN SCHEMA goals FROM app_core_service_efn;
    """)
    pass
