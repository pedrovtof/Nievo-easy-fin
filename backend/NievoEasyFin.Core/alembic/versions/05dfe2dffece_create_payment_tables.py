"""create payment tables

Revision ID: 05dfe2dffece
Revises: 7159b0d736ad
Create Date: 2026-10-10 13:58:11.232906

"""
from typing import Sequence, Union

from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision: str = '05dfe2dffece'
down_revision: Union[str, Sequence[str], None] = '7159b0d736ad'
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None


def upgrade() -> None:
    """Upgrade schema."""
    op.execute("""
        CREATE TABLE IF NOT EXISTS payment.transaction (
            id SERIAL  PRIMARY KEY,
            name VARCHAR(150),
            description VARCHAR(255),
            user_id INT,
            transaction_status_id INT,
            categorie_id INT,
            pay_date TIMESTAMP,
            amount INT,
            card_id INT,
            bank_id INT,
            transaction_type_id INT,
            scheduler_id INT,
            created_at TIMESTAMP without time zone DEFAULT now() NOT NULL,
            updated_at TIMESTAMP without time zone
        );

        CREATE TABLE IF NOT EXISTS payment.transaction_status (
            id SERIAL  PRIMARY KEY,
            name VARCHAR(150),
            description VARCHAR(255),
            active bool DEFAULT true NOT NULL,
            created_at TIMESTAMP without time zone DEFAULT now() NOT NULL,
            updated_at TIMESTAMP without time zone
        );

        CREATE TABLE IF NOT EXISTS payment.scheduler_type (
            id SERIAL  PRIMARY KEY,
            name VARCHAR(150),
            description VARCHAR(255),
            cron_tab VARCHAR(50),
            active bool DEFAULT true NOT NULL,
            created_at TIMESTAMP without time zone DEFAULT now() NOT NULL,
            updated_at TIMESTAMP without time zone
        );
    
        CREATE TABLE IF NOT EXISTS payment.transaction_type (
            id SERIAL  PRIMARY KEY,
            name VARCHAR(150),
            description VARCHAR(255),
            active bool DEFAULT true NOT NULL,
            created_at TIMESTAMP without time zone DEFAULT now() NOT NULL,
            updated_at TIMESTAMP without time zone
        );
    """)
    pass


def downgrade() -> None:
    """Downgrade schema."""
    op.execute("""
        DROP TABLE IF EXISTS "payment"."transaction" CASCADE;
    
        DROP TABLE IF EXISTS "payment"."transaction_status" CASCADE;

        DROP TABLE IF EXISTS "payment"."scheduler_type" CASCADE;

        DROP TABLE IF EXISTS "payment"."transaction_type" CASCADE;
    """)
    pass
