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
        INSERT INTO payment.transaction_type (name, description, active, created_at, updated_at)
        VALUES 
            ('Pix', 'Transferência instantânea via Pix', true, NOW(), NOW()),
            ('Cartão de Crédito', 'Pagamento realizado via cartão de crédito', true, NOW(), NOW()),
            ('Cartão de Débito', 'Pagamento realizado via cartão de débito', true, NOW(), NOW()),
            ('Boleto', 'Pagamento via boleto bancário', true, NOW(), NOW()),
            ('Transferência Bancária', 'Transferência tradicional entre contas (TED/DOC)', true, NOW(), NOW()),
            ('Dinheiro', 'Pagamento realizado em espécie', true, NOW(), NOW()),
            ('Carteira Digital', 'Pagamento utilizando serviços como Apple Pay ou Google Pay', true, NOW(), NOW()),
            ('Vale Benefício', 'Pagamento com vales como alimentação, refeição ou cultura', true, NOW(), NOW()),
            ('Reembolso', 'Devolução de um valor pago em uma transação anterior', true, NOW(), NOW()),
            ('Contestação', 'Estorno forçado por contestação do titular (Chargeback)', true, NOW(), NOW()),
            ('Assinatura', 'Cobrança recorrente de um serviço', true, NOW(), NOW());
    """)
    pass


def downgrade() -> None:
    """Downgrade schema."""
    op.execute("""
        DELETE FROM payment.transaction a WHERE EXISTS(
            SELECT 1  
            FROM payment.transaction_type b
            WHERE a.transaction_type_id = b.id
                AND b.name IN (
                    'Pix',
                    'Cartão de Crédito',
                    'Cartão de Débito',
                    'Boleto',
                    'Transferência Bancária',
                    'Dinheiro',
                    'Carteira Digital',
                    'Vale Benefício',
                    'Reembolso',
                    'Contestação',
                    'Assinatura'
                )
        );

        DELETE FROM payment.transaction_type     
        WHERE  name IN (
            'Pix',
            'Cartão de Crédito',
            'Cartão de Débito',
            'Boleto',
            'Transferência Bancária',
            'Dinheiro',
            'Carteira Digital',
            'Vale Benefício',
            'Reembolso',
            'Contestação',
            'Assinatura'
        );
    """)
    pass
