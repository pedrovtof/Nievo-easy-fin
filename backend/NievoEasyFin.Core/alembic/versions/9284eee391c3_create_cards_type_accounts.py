"""create cards type accounts

Revision ID: 9284eee391c3
Revises: 261ef0065a37
Create Date: 2026-06-28 18:05:00.091912

"""
from typing import Sequence, Union

from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision: str = '9284eee391c3'
down_revision: Union[str, Sequence[str], None] = '261ef0065a37'
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None


def upgrade() -> None:
    """Upgrade schema."""
    op.execute("""
        INSERT INTO accounts.bank_card_type ("name", description, active, created_at, updated_at)
        VALUES
        ('Crédito', 'Compras a prazo com fechamento de fatura mensal', true, now(), now()),
        ('Débito', 'Compras com desconto imediato no saldo da conta', true, now(), now()),
        ('Múltiplo', 'Cartão único com as funções de crédito e débito', true, now(), now()),
        ('Pré-pago', 'Necessita de recarga prévia de saldo para uso (ex: mesada, cartões de viagem)', true, now(), now()),
        ('Benefícios', 'Cartões de vale-alimentação, refeição ou flexíveis (ex: VR, Caju, Ticket)', true, now(), now());
    """)
    pass


def downgrade() -> None:
    """Downgrade schema."""
    op.execute("""
        DELETE FROM accounts.bank_card_type
        WHERE "name" IN (
            'Crédito', 
            'Débito', 
            'Múltiplo', 
            'Pré-pago', 
            'Benefícios'
        );
    """)
    pass
