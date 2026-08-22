"""create flag card

Revision ID: a6416d08e6b3
Revises: 9284eee391c3
Create Date: 2026-08-22 07:29:47.790215

"""
from typing import Sequence, Union

from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision: str = 'a6416d08e6b3'
down_revision: Union[str, Sequence[str], None] = '9284eee391c3'
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None


def upgrade() -> None:
    """Upgrade schema."""
    op.execute("""

        CREATE TABLE IF NOT EXISTS accounts.bank_card_flag(
            id SERIAL PRIMARY KEY,
            name VARCHAR(150),
            description VARCHAR(255),
            active  boolean DEFAULT true NOT NULL,
            created_at TIMESTAMP without time zone DEFAULT now() NOT NULL,
            updated_at TIMESTAMP without time zone
        );

        GRANT USAGE ON SCHEMA accounts TO cross_database_user;
        
        GRANT USAGE ON SCHEMA accounts TO app_core_service_efn;

        GRANT SELECT,INSERT,UPDATE ON TABLE accounts."bank_card_flag" TO app_core_service_efn;
        
        ALTER TABLE "accounts"."bank_card" ADD COLUMN flag_id INTEGER NOT NULL;

        DO $$
            BEGIN
            IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_bank_card_flag') THEN
                ALTER TABLE accounts.bank_card
                ADD CONSTRAINT fk_bank_card_flag FOREIGN KEY (flag_id) REFERENCES accounts.bank_card_flag(id);
            END IF;
            
            CREATE UNIQUE INDEX IF NOT EXISTS idx_unic_bank_card_flag_name ON accounts.bank_card_flag ("name");
        END $$;
    """)
    pass


def downgrade() -> None:
    """Downgrade schema."""
    op.execute("""
       DO $$
            BEGIN
            IF EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_bank_card_flag') THEN
                ALTER TABLE accounts.bank_card
                DROP CONSTRAINT fk_bank_card_flag;
            END IF;
        END $$;

        ALTER TABLE "accounts"."bank_card" DROP COLUMN flag_id;

        DROP TABLE IF EXISTS "accounts"."bank_card_flag" CASCADE;
    """)
    pass
