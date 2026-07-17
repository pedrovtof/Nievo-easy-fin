"""create constrains accounts

Revision ID: b1de12c452e5
Revises: 070fecfa21da
Create Date: 2026-06-28 15:55:44.695853

"""
from typing import Sequence, Union

from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision: str = 'b1de12c452e5'
down_revision: Union[str, Sequence[str], None] = '070fecfa21da'
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None


def upgrade() -> None:
    """Upgrade schema."""
    op.execute("""
        DO $$
            BEGIN
            IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_user_bank_bank') THEN
                ALTER TABLE accounts.user_bank
                ADD CONSTRAINT fk_user_bank_bank FOREIGN KEY (bank_id) REFERENCES accounts.bank(id);
            END IF;

            IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_bank_bank_type') THEN
                ALTER TABLE accounts.bank
                ADD CONSTRAINT fk_bank_bank_type FOREIGN KEY (bank_type) REFERENCES accounts.bank_type(id);
            END IF;

            IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_bank_card_bank_id') THEN
                ALTER TABLE accounts.bank_card
                ADD CONSTRAINT fk_bank_card_bank_id FOREIGN KEY (bank_id) REFERENCES accounts.bank(id);
            END IF;

            IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_bank_card_card_type') THEN
                ALTER TABLE accounts.bank_card
                ADD CONSTRAINT fk_bank_card_card_type FOREIGN KEY (card_type) REFERENCES accounts.bank_card_type(id);
            END IF;
        END $$;

        GRANT SELECT, USAGE ON ALL SEQUENCES IN SCHEMA accounts TO app_core_service_efn;

        CREATE UNIQUE INDEX IF NOT EXISTS idx_unic_user_bank ON accounts.user_bank (user_id,bank_id);

    """)
    pass


def downgrade() -> None:
    """Downgrade schema."""
    op.execute("""
        DO $$
            BEGIN
            IF EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_user_bank_bank') THEN
                ALTER TABLE accounts.user_bank
                DROP CONSTRAINT fk_user_bank_bank;
            END IF;

            IF EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_bank_bank_type') THEN
                ALTER TABLE accounts.bank
                DROP CONSTRAINT fk_bank_bank_type;
            END IF;

            IF EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_bank_card_bank_id') THEN
               ALTER TABLE accounts.bank_card
               DROP CONSTRAINT fk_bank_card_bank_id;
            END IF;

            IF EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_bank_card_card_type') THEN
               ALTER TABLE accounts.bank_card
               DROP CONSTRAINT fk_bank_card_card_type;
            END IF;
        END $$;

        REVOKE SELECT, USAGE ON ALL SEQUENCES IN SCHEMA accounts FROM app_core_service_efn;

    """)
    pass
