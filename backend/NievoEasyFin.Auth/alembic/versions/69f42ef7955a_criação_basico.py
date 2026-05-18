"""criação basico

Revision ID: 69f42ef7955a
Revises: 
Create Date: 2026-05-17 13:35:01.564390

"""
from typing import Sequence, Union

from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision: str = '69f42ef7955a'
down_revision: Union[str, Sequence[str], None] = None
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None


def upgrade() -> None:
    """Upgrade schema."""
    op.execute("""
        CREATE SCHEMA IF NOT EXISTS user_details;

        CREATE SCHEMA IF NOT EXISTS journey;
    """)
    pass


def downgrade() -> None:
    """Downgrade schema."""
    op.execute("""
       DROP SCHEMA IF EXISTS user_details;

       DROP SCHEMA IF EXISTS journey;
    """)
    pass
