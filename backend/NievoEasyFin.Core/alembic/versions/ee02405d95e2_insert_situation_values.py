"""Insert situation values

Revision ID: ee02405d95e2
Revises: e30ce4be995a
Create Date: 2026-11-12 14:31:33.850432

"""
from typing import Sequence, Union

from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision: str = 'ee02405d95e2'
down_revision: Union[str, Sequence[str], None] = 'e30ce4be995a'
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None


def upgrade() -> None:
    """Upgrade schema."""
    op.execute("""
        INSERT INTO payment.transaction_situation ( "name", description, created_at, updated_at)
        VALUES
            ( 'Entrada', 'Entrada de valor', now(), now()),
            ( 'Saida', 'Saida de valor', now(), now());
        """)
    pass


def downgrade() -> None:
    """Downgrade schema."""
    op.execute("""
        DELETE FROM payment.transaction a WHERE EXISTS(
            SELECT 1  
            FROM payment.transaction_situation b
            WHERE a.transaction_situation_id = b.id
                AND name IN (
                'Entrada',
                'Saida'
            )
        );

        DELETE FROM payment.transaction_situation WHERE name in(
            'Entrada',
            'Saida'
        );
    """)
    pass
