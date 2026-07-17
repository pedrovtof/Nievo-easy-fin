"""create bank type accounts

Revision ID: 261ef0065a37
Revises: b1de12c452e5
Create Date: 2026-06-28 17:43:46.224776

"""
from typing import Sequence, Union

from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision: str = '261ef0065a37'
down_revision: Union[str, Sequence[str], None] = 'b1de12c452e5'
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None


def upgrade() -> None:
    """Upgrade schema."""
    op.execute("""
        INSERT INTO accounts.bank_type ("name", description, active, created_at, updated_at)
        VALUES
        ('Tradicional', 'Bancos com agências físicas e varejo', true, now(), now()),
        ('Digital', 'Bancos digitais e fintechs', true, now(), now()),
        ('Corretora', 'Instituições focadas em investimentos', true, now(), now()),
        ('Carteira Digital', 'Aplicativos de pagamento rápido e wallets', true, now(), now()),
        ('Conta Global', 'Contas em moeda estrangeira e câmbio', true, now(), now()),
        ('Benefícios', 'Vales e benefícios corporativos', true, now(), now())
        ;
    """)
    pass


def downgrade() -> None:
    """Downgrade schema."""
    op.execute("""
        DELETE FROM accounts.bank_type
        WHERE "name" IN (
            'Tradicional', 
            'Digital', 
            'Corretora', 
            'Carteira Digital', 
            'Conta Global', 
            'Benefícios'
        );
    """)
    pass
