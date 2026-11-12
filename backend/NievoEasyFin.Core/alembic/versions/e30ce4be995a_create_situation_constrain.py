"""Create situation constrain

Revision ID: e30ce4be995a
Revises: a3b8cb1f9b53
Create Date: 2026-11-12 14:18:21.179797

"""
from typing import Sequence, Union

from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision: str = 'e30ce4be995a'
down_revision: Union[str, Sequence[str], None] = 'a3b8cb1f9b53'
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None


def upgrade() -> None:
    """Upgrade schema."""
    op.execute("""

        DO $$
            BEGIN
            IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_payment_transaction_situation') THEN

                ALTER TABLE payment.transaction add column transaction_situation_id int;
            
                ALTER TABLE payment.transaction
                ADD CONSTRAINT fk_payment_transaction_situation FOREIGN KEY (transaction_situation_id) REFERENCES payment.transaction_situation(id);
            END IF;
        END $$;


    """)
    pass


def downgrade() -> None:
    """Downgrade schema."""
    op.execute("""
        DO $$
            BEGIN
            IF EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_payment_transaction_situation') THEN
                ALTER TABLE payment."transaction" DROP CONSTRAINT fk_payment_transaction_situation;

                ALTER TABLE payment."transaction" DROP COLUMN transaction_situation_id;
            END IF;
        END $$;
    """)
    pass
