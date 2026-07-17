"""create schemas

Revision ID: 89cb77ddb0b0
Revises: 
Create Date: 2026-06-28 14:56:52.477760

"""
from typing import Sequence, Union

from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision: str = '89cb77ddb0b0'
down_revision: Union[str, Sequence[str], None] = None
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None


def upgrade() -> None:
    """Upgrade schema."""
    op.execute("""
        CREATE SCHEMA IF NOT EXISTS accounts;

        CREATE SCHEMA IF NOT EXISTS goals;

        CREATE SCHEMA IF NOT EXISTS payment;
    """)
    pass


def downgrade() -> None:
    """Downgrade schema."""
    op.execute("""
        DROP SCHEMA IF EXISTS accounts;

        DROP SCHEMA IF EXISTS goals;

        DROP SCHEMA IF EXISTS payment;
    """)
    pass
