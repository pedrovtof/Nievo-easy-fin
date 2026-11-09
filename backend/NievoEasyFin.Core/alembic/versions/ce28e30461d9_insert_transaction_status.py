"""insert transaction status

Revision ID: ce28e30461d9
Revises: a98361d60409
Create Date: 2026-10-12 15:27:22.616296

"""
from typing import Sequence, Union

from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision: str = 'ce28e30461d9'
down_revision: Union[str, Sequence[str], None] = 'a98361d60409'
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None


def upgrade() -> None:
    """Upgrade schema."""
    op.execute("""
        INSERT INTO payment.transaction_status (name, description, active, created_at, updated_at)
        VALUES 
            ('Efetivado', 'Transação concluída e debitada/creditada com sucesso', true, NOW(), NOW()),
            ('Pendente', 'Transação aguardando processamento ou confirmação', true, NOW(), NOW()),
            ('Agendado', 'Transação programada para acontecer em uma data futura', true, NOW(), NOW()),
            ('Atrasado', 'Transação que passou da data de vencimento e não foi liquidada', true, NOW(), NOW()),
            ('Cancelado', 'Transação cancelada pelo usuário ou sistema antes da conclusão', true, NOW(), NOW()),
            ('Estornado', 'Transação revertida ou dinheiro devolvido após ter sido efetivada', true, NOW(), NOW());
    """)
    pass


def downgrade() -> None:
    """Downgrade schema."""
    op.execute("""
        DELETE FROM payment.transaction a WHERE EXISTS(
            SELECT 1  
            FROM payment.transaction_status b
            WHERE a.transaction_status_id = b.id
                AND b.name IN (
                    'Efetivado',
                    'Pendente',
                    'Agendado',
                    'Atrasado',
                    'Cancelado',
                    'Estornado'
                )
        );

        DELETE FROM payment.transaction_status
        WHERE name IN (
            'Efetivado',
            'Pendente',
            'Agendado',
            'Atrasado',
            'Cancelado',
            'Estornado'
        );
    """)
    pass
