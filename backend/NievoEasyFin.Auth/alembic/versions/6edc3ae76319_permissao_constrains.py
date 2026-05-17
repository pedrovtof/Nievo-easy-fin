"""permissao constrains

Revision ID: 6edc3ae76319
Revises: cfc9672516f6
Create Date: 2026-05-17 14:54:32.269645

"""
from typing import Sequence, Union

from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision: str = '6edc3ae76319'
down_revision: Union[str, Sequence[str], None] = 'cfc9672516f6'
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None


def upgrade() -> None:
    """Upgrade schema."""
    op.execute("""
        GRANT SELECT, USAGE ON ALL SEQUENCES IN SCHEMA user_details TO app_signup_service_efn;
               
        GRANT SELECT, USAGE ON ALL SEQUENCES IN SCHEMA journey TO app_signup_service_efn;
    """)
    pass


def downgrade() -> None:
    """Downgrade schema."""
    op.execute("""
        REVOKE SELECT, USAGE ON ALL SEQUENCES IN SCHEMA user_details FROM app_signup_service_efn;
               
        REVOKE SELECT, USAGE ON ALL SEQUENCES IN SCHEMA journey FROM app_signup_service_efn;
    """)
    pass
