# DbDiagram (Core Schema - DBML)

generate with

```sql
pg_dump -h <host> -p <port> -d <database_name> -U <username> -s -F p -E UTF-8 -f <output_path>
```

and import at <https://dbdiagram.io/>

```dbml
Project nievo_easy_fin_core {
  database_type: 'PostgreSQL'
  Note: 'Nievo EasyFin Core Relational Schema (accounts, goals, payment)'
}

Table accounts.bank {
  id serial [pk, increment]
  name varchar(150) [not null]
  bank_type integer [not null]
  active boolean [not null, default: true]
  created_at timestamp [not null, default: `now()`]
  updated_at timestamp [not null, default: `now()`]
}

Table accounts.bank_card {
  id serial [pk, increment]
  bank_id integer [not null]
  name varchar(150) [not null]
  card_type integer [not null]
  flag_id integer [not null]
  active boolean [not null, default: true]
  created_at timestamp [not null, default: `now()`]
  updated_at timestamp [not null, default: `now()`]
}

Table accounts.user_bank_card {
  id serial [pk, increment]
  bank_id integer [not null]
  name varchar(150) [not null]
  card_id integer [not null]
  active boolean [not null, default: true]
  user_id integer [not null]
  expired_at timestamp [not null]
  created_at timestamp [not null, default: `now()`]
  updated_at timestamp [not null, default: `now()`]

  indexes {
    (card_id, user_id) [unique, name: 'idx_unic_user_bank_card']
  }

  Note: 'User wallet cards table with unique index idx_unic_user_bank_card on (card_id, user_id).'
}

Ref fk_bank_card_bank: accounts.bank_card.bank_id > accounts.bank.id
Ref fk_user_bank_card_bank: accounts.user_bank_card.bank_id > accounts.bank.id
Ref fk_user_bank_card_card: accounts.user_bank_card.card_id > accounts.bank_card.id

Table payment.transaction_situation {
  id serial [pk, increment]
  name varchar(150) [not null, note: 'Entrada ou Saida']
  description varchar(255)
  active boolean [not null, default: true]
  created_at timestamp [not null, default: `now()`]
  updated_at timestamp
}

Table payment.transaction_status {
  id serial [pk, increment]
  name varchar(150) [not null]
  description varchar(255)
  active boolean [not null, default: true]
  created_at timestamp [not null, default: `now()`]
  updated_at timestamp
}

Table payment.transaction_type {
  id serial [pk, increment]
  name varchar(150) [not null]
  description varchar(255)
  active boolean [not null, default: true]
  created_at timestamp [not null, default: `now()`]
  updated_at timestamp
}

Table payment.scheduler_type {
  id serial [pk, increment]
  name varchar(150) [not null]
  description varchar(255)
  cron_tab varchar(100)
  active boolean [not null, default: true]
  created_at timestamp [not null, default: `now()`]
  updated_at timestamp
}

Table payment.transaction {
  id uuid [pk, default: `gen_random_uuid()`]
  name varchar(150) [not null]
  description varchar(255)
  user_id integer [not null]
  transaction_status_id integer [not null]
  transaction_type_id integer [not null]
  transaction_situation_id integer [not null, note: 'FK fk_payment_transaction_situation']
  categorie_id integer
  pay_date timestamp [not null]
  amount decimal [not null]
  card_id integer
  bank_id integer
  scheduler_id integer
  created_at timestamp [not null, default: `now()`]
  updated_at timestamp
}

Ref fk_payment_transaction_situation: payment.transaction.transaction_situation_id > payment.transaction_situation.id
Ref fk_payment_transaction_status: payment.transaction.transaction_status_id > payment.transaction_status.id
Ref fk_payment_transaction_type: payment.transaction.transaction_type_id > payment.transaction_type.id
Ref fk_payment_transaction_scheduler: payment.transaction.scheduler_id > payment.scheduler_type.id
```
