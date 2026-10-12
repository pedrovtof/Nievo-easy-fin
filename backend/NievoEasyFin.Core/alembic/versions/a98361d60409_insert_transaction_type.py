"""insert transaction type

Revision ID: a98361d60409
Revises: cf4aead7875c
Create Date: 2026-10-12 12:58:02.252287

"""
from typing import Sequence, Union

from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision: str = 'a98361d60409'
down_revision: Union[str, Sequence[str], None] = 'cf4aead7875c'
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None


def upgrade() -> None:
    """Upgrade schema."""
    op.execute("""
        INSERT INTO payment.transaction_type ( name, description, active bool, created_at, updated_at)
        VALUES (
            
        )
    """)
    pass


def downgrade() -> None:
    """Downgrade schema."""
    op.execute("""""")
    pass
