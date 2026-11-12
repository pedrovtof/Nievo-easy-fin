"""Create situation table for transaction

Revision ID: a3b8cb1f9b53
Revises: 130900e3dce0
Create Date: 2026-11-12 14:15:10.521140

"""
from typing import Sequence, Union

from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision: str = 'a3b8cb1f9b53'
down_revision: Union[str, Sequence[str], None] = '130900e3dce0'
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None


def upgrade() -> None:
    """Upgrade schema."""
    op.execute("""
            CREATE TABLE IF NOT EXISTS payment.transaction_situation (
                id serial PRIMARY KEY,
                name VARCHAR(150),
                description VARCHAR(255),
                active  boolean DEFAULT true NOT NULL,
                created_at TIMESTAMP without time zone DEFAULT now() NOT NULL,
                updated_at TIMESTAMP without time zone
            );

            GRANT SELECT,INSERT,UPDATE ON TABLE payment."transaction_situation" TO app_core_service_efn;
    """)
    pass


def downgrade() -> None:
    """Downgrade schema."""
    op.execute("""
        DROP TABLE IF EXISTS "payment"."transaction_situation" CASCADE;
    """)
    pass
