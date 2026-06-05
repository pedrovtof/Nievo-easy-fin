"""Create constrain accept_terms 

Revision ID: 6848d9de6102
Revises: e988be4b9630
Create Date: 2026-06-04 13:44:49.213684

"""
from typing import Sequence, Union

from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision: str = '6848d9de6102'
down_revision: Union[str, Sequence[str], None] = 'e988be4b9630'
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None


def upgrade() -> None:
    """Upgrade schema."""
    op.execute("""
        DO $$
            BEGIN
            IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_user_accepted_terms_term_id') THEN
                ALTER TABLE journey.users_accepted_terms
                ADD CONSTRAINT fk_user_accepted_terms_term_id FOREIGN KEY (accept_id) REFERENCES journey.accept_terms(id);
            END IF;

            IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_user_accepted_terms_user_id') THEN
                ALTER TABLE journey.users_accepted_terms
                ADD CONSTRAINT fk_user_accepted_terms_user_id FOREIGN KEY (user_id) REFERENCES user_details.user(id);
            END IF;
        END $$;
    """)
    pass


def downgrade() -> None:
    """Downgrade schema."""
    op.execute("""
        DO $$
            BEGIN
            IF EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_user_accepted_terms_term_id') THEN
                ALTER TABLE journey.users_accepted_terms
                DROP CONSTRAINT fk_user_accepted_terms_term_id;
            END IF;

            IF EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_user_accepted_terms_user_id') THEN
                ALTER TABLE journey.users_accepted_terms
                DROP CONSTRAINT fk_user_accepted_terms_user_id;
            END IF;
        END $$;
    """)
    pass
