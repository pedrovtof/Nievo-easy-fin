"""create payments constrains

Revision ID: cf4aead7875c
Revises: 05dfe2dffece
Create Date: 2026-10-11 17:26:03.050494

"""
from typing import Sequence, Union

from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision: str = 'cf4aead7875c'
down_revision: Union[str, Sequence[str], None] = '05dfe2dffece'
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None


def upgrade() -> None:
    """Upgrade schema."""
    op.execute("""
        DO $$
            BEGIN
            IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_payment_transaction_status') THEN
                ALTER TABLE payment.transaction
                ADD CONSTRAINT fk_payment_transaction_status FOREIGN KEY (transaction_status_id) REFERENCES payment.transaction_status(id);
            END IF;

            IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_payment_transaction_scheduler') THEN
                ALTER TABLE payment.transaction
                ADD CONSTRAINT fk_payment_transaction_scheduler FOREIGN KEY (scheduler_id) REFERENCES payment.scheduler_type(id);
            END IF;

            IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_payment_transaction_type') THEN
                ALTER TABLE payment.transaction
                ADD CONSTRAINT fk_payment_transaction_type FOREIGN KEY (transaction_type_id) REFERENCES payment.transaction_type(id);
            END IF;

            IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_payment_transaction_user_card') THEN
                ALTER TABLE payment.transaction
                ADD CONSTRAINT fk_payment_transaction_user_card FOREIGN KEY (card_id) REFERENCES accounts.user_bank_card(id);
            END IF;

            IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_payment_transaction_user_bank') THEN
                ALTER TABLE payment.transaction
                ADD CONSTRAINT fk_payment_transaction_user_bank FOREIGN KEY (bank_id) REFERENCES accounts.user_bank(id);
            END IF;

            IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_payment_transaction_category') THEN
                ALTER TABLE payment.transaction
                ADD CONSTRAINT fk_payment_transaction_category FOREIGN KEY (categorie_id) REFERENCES goals.category(id);
            END IF;
        END $$;

        GRANT SELECT, USAGE ON ALL SEQUENCES IN SCHEMA payment TO app_core_service_efn;

        CREATE  INDEX IF NOT EXISTS idx_unic_payment_user ON payment.transaction (user_id);
        
    """)
    pass


def downgrade() -> None:
    """Downgrade schema."""
    op.execute("""
        DO $$
            BEGIN
            IF EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_payment_transaction_status') THEN
                ALTER TABLE payment.transaction
                DROP CONSTRAINT fk_payment_transaction_status;
            END IF;

            IF EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_payment_transaction_scheduler') THEN
                ALTER TABLE payment.transaction
                DROP CONSTRAINT fk_payment_transaction_scheduler;
            END IF;

            IF EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_payment_transaction_type') THEN
                ALTER TABLE payment.transaction
                DROP CONSTRAINT fk_payment_transaction_type;
            END IF;

            IF EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_payment_transaction_user_card') THEN
                ALTER TABLE payment.transaction
                DROP CONSTRAINT fk_payment_transaction_user_card;
            END IF;

            IF EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_payment_transaction_user_bank') THEN
                ALTER TABLE payment.transaction
                DROP CONSTRAINT fk_payment_transaction_user_bank;
            END IF;

            IF EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_payment_transaction_category') THEN
                ALTER TABLE payment.transaction
                DROP CONSTRAINT fk_payment_transaction_category;
            END IF;
        END $$;

        REVOKE SELECT, USAGE ON ALL SEQUENCES IN SCHEMA payment FROM app_core_service_efn;
    """)
    pass
