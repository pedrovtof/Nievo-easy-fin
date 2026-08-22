"""insert flag card registers

Revision ID: ec155a6d740b
Revises: a6416d08e6b3
Create Date: 2026-08-22 08:01:44.734621

"""
from typing import Sequence, Union

from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision: str = 'ec155a6d740b'
down_revision: Union[str, Sequence[str], None] = 'a6416d08e6b3'
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None


def upgrade() -> None:
    """Upgrade schema."""
    op.execute("""
        INSERT INTO accounts.bank_card_flag (name, description, active, created_at, updated_at)
        VALUES 
            ('Visa', 'Bandeira internacional Visa', true, now(), null),
            ('Mastercard', 'Bandeira internacional Mastercard', true, now(), null),
            ('American Express', 'Bandeira internacional American Express (Amex)', true, now(), null),
            ('Elo', 'Bandeira nacional Elo', true, now(), null),
            ('Hipercard', 'Bandeira nacional Hipercard', true, now(), null),
            ('Diners Club', 'Bandeira internacional Diners Club', true, now(), null),
            ('Discover', 'Bandeira internacional Discover', true, now(), null),
            ('JCB', 'Bandeira internacional JCB (Japan Credit Bureau)', true, now(), null),
            ('Aura', 'Bandeira nacional Aura', true, now(), null),
            ('Cabal', 'Bandeira regional Cabal', true, now(), null),
            ('UnionPay', 'Bandeira internacional UnionPay', true, now(), null),
            ('Sorocred', 'Bandeira nacional Sorocred', true, now(), null),
            ('Banricompras', 'Bandeira regional Banricompras', true, now(), null);
    """)
    pass


def downgrade() -> None:
    """Downgrade schema."""
    op.execute("""
            DELETE FROM accounts.bank_card a WHERE EXISTS(
                SELECT 1  
                FROM accounts.bank_card_flag b
                WHERE a.flag_id = b.id
                    AND name IN (
                    'Visa', 
                    'Mastercard', 
                    'American Express', 
                    'Elo', 
                    'Hipercard', 
                    'Diners Club', 
                    'Discover', 
                    'JCB', 
                    'Aura', 
                    'Cabal', 
                    'UnionPay', 
                    'Sorocred', 
                    'Banricompras'
                )
            );

            DELETE FROM accounts.bank_card_flag
            WHERE name IN (
                'Visa', 
                'Mastercard', 
                'American Express', 
                'Elo', 
                'Hipercard', 
                'Diners Club', 
                'Discover', 
                'JCB', 
                'Aura', 
                'Cabal', 
                'UnionPay', 
                'Sorocred', 
                'Banricompras'
            );
    """)
    pass
