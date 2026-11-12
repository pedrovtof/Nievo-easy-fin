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
```
