"""create basic tables schema account

Revision ID: 070fecfa21da
Revises: 89cb77ddb0b0
Create Date: 2026-06-28 15:04:48.513730

"""
from typing import Sequence, Union

from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision: str = '070fecfa21da'
down_revision: Union[str, Sequence[str], None] = '89cb77ddb0b0'
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None


def upgrade() -> None:
    """Upgrade schema."""
    op.execute("""
        CREATE TABLE IF NOT EXISTS "accounts"."user_bank" (
            "id" SERIAL PRIMARY KEY,
            "nick_name" VARCHAR(150),
            "amount" INTEGER,
            "active" boolean DEFAULT true NOT NULL,
            "bank_id" INTEGER,
            "user_id" INTEGER,
            "created_at" TIMESTAMP without time zone DEFAULT now() NOT NULL,
            "updated_at" TIMESTAMP without time zone
        );

        CREATE TABLE IF NOT EXISTS "accounts"."bank" (
            "id" SERIAL PRIMARY KEY,
            "name" VARCHAR(150),
            "bank_type" INT,
            "active" boolean DEFAULT true NOT NULL,
            "created_at" TIMESTAMP without time zone DEFAULT now() NOT NULL,
            "updated_at" TIMESTAMP without time zone
        );

        CREATE TABLE IF NOT EXISTS "accounts"."bank_type" (
            "id" SERIAL PRIMARY KEY,
            "name" VARCHAR(150),
            "description" VARCHAR(255),
            "active" boolean DEFAULT true NOT NULL ,
            "created_at" TIMESTAMP without time zone DEFAULT now() NOT NULL,
            "updated_at" TIMESTAMP without time zone
        );

        CREATE TABLE IF NOT EXISTS "accounts"."bank_card" (
            "id" SERIAL PRIMARY KEY,
            "bank_id" INTEGER,
            "user_id" INTEGER,
            "name" VARCHAR(150),
            "card_type" INTEGER,
            "expire_at" TIMESTAMP without time zone,
            "active" boolean DEFAULT true NOT NULL,
            "created_at" TIMESTAMP without time zone DEFAULT now() NOT NULL,
            "updated_at" TIMESTAMP without time zone
        );

        CREATE TABLE IF NOT EXISTS "accounts"."bank_card_type" (
            "id" SERIAL PRIMARY KEY,
            "name" VARCHAR(150),
            "description" VARCHAR(255),
            "active" boolean DEFAULT true NOT NULL,
            "created_at" TIMESTAMP without time zone DEFAULT now() NOT NULL,
            "updated_at" TIMESTAMP without time zone
        );

        GRANT USAGE ON SCHEMA accounts TO cross_database_user;

        GRANT USAGE ON SCHEMA accounts TO app_core_service_efn;

        GRANT SELECT,INSERT,UPDATE ON TABLE accounts."bank_card" TO app_core_service_efn;

        GRANT SELECT,INSERT,UPDATE ON TABLE accounts."user_bank" TO app_core_service_efn;

        GRANT SELECT,INSERT,UPDATE ON TABLE accounts."bank" TO app_core_service_efn;

        GRANT SELECT,INSERT,UPDATE ON TABLE accounts."bank_type" TO app_core_service_efn;

        GRANT SELECT,INSERT,UPDATE ON TABLE accounts."bank_card_type" TO app_core_service_efn;
    """)
    pass


def downgrade() -> None:
    """Downgrade schema."""
    op.execute("""
        DROP TABLE IF EXISTS "accounts"."bank_card" CASCADE;

        DROP TABLE IF EXISTS "accounts"."user_bank" CASCADE;

        DROP TABLE IF EXISTS "accounts"."bank" CASCADE;
        
        DROP TABLE IF EXISTS "accounts"."bank_type" CASCADE;

        DROP TABLE IF EXISTS "accounts"."bank_card_type" CASCADE;
    """)
    pass
