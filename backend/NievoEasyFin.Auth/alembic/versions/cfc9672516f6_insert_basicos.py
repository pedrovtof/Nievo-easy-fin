"""insert basicos

Revision ID: cfc9672516f6
Revises: 45c94e899442
Create Date: 2026-05-17 14:40:33.650615

"""
from typing import Sequence, Union

from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision: str = 'cfc9672516f6'
down_revision: Union[str, Sequence[str], None] = '45c94e899442'
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None


def upgrade() -> None:
    """Upgrade schema."""
    op.execute("""
        INSERT INTO journey.sso_provider (\"name\",description,created_at,updated_at,active) VALUES
        ('google','Google login sso','2026-04-04 12:24:29.679','2026-04-04 12:24:29.679',true);

        INSERT INTO user_details.user_status (\"name\",description,created_at,updated_at,active) VALUES
        ('Inativo','Usuário inativo','2026-04-14 08:29:50.997',NULL,1),
        ('Ativo','Usuário ativo','2026-03-29 19:04:48.446',NULL,1);
    """)
    pass


def downgrade() -> None:
    """Downgrade schema."""
    op.execute("""
        DELETE FROM journey.user_provider_sso a WHERE EXISTS (
            SELECT 1 FROM journey.sso_provider b
            WHERE a.sso_provider_id = b.id
            LIMIT 1
        );
               
        DELETE FROM journey.sso_provider WHERE \"name\" = 'google';
        
        DELETE FROM user_details.\"user\" a WHERE EXISTS (
            SELECT 1 FROM user_details.user_status b
            WHERE a.status_id = b.id
            LIMIT 1
        );
 
        DELETE FROM user_details.user_status WHERE \"name\" in ('Inativo','Ativo');
    """)
    pass
