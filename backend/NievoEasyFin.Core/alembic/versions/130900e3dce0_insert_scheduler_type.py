"""insert scheduler type

Revision ID: 130900e3dce0
Revises: ce28e30461d9
Create Date: 2026-10-12 15:36:30.703734

"""
from typing import Sequence, Union

from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision: str = '130900e3dce0'
down_revision: Union[str, Sequence[str], None] = 'ce28e30461d9'
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None


def upgrade() -> None:
    """Upgrade schema."""
    op.execute("""
        INSERT INTO payment.scheduler_type (name, description, cron_tab, active, created_at, updated_at)
        VALUES 
            ('Diário', 'Repete todos os dias', '0 0 * * *', true, NOW(), NOW()),
            ('Semanal', 'Repete uma vez por semana (todo domingo)', '0 0 * * 0', true, NOW(), NOW()),
            ('Quinzenal', 'Repete a cada 15 dias (dias 1 e 15)', '0 0 1,15 * *', true, NOW(), NOW()),
            ('Mensal', 'Repete uma vez por mês', '0 0 1 * *', true, NOW(), NOW()),
            ('Bimestral', 'Repete a cada dois meses', '0 0 1 */2 *', true, NOW(), NOW()),
            ('Trimestral', 'Repete a cada três meses', '0 0 1 */3 *', true, NOW(), NOW()),
            ('Semestral', 'Repete a cada seis meses', '0 0 1 */6 *', true, NOW(), NOW()),
            ('Anual', 'Repete uma vez por ano', '0 0 1 1 *', true, NOW(), NOW());
    """)
    pass


def downgrade() -> None:
    """Downgrade schema."""
    op.execute("""
        DELETE FROM payment.transaction a WHERE EXISTS(
            SELECT 1  
            FROM payment.scheduler_type b
            WHERE a.scheduler_id = b.id
                AND b.name IN (
                    'Diário',
                    'Semanal',
                    'Quinzenal',
                    'Mensal',
                    'Bimestral',
                    'Trimestral',
                    'Semestral',
                    'Anual'
                )
        );

        DELETE FROM payment.scheduler_type
        WHERE name IN (
            'Diário',
            'Semanal',
            'Quinzenal',
            'Mensal',
            'Bimestral',
            'Trimestral',
            'Semestral',
            'Anual'
        );
    """)
    pass
